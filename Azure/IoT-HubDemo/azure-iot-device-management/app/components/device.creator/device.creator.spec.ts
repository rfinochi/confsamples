/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {
    beforeEach,
    beforeEachProviders,
    describe,
    expect,
    it,
    inject,
    injectAsync,
    setBaseTestProviders
} from '@angular/core/testing';

import {BaseRequestOptions, Response, ResponseOptions, Http} from '@angular/http';
import {MockBackend, MockConnection} from '@angular/http/testing';
import {provide} from '@angular/core';
import {FormBuilder} from '@angular/common';

import {Device, MAX_FILE_SIZE_BYTES, ACCEPTED_FILE_EXTENSIONS} from './device.creator';
import {Device as DeviceModel} from '../../models/index';
import {DataService} from '../../services/index';
import {Resources} from '../../core/index';
import {ServicePropertiesForm} from '../device.service.properties.form/device.service.properties.form';
import {TagInput} from '../common.tag.input/common.tag.input';
import {KeyValueInput} from '../common.keyvalue.input/common.keyvalue.input';
import {ToggleInput} from '../common.toggle.input/common.toggle.input';

import {Subject, BehaviorSubject} from 'rxjs/Rx';

describe('Device Tests', () => {
    beforeEachProviders(() => [
        FormBuilder
    ]);

    let device: Device;
    let dataService: DataService;
    let fakeHttp: Http;
    let inspectReq;
    let succeedWith;

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
            },
            'devices:add': {
                'href': '/api/devices/add',
                'title': 'add'
            }
        }
    };

    beforeEachProviders(() => [
        ServicePropertiesForm,
        KeyValueInput,
        TagInput,
        ToggleInput
    ]);

    beforeEach(inject(
        [FormBuilder, ServicePropertiesForm, KeyValueInput, TagInput, ToggleInput], 
        (formBuilder: FormBuilder, servicePropForm: ServicePropertiesForm, keyValueInput: KeyValueInput, tagInput: TagInput, toggleInput: ToggleInput) => {
            inspectReq = () => { };

            let d = new DeviceModel();
            let s = new Subject();

            succeedWith = (cb) => {
                let res = new Response(new ResponseOptions({ body: JSON.stringify(cb(d)) }));
                s.next(res);
            };

            fakeHttp = <any>{
                request: (url, opts, headers) => {

                    let parsedJson = JSON.parse(opts.body);                
                    
                    if (parsedJson != null) {
                        inspectReq(parsedJson);
                    }

                    return s;
                },
                get: () => {
                    return new BehaviorSubject(new Response(new ResponseOptions({ body: JSON.stringify(mockDiscovery) })));
                }
            };

            servicePropForm.propertiesValueControl = keyValueInput;
            servicePropForm.tagControl = tagInput;
            servicePropForm.enabledControl = toggleInput;

            servicePropForm.propertiesValueControl.ngOnChanges(<any>{});
            servicePropForm.propertiesValueControl.ngOnInit();
        
            servicePropForm.tagControl.ngOnChanges();
        
            servicePropForm.ngOnChanges(<any>{
                device: {
                    previousValue: {}
                }
            });

            servicePropForm.ngOnInit();
            
            dataService = new DataService(fakeHttp);
            device = new Device(formBuilder, dataService);

            device.servicePropertiesForm = servicePropForm;
        }
    ));

    it('Should Construct', () => {
        expect(device).toBeDefined();
    });

    it('Should disable bulkAdd before it gets the endpoint from discovery service', () => {
        device.bulkAddValidFile();
        expect(device.model.bulkAddIsDisabled).toBe(true);
    });

    it('Should enable bulkAdd once it gets the endpoint from discovery service', () => {
        let oldLoaded = device.bulkAddLoaded;

        device.bulkAddLoaded = () => {
            oldLoaded.call(device);
            device.bulkAddValidFile();
            expect(device.model.bulkAddIsDisabled).toBe(false);
        };

        device.ngOnInit();
    });

    describe('Bulk add api behavior', () => {
        beforeEach((done) => {

            let oldLoaded = device.bulkAddLoaded;

            device.bulkAddLoaded = () => {
                oldLoaded.call(device);
                device.bulkAddValidFile();

                spyOn(device.model.bulkAddFileUploader, 'uploadAll');
                device.bulkAddLoaded = oldLoaded;

                device.addDevices();

                done();
            };

            device.ngOnInit();
        });

        it('Should disable bulkAdd and display progress when api is called', () => {
            expect(device.model.bulkAddIsDisabled).toBe(true);
        });

        it('Should alert when devices are succesfully added by api', () => {
            let res = JSON.stringify({
                _import: {
                    total: 10,
                    success: 10,
                    error: 0
                }
            });

            device.onBulkAddSuccess({ file: { name: '' } }, res, null, null);

            expect(device.bulkAlert).not.toBeNull();
            expect(device.bulkAlert.type).toEqual('success');
        });

        it('Should alert when there were partial errors ading devices', () => {
            let res = JSON.stringify({
                _import: {
                    total: 10,
                    error: 10,
                    success: 0
                }
            });

            device.onBulkAddSuccess({ file: { name: '' } }, res, null, null);

            expect(device.bulkAlert).not.toBeNull();
            expect(device.bulkAlert.type).toEqual('danger');
        });

        it('Should alert when devices are unsuccesfully added by api', () => {
            device.onBulkAddError({ file: { name: '' } }, JSON.stringify({ _error: { message: '' } }), null, null);

            expect(device.bulkAlert).not.toBeNull();
            expect(device.bulkAlert.type).toEqual('danger');
        });
    });

    describe('Bulk add file validations', () => {
        let mockItem;

        beforeEach(() => {
            mockItem = {
                file: {
                    size: MAX_FILE_SIZE_BYTES,
                    name: 'test' + ACCEPTED_FILE_EXTENSIONS[0]
                }
            };

            device.bulkAddLoaded();
        });

        it('Should disable bulkAdd and alert error when wrong file type is selected', () => {
            mockItem.file.name += 'a';
            device.onAfterAddingBulkAddFile(mockItem);

            expect(device.model.bulkAddIsDisabled).toBe(true);
            expect(device.bulkAlert).not.toBeNull();
        });

        it('Should disable bulkAdd and alert error when selected file is too large', () => {
            mockItem.file.size += 1;
            device.onAfterAddingBulkAddFile(mockItem);

            expect(device.model.bulkAddIsDisabled).toBe(true);
            expect(device.bulkAlert).not.toBeNull();
        });

        it('Should enable bulkAdd when file constraints are met', () => {
            device.onAfterAddingBulkAddFile(mockItem);

            expect(device.model.bulkAddIsDisabled).toBe(false);
            expect(device.bulkAlert).toBeNull();
        });

        it('Should remove previous alerts when file selected', () => {
            mockItem.file.size += 1;
            device.onAfterAddingBulkAddFile(mockItem);

            expect(device.bulkAlert).not.toBeNull();

            device.onAfterAddingBulkAddFile(mockItem);

            expect(device.bulkAlert).not.toBeNull();

            mockItem.file.size -= 1;
            device.onAfterAddingBulkAddFile(mockItem);

            expect(device.bulkAlert).toBeNull();
        });

    });

    describe('Single add api behavior', () => {
        beforeEach((done) => {
            device.bulkAddLoaded = () => {
                done();
            };

            device.ngOnInit();
        });

        it('should disable save button before a device id is added', () => {
            expect(device.addFormValid()).toBe(false);
        });

        it('Should send correct payload for regular devices', (done) => {
            device.servicePropertiesForm.deviceIdControl().updateValue('foo');
            device.servicePropertiesForm.primaryKeyControl().updateValue('bar');
            device.servicePropertiesForm.secondaryKeyControl().updateValue('baz');
            device.servicePropertiesForm.tagControl.tags.push('fub');

            expect(device.addFormValid()).toBe(true);

            inspectReq = (d: DeviceModel, subject) => {
                expect(d.deviceId).toBe('foo');
                expect(d.authentication.symmetricKey.primaryKey).toBe('bar');
                expect(d.authentication.symmetricKey.secondaryKey).toBe('baz');
                expect(d.serviceProperties.tags[0]).toBe('fub');
                done();
            };

            device.addDevice();
        });

        it('should not have a createdDevice', () => {
            expect(device.createdDevice).toBeFalsy();
        });

        it('Should show progress when attempting to add a device', () => {
            device.servicePropertiesForm.deviceIdControl().updateValue('foo');
            device.addDevice();
        });

        describe('onSuccess', () => {
            let returnedDevice;

            beforeEach(() => {

                device.servicePropertiesForm.deviceIdControl().updateValue('foo');

                device.addDevice();

                succeedWith((d: DeviceModel) => {
                    d.authentication.symmetricKey.primaryKey = 'bar';
                    d.authentication.symmetricKey.secondaryKey = 'baz';
                    d.serviceProperties.tags[0] = 'fub';

                    d['_deviceConnectionString'] = 'fuz';

                    returnedDevice = d;

                    return d;
                });
            });

            it('Should alert success', () => {
                expect(device.creationAlert).not.toBeNull();
                expect(device.creationAlert.msg).toBe(Resources.Device.titleAlertAddDeviceSuccess);
            });

            it('Should close bulk add alert', () => {
                expect(device.bulkAlert).toBeNull();
            });

            it('Should update createdDevice', () => {
                expect(device.createdDevice).toBeTruthy();
            });

            it('Should update deviceConnectionString', () => {
                expect(device.deviceConnectionString).toEqual('fuz');
            });

            it('Should be able to reset the form', () => {
                device.resetAddDeviceForm();

                expect(device.alert).toBeNull();
                expect(device.bulkAlert).toBeNull();
                expect(device.createdDevice).toBeNull();
            });
        });

        describe('onError', () => {
            let errMsg = ' foobar';

            beforeEach((done) => {
                device.servicePropertiesForm.deviceIdControl().updateValue('foo');

                device.addDevice();

                device.onSingleAddError({
                    json: () => {
                        return { _error: { message: errMsg } };
                    }
                });

                done();
            });

            it('Should alert error from response', () => {
                expect(device.alert).not.toBeNull();
                expect(device.alert.msg).toBe(errMsg);
            });

            it('Should close bulk add alert', () => {
                expect(device.bulkAlert).toBeNull();
            });
        });
    });
});
