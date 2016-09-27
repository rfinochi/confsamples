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

import {Router, RouteParams} from '@angular/router-deprecated';

import {DeviceGrid} from './device.grid';
import {HalButton, Device, IGridConfiguration, AlertType, IGridFilter} from '../../models/index';
import {clone, Resources} from '../../core/index';
import {DataService, ConfigurationService} from '../../services/index';
import {Observable, BehaviorSubject} from 'rxjs/Rx';
import {Http, Response, ResponseOptions} from '@angular/http';
import {DeviceGridSource} from './device.grid.source';
import {Subject} from 'rxjs/Rx';
import {FilterValue} from '../common.filter/common.filter';


function checkUIReset(grid: DeviceGrid, relsToRestore: any) {
    expect(grid.schedulerVisible).toBe(false);
    expect(grid.currentActiveNavButton).toBe(null);

    expect(grid.editLoadingDevice).toBe(false);
    expect(grid.editFormVisible).toBe(false);

    expect(grid.currentJob).toBe(null);

    expect(grid.currentValidRels).toBe(relsToRestore);
    expect(grid.location.go).toHaveBeenCalledWith('');
}

describe('Device Grid Tests', () => {

    let grid: DeviceGrid;

    let fakeState: ConfigurationService;
    let dataService: DataService;
    let fakeHttp: Http;

    let fakeGridSource: DeviceGridSource;
    let fakeSubscribe;

    let fakeEditButton = new HalButton('Edit', null, 'devices:edit', 'POST');
    let fakeDeleteButton = new HalButton('', null, 'devices:delete', 'POST');
    let fakeExportButton = new HalButton('', null, 'devices:export', 'POST');
    let fakeFirmwareButton = new HalButton('', null, 'jobs:jobFirmware', 'POST');
    let fakeResetButton = new HalButton('', null, 'jobs:jobReset', 'POST');
    let fakeRebootButton = new HalButton('', null, 'jobs:jobReboot', 'POST');

    let router: Router;
    let router_params: RouteParams;

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

    var config1 = {
        columns: [
            { 'name': 'Device Id', 'model': 'default', 'key': 'deviceId' },
            { 'name': 'Battery Level', 'model': 'system', 'key': 'batteryLevel' },
            { 'name': 'External Temperature', 'model': 'custom', 'key': 'temperature' },
            { 'name': 'Room', 'model': 'service', 'key': 'room' }
        ],
        filters: [
            { 'name': 'Device ID', 'model': 'default', 'key': 'deviceId', 'in': ['id1'], 'isArray': false },
            { 'name': 'Tags', 'model': 'service', 'key': 'tags', 'in': ['tag1'], 'isArray': true }
        ]
    };

    var config2: IGridConfiguration = {
        name: 'Config 2',
        columns: [{ name: 'Device Id', model: 'default', key: 'deviceId', 'indexed': true }],
        filters: []
    };

    var config3: IGridConfiguration = {
        name: 'Config 3',
        columns: [
            { name: 'Device Id', model: 'default', key: 'deviceId', 'indexed': true },
            { name: 'Status', model: 'system', key: 'status', 'indexed': true }
        ],
        filters: [
            { name: 'Status', model: 'system', key: 'status', in: ['Running'], isArray: false }
        ]
    };

    beforeEach(() => {

        var availableDeviceGridColumns = new BehaviorSubject([]);
        var availableDeviceGridFilters = new BehaviorSubject([]);
        var currentDeviceGridView = new BehaviorSubject(config1);
        var availableDeviceGridViews = new BehaviorSubject([config1, config2, config3]);

        fakeState = <any>{
            CurrentDeviceGridView: currentDeviceGridView,
            AvailableDeviceGridViews: availableDeviceGridViews
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

        dataService = new DataService(fakeHttp);

        fakeSubscribe = {
            subscribe: () => { }
        };

        fakeGridSource = new DeviceGridSource(dataService);

        // These need to be injected for real tests 
        router = <any>{};
        router_params = <any>{};

        grid = new DeviceGrid(fakeGridSource, fakeState, dataService, router_params, <any>{ group: () => { } }, <any>{ go: () => { } });

        spyOn(grid.location, 'go');
    });

    it('Should Construct', () => {
        expect(grid).toBeDefined();
    });

    it('should start job successfully', () => {
        spyOn(grid, 'gridNavAction');
        grid.currentJobRel = 'test:test';
        grid.startJob();
        expect(grid.gridNavAction).toHaveBeenCalledWith(new HalButton(
            null,
            null,
            'test:test',
            'POST'
        ));
    });

    it('should update grid configuration when the state changes', () => {
        expect(grid.gridConfiguration.columns.length).toEqual(config1.columns.length);
        expect(grid.gridConfiguration.columns[0].header()).toEqual(config1.columns[0].name);
        expect(grid.gridConfiguration.columns[1].header()).toEqual(config1.columns[1].name);
        expect(grid.gridConfiguration.columns[2].header()).toEqual(config1.columns[2].name);
        expect(grid.gridConfiguration.columns[3].header()).toEqual(config1.columns[3].name);

        var datum: any = {
            deviceId: 'default',
            etag: 'test',
            generationId: 'test',
            lastActivityTime: new Date(),
            deviceProperties: {
                batteryLevel: {
                    value: 65
                }
            },
            customProperties: {
                temperature: {
                    value: 888
                }
            },
            serviceProperties: {
                room: 'A'
            }
        };

        expect(grid.gridConfiguration.columns[0].value(datum)).toEqual(datum.deviceId);
        expect(grid.gridConfiguration.columns[1].value(datum)).toEqual(datum.deviceProperties['batteryLevel'].value);
        expect(grid.gridConfiguration.columns[2].value(datum)).toEqual(datum.customProperties['temperature'].value);
        expect(grid.gridConfiguration.columns[3].value(datum)).toEqual(datum.serviceProperties['room']);
    });

    it('should handle null values with empty strings', () => {
        expect(grid.gridConfiguration.columns.length).toEqual(config1.columns.length);
        expect(grid.gridConfiguration.columns[0].header()).toEqual(config1.columns[0].name);
        expect(grid.gridConfiguration.columns[1].header()).toEqual(config1.columns[1].name);
        expect(grid.gridConfiguration.columns[2].header()).toEqual(config1.columns[2].name);
        expect(grid.gridConfiguration.columns[3].header()).toEqual(config1.columns[3].name);

        var datum: any = {
            deviceId: null,
            eTag: 'test',
            generationId: 'test',
            lastActivityTime: new Date(),
            deviceProperties: {
                batteryLevel: null
            },
            customProperties: {
                temperature: null
            },
            serviceProperties: {
                room: null
            }
        };

        expect(grid.gridConfiguration.columns[0].value(datum)).toEqual('');
        expect(grid.gridConfiguration.columns[1].value(datum)).toEqual('');
        expect(grid.gridConfiguration.columns[2].value(datum)).toEqual('');
        expect(grid.gridConfiguration.columns[3].value(datum)).toEqual('');
    });

    it('should be able to select configuration', () => {
        grid.selectConfiguration(config2);
        
        // Verifying that name, columns and filters match Config 2.
        expect(grid.selectedConfiguration.value.name).toEqual('Config 2');
        expect(grid.selectedConfiguration.value.columns.length).toEqual(1);
        expect(grid.selectedConfiguration.value.columns[0].key).toEqual('deviceId');
        expect(grid.selectedConfiguration.value.filters.length).toEqual(0);

        grid.selectConfiguration(config3);
        
        // Verifying that name, columns and filters match Config 3.
        expect(grid.selectedConfiguration.value.name).toEqual('Config 3');
        expect(grid.selectedConfiguration.value.columns.length).toEqual(2);
        expect(grid.selectedConfiguration.value.columns[0].key).toEqual('deviceId');
        expect(grid.selectedConfiguration.value.columns[1].key).toEqual('status');
        expect(grid.selectedConfiguration.value.filters.length).toEqual(1);
        expect(grid.selectedConfiguration.value.filters[0].key).toEqual('status');
    });

    it('should be able to update configuration', () => {
        // Specifying name of existing configuration to update it.
        grid.selectedConfiguration.value.name = 'Config 3';
        grid.saveConfiguration();
        expect(grid.availableConfigurations.length).toEqual(3);
    });
    
    it('should be able to save new configuration', () => {
        // Changing name to save configuration as new one.
        grid.selectedConfiguration.value.name = 'New config';
        grid.saveConfiguration();

        expect(grid.availableConfigurations.length).toEqual(4);
    });
    
    it('should not change selectedConfiguration when saveConfiguration', () => {
        spyOn(grid.selectedConfiguration, 'next');
        spyOn(grid, 'applyFilter');
        
        grid.saveConfiguration();
        expect(grid.availableConfigurations.length).toEqual(3);
        expect(grid.selectedConfiguration.next).not.toHaveBeenCalled();
        expect(grid.applyFilter).toHaveBeenCalled();
    });

    it('should change CurrentDeviceGridView if a new filter or column selection is applied', () => {
        spyOn(fakeState.CurrentDeviceGridView, 'next');

        var column = grid.selectedConfiguration.value.columns[0];
        column.name = 'Device Id';
        column.key = 'deviceId';
        grid.selectedConfiguration.value.columns[0] = column;

        var newFilters: FilterValue<IGridFilter>[] = [
            {
                option: {
                    'name': 'Device ID', 'model': 'default', 'key': 'deviceId', 'in': [], 'isArray': false
                },
                value: 'id1'
            }, {
                option: {
                    'name': 'Status', 'model': 'default', 'key': 'status', 'in': [], 'isArray': true
                },
                value: 'tag1'
            }
        ];

        grid.currentFilters.next(newFilters);
        grid.applyFilter();
        expect(fakeState.CurrentDeviceGridView.next).toHaveBeenCalled();
    });

    it('should not change CurrentDeviceGridView if an identical filter or column selection is applied', () => {
        spyOn(fakeState.CurrentDeviceGridView, 'next');

        var column = grid.selectedConfiguration.value.columns[0];
        column.name = 'Device Id';
        column.key = 'deviceId';
        grid.selectedConfiguration.value.columns[0] = column;

        var newFilters: FilterValue<IGridFilter>[] = [
            { 
                option: {
                    'name': 'Device ID', 'model': 'default', 'key': 'deviceId', 'in': [], 'isArray': false
                },
                value: 'id1'
            }, {
                option: {
                    'name': 'Tags', 'model': 'service', 'key': 'tags', 'in': [], 'isArray': true
                },
                value: 'tag1'
            }
        ];

        grid.currentFilters.next(newFilters);
        grid.applyFilter();
        expect(fakeState.CurrentDeviceGridView.next).not.toHaveBeenCalled();
    });

    it('should not add null filters to CurrentDeviceGridView when the filters are applied', () => {
        spyOn(fakeState.CurrentDeviceGridView, 'next');

        var column = grid.selectedConfiguration.value.columns[0];
        column.name = 'Device Id';
        column.key = 'deviceId';
        grid.selectedConfiguration.value.columns[0] = column;

        var newFilters: FilterValue<IGridFilter>[] = [
            {
                option: null,
                value: ''
            },
            {
                option: {
                    'name': 'Tags', 'model': 'service', 'key': 'tags', 'in': [], 'isArray': true
                },
                value: 'tag1'
            }
        ];

        grid.currentFilters.next(newFilters);

        grid.applyFilter();
        expect(fakeState.CurrentDeviceGridView.next).toHaveBeenCalled();
        expect(grid.selectedConfiguration.value.filters.length).toEqual(1);
    });

    it('closes confirm, making callback', () => {
        var called = false;
        grid.onConfirm = function() {
            called = true;
        };

        grid.isConfirming = true;

        grid.confirm();

        expect(called).toBeTruthy();
        expect(grid.onConfirm).toBeNull();
        expect(grid.isConfirming).toBeFalsy();
    });

    it('cancels confirm, without making callback', () => {
        var called = false;
        grid.onConfirm = function() {
            called = true;
        };

        grid.isConfirming = true;

        grid.cancel();

        expect(called).toBeFalsy();
        expect(grid.onConfirm).toBeNull();
        expect(grid.isConfirming).toBeFalsy();
    });

    it('cancels confirm, without making callback', () => {
        var called = false;
        grid.onCancel = function() {
            called = true;
        };

        grid.isConfirming = true;

        grid.confirm();

        expect(called).toBeFalsy();
        expect(grid.onCancel).toBeNull();
        expect(grid.isConfirming).toBeFalsy();
    });

    it('cancels confirm, making callback', () => {
        var called = false;
        grid.onCancel = function() {
            called = true;
        };

        grid.isConfirming = true;

        grid.cancel();

        expect(called).toBeTruthy();
        expect(grid.onCancel).toBeNull();
        expect(grid.isConfirming).toBeFalsy();
    });

    it('prompts for job parameters', () => {
        let message = 'trying to prompt for params';
        let fakeParams = { params: 'stuff' };
        spyOn(grid, 'scheduleJob');

        grid.promptJobParams(message, fakeFirmwareButton, 'A fake firmware update');

        expect(grid.alert).toBeNull();
        expect(grid.schedulerVisible).toBeTruthy();
        expect(grid.isConfirming).toBeFalsy();

        grid.onJobSchedulerSuccess(fakeParams);

        expect(grid.isConfirming).toBeTruthy();
        expect(grid.confirmMessage).toEqual(message);
        expect(grid.onConfirm).toBeDefined();

        expect(grid.scheduleJob).not.toHaveBeenCalled();

        grid.onConfirm();

        expect(grid.schedulerVisible).toBeFalsy();
        expect(grid.scheduleJob).toHaveBeenCalledWith(fakeParams);
    });

    it('toggles editor', () => {
        grid.editorVisible = false;
        grid.toggleEditor();
        expect(grid.editorVisible).toBeTruthy();
        grid.toggleEditor();
        expect(grid.editorVisible).toBeFalsy();
    });

    it('updates selection', () => {
        grid.selectedDevices = [];
        var d1 = new Device();
        d1.deviceId = 'test1';
        grid.updateSelection([d1]);
        expect(grid.selectedDevices.length).toEqual(1);
        expect(grid.selectedDevices[0].deviceId).toEqual(d1.deviceId);
    });

    it('prompts for parameters on firmware update', () => {
        grid.selectedDevices = [new Device()];

        spyOn(grid, 'relReduce');
        spyOn(grid, 'promptJobParams');
        grid.gridNavAction(fakeFirmwareButton);

        expect(grid.currentJob).toEqual(grid.Resources.Common.firmwareUpdate);
        expect(grid.promptJobParams).toHaveBeenCalledWith(grid.Resources.ModalPrompts.firmwareUpdate, fakeFirmwareButton, grid.currentJob);
        
        expect(grid.editorVisible).toEqual(false);
        expect(grid.isConfirming).toEqual(false);
    });

    it('confirms for reboot', () => {
        grid.selectedDevices = [new Device()];

        spyOn(grid, 'relReduce');
        grid.gridNavAction(fakeRebootButton);

        expect(grid.confirmMessage).toEqual(grid.Resources.ModalPrompts.reboot);
        expect(grid.confirmTitle).toEqual(grid.Resources.ConfirmRebootDevices);
        expect(grid.onConfirm).toEqual(grid.scheduleJob);
        expect(grid.currentJob).toEqual(grid.Resources.Common.reboot);
    });

    it('confirms for factory reset', () => {
        grid.selectedDevices = [new Device()];

        spyOn(grid, 'relReduce');
        grid.gridNavAction(fakeResetButton);

        expect(grid.confirmMessage).toEqual(grid.Resources.ModalPrompts.factoryReset);
        expect(grid.confirmTitle).toEqual(grid.Resources.ConfirmFactoryResetDevices);
        expect(grid.onConfirm).toEqual(grid.scheduleJob);
        expect(grid.currentJob).toEqual(grid.Resources.Common.factoryReset);
    });

    it('confirms for export devices', () => {
        grid.selectedDevices = [new Device()];

        spyOn(grid, 'relReduce');
        grid.gridNavAction(fakeExportButton);

        expect(grid.confirmMessage).toEqual(grid.Resources.ModalPrompts.export);
        expect(grid.confirmTitle).toEqual(grid.Resources.ConfirmExportDevices);
        expect(grid.onConfirm).toEqual(grid.exportDevices);
    });

    it('on unexpected search key press doesn\'t search', () => {
        let fakeEvent = { keyCode: 19 };
        spyOn(grid, 'findDevice');
        grid.searchKeyPress(<any>fakeEvent);
        expect(grid.findDevice).not.toHaveBeenCalled();
    });

    it('on enter search key press searches with a valid device ID', () => {
        let fakeEvent = { keyCode: 13 };
        spyOn(grid, 'findDevice');
        grid.searchDevice = 'testId';

        grid.searchKeyPress(<any>fakeEvent);
        expect(grid.findDevice).toHaveBeenCalled();
    });

    it('on enter search key press doesn\'t searche with an empty device ID', () => {
        let fakeEvent = { keyCode: 13 };
        spyOn(grid, 'findDevice');
        grid.searchDevice = '';

        grid.searchKeyPress(<any>fakeEvent);
        
        expect(grid.findDevice).not.toHaveBeenCalled();
        expect(grid.alert).not.toBe(null);
    });

    it('selects device from grid source when opening details from route', () => {
        // setup
        grid.routeParams.get = key => 'device-id-1111';
        spyOn(grid.gridSource, 'select');
        spyOn(grid, 'openEditForm');
        spyOn(grid, 'relReduce');
        grid.gridSource.currentValidRels.first = <any>(() => { return { subscribe: <Function>(callback) => callback() }; });
        // action
        grid.ngOnInit();
        // expectation
        expect(grid.gridSource.select).toHaveBeenCalledWith('device-id-1111');
        expect(grid.openEditForm).toHaveBeenCalled();
    });

    describe('delete', () => {

        it('should alert an error message when no devices are selected', () => {
            grid.gridNavAction(fakeDeleteButton);
            expect(grid.alert).not.toBeNull();
        });

        describe('with valid payload', () => {
            let confirm,
                d1: Device,
                d2: Device;

            beforeEach(() => {
                d1 = new Device();
                d1.deviceId = 'test1';
                d2 = new Device();
                d2.deviceId = 'test2';

                grid.selectedDevices.push(d1);
                grid.selectedDevices.push(d2);

                spyOn(grid, 'relReduce');
                grid.gridNavAction(fakeDeleteButton);
            });

            it('should prompt confirmation before taking action', () => {
                expect(grid.confirmMessage).toBe(grid.Resources.ModalPrompts.delete);
                expect(grid.confirmTitle).toBe(grid.Resources.ConfirmDeleteTitle);
                expect(grid.isConfirming).toBe(true);
            });

            it('should make the api call if devices are selected', () => {
                spyOn(fakeSubscribe, 'subscribe');
                spyOn(dataService, 'deleteDevices').and.callFake(() => fakeSubscribe);

                grid.onConfirm();

                expect(fakeSubscribe.subscribe).toHaveBeenCalledWith(jasmine.any(Function), jasmine.any(Function));
            });

            it('should force update the grid and alert success when devices are successfuly deleted', () => {
                spyOn(fakeGridSource, 'update');
                spyOn(grid, 'resetUI');
                spyOn(dataService, 'deleteDevices').and.callFake(() => fakeSubscribe);

                grid.onConfirm();
                grid.onDeleteDevicesSuccess(<any>{ data: { _import: { error: 0 } }, rels: {} });
                expect(grid.alert.msg).toEqual(Resources.DeviceGrid.titleAlertDeviceDeletedSuccess);
                expect(grid.alert.type).toEqual(AlertType.Success);
                expect(fakeGridSource.update).toHaveBeenCalled();
                expect(grid.resetUI).toHaveBeenCalled();
            });


            it('should force update the grid and alert danger when devices are partially deleted', () => {
                spyOn(fakeGridSource, 'update');
                spyOn(grid, 'resetUI');
                spyOn(dataService, 'deleteDevices').and.callFake(() => fakeSubscribe);

                grid.onConfirm();
                grid.onDeleteDevicesSuccess(<any>{ data: { _import: { error: 1, total: 3, success: 2 } }, rels: {} });
                expect(grid.alert.msg).toContain(1);
                expect(grid.alert.msg).toContain(3);
                expect(grid.alert.msg).toContain(2);
                expect(grid.alert.type).toEqual(AlertType.Danger);
                expect(fakeGridSource.update).toHaveBeenCalled();
                expect(grid.resetUI).toHaveBeenCalled();
            });


            it('should force update the grid and alert danger when delete devices fails', () => {
                spyOn(grid, 'resetUI');
                spyOn(dataService, 'deleteDevices').and.callFake(() => fakeSubscribe);
                
                var response = {
                    json: () => {
                        return { _error: { message: 'Failed' } };
                    }
                };
                
                grid.onConfirm();
                grid.onDeleteDevicesError(response);
                expect(grid.alert.msg).toEqual('Failed');
                expect(grid.alert.type).toEqual(AlertType.Danger);
                expect(grid.resetUI).toHaveBeenCalled();
            });
        });
    });

    describe('edit', () => {
        it('should alert an error message when no devices are selected', () => {
            grid.gridNavAction(fakeEditButton);
            expect(grid.alert).not.toBeNull();
        });

        it('should alert an error message when more than one device is selected', () => {
            grid.selectedDevices.push(new Device());
            grid.selectedDevices.push(new Device());

            grid.gridNavAction(fakeEditButton);
            expect(grid.alert).not.toBeNull();
        });

        it('should close edit form when multiple devices are selected and edit is clicked again', () => {
            grid.editFormVisible = true;

            grid.selectedDevices.push(new Device());
            grid.selectedDevices.push(new Device());

            grid.gridNavAction(fakeEditButton);
            expect(grid.editFormVisible).toBe(false);
        });

        describe('with correct number of selected devices', () => {
            let device: Device;

            beforeEach(() => {
                device = new Device();
                device.deviceId = 'test1';

                grid.currentValidRels = <any>{ test: 'test' };

                grid.selectedDevices.push(device);

                grid.servicePropertiesForm = <any>{
                    toDevice: () => { },
                    form: {
                        dirty: false
                    }
                };

                grid.devicePropertiesForm = <any>{
                    toDeviceProperties: () => { },
                    toConfigurationProperties: () => { },
                    getChanges: () => { return {}; }
                };
            });

            describe('state before api call finishes', () => {
                beforeEach(() => {
                    spyOn(grid, 'relReduce');
                    spyOn(grid.dataService, 'getDevice').and.returnValue({ subscribe: () => { } });

                    grid.gridNavAction(fakeEditButton);
                });

                it('should reduce the rel down to the current action', () => {
                    expect(grid.relReduce).toHaveBeenCalled();
                    expect(grid.currentActiveNavButton).toEqual(fakeEditButton);
                });

                it('should enter loading state', () => {
                    expect(grid.editLoadingDevice).toBe(true);
                });

                it('should call getDevice with the right parameters', () => {
                    expect(grid.dataService.getDevice).toHaveBeenCalledWith('test1', { test: 'test' });
                });
            });

            describe('state after api call finishes', () => {
                let mockSubscribe,
                    onSuccess,
                    onError,
                    getDeviceLinks,
                    device,
                    relsToRestore,
                    updatedDevice,
                    updateDeviceJob,
                    setServicePropertiesJob,
                    writeDevicePropertiesJob,
                    updateCongifurationJob;

                beforeEach(() => {
                    mockSubscribe = {
                        subscribe: (successHandler, errorHandler) => {
                            onSuccess = successHandler;
                            onError = errorHandler;
                        }
                    };

                    spyOn(grid, 'relReduce');
                    spyOn(grid.dataService, 'getDevice').and.returnValue(mockSubscribe);

                    grid.relsToRestore = relsToRestore = <any>{ foo: 'bar' };

                    grid.gridNavAction(fakeEditButton);
                });

                describe('edit form opened successfully', () => {
                    beforeEach(() => {
                        getDeviceLinks = { foo: 'bar' };
                        device = new Device();

                        device.deviceId = 'foobar';

                        device['_deviceConnectionString'] = 'foobarbaz';

                        onSuccess({ links: getDeviceLinks, data: device });
                    });

                    it('should change location', () => {
                        expect(grid.location.go).toHaveBeenCalledWith('/edit/foobar');
                    });

                    it('should exit the loading state', () => {
                        expect(grid.editLoadingDevice).toBe(false);
                    });

                    it('should make the edit form visible', () => {
                        expect(grid.editFormVisible).toBe(true);
                    });

                    it('should set the currentValidRels', () => {
                        expect(grid.currentValidRels).toEqual(getDeviceLinks);
                    });

                    it('should set the editableDevice', () => {
                        expect(grid.editableDevice).toEqual(device);
                    });

                    it('should set the deviceConnectionString', () => {
                        expect(grid.deviceConnectionString).toEqual('foobarbaz');
                    });

                    describe('on submit', () => {
                        beforeEach(() => {
                            updatedDevice = new Device();
                            updatedDevice.deviceId = 'foo';

                            updatedDevice.serviceProperties.properties = { prop: 'baz' };

                            // fake out subcomponents
                            spyOn(grid.servicePropertiesForm, 'toDevice').and.returnValue(updatedDevice);

                            // fake out dataservice calls
                            updateDeviceJob = { prop: 'updateDevice' };
                            setServicePropertiesJob = { prop: 'setServiceProperties' };
                            writeDevicePropertiesJob = { prop: 'writeDeviceProperties' };
                            updateCongifurationJob = { prop: 'updateCongifuration' };

                            spyOn(grid.dataService, 'updateDevice').and.returnValue(updateDeviceJob);

                            spyOn(grid.dataService, 'setServiceProperties').and.returnValue(setServicePropertiesJob);

                            spyOn(grid.dataService, 'writeDeviceProperties').and.returnValue(writeDevicePropertiesJob);

                            spyOn(grid.dataService, 'updateCongifuration').and.returnValue(updateCongifurationJob);

                            // fake out forkJoin
                            spyOn(Observable, 'forkJoin').and.returnValue(mockSubscribe);

                            grid.onBulkEditFormSubmit();
                        });

                        it('should collect edit params for apis from subcomponents', () => {
                            expect(grid.servicePropertiesForm.toDevice).toHaveBeenCalled();
                        });

                        it('should enter loading state', () => {
                            expect(grid.editLoadingDevice).toBe(true);
                        });

                        it('should reset ui and alert success if api calls succeeded', () => {
                            onSuccess([{}, {}, {}, {}]);
                            checkUIReset(grid, relsToRestore);
                            expect(grid.alert).not.toBe(null);
                        });

                        it('should reset ui and alert error if api calls error', () => {
                            onError({});
                            checkUIReset(grid, relsToRestore);
                            expect(grid.alert).not.toBe(null);
                        });
                    });

                    describe('on cancel', () => {
                        it('should reset ui and alert cancelation', () => {
                            grid.onBulkEditFormCancel();

                            expect(grid.alert).not.toBeNull();
                            checkUIReset(grid, relsToRestore);
                        });
                    });
                });

                describe('edit form could not open', () => {
                    beforeEach(() => {
                        onError({});
                    });

                    it('should reset the UI', () => {
                        checkUIReset(grid, relsToRestore);
                    });

                    it('should alert an error', () => {
                        expect(grid.alert).not.toBe(null);
                        expect(grid.alert.type).toBe(AlertType.Danger);
                        expect(grid.alert.msg).toBe(Resources.DeviceGrid.titleAlertGetDeviceError);
                    });
                });
            });
        });
    });
});
