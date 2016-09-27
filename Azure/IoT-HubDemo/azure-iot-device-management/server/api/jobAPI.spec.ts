/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/// <reference path="../typings/tsd.d.ts" />

import * as request from 'supertest';
import * as express from 'express';
import * as cookieParser from 'cookie-parser';
import * as bodyParser from 'body-parser';
import {JobAPI} from './jobAPI';
import * as uuid from 'node-uuid';
import * as iothub from 'azure-iothub';
import Registry = iothub.Registry;
import Device = iothub.Device;

describe('Job API Tests', () => {

    let deviceId: string;
    let jobAPI: JobAPI;
    let registry: Registry;
    let response: any;
    let request: any;
    let jobId: string;

    beforeAll(() => {
        let connStr: string = 'HostName=invalid.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=hQa2DxgzwieLAzx3ZpIsjRu1/2wnuKemhiuUqqFyz0M=';
        jobAPI = new JobAPI(connStr);
        expect(jobAPI).toBeDefined();

        
    });

    beforeEach(() => {
        deviceId = uuid.v4();
          
        // Add device
        let device: Device = new Device();
        device.deviceId = deviceId;
        device.serviceProperties.tags[0] = 'tag1';
        device.serviceProperties.tags[1] = 'tag2';
    
        request = jasmine.createSpyObj('request', ['params', 'body']);
        response = jasmine.createSpyObj('response', ['json', 'locals']);
    });

    describe('RebootDevices', () => {
        it('should return a job', (done) => {
            let expected = {
                jobId : 'jobId1'
            };

            request.body = {
                deviceIds: [deviceId],
                packageUrl: 'http://bing.com/',
                timeOutInMin: 5
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result.jobId).toBeDefined();
                done();
            });

            spyOn(jobAPI.jobClient, 'scheduleFirmwareUpdate').and.callFake(function (jobId, deviceIds, packageUri, timeOutInMin, callback) {
                callback(null, expected);
            });

            jobAPI.FirmwareUpdateDevices(request, response, null);
        });
        
        it('passes error onto next', (done) => {
            let error = new Error();

            response.json.and.callFake(function(result) {
                done.fail();
            });

            spyOn(jobAPI.jobClient, 'scheduleFirmwareUpdate').and.callFake(function (jobId, deviceIds, packageUri, timeOutInMin, callback) {
                callback(error, null);
            });

            jobAPI.FirmwareUpdateDevices(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
    });

    describe('RebootDevices', () => {
        it('should return a job', (done) => {
            let expected = {
                jobId : 'jobId1'
            };

            request.body = {
                deviceIds: [deviceId],
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result.jobId).toBeDefined();
                done();
            });

            spyOn(jobAPI.jobClient, 'scheduleReboot').and.callFake(function (jobId, deviceIds, callback) {
                callback(null, expected);
            });

            jobAPI.RebootDevices(request, response, function(error) {
                done.fail();
            });
        });
        
        it('passes error onto next', (done) => {
            let error = new Error();

            response.json.and.callFake(function(result) {
                done.fail();
            });

            spyOn(jobAPI.jobClient, 'scheduleReboot').and.callFake(function (jobId, deviceIds, callback) {
                callback(error, null);
            });

            jobAPI.RebootDevices(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
    });

    describe('FactoryResetDevices', () => {
        it('should return a job', (done) => {
            let expected = {
                jobId : 'jobId1'
            };

            request.body = {
                deviceIds: [deviceId],
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result.jobId).toBeDefined();
                done();
            });

            spyOn(jobAPI.jobClient, 'scheduleFactoryReset').and.callFake(function (jobId, deviceIds, callback) {
                callback(null, expected);
            });

            jobAPI.FactoryResetDevices(request, response, function(error) {
                done.fail();
            });
        });
        
        it('passes error onto next', (done) => {
            let error = new Error();

            response.json.and.callFake(function(result) {
                done.fail();
            });

            spyOn(jobAPI.jobClient, 'scheduleFactoryReset').and.callFake(function (jobId, deviceIds, callback) {
                callback(error, null);
            });

            jobAPI.FactoryResetDevices(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
    });

    describe('ReadDeviceProperty', () => {
        it('should return a job', (done) => {
            let expected = {
                jobId : 'jobId1'
            };

            request.body = {
                deviceIds: [deviceId],
                propertyName: 'systemProp1'
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result.jobId).toBeDefined();
                done();
            });

            spyOn(jobAPI.jobClient, 'scheduleDevicePropertyRead').and.callFake(function (jobId, deviceIds, propertyName, callback) {
                callback(null, expected);
            });

            jobAPI.ReadDeviceProperty(request, response, function(error) {
                done.fail();
            });
        });
        
        it('passes error onto next', (done) => {
            let error = new Error();

            response.json.and.callFake(function(result) {
                done.fail();
            });

            spyOn(jobAPI.jobClient, 'scheduleDevicePropertyRead').and.callFake(function (jobId, deviceIds, propertyName, callback) {
                callback(error, null);
            });

            jobAPI.ReadDeviceProperty(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
        
    });

    describe('WriteDeviceProperties', () => {
        it('should return a job', (done) => {
            let expected = {
                jobId : 'jobId1'
            };

            request.body = {
                deviceIds: [deviceId],
                properties: { 'systemProp1': 'test' }
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result.jobId).toBeDefined();
                done();
            });

            spyOn(jobAPI.jobClient, 'scheduleDevicePropertyWrite').and.callFake(function (jobId, deviceIds, properties, callback) {
                callback(null, expected);
            });

            jobAPI.WriteDeviceProperties(request, response, function(error) {
                done.fail();
            });
        });
        
        it('passes error onto next', (done) => {
            let error = new Error();

            response.json.and.callFake(function(result) {
                done.fail();
            });

            spyOn(jobAPI.jobClient, 'scheduleDevicePropertyWrite').and.callFake(function (jobId, deviceIds, value, callback) {
                callback(error, null);
            });

            jobAPI.WriteDeviceProperties(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
    });

    describe('UpdateConfigurationDevices', () => {
        it('should return a job', (done) => {
            let expected = {
                jobId : 'jobId1'
            };

            request.body = {
                deviceIds: [deviceId],
                value: 'configValue'
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result.jobId).toBeDefined();
                done();
            });

            spyOn(jobAPI.jobClient, 'scheduleDeviceConfigurationUpdate').and.callFake(function (jobId, deviceIds, value, callback) {
                callback(null, expected);
            });

            jobAPI.UpdateConfigurationDevices(request, response, function(error) {
                done.fail();
            });
        });
        
        it('on error passes onto next', (done) => {
            let error = new Error();

            response.json.and.callFake(function(result) {
                done.fail();
            });

            spyOn(jobAPI.jobClient, 'scheduleDeviceConfigurationUpdate').and.callFake(function (jobId, deviceIds, value, callback) {
                callback(error, null);
            });

            jobAPI.UpdateConfigurationDevices(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
    });

    describe('GetJob', () => {
        
        it('should get a job', (done) => {
            let expected = {
                jobId : 'jobId1'
            };

            request.params = {
                Id: jobId,
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result.jobId).toBeDefined();
                expect(result.jobId).toEqual('jobId1');
                done();
            });

            spyOn(jobAPI.jobClient, 'getJob').and.callFake(function (jobId, callback) {
                callback(null, expected);
            });

            jobAPI.GetJob(request, response, function(error) {
                done.fail();
            });
        });
        
        it('should pass an error onto next handler', (done) => {
            let error = new Error();
            
            response.json.and.callFake(function(result) {
                done.fail();
            });
            
            spyOn(jobAPI.jobClient, 'getJob').and.callFake(function (query, callback) {
                callback(error, null);
            });
            
            jobAPI.GetJob(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
    });

    describe('QueryJobHistory', () => {
        it('should return jobs', (done) => {
            let expected = {
                _results: []
            };

            response.json.and.callFake(function(result) {
                expect(result).toBeDefined();
                expect(result._results).toBeDefined();
                done();
            });

            spyOn(jobAPI.jobClient, 'queryJobHistory').and.callFake(function (query, callback) {
                callback(null, expected);
            });

            jobAPI.QueryJobHistory(request, response, function(error) {
                done.fail();
            });
        });
        
        it('should pass an error onto next handler', (done) => {
            let error = new Error();
            
            response.json.and.callFake(function(result) {
                done.fail();
            });
            
            spyOn(jobAPI.jobClient, 'queryJobHistory').and.callFake(function (query, callback) {
                callback(error, null);
            });
            
            jobAPI.QueryJobHistory(request, response, nextError => {
                expect(nextError).toEqual(error);
                done();
            });
        });
    });
});