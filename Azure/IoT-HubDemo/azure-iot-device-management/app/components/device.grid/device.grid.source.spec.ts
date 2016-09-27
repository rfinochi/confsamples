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

import {DeviceGridSource} from './device.grid.source';
import {DataService, ConfigurationService} from '../../services/index';
import {Observable, BehaviorSubject, Subject} from 'rxjs/Rx';
import {Device, QueryExpression} from '../../models/index';
import {Http, Response, ResponseOptions} from '@angular/http';


describe('Device Grid Source Tests', () => {

    let gridSource: DeviceGridSource;
    let fakeDataService: DataService;
    let fakeGetDevicesCall: Observable<Device[]>;
    let fakeConfigurationState: ConfigurationService;
    let baseSetInterval: (handler: Function, timeout?: any, ...args: any[]) => number;
    let fakeHttp;
    let fakeSubscribe;

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

    const fakeIntervalId = 6;

    beforeEach(() => {

        fakeHttp = <any>{
            delete: () => {
                return fakeSubscribe;
            },
            request: (url, opts, headers) => {
                return fakeSubscribe;
            },
            get: () => {
                return new BehaviorSubject(new Response(new ResponseOptions({ body: JSON.stringify(mockDiscovery) })));
            }
        };

        fakeDataService = new DataService(fakeHttp);
        spyOn(fakeDataService, 'getDevicesFromQuery').and.callFake(() => fakeGetDevicesCall);
        spyOn(fakeDataService, 'getRegistryStatistics').and.callFake(() => fakeGetDevicesCall);

        fakeGetDevicesCall = jasmine.createSpyObj('Observable', ['subscribe', 'do']);

        (<any>fakeGetDevicesCall.do).and.callFake((cb: Function) => {
            cb({ data: [] });
            return fakeGetDevicesCall;
        });

        baseSetInterval = setInterval;
        window.setInterval = <any>jasmine.createSpy('setInterval').and.callFake(() => fakeIntervalId);

        gridSource = new DeviceGridSource(fakeDataService);
    });

    it('Should Construct', () => {
        expect(gridSource).toBeDefined();
        gridSource.poll(false);
    });

    

    describe('on successful updates', () => {

        beforeEach(() => {
            gridSource.open();
        });

        it('updates devices correctly', () => {
            gridSource.update();
            expect(fakeDataService.getDevicesFromQuery).toHaveBeenCalledWith(undefined, undefined, new QueryExpression(), undefined);
            expect(fakeGetDevicesCall.subscribe).toHaveBeenCalledWith(jasmine.any(Function));
            gridSource.poll(false);
        });

        it('updates last updated time correctly', () => {
            let startTime = gridSource.lastUpdated;

            gridSource.update();

            expect(gridSource.lastUpdated).not.toBe(startTime);
        });

        it('updates registry stats correctly', () => {
            let fakeRegistryRes = new Subject(),
                fakeRegistryStats = {
                    totalDeviceCount: 10,
                    enabledDeviceCount: 10,
                    disabledDeviceCount: 10
                };

            (<any>fakeDataService.getRegistryStatistics).and.callFake(() => fakeRegistryRes);

            gridSource.update();
            fakeRegistryRes.next({ data: fakeRegistryStats });

            expect(gridSource.registryStats).toEqual(fakeRegistryStats);
        });

        it('updates visible row count correctly', () => {
            gridSource.visibleRowCount = 10;

            gridSource.update();

            expect(gridSource.visibleRowCount).toBe(0);
        });
    });

    afterAll(() => {
        window.setInterval = baseSetInterval;
    });

    it('change tags model correctly', () => {
        let model = gridSource.changeTagsModel('tags', 'service');
        expect(model).toEqual('default');

        model = gridSource.changeTagsModel('manufacturer', 'system');
        expect(model).toEqual('system');
    });

    it('checkIfAllAreIndexed should work correctly', () => {
        let columns = [{ indexed: true }, { indexed: false }, { indexed: true }];
        let areIndexed = gridSource.checkIfAllAreIndexed(columns);
        expect(areIndexed).toEqual(false);

        columns = [{ indexed: true }, { indexed: true }, { indexed: true }];
        areIndexed = gridSource.checkIfAllAreIndexed(columns);
        expect(areIndexed).toEqual(true);
    });

    it('getSortOrder should get the sort order (asc/desc) correct', () => {
        let order = gridSource.getSortOrder(true);
        expect(order).toEqual('asc');

        order = gridSource.getSortOrder(false);
        expect(order).toEqual('desc');
    });
});
