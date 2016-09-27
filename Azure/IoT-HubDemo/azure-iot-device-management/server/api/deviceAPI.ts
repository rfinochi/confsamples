/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import * as express from 'express';
import Request = express.Request;
import Response = express.Response;
import * as async from 'async';

import {ServerError} from '../core/serverError';
import {readFileSync, unlinkSync} from 'fs';
import {QueryExpression} from '../core/queryExpression';
import {Readable} from 'stream';
import {logging, logISR, logOSR} from '../core/logger';

import {route, middleware, hal, provides} from '@azure-iot/hal/api';
import {Method} from '@azure-iot/hal/types';

import * as uuid from 'node-uuid';
import * as iothub from 'azure-iothub';
import Registry = iothub.Registry;
import Device = iothub.Device;

let converter = require('json-2-csv');
let multipart = require('connect-multiparty');

const CSV = 'application/csv';
const HOSTNAME = 'HostName=';

// using DMUX specific convenience methods
@provides('devices')
@logging()
export class DeviceAPI {

    @logOSR('AzureIotHubRegistry', 'azure-iothub')
    public registry: Registry;
    public host: string;

    constructor(connectionString: string) {
        this.registry = Registry.fromConnectionString(connectionString);
        this.host = this.connectionStringToHost(connectionString);
    }

    private connectionStringToHost(connectionString: string): string {
        let dirtyHost = connectionString.split(HOSTNAME)[1];
        return dirtyHost.split(';')[0];
    }

    private addDeviceConnectionString(result: Device & { _deviceConnectionString: string }) {
        let deviceId = result.deviceId,
            sharedAccessKey = result.authentication.symmetricKey.primaryKey;

        result._deviceConnectionString = `${HOSTNAME + this.host};DeviceId=${deviceId};SharedAccessKey=${sharedAccessKey}`;
    }

    @route(Method.POST, '/export')
    @provides('export')
    @logISR()
    ExportDevices(req: express.Request, res: Response, next) {
        let devices = req.body,
            deviceCount = devices.length,
            options = { checkSchemaDifferences: false };

        converter.json2csv(devices, function(err, csv) {
            if (err) {
                return next(err);
            } else {
                res.set({
                    'Content-Type': CSV,
                    'Content-Disposition': `${deviceCount}_devices.csv`
                });

                let stream = new Readable();
                stream.push(csv);
                stream.push(null);

                stream.pipe(res);
            }
        }, options);
    };

    @route(Method.GET, '/registryStatistics')
    @provides('registryStatistics', {
        discoverable: true
    })
    @hal()
    @logISR()
    GetRegistryStatistics(req: express.Request, res: express.Response, next) {
        this.registry.getRegistryStatistics(function(err, result) {
            if (err) {
                return next(err);
            } else {
                res.json(result);
            }
        });
    }

    @route(Method.GET, '/new')
    @provides('new', {
        discoverable: true,
        title: 'new',
        description: 'Provide a new device object with id'
    })
    @hal('add')
    @logISR()
    NewDevice(req: express.Request, res: express.Response & hal.Response, next) {
        var device = new Device();
        device.deviceId = uuid.v4();
        res.json(device);
    };

    @route(Method.GET, '/:Id')
    @provides('get', {
        discoverable: true,
        title: 'getDevice',
        description: 'Returns the most current Device from the Device Registry'
    })
    @hal('edit', 'servicePropWrite', 'jobs:devicePropWrite', 'jobs:jobConfigurationUpdate')
    @logISR()
    GetDevice(req: express.Request, res: express.Response, next) {
        this.registry.get(req.params.Id, (error, result) => {
            if (error) {
                return next(error);
            }

            this.addDeviceConnectionString(result);

            res.json(result);
        });
    };

    @route(Method.POST, '/queryGets')
    @provides('queryGets', {
        discoverable: true,
        title: 'gets',
        description: 'Returns list of devices via query'
    })
    @hal('get', 'delete', 'edit', 'export', 'jobs:jobReboot', 'jobs:jobFirmware', 'jobs:jobReset')
    @logISR()
    QueryDevices(req: Request, res: Response & hal.Response, next) {
        var query: QueryExpression = req.body;

        this.registry.queryDevices(query, function(error, result) {
            if (error) {
                return next(error);
            }
            res.json({ _results: result });
        });
    };

    @route(Method.POST, '/')
    @provides('add')
    @hal('get', 'queryGets', 'delete', 'edit', 'export', 'jobs:jobReboot', 'jobs:jobFirmware', 'jobs:jobReset')
    @logISR()
    AddDevice(req: express.Request, res: express.Response, next) {
        var device: Device = req.body;

        this.registry.create(device, (error, result) => {
            // we will not throw 500 for partial errors//
            if (error) {
                return next(error);
            } else {
                this.addDeviceConnectionString(result);

                res.json(result);
            }
        });
    };

    @route(Method.POST, '/newBulk')
    @provides('newBulk', {
        discoverable: true,
        title: 'newBulk',
        description: 'Provides ability to create max 100 devices using csv data'
    })
    @middleware(multipart())
    @hal('get', 'queryGets', 'delete', 'edit', 'export', 'jobs:jobReboot', 'jobs:jobFirmware', 'jobs:jobReset')
    @logISR()
    AddDevicesCsv(req: express.Request, res: express.Response, next) {
        let file,
            totalCount = 0,
            succcesCount = 0,
            errorCount = 0,
            devices = [],
            errorResult = {},
            files = (<any>req).files.file;

        // only honor one file
        if (Array.isArray(files)) {
            file = files[0];
        } else {
            file = files;
        }
        let csv = readFileSync(file.path, { encoding: 'utf8' }),
            options = { TRIM_HEADER_FIELDS: true, TRIM_FIELD_VALUES: true };

        converter.csv2json(csv, function(err, jsonDevices) {
            if (err) throw err;

            jsonDevices.forEach(d => devices.push(d));
        }, options);

        totalCount = devices.length;

        if (totalCount > 100) {
            return next(new ServerError('Max device count exceeded. Only 100 allowed'));
        }

        async.each(devices, (device: Device, callback) => {
            this.registry.create(device, function(error, result) {
                // we will not throw 500 for partial errors//
                if (error) {
                    errorResult[device.deviceId] = JSON.parse(error.responseBody).Message;
                    errorCount++;
                } else {
                    succcesCount++;
                }

                callback();
            });
        }, (error) => {
            res.json({
                _import: {
                    total: totalCount,
                    success: succcesCount,
                    error: errorCount
                },
                _errors: errorResult
            });
        });

        // remove temp file multipart creates
        unlinkSync(file.path);
    };

    @route(Method.DELETE, '/')
    @provides('delete')
    @logISR()
    DeleteDevices(req: express.Request, res: express.Response, next) {
        let totalCount = 0,
            succcesCount = 0,
            errorCount = 0,
            errorResult = {};

        if (!req.body) {
            return next(new ServerError('Request payload is empty'));
        }

        if (req.body.length) {
            async.each(req.body, (deviceId: string, callback) => {
                totalCount++;

                this.registry.delete(deviceId, function(error, result) {
                    if (error) {
                        // we will not throw 500 for partial errors
                        errorResult[deviceId] = JSON.parse(error.responseBody).Message;
                        errorCount++;
                    }
                    else {
                        succcesCount++;
                    }

                    callback();
                });
            }, (error) => {
                res.json({
                    _import: {
                        total: totalCount,
                        success: succcesCount,
                        error: errorCount
                    },
                    _errors: errorResult
                });
            });
        } else {
            return next(new ServerError('Required list of device IDs is empty'));
        }
    }

    @route(Method.PUT, '/edit')
    @provides('edit')
    @hal('get')
    @logISR()
    EditDevice(req: express.Request, res: express.Response, next) {

        if (!req.body) {
            return next(new ServerError('Request payload is empty'));
        }

        this.registry.update(req.body, function(error, result) {
            // we will not throw 500 for partial errors//
            if (error) {
                return next(error);
            } else {
                res.json(result);
            }
        });
    };

    @route(Method.POST, '/servicePropWrite')
    @provides('servicePropWrite')
    @hal('get')
    @logISR()
    SetServiceProperties(req: express.Request, res: express.Response, next) {
        if (!req.body) {
            return next(new ServerError('Request payload is empty'));
        }

        let { deviceId, serviceProperties } = req.body;

        this.registry.setServiceProperties(deviceId, serviceProperties, function(error, result) {
            if (error) {
                return next(error);
            } else {
                res.json(result);
            }
        });
    };
}