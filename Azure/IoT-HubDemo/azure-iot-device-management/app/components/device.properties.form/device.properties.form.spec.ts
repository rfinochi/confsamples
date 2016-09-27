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
import {Subject} from 'rxjs/Rx';

import {DevicePropertiesForm} from './device.properties.form';
import {Device, AlertType} from '../../models/index';
import {Resources} from '../../core/index';

let configurationKey = 'configuration';
let initialSuffix = '_test';
let updatedSuffix = '_updated';

function createDevice(devicePropForm: DevicePropertiesForm, propertySuffix: string): Device {
    let device = new Device();

    devicePropForm.readonlyProperties.forEach((prop) => {
        device.deviceProperties[prop.key] = { value: prop.key + propertySuffix };
    });

    devicePropForm.writeableProperties.forEach((prop) => {
        device.deviceProperties[prop.key] = { value: prop.key + propertySuffix };
    });

    device[configurationKey] = configurationKey + propertySuffix;

    return device;
}

function checkFieldValues(devicePropForm: DevicePropertiesForm, propertySuffix: string) {
    expect(devicePropForm.form).toBeDefined();

    devicePropForm.readonlyProperties.forEach((prop) => {
        expect(devicePropForm.form.controls[prop.key].value).toEqual(prop.key + propertySuffix);
    });

    devicePropForm.writeableProperties.forEach((prop) => {
        expect(devicePropForm.form.controls[prop.key].value).toEqual(prop.key + propertySuffix);
    });
}

describe('DevicePropertiesForm Tests', () => {
    beforeEachProviders(() => [
        FormBuilder
    ]);
    let devicePropForm: DevicePropertiesForm;
    
    beforeEach(inject([FormBuilder], (formBuilder: FormBuilder) => {
        devicePropForm = new DevicePropertiesForm(formBuilder);

        devicePropForm.device = createDevice(devicePropForm, initialSuffix);

        devicePropForm.ngOnInit();
    }));
    
    it('should construct', () => {
        expect(devicePropForm).toBeDefined();
    });
    
    it('should initialize form onInit', () => {
        checkFieldValues(devicePropForm, initialSuffix);
    });
});