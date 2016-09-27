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

import {StorageKeys, ConfigurationService} from './configuration.service';
import {DefaultDeviceGridConfiguration} from '../../models/device.grid.configuration';

describe('Device Grid Configuration Tests', () => {

    beforeAll(() => {
        Object.keys(StorageKeys).forEach(key => {
            StorageKeys[key] += '.test';
        });
    });

    beforeEach(() => {
        // clear out storage
        sessionStorage.clear();
        localStorage.clear();
    });
    
    it('uses default device grid configuration when none present', done => {
        var state = new ConfigurationService();
        state.CurrentDeviceGridView.subscribe(config => {
            expect(config).toEqual(DefaultDeviceGridConfiguration);
            done();
        });
    });
    
    it('uses session storage grid configuration if present', done => {
        var fakeConfig = { name: 'test' };
        sessionStorage.setItem(StorageKeys.CurrentDeviceGridView, JSON.stringify(fakeConfig));
        var state = new ConfigurationService();
        state.CurrentDeviceGridView.subscribe(config => {
            expect(config).toEqual(fakeConfig);
            done();
        });
    });
    
    afterEach(() => {
        // clear out storage
        sessionStorage.clear();
        localStorage.clear();
    });
});
