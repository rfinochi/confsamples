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

import {HistoryGridSource} from './job.history.grid.source';
import {DataService, ConfigurationService} from '../../services/index';
import {Observable, BehaviorSubject} from 'rxjs/Rx';
import {Job, QueryExpression} from '../../models/index';
import {Http, Response, ResponseOptions} from '@angular/http';


describe('History Grid Source Tests', () => {

    let gridSource: HistoryGridSource;
    let fakeDataService: DataService;
    let fakeGetJobsCall: Observable<Job[]>;
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
        spyOn(fakeDataService, 'getJobs').and.callFake(() => fakeGetJobsCall);

        fakeGetJobsCall = jasmine.createSpyObj('Observable', ['subscribe']);
        
        baseSetInterval = setInterval;
        window.setInterval = <any>jasmine.createSpy('setInterval').and.callFake(() => fakeIntervalId);
        
        /**
         * Construct the grid source with the fake data
         */
        gridSource = new HistoryGridSource(fakeDataService);
    });

    it('Should Construct', () => {
        expect(gridSource).toBeDefined();
        gridSource.poll();
    });

    it('updates correctly', () => {
        var view = gridSource.open();
        gridSource.update();
        expect(fakeDataService.getJobs).toHaveBeenCalledWith(undefined, undefined, new QueryExpression());
        expect(fakeGetJobsCall.subscribe).toHaveBeenCalledWith(jasmine.any(Function));
        gridSource.poll();
    });

    afterAll(() => {
        window.setInterval = baseSetInterval;
    });
});

        