/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/// <reference path="../typings/tsd.d.ts" />

import * as request from 'supertest';
import * as express from 'express';
import * as cookieParser from 'cookie-parser';
import * as bodyParser from 'body-parser';
import {DeviceAPI} from './deviceAPI';
import {Config} from '../config';
import * as uuid from 'node-uuid';
import {LogicalOperatorType, LogicalExpression, AggregationOperatorType, AggregationProperty,
    AggregationExpression, ProjectionExpression, SortOrder, SortExpression, QueryExpression,
    ComparisonExpression, ComparisonOperatorType, QueryProperty} from '../core/queryExpression';
import {Writable} from 'stream';
import {ServerError} from '../core/serverError';

let converter = require('json-2-csv');

describe('Device API', () => {
    let response: any,
        request: any;

    beforeAll(done => {
        Config.initialize().then(config => {
            let connStr: string = config.IotHubConnectionString;
            this.deviceAPI = new DeviceAPI(connStr);

            expect(this.deviceAPI).toBeDefined();

            request = jasmine.createSpyObj('request', ['params', 'body']);
            response = jasmine.createSpyObj('response', ['json', 'locals', 'set']);
        }).then(done, done.fail);
    });

    describe('Device CRUD - Tests', () => {
        beforeAll((done) => {
            spyOn(this.deviceAPI.registry, 'queryDevices');
            done();
        });

        it('New Devices', (done) => {
            response.json.and.callFake(function (result) {
                expect(result).toBeDefined();
                expect(result.deviceId).toBeDefined();
                done();
            });

            this.deviceAPI.NewDevice(request, response, function (error) {
                done.fail(error);
            });
        });

        it('Should Add devices', (done) => {

            var deviceId = uuid.v4();
            var expected = { 
                deviceId: deviceId,
                authentication: { 
                    symmetricKey: {
                        primaryKey: 'primKey' 
                    }
                } 
            };

            request.body = expected;

            response.json.and.callFake(function (actual) {
                expect(actual.deviceId).toEqual(expected.deviceId);
                done();
            });

            spyOn(this.deviceAPI.registry, 'create').and.callFake(function (device, callback) {
                callback(null, expected);
            });

            this.deviceAPI.AddDevice(request, response, function (error, res) {
                let correctConnString = `${this.deviceApi.host};DeviceId=${deviceId};SharedAccessKey=${expected.authentication.symmetricKey.primaryKey}`;
                expect(res['_deviceConnectionString']).toEqual(correctConnString);
                done.fail(error);
            });

        });

        it('Should handle failing to create a device', (done) => {
            let expected = 'test';

            spyOn(this.deviceAPI.registry, 'create').and.callFake(function (device, callback) {
                callback(expected, null);
            });

            this.deviceAPI.AddDevice(request, response, function (error, res) {
                expect(error).toEqual(expected);
                done();
            });
        });

        it('Should handle successfuly getting a device', (done) => {

            var deviceId = uuid.v4();
            var expected = { 
                deviceId: deviceId,
                status: true,
                authentication: { 
                    symmetricKey: {
                        primaryKey: 'primKey' 
                    }
                } 
            };

            // this is a GET test so modify the req
            request.params.Id = expected.deviceId;

            response.json.and.callFake(function (result) {
                expect(result.deviceId).toEqual(expected.deviceId);
                expect(result.status).toEqual(true);
                done();
            });

            spyOn(this.deviceAPI.registry, 'get').and.callFake(function (device, callback) {
                callback(null, expected);
            });

            this.deviceAPI.GetDevice(request, response, function (error, res) {
                let correctConnString = `${this.deviceApi.host};DeviceId=${deviceId};SharedAccessKey=${expected.authentication.symmetricKey.primaryKey}`;
                expect(res['_deviceConnectionString']).toEqual(correctConnString);
                done.fail(error);
            });

        });

        it('Should handle failing to get a device', (done) => {
            let expected = 'test';

            spyOn(this.deviceAPI.registry, 'get').and.callFake(function (device, callback) {
                callback(expected, null);
            });

            this.deviceAPI.GetDevice(request, response, function (error, res) {
                expect(error).toEqual(expected);
                done();
            });
        });

        it('Should Edit Devices', (done) => {

            var deviceId = uuid.v4();
            var expected = { 'deviceId': deviceId };

            // this is a POST test so modify the res
            request.body = expected;

            response.json.and.callFake(function (actual) {
                expect(actual.deviceId).toEqual(expected.deviceId);
                done();
            });

            spyOn(this.deviceAPI.registry, 'update').and.callFake(function (device, callback) {
                callback(null, expected);
            });

            this.deviceAPI.EditDevice(request, response, function (error) {
                done.fail(error);
            });

        });

        it('Should handle failing to update a device because of missing params', (done) => {
            request.body = null;

            this.deviceAPI.EditDevice(request, response, function (error, res) {
                expect(error instanceof ServerError).toBe(true);
                done();
            });
        });

        it('Should handle failing to update a device', (done) => {
            let expected = 'test';

            request.body = {};

            spyOn(this.deviceAPI.registry, 'update').and.callFake(function (device, callback) {
                callback(expected, null);
            });

            this.deviceAPI.EditDevice(request, response, function (error, res) {
                expect(error).toEqual(expected);
                done();
            });
        });

        it('Should Update Service Properties', (done) => {

            let deviceId = uuid.v4(),
                serviceProperties = {
                    'testProp': 'testVal'
                },
                expected = { 
                    'deviceId': deviceId,
                    'serviceProperties': serviceProperties 
                };

            // this is a POST test so modify the res
            request.body = expected;

            response.json.and.callFake(function (actual) {
                expect(actual).toEqual(expected);
                done();
            });

            spyOn(this.deviceAPI.registry, 'setServiceProperties').and.callFake(function (deviceId, serviceProperties, callback) {
                expect(deviceId).toEqual(expected.deviceId);
                expect(serviceProperties).toEqual(expected.serviceProperties);
                callback(null, expected);
            });

            this.deviceAPI.SetServiceProperties(request, response, function (error) {
                done.fail(error);
            });

        });

        it('Should handle failing to update service properties because of missing params', (done) => {
            request.body = null;

            this.deviceAPI.SetServiceProperties(request, response, function (error, res) {
                expect(error instanceof ServerError).toBe(true);
                done();
            });
        });

        it('Should handle failing to update service properties', (done) => {
            let expected = 'test';

            request.body = {};

            spyOn(this.deviceAPI.registry, 'setServiceProperties').and.callFake(function (deviceId, serviceProperties, callback) {
                callback(expected, null);
            });

            this.deviceAPI.SetServiceProperties(request, response, function (error, res) {
                expect(error).toEqual(expected);
                done();
            });
        });

        it('Should Delete Devices', (done) => {

            var deviceIds = ['test1', 'test2', 'test3'];
            var expected = [null, null, null];

            // this is a POST test so modify the res
            request.body = deviceIds;

            response.json.and.callFake(function (actual) {
                expect(actual._import).not.toEqual(null);
                expect(actual._errors).toEqual({});
                expect(actual._import.total).toEqual(3);
                expect(actual._import.success).toEqual(3);
                expect(actual._import.error).toEqual(0);
                done();
            });

            spyOn(this.deviceAPI.registry, 'delete').and.callFake(function (device, callback) {
                callback(null, expected);
            });

            this.deviceAPI.DeleteDevices(request, response, function (error) {
                done.fail(error);
            });

        });

        it('Delete Devices - Partial Errors', (done) => {

            let deviceIds = ['test1', 'test2', 'test3'],
                error = {
                    responseBody: JSON.stringify({
                        Message : 'test'
                    })
                },
                expected = {
                    'test1' : 'test', 
                    'test2' : 'test', 
                    'test3' : 'test'
                };

            // this is a POST test so modify the res
            request.body = deviceIds;

            response.json.and.callFake(function (actual) {
                expect(actual._import).not.toEqual(null);
                expect(actual._errors).toEqual(expected);
                expect(actual._import.total).toEqual(3);
                expect(actual._import.success).toEqual(0);
                expect(actual._import.error).toEqual(3);
                done();
            });

            spyOn(this.deviceAPI.registry, 'delete').and.callFake(function (device, callback) {
                callback(error, null);
            });

            this.deviceAPI.DeleteDevices(request, response, function (error) {
                done.fail(error);
            });
        });

        it('Delete Devices - Empty Request Body', (done) => {
            request.body = null;

            this.deviceAPI.DeleteDevices(request, response, function (error) {
                expect(error instanceof ServerError).toBeDefined();
                done();
            });

        });

        it('Delete Devices - Empty List of devices', (done) => {
            request.body = [];

            this.deviceAPI.DeleteDevices(request, response, function (error, res) {
                expect(error instanceof ServerError).toBe(true);
                done();
            });
        });

        describe('Get Registry Statistics', () => {
            let fakeAPI;

            beforeEach(() => {
                spyOn(this.deviceAPI.registry, 'getRegistryStatistics').and.callFake(cb => fakeAPI(cb));
            });

            it('should return the result on success', (done) => {
                let expected = 'test';

                fakeAPI = (cb) => {
                    cb(null, expected);
                };

                response.json.and.callFake(function (actual) {
                    expect(actual).toEqual(expected);
                    done();
                });

                this.deviceAPI.GetRegistryStatistics(request, response, null);
            });

            it('should foreward errors to the error handler', (done) => {
                let expected = 'test';

                fakeAPI = (cb) => {
                    cb(expected, null);
                };

                this.deviceAPI.GetRegistryStatistics(request, response, (error) => {
                    expect(error).toEqual(expected);
                    done();
                });
            });
        });

        describe('Export', () => {
            var expected,
                fakeJSON2CSV,
                response;

            beforeEach(() => {
                expected = [
                    { deviceId: uuid.v4() },
                    { deviceId: uuid.v4() },
                    { deviceId: uuid.v4() }
                ];

                request.body = expected;

                spyOn(converter, 'json2csv').and.callFake((devices, cb) => fakeJSON2CSV(devices, cb));

                response = new Writable();

                response.set = () => {};

                spyOn(response, 'set');
                spyOn(response, 'write');
            });

            it('On Successful conversion', (done) => {
                let expectedCSV = 'test',
                    expectedContentType = 'application/csv',
                    expectedFileName = `${expected.length}_devices.csv`;

                fakeJSON2CSV = (devices, cb) => {
                    cb(null, expectedCSV);
                };

                response.set.and.callFake(function (headers) {
                    expect(headers['Content-Type']).toEqual(expectedContentType);
                    expect(headers['Content-Disposition']).toEqual(expectedFileName);
                });

                response.write.and.callFake(function (buffer) {
                    expect(buffer.toString()).toEqual(expectedCSV);
                    done();
                });

                this.deviceAPI.ExportDevices(request, response, function (error, res) {
                    done.fail(error);
                });
            });

            it('On Unsuccessful conversion', (done) => {
                let expectedError = 'test';

                fakeJSON2CSV = (devices, cb) => {
                    cb(expectedError, null);
                };

                this.deviceAPI.ExportDevices(request, response, function (error, res) {
                    expect(error).toEqual(expectedError);
                    done();
                });
            });
        });
    });

    describe('Query Tests', () => {
        var deviceAPI: DeviceAPI;
        var deviceId1: string;

        it('Should handle failing to Query a device', (done) => {
            let expected = 'test';

            spyOn(this.deviceAPI.registry, 'queryDevices').and.callFake(function (query, callback) {
                callback(expected, null);
            });

            this.deviceAPI.QueryDevices(request, response, function (error, res) {
                expect(error).toEqual(expected);
                done();
            });
        });

        it('Should Add devices', (done) => {
            request.body = { deviceId: uuid.v4(), serviceProperties: { tags: ['tag1', 'tag2'] } };

            deviceId1 = request.body.deviceId;

            response.json.and.callFake(function (result) {
                expect(result).toBeDefined();
                expect(result.deviceId = request.body.deviceId);
                done();
            });

            this.deviceAPI.AddDevice(request, response, function (error) {
                done.fail();
                done(error);
            });
        });

        it('Should QueryDevices - query all', (done) => {
            request.body = {
            };

            response.json.and.callFake(function (result) {
                expect(result).toBeDefined();
                expect(result._results).toBeDefined();
                done();
            });

            this.deviceAPI.QueryDevices(request, response, function (error) {
                done.fail();
                done(error);
            });
        });

        it('Should QueryDevices - filter by tags', (done) => {
            let qe = new QueryExpression();
            let ce = new ComparisonExpression(
                new QueryProperty('tags'),
                ['tag1'],
                ComparisonOperatorType.All);
            qe.filter = ce;

            request.body = qe;

            response.json.and.callFake(function (result) {
                expect(result).toBeDefined();
                expect(result._results.length).toBeDefined();
                expect(result._results).toBeDefined();
                expect(result._results[0].serviceProperties.tags.indexOf('tag1') !== -1).toBe(true);
                done();
            });

            this.deviceAPI.QueryDevices(request, response, function (error) {
                done.fail();
                done(error);
            });
        });


        it('Should QueryDevices - sort by deviceId', (done) => {
            let qe = new QueryExpression();
            let se = new SortExpression(SortOrder.desc, new QueryProperty('deviceId'));
            qe.sort[0] = se;

            request.body = qe;

            response.json.and.callFake(function (result) {
                expect(result).toBeDefined();
                expect(result._results.length).toBeDefined();
                expect(result._results).toBeDefined();
                done();
            });

            this.deviceAPI.QueryDevices(request, response, function (error) {
                done.fail();
                done(error);
            });
        });

        it('Should QueryDevices - projection of deviceId', (done) => {
            let qe = new QueryExpression();
            let pe = new ProjectionExpression(true, [new QueryProperty('deviceId')]);
            qe.project = pe;

            request.body = qe;

            response.json.and.callFake(function (result) {
                expect(result).toBeDefined();
                expect(result._results.length).toBeDefined();
                expect(result._results).toBeDefined();
                expect(result._results[0].deviceId).toBeDefined();
                done();
            });

            this.deviceAPI.QueryDevices(request, response, function (error) {
                done.fail();
                done(error);
            });
        });

        it('Should QueryDevices - Select deviceId != deviceToBeAddedId AND (tag == tag1 OR tag == tag2)', (done) => {
            let qe = new QueryExpression();

            let ce1 = new ComparisonExpression(
                new QueryProperty('tags'),
                ['tag1'],
                ComparisonOperatorType.All);

            let ce2 = new ComparisonExpression(
                new QueryProperty('tags'),
                ['tag2'],
                ComparisonOperatorType.All);

            let ce3 = new ComparisonExpression(
                new QueryProperty('deviceId'),
                deviceId1,
                ComparisonOperatorType.NotEquals);

            let le = new LogicalExpression(LogicalOperatorType.or, [ce1, ce2]);

            let le2 = new LogicalExpression(LogicalOperatorType.and, [le, ce3]);
            qe.filter = le2;

            request.body = qe;

            response.json.and.callFake(function (result) {
                expect(result).toBeDefined();
                expect(result._results.length).toBeDefined();
                expect(result._results).toBeDefined();
                done();
            });

            this.deviceAPI.QueryDevices(request, response, function (error) {
                done.fail();
                done(error);
            });
        });
    });
});