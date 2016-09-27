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

import {AppHeader} from './application.header';
import {DataService} from '../../services/index';
import {BehaviorSubject} from 'rxjs/Rx';
import {Http, Response, ResponseOptions} from '@angular/http';

describe('App Header Tests', () => {

    let appHeader: AppHeader;

    beforeEach(() => {
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

        let fakeSubscribe = {
            subscribe: () => { }
        };

        let
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
        let dataService = new DataService(fakeHttp);
        appHeader = new AppHeader(dataService);
    });

    it('Should Construct', () => {
        expect(appHeader).toBeDefined();
    });

});
