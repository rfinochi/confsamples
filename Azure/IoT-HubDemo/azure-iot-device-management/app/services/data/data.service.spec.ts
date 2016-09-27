/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {
    beforeEach,
    beforeEachProviders,
    describe,
    expect,
    it,
    inject,
    injectAsync
} from '@angular/core/testing';
import {Http, Response, ResponseOptions, Headers} from '@angular/http';
import {DataService} from './data.service';
import {BehaviorSubject} from 'rxjs/Rx';
import {Device} from '../../models/device';
import {LoggerService} from '../logging/logger.service';

describe('Data Service Tests', () => {

    let dataService: DataService;
    let fakeHttp: Http;

    let mockDiscovery = {
        '_links': {
            'self': {
                'href': '/'
            },
            'curies': [
                {
                    'href': '/api/devices/docs/devices/{rel}',
                    'templated': true,
                    'name': 'devices'
                },
                {
                    'href': '/api/jobs/docs/jobs/{rel}',
                    'templated': true,
                    'name': 'jobs'
                }
            ],
            'devices:new': {
                'href': '/api/devices/new',
                'title': 'new'
            },
            'devices:queryGets': {
                'href': '/api/devices/query',
                'title': 'gets'
            },
            'devices:newBulk': {
                'href': '/api/devices/newBulk',
                'title': 'newBulk'
            },
            'devices:get': {
                'href': '/api/devices/{Id}',
                'templated': true,
                'title': 'get'
            },
            'devices:gets': {
                'href': '/api/devices/',
                'title': 'gets'
            },
            'jobs:history': {
                'href': '/api/jobs/',
                'title': 'history'
            },
            'jobs:get': {
                'href': '/api/jobs/{Id}',
                'templated': true,
                'title': 'get'
            }
        }
    };

    beforeEach(() => {
        fakeHttp = <any>{
            get: () => {
                return new BehaviorSubject(new Response(new ResponseOptions({ body: JSON.stringify(mockDiscovery) })));
            },
            request: () => {
                return new BehaviorSubject(new Response(new ResponseOptions({ body: JSON.stringify({}) })));
            }
        };
    });

    it('Should Construct', () => {
        var target = new DataService(fakeHttp, new LoggerService());
        expect(target).toBeDefined();
    });


    it('Should set Discovery()', () => {
        var target = new DataService(fakeHttp, new LoggerService());
        expect(target).toBeDefined();

        target.discovery().subscribe(actual => {
            expect(actual['self']).toBeDefined();
            expect(actual['curies']).toBeDefined();
            expect(actual['curies'].length).toEqual(2);
        });
    });

    it('Should Get a Device', (done) => {
        var target = new DataService(fakeHttp, new LoggerService());

        let expected = new Device();
        expected.deviceId = 'test';
        expected.etag = 'test-etag';

        let mockDevice = {
            '_links': {
                'self': {
                    'href': '/'
                }
            },
            'deviceId': 'test',
            'etag': 'test-etag'
        };

        let fakeRequest = new BehaviorSubject(new Response(new ResponseOptions({
            body: JSON.stringify(mockDevice)
        })));

        // This fakes out the get operation that will be done by the get devices, returning the fake data above
        fakeHttp.request = url => fakeRequest;

        target.getDevice(expected.deviceId).subscribe(actual => {
            expect(actual.links).toBeDefined();
            expect(actual.data).toBeDefined();
            expect(actual.data.deviceId).toEqual(expected.deviceId);
            expect(actual.data.etag).toEqual(expected.etag);
            done();
        });
    });

    it('Should Get Devices', (done) => {
        var target = new DataService(fakeHttp, new LoggerService());

        let expected = new Device();
        expected.deviceId = 'test';
        expected.etag = 'test-etag';

        let fakeDevices = {
            '_links': {
                'self': {
                    'href': '/'
                }
            },
            '_results': [expected]
        };

        // make a fake body for the fake request
        let fakeRequest = new BehaviorSubject(new Response(new ResponseOptions({
            body: JSON.stringify(fakeDevices)
        })));

        // This fakes out the get operation that will be done by the get devices, returning the fake data above
        fakeHttp.request = url => fakeRequest;

        target.getDevicesFromQuery().subscribe(actual => {
            expect(actual.links).toBeDefined();
            expect(actual.data).toBeDefined();
            expect(actual.data[0].deviceId).toEqual(expected.deviceId);
            expect(actual.data[0].etag).toEqual(expected.etag);
            done();
        });
    });
});
