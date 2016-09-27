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

import {HalButton, IGridConfiguration, Job, IGridFilter} from '../../models/index';
import {ConfigurationService, DataService} from '../../services/index';
import {Observable, BehaviorSubject, Subject} from 'rxjs/Rx';
import {HistoryGrid} from './job.history.grid';
import {Http, Response, ResponseOptions} from '@angular/http';
import {HistoryGridSource} from './job.history.grid.source';
import {FilterValue} from '../common.filter/common.filter';
import {Resources} from '../../core/index';

describe('History Tests', () => {

    let history: HistoryGrid;
    let fakeState: ConfigurationService;
    let fakeHttp: Http;
    let fakeDataService: DataService;
    let fakeGridSource: HistoryGridSource;
    let fakeSubscribe;

    let fakeDetailsButton = new HalButton('Details', null, 'jobs:history', 'POST');

    let mockDiscovery = {
        '_links': {
            'self': {
                'href': '/'
            },
            'curies': [
                {
                    'href': '/api/jobs/docs/jobs/{rel}',
                    'templated': true,
                    'name': 'jobs'
                }
            ],
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

    var config1: IGridConfiguration = {
        name: 'Config 1',
        columns: [
            { 'name': 'Action Id', 'model': 'default', 'key': 'jobId', 'indexed': true },
            { 'name': 'Status', 'model': 'default', 'key': 'status', 'indexed': true }
        ],
        filters: [
            { 'name': 'Action Id', 'model': 'default', 'key': 'jobId', 'in': ['id1'], 'isArray': false },
            { 'name': 'Status', 'model': 'default', 'key': 'status', 'in': ['completed'], 'isArray': false }
        ]
    };

    var config2: IGridConfiguration = {
        name: 'Config 2',
        columns: [{ name: 'Action Id', model: 'default', key: 'jobId', 'indexed': true }],
        filters: []
    };

    var config3: IGridConfiguration = {
        name: 'Config 3',
        columns: [
            { name: 'Action Id', model: 'default', key: 'jobId', 'indexed': true },
            { name: 'Status', model: 'default', key: 'status', 'indexed': true }
        ],
        filters: [
            { name: 'Status', model: 'default', key: 'status', in: ['Running'], isArray: false }
        ]
    };

    beforeEach(() => {

        var availableGridColumns = new BehaviorSubject([]);
        var availableGridFilters = new BehaviorSubject([]);
        var currentGridView = new BehaviorSubject(config1);
        var availableGridViews = new BehaviorSubject([config1, config2, config3]);

        fakeState = <any>{
            CurrentHistoryGridView: currentGridView,
            AvailableHistoryGridViews: availableGridViews
        };

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

        fakeSubscribe = {
            subscribe: () => { }
        };

        fakeGridSource = new HistoryGridSource(dataService);

        // These need to be injected for real tests 
        let router_params = <any>{};

        history = new HistoryGrid(fakeGridSource, fakeState, router_params, fakeDataService, <any>{ go: () => { } });
    });

    it('Should Construct', () => {
        expect(history).toBeDefined();
    });

    it('should update grid configuration when the state changes', () => {
        expect(history.gridConfiguration.columns.length).toEqual(config1.columns.length);
        expect(history.gridConfiguration.columns[0].header()).toEqual(config1.columns[0].name);
        expect(history.gridConfiguration.columns[1].header()).toEqual(config1.columns[1].name);

        var datum: any = {
            jobId: 'jobId1',
            status: 'completed'
        };

        expect(history.gridConfiguration.columns[0].value(datum)).toEqual(datum.jobId);
    });

    it('should handle null values with empty strings', () => {
        var datum: any = {
            jobId: null,
            status: ''
        };

        expect(history.gridConfiguration.columns[0].value(datum)).toEqual('');
        expect(history.gridConfiguration.columns[1].value(datum)).toEqual('');
    });

    it('should be able to select configuration', () => {
        history.selectConfiguration(config2);
        
        // Verifying that name, columns and filters match Config 2.
        expect(history.selectedConfiguration.value.name).toEqual('Config 2');
        expect(history.selectedConfiguration.value.columns.length).toEqual(1);
        expect(history.selectedConfiguration.value.columns[0].key).toEqual('jobId');
        expect(history.selectedConfiguration.value.filters.length).toEqual(0);

        history.selectConfiguration(config3);
        
        // Verifying that name, columns and filters match Config 3.
        expect(history.selectedConfiguration.value.name).toEqual('Config 3');
        expect(history.selectedConfiguration.value.columns.length).toEqual(2);
        expect(history.selectedConfiguration.value.columns[0].key).toEqual('jobId');
        expect(history.selectedConfiguration.value.columns[1].key).toEqual('status');
        expect(history.selectedConfiguration.value.filters.length).toEqual(1);
        expect(history.selectedConfiguration.value.filters[0].key).toEqual('status');
    });

    it('should be able to update configuration', () => {
        // Specifying name of existing configuration to update it.
        history.selectedConfiguration.value.name = 'Config 3';
        history.saveConfiguration();
        expect(history.availableConfigurations.length).toEqual(3);
    });

    it('should be able to save new configuration', () => {
        // Changing name to save configuration as new one.
        history.selectedConfiguration.value.name = 'New config';
        history.saveConfiguration();

        expect(history.availableConfigurations.length).toEqual(4);
    });

    it('should not change selectedConfiguration when saveConfiguration', () => {
        spyOn(history.selectedConfiguration, 'next');
        spyOn(history, 'applyFilter');

        history.saveConfiguration();
        expect(history.availableConfigurations.length).toEqual(3);
        expect(history.selectedConfiguration.next).not.toHaveBeenCalled();
        expect(history.applyFilter).toHaveBeenCalled();
    });

    it('should change CurrentHistoryGridView if a new filter or column selection is applied', () => {
        spyOn(fakeState.CurrentHistoryGridView, 'next');

        var column = history.selectedConfiguration.value.columns[0];
        column.name = 'Action Id';
        column.key = 'jobId';
        history.selectedConfiguration.value.columns[0] = column;

        var newFilters: FilterValue<IGridFilter>[] = [
            {
                option: {
                    'name': 'Status', 'model': 'default', 'key': 'status', 'in': [], 'isArray': false
                },
                value: 'completed'
            }
        ];

        history.currentFilters.next(newFilters);
        history.applyFilter();
        expect(fakeState.CurrentHistoryGridView.next).toHaveBeenCalled();
    });

    it('should not change CurrentHistoryGridView if an identical filter or column selection is applied', () => {
        spyOn(fakeState.CurrentHistoryGridView, 'next');

        var column = history.selectedConfiguration.value.columns[0];
        column.name = 'Action Id';
        column.key = 'jobId';
        history.selectedConfiguration.value.columns[0] = column;

        var newFilters: FilterValue<IGridFilter>[] = [
            { 
                option: {
                    'name': 'Action Id', 'model': 'default', 'key': 'jobId', 'in': [], 'isArray': false
                },
                value: 'id1'
            }, {
                option: {
                    'name': 'Status', 'model': 'default', 'key': 'status', 'in': [], 'isArray': false
                },
                value: 'completed'
            }
        ];

        history.currentFilters.next(newFilters);
        history.applyFilter();
        expect(fakeState.CurrentHistoryGridView.next).not.toHaveBeenCalled();
    });
    
    it('should not add null filters to CurrentHistoryGridView when the filters are applied', () => {
        spyOn(fakeState.CurrentHistoryGridView, 'next');

       var column = history.selectedConfiguration.value.columns[0];
        column.name = 'Action Id';
        column.key = 'jobId';
        history.selectedConfiguration.value.columns[0] = column;

        var newFilters: FilterValue<IGridFilter>[] = [
            { 
                option: null,
                value: ''
            }, {
                option: {
                    'name': 'Status', 'model': 'default', 'key': 'status', 'in': [], 'isArray': false
                },
                value: 'completed'
            }
        ];

        history.currentFilters.next(newFilters);

        history.applyFilter();
        expect(fakeState.CurrentHistoryGridView.next).toHaveBeenCalled();
        expect(history.selectedConfiguration.value.filters.length).toEqual(1);
    });

    it('toggles editor', () => {
        history.editorVisible = false;
        history.toggleEditor();
        expect(history.editorVisible).toBeTruthy();
        history.toggleEditor();
        expect(history.editorVisible).toBeFalsy();
    });

    it('doesn\'t makes requests if details hidden', () => {
        history.hideDetails = true;
        spyOn(history, 'queryJob');
        spyOn(history, 'fetchCurrentJob');
        spyOn(history, 'resetDetails');
        history.updateSelection(<any>[{}]);
        expect(history.currentSelectedGridItem).not.toBeNull();
        expect(history.fetchCurrentJob).not.toHaveBeenCalled();
        expect(history.resetDetails).not.toHaveBeenCalled();
    });

    it('doesn\'t make requests if details visible but item is not selected', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history, 'resetDetails');

        history.hideDetails = false;
        history.updateSelection(<any>[null]);
        expect(history.currentSelectedGridItem).toBeNull();
        expect(history.fetchCurrentJob).not.toHaveBeenCalled();
        expect(history.resetDetails).toHaveBeenCalled();
    });

    it('does make requests if details visible and an item is selected', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history, 'resetDetails');

        history.hideDetails = false;
        history.updateSelection(<any>[{}]);
        expect(history.currentSelectedGridItem).not.toBeNull();
        expect(history.fetchCurrentJob).toHaveBeenCalled();
        expect(history.resetDetails).not.toHaveBeenCalled();
    });

    it('if hidden, when opened with a selected job makes request', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history, 'resetDetails');

        history.hideDetails = true;
        history.currentSelectedGridItem = <any>{};

        history.gridNavAction(history.gridNavConfigurationJobActions['GridNavButtons'][0]);
        expect(history.fetchCurrentJob).toHaveBeenCalled();
        expect(history.resetDetails).not.toHaveBeenCalled();
    });

    it('it doesn\'t make a request if opened without a selected job', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history, 'resetDetails');

        history.hideDetails = true;
        history.gridNavAction(history.gridNavConfigurationJobActions['GridNavButtons'][0]);
        expect(history.fetchCurrentJob).not.toHaveBeenCalled();
        expect(history.resetDetails).not.toHaveBeenCalled();
    });

    it('should make a request and force open details when a row is double clicked', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history, 'forceOpenDetails');

        history.hideDetails = true;

        history.onJobDoubleClick(<any>{ jobId: '' });
        expect(history.fetchCurrentJob).not.toHaveBeenCalled();
        expect(history.forceOpenDetails).toHaveBeenCalled();
    });

    it('should make a request when a row is double clicked but shouldn\'t force open details', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history, 'forceOpenDetails');

        history.hideDetails = false;

        history.onJobDoubleClick(<any>{ jobId: '' });

        expect(history.fetchCurrentJob).toHaveBeenCalled();
        expect(history.forceOpenDetails).not.toHaveBeenCalled();
    });

    it('should make a request when a valid job is searched', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history.gridSource, 'select');

        history.hideDetails = true;

        history.searchJob = 'jobId';
        history.findJob();

        expect(history.gridSource.select).toHaveBeenCalled();
        expect(history.fetchCurrentJob).toHaveBeenCalled();
    });

    it('shouldn\'t make a request when an empty string is searched', () => {
        spyOn(history, 'queryJob').and.returnValue({ subscribe: () => { } });
        spyOn(history, 'fetchCurrentJob');
        spyOn(history.gridSource, 'select');

        history.hideDetails = true;

        history.searchJob = '';
        history.findJob();

        expect(history.gridSource.select).not.toHaveBeenCalled();
        expect(history.fetchCurrentJob).not.toHaveBeenCalled();
    });

    it('on unexpected search key press doesn\'t search', () => {
        let fakeEvent = { keyCode: 19 };
        spyOn(history, 'findJob');
        history.searchKeyPress(<any>fakeEvent);
        expect(history.findJob).not.toHaveBeenCalled();
    });

    it('on enter search key press searches', () => {
        let fakeEvent = { keyCode: 13 };
        spyOn(history, 'findJob');
        history.searchKeyPress(<any>fakeEvent);
        expect(history.findJob).toHaveBeenCalled();
    });

    it('selects job from grid source when opening details from route', () => {
        // setup
        history.routeParams.get = key => 'job-id-1111';
        spyOn(history.gridSource, 'select');
        spyOn(history, 'forceOpenDetails');
        history.gridSource.currentValidRels.subscribe = <Function>(callback) => callback();
        // action
        history.ngOnInit();
        // expectation
        expect(history.gridSource.select).toHaveBeenCalledWith('job-id-1111');
        expect(history.forceOpenDetails).toHaveBeenCalled();
    });

    describe('grid configuration', () => {
        describe('handles special case: parent-child', () => {
            beforeEach(() => {
                history.selectedConfiguration = <any>{
                    value: {
                        columns: [
                            {
                                name: 'TEST',
                                model: 'calculated',
                                key: 'parent-child',
                                indexed: false
                            }
                        ]
                    }
                };
            });

            it('calculates parent', () => {
                history.setGridConfiguration();

                let column = history.gridConfiguration.columns[0];

                expect(column.header()).toEqual('TEST');
                expect(column.value(<any>{parentJobId: 'id'})).toEqual(Resources.JobProperties.IsChild);
            });

            it('calculates child', () => {
                history.setGridConfiguration();

                let column = history.gridConfiguration.columns[0];

                expect(column.header()).toEqual('TEST');
                expect(column.value(<any>{parentJobId: null})).toEqual(Resources.JobProperties.IsParent);
            });
        });
    });

    describe('fetching the current job', () => {
        let succeedWith,
            failWith;

        beforeEach(() => {
            let mockSubject = new Subject();

            spyOn(history, 'queryJob').and.returnValue(mockSubject);
            spyOn(history.location, 'go');

            succeedWith = (data) => {
                mockSubject.next(data);
            };

            failWith = (data) => {
                mockSubject.error(data);
            };

            history.fetchCurrentJob('test');
        });

        it('should only alert an error if the job wan\'t not found', () => {
            succeedWith({ data: [] });

            expect(history.alert).toBeTruthy();
            expect(history.location.go).toHaveBeenCalledWith('/history');
        });

        it('should only alert an error if the job status was unknown', () => {
            succeedWith({ data: [{ status: 'unknown' }] });

            expect(history.alert).toBeTruthy();
            expect(history.location.go).toHaveBeenCalledWith('/history');
        });

        it('should open the details, set the current job, and change location when a job is found', () => {
            succeedWith({ data: [{ status: '', jobId: 'test' }] });

            expect(history.hideDetails).toBe(false);
            expect(history.alert).toBeFalsy();
            expect(history.location.go).toHaveBeenCalledWith(`/job/test`);
        });

        it('should alert and keep details closed on failure', () => {
            failWith({
                json: () => {
                    return { _error: {} };
                }
            });

            expect(history.alert).not.toBeNull();
            expect(history.hideDetails).toBe(true);
        });
    });
});