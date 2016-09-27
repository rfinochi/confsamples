/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import * as express from 'express';
import Request = express.Request;
import Response = express.Response;
import {Authentication} from '@azure-iot/authentication';

// var favicon = require('serve-favicon');
import * as logger from 'morgan';
import * as cookieParser from 'cookie-parser';
import * as bodyParser from 'body-parser';
import {ServerError} from './core/serverError';
import * as appRoute from './routes/app';

import {route, hal} from '@azure-iot/hal/api';
import {Method, LinkRelation} from '@azure-iot/hal/types';

import {DeviceAPI} from './api/deviceAPI';
import {JobAPI} from './api/jobAPI';

import {Config} from './config';

export async function initialize(): Promise<express.Express> {
    const app = express();
    
    // uncomment after placing your favicon in /public
    // app.use(favicon(path.join(__dirname, 'public', 'favicon.ico')));
    
    const config = Config.get();

    // if caching is not enabled, then we need to disable it
    if (!config.CachingEnabled) {
        // disables etag caching (304s)
        app.disable('etag');
        // force all responses to have a no-cache header; can be overriden in handlers
        app.use((req, res, next) => {
            res.header('Cache-Control', 'no-cache, no-store, must-revalidate');
            res.header('Pragma', 'no-cache');
            res.header('Expires', '0');
            next();
        });
    }

    if (config.Auth) {
        // initialize authentication module and set up middleware that 
        // ensures the user is authenticated:
        const auth = await Authentication.initialize(
            app,
            config.Auth.loginUrl,
            config.Auth.sessionSecret,
            config.Auth.mongoUri);
        
        app.use(auth.ensureAuthenticated);   
    }
    
    if (config.ConsoleReporting === 'both' || config.ConsoleReporting === 'client') {
        app.use(logger('dev'));
    }
    
    app.use(bodyParser.json());
    app.use(bodyParser.urlencoded({ extended: false }));
    app.use(cookieParser());
    
    // initialize routes:
    app.use(appRoute);
    
    app.use('/api/devices', route(new DeviceAPI(config.IotHubConnectionString)));
    app.use('/api/jobs', route(new JobAPI(config.IotHubConnectionString)));
    app.use('/api/discovery', hal.discovery);

    // catch 404 and forward to error handler
    app.use('/*', error404Handler);

    // error handlers
    app.use(error500Handler);
    
    return app;
}

export function error404Handler(req: Request, res: Response, next: Function) {
    var err = new ServerError('Not Found');
    err.status = 404;
    next(err);
}

export function error500Handler(err: ServerError, req: Request, res: Response, next: Function) {
    // SDK embedding response in error object which makes stringify throw because
    // of the circular reference...reaching out to sdk team to fix longterm
    if (err['response']) {
        delete err['response'];
    }

    res.status(err.status || 500);
    res.send({
        _error: err
    });
}
