/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import * as express from 'express';
import Request = express.Request;
import Response = express.Response;
import * as async from 'async';
import * as iothub from 'azure-iothub';
import * as uuid from 'node-uuid';

import {QueryExpression} from '../core/queryExpression';
import {route, middleware, hal, provides} from '@azure-iot/hal/api';
import {Method, LinkRelation} from '@azure-iot/hal/types';
import {logging, logISR, logOSR} from '../core/logger';
import {BunyanLogger} from '@azure-iot/logging/server';

import JobResponse = iothub.JobResponse;
import JobClient = iothub.JobClient;

@provides('jobs')
@logging()
export class JobAPI {

    @logOSR('AzureIotHubJobClient', 'azure-iothub')
    jobClient: JobClient;

    constructor(connectionString: string) {
        this.jobClient = JobClient.fromConnectionString(connectionString);
    }

    @route(Method.POST, '/')
    @provides('history', {
        discoverable: true,
        title: 'history',
        description: 'Gets the history of jobs run for this IoT Hub'
    })
    @hal('get')
    @logISR('1.0')
    QueryJobHistory(req: express.Request, res: express.Response & hal.Response, next) {
        var query: QueryExpression = req.body;
        this.jobClient.queryJobHistory(query, function(error, result: JobResponse[]) {
            if (error) {
                return next(error);
            }
            res.json({ _results: result });
        });
    };

    @route(Method.GET, '/:Id')
    @provides('get')
    @hal()
    @logISR()
    GetJob(req: express.Request, res: express.Response, next) {
        this.jobClient.getJob(req.params.Id, function(error, result: JobResponse) {
            if (error) {
                return next(error);
            }
            res.json(result);
        });
    };

    @route(Method.POST, '/firmwareUpdate')
    @provides('jobFirmware')
    @hal('get')
    @logISR()
    FirmwareUpdateDevices(req: express.Request, res: express.Response, next) {
        let jobId: string = uuid.v4();
        let params = req.body;
        let deviceIds = params.deviceIds;
        let packageUri = params.packageUrl;
        let timeOutInMin = params.timeOutInMin;

        this.jobClient.scheduleFirmwareUpdate(jobId, deviceIds, packageUri, timeOutInMin, function(error, result: JobResponse) {
            if (error) {
                return next(error);
            }
            res.json(result);
        });
    };

    @route(Method.POST, '/reboot')
    @provides('jobReboot')
    @hal('get')
    @logISR()
    RebootDevices(req: express.Request, res: express.Response, next) {
        let jobId: string = uuid.v4();
        let params = req.body;
        let deviceIds = params.deviceIds;

        this.jobClient.scheduleReboot(jobId, deviceIds, function(error, result: JobResponse) {
            if (error) {
                return next(error);
            }
            res.json(result);
        });
    };

    @route(Method.POST, '/factoryReset')
    @provides('jobReset')
    @hal('get')
    @logISR()
    FactoryResetDevices(req: express.Request, res: express.Response, next) {
        let jobId: string = uuid.v4();
        let params = req.body;
        let deviceIds = params.deviceIds;

        this.jobClient.scheduleFactoryReset(jobId, deviceIds, function(error, result: JobResponse) {
            if (error) {
                return next(error);
            }
            res.json(result);
        });
    };

    @route(Method.POST, '/read')
    @provides('jobRead')
    @hal('get')
    @logISR()
    ReadDeviceProperty(req: express.Request, res: express.Response, next) {
        let jobId: string = uuid.v4();
        let params = req.body;
        let deviceIds = params.deviceIds;
        let propertyName = params.propertyName;

        this.jobClient.scheduleDevicePropertyRead(jobId, deviceIds, propertyName, function(error, result: JobResponse) {
            if (error) {
                return next(error);
            }
            res.json(result);
        });
    };

    @route(Method.POST, '/write')
    @provides('devicePropWrite')
    @hal('get')
    @logISR()
    WriteDeviceProperties(req: express.Request, res: express.Response, next) {
        let jobId: string = uuid.v4();
        let params = req.body;
        let deviceIds = params.deviceIds;
        let properties = params.properties;

        this.jobClient.scheduleDevicePropertyWrite(jobId, deviceIds, properties, function(error, result: JobResponse) {
            if (error) {
                return next(error);
            }
            res.json(result);
        });
    };

    @route(Method.POST, '/configurationUpdate')
    @provides('jobConfigurationUpdate')
    @hal('get')
    @logISR()
    UpdateConfigurationDevices(req: express.Request, res: express.Response, next) {
        let jobId: string = uuid.v4();
        let params = req.body;
        let deviceIds = params.deviceIds;
        let value = params.value;

        this.jobClient.scheduleDeviceConfigurationUpdate(jobId, deviceIds, value, function(error, result: JobResponse) {
            if (error) {
                return next(error);
            }
            res.json(result);
        });
    };
}
