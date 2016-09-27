/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import * as express from 'express';
import * as fs from 'fs';
import * as path from 'path';

import {BunyanLogger, OutgoingServiceRequest} from '@azure-iot/logging/server';
import {middleware} from '@azure-iot/hal/api';
import {ExpressMiddleware} from '@azure-iot/logging/server';
import {DMUXLogStream} from './dmuxLogStream';
import {shadow} from './shadow';
import {Config} from '../config';

const packageJSON = require(path.join(__dirname, '../../../package.json'));

const DEFAULT_LOG_NAME = 'DMUX';

const LOG_ISR_KEY = Symbol('LOG_ISR_KEY');
const LOG_OSR_KEY = Symbol('LOG_OSR_KEY');
const OSR_METHODS_KEY = Symbol('OSR_METHODS_KEY');

// needed for extending classes on generic types in an expression
interface IAny {};

interface IOSRLoggingTarget {
    instanceName: string; 
    version: string;
    namespace: string;
    methodNames: string[];
}

interface IISRLoggingTarget { 
    methodName: string; 
    version: string;
}

interface IUserConfig {
    ConsoleReporting: string;
    LogLevel: string;
    IotHubConnectionString: string;
}

type ExpressHandlerDescriptor = TypedPropertyDescriptor<express.RequestHandler>;

type ConstructorFunction = new (...args: any[]) => IAny;

// general decorators to be promoted to the logging lib

/**
 * Property Decorator Factory that collects metadata used to instrument OSR logging at runtime
 * @param {string}   nameSpace   namespace to use in the logged operation name
 * @param {string}   packageName npm package name of the OSR sdk/api/package
 * @param {string[]} methodNames explicit override of method names to log 
 */
export function logOSR(nameSpace: string, packageName: string, methodNames?: string[]) {
    return function(target: Object, prop: string) {
        if (!target[LOG_OSR_KEY]) {
            Object.defineProperty(target, LOG_OSR_KEY, { value: [] });
        }           

        let version = packageJSON.dependencies[packageName],
            methodOverrides = methodNames && methodNames.length ? methodNames : null;

        target[LOG_OSR_KEY].push({instanceName: prop, namespace: nameSpace, version: version, methodNames: methodOverrides });
    };
}

/**
 * Iterates through each property that has been marked for OSR logging and replaces it with a shadow object.
 * This shadow logs function calls and calls the underlying instance
 * @param  {BunyanLogger}        logger     the logger to use
 * @param  {ConstructorFunction} target     the original constructor function
 * @param  {IOSRLoggingTarget[]} properties the metadata for the properties to log
 * @param  {string}              iotHubConnStr  the connection string to the Iot Hub. 
 */
function instrumentOSRLogging(logger: BunyanLogger, target: ConstructorFunction, properties: IOSRLoggingTarget[], iotHubConnStr: string): void {
    properties.forEach((loggableTarget) => {
        let logObjectName = loggableTarget.instanceName,
            logMethods = loggableTarget.methodNames,
            logNamespace = loggableTarget.namespace || 'default',
            logObject = this[logObjectName],
            version = loggableTarget.version;
            
        // resolve the filter strategy to use
        let filter = shadowOSRFunctionFilter(logMethods);

        // create the function that logs OSR information tied to the specific proprty/namespace/version/host
        let osrLogger = createShadowOSRLogger(logger, logNamespace, target.name, logObjectName, version, iotHubConnStr);

        // replace the original property with a shadow object that emits OSR logs 
        this[logObjectName] = shadow(logObject, filter, osrLogger);
    });
}

function extendConstructor(target: ConstructorFunction, newFunctionality: Function): ConstructorFunction {
    let newClass = class extends target {
        constructor(...args: any[]) {
            super(...args);
            newFunctionality.apply(this, [target]);
        }
    };

    return newClass;
}

// proxies a node callback and does the actual OSR log before delegating to the original
function logNodeOSR(logger: BunyanLogger, operationName: string, connectionString: string, version: string, context: string, args: any[], callback: (...args: any[]) => any): (err: any) => void {
    let start = process.hrtime();
    return function(err: any) {
        let duration = process.hrtime(start);
        let durationMs = Math.floor((duration[0] * 1e3) + (duration[1] * 1e-6));

        let osr = new OutgoingServiceRequest(
            operationName,
            durationMs,
            connectionString,
            err ? false : true
        );

        osr['args'] = args; 
        osr['context'] = context;
        osr['version'] = version;

        logger.informational(osr);

        callback(...arguments);
    };
};

// wraps all the information needed to log an OSR
function createShadowOSRLogger(logger: BunyanLogger, logNamespace: string, className: string, logObjectName: string, version: string, iotHubConnStr: string) {
    // changing args to log before delegating to the original object/method
    return function(instance: any, methodName: string, originalMethod: any, args: any[]): any {
        let operationName = `${logNamespace}.${methodName}`,
            context = `${className}.${logObjectName}.${methodName}`,
            connectionString = iotHubConnStr,
            originalCallback = args.pop();
            
        // replaces the node callback 
        args.push(logNodeOSR(logger, operationName, connectionString, version, context, args, originalCallback));

        return originalMethod.apply(instance, args);
    };
}

function shadowOSRFunctionFilter(logMethods?: string[]) {
    if (logMethods) {
        return function(prop: string): boolean {
            return logMethods.indexOf(prop) !== -1;
        };
    } else {
        return function(prop: string): boolean {
            return (prop[0] !== '_');
        };
    }
}

// Hal Specific logic (dependent on @middleware)

/**
 * Method Decorator Factory that collects metadata for ISR logging
 * @param {string} version specific API version to log
 */
export function logISR(version?: string) {
    return function(target: Object, methodName?: string | symbol, descriptor?: ExpressHandlerDescriptor): ExpressHandlerDescriptor | void {
        if (!target[LOG_ISR_KEY]) {
            Object.defineProperty(target, LOG_ISR_KEY, { value: [] });
        }           

        target[LOG_ISR_KEY].push({ methodName: methodName, version: version });
    };
};

/**
 * Class Decorator Factory to enable logging.
 * @return a class decorator that enables ISR and OSR logs.
 */
export function logging() {
    // we're creating a decorator factory here instead of a plain decorator
    // because we want to call Config.get() only when the constructor is called,
    // not when target is included in a different file. This enables start.ts
    // to initialize Config first. 
    return function (target: new (...args: any[]) => any) {
        return <new (...args: any[]) => any>extendConstructor(target, function(target) {
            const userConfig = Config.get();
            let loggingMethods: IISRLoggingTarget[] = target.prototype[LOG_ISR_KEY] || [];
            let logger = new BunyanLogger({
                name: DEFAULT_LOG_NAME,
                streams: [
                    { 
                        level: userConfig.LogLevel || 'trace', 
                        stream: new DMUXLogStream(userConfig.ConsoleReporting)
                    }
                ]
            });
                
            enableExceptionLogging(logger, target);
            
            loggingMethods.forEach((method) => {
                instrumentISRLogging(logger, target, method.methodName, method.version || packageJSON.version);
            });
            
            let loggingAPIProperties: IOSRLoggingTarget[] = target.prototype[LOG_OSR_KEY] || []; 
            instrumentOSRLogging.apply(this, [logger, target, loggingAPIProperties, userConfig.IotHubConnectionString]);
        });
    };
};

// wraps HALs @middleware decorator and ISR logging middleware for a method
function instrumentISRLogging(logger: BunyanLogger, target: Function, methodName: string, logVersion: string): void {
    let logName = `${target.name}.${methodName}`;
    let addISRLogging: (...args: any[]) => ExpressHandlerDescriptor = middleware(ExpressMiddleware.logISR(logger, logName, logVersion));

    let proto = target.prototype,
        propDescriptor = Object.getOwnPropertyDescriptor(proto, methodName);

    let decoratedDescriptor = addISRLogging(proto, methodName, propDescriptor);
    Object.defineProperty(proto, methodName, decoratedDescriptor);
}

// wraps HALs @middleware decorator and Exception logging for a class
function enableExceptionLogging(logger: BunyanLogger, target: Function) {
    let addExceptionLogging = middleware(ExpressMiddleware.logExceptions(logger), { error: true});
    addExceptionLogging(target);
}