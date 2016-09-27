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

import {FormBuilder} from '@angular/common';
import {OnChanges} from '@angular/core';

import {ServicePropertiesForm} from './device.service.properties.form';
import {Device, AlertType} from '../../models/index';
import {clone} from '../../core/index';
import {TagInput} from '../common.tag.input/common.tag.input';
import {KeyValueInput} from '../common.keyvalue.input/common.keyvalue.input';
import {ToggleInput} from '../common.toggle.input/common.toggle.input';

let initialSuffix = '_test';
let updatedSuffix = '_updated';

function createDevice(propertySuffix: string): Device {
    beforeEachProviders(() => [
        FormBuilder
    ]);
    let device = new Device();

    device.deviceId = 'deviceId' + propertySuffix;
    device.authentication.symmetricKey.primaryKey = 'primaryKey' + propertySuffix;
    device.authentication.symmetricKey.secondaryKey = 'secondaryKey' + propertySuffix;

    device.serviceProperties.properties = {
        test: 'test' + propertySuffix
    };

    device.serviceProperties.tags = ['test' + propertySuffix];

    device.status = 'disabled';

    return device;
}

function checkFieldValues(servicePropForm: ServicePropertiesForm, propertySuffix: string) {
    // the clones are because the matchers check for the type of sub properties instead of just structure
    let currentDeviceValue = clone(servicePropForm.toDevice()),
        correctDeviceValue = clone(createDevice(propertySuffix));

    correctDeviceValue.status = servicePropForm.device ? servicePropForm.device.status : 'disabled';

    expect(currentDeviceValue).toEqual(correctDeviceValue);

    if (servicePropForm.device) {
        expect(servicePropForm.form.controls['connectionString'].value).toBe('test_deviceConfigurationString' + propertySuffix);
    } else {
        expect(servicePropForm.form.controls['connectionString'].value).toBeNull();
    }
}

function simulateInitialLifetimeMethods(servicePropForm: ServicePropertiesForm) {
    servicePropForm.propertiesValueControl.ngOnChanges(<any>{});
    servicePropForm.propertiesValueControl.ngOnInit();

    servicePropForm.tagControl.ngOnChanges();

    servicePropForm.ngOnChanges(<any>{
        device: {
            previousValue: {}
        }
    });
    servicePropForm.ngOnInit();
}

function bindDeviceInput(servicePropForm: ServicePropertiesForm,  propertySuffix?: string): Device {

    let device = propertySuffix ? createDevice(propertySuffix) : null;
    let devConnStr = propertySuffix ? 'test_deviceConfigurationString' + propertySuffix : null;

    let propertiesChanges: any = {
        keyValueMap: {
            previousValue: servicePropForm.propertiesValueControl.keyValueMap
        }
    };

    // inject device
    servicePropForm.device = device;

    // rebind subcomponent inputs
    servicePropForm.propertiesValueControl.keyValueMap = <any>servicePropForm.properties();
    servicePropForm.tagControl.tags = servicePropForm.tags();            
    servicePropForm.enabledControl.value = servicePropForm.enabled();
    
    // call onchange to trigger updates in child components
    servicePropForm.propertiesValueControl.ngOnChanges(propertiesChanges);
    servicePropForm.tagControl.ngOnChanges();

    servicePropForm.deviceConnectionString = devConnStr; 

    return device;
}

describe('ServicePropertiesForm Tests', () => {
    beforeEachProviders(() => [
        FormBuilder
    ]);
    let servicePropForm: ServicePropertiesForm;

    beforeEachProviders(() => [
        KeyValueInput,
        TagInput,
        ToggleInput
    ]);
    
    beforeEach(inject(
        [FormBuilder, KeyValueInput, TagInput, ToggleInput], 
        (formBuilder: FormBuilder, keyValueInput: KeyValueInput, tagInput: TagInput, toggleInput: ToggleInput) => {
            servicePropForm = new ServicePropertiesForm(formBuilder);

            // inject child components in place of @ViewChild(...)
            servicePropForm.propertiesValueControl = keyValueInput;
            servicePropForm.tagControl = tagInput;
            servicePropForm.enabledControl = toggleInput;
        }
    ));
    
    it('should construct', () => {
        expect(servicePropForm).toBeDefined();
    });

    describe('Starting with a device', () => {
        beforeEach(() => {
            let device = bindDeviceInput(servicePropForm, initialSuffix);            

            simulateInitialLifetimeMethods(servicePropForm);
        });  

        it('initializes the form fields', () => {
            checkFieldValues(servicePropForm, initialSuffix);
        });

        it('should not be readonly', () => {
            expect(servicePropForm.isReadonly).toBe(false);
        });

    });    

    describe('Starting without a device', () => {
        beforeEach(() => {
            simulateInitialLifetimeMethods(servicePropForm);            
        });  

        it('should not be readonly', () => {
            expect(servicePropForm.isReadonly).toBe(false);
        });

        describe('Device input updates to be truthy', () => {
            let updatedDevice;

            beforeEach(() => {
                updatedDevice = bindDeviceInput(servicePropForm, updatedSuffix);

                servicePropForm.ngOnChanges(<any>{
                    device: {
                        previousValue: null
                    }
                });
            });

            it('should enter readonly state', () => {
                expect(servicePropForm.isReadonly).toBe(true);
            });

            it('should update the form fields', () => {
                checkFieldValues(servicePropForm, updatedSuffix);
            });

            describe('Device input is reset to be falsy', () => {

                beforeEach(() => {
                    let previousDevice = servicePropForm.device;

                    bindDeviceInput(servicePropForm);

                    servicePropForm.ngOnChanges(<any>{
                        device: {
                            previousValue: previousDevice
                        }
                    });
                });

                it('should exit readonly state', () => {
                    expect(servicePropForm.isReadonly).toBe(false);
                });

                it('should clear the form', () => {
                    // test that the functions bound in the template will clear the inputs
                    expect(servicePropForm.tags()).toBeFalsy();
                    expect(servicePropForm.properties()).toBeFalsy();
                    
                    // enabled by default
                    expect(servicePropForm.enabled()).toBeTruthy();

                    expect(servicePropForm.deviceIdControl().value).toBeFalsy();
                    expect(servicePropForm.connectionStringControl().value).toBeFalsy();
                    expect(servicePropForm.primaryKeyControl().value).toBeFalsy();
                    expect(servicePropForm.secondaryKeyControl().value).toBeFalsy();
                });
            });
        });
    });
});