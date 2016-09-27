/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Injectable} from '@angular/core';
import {BehaviorSubject} from 'rxjs/Rx';
import {DefaultDeviceGridConfiguration} from '../../models/device.grid.configuration';
import {DefaultHistoryGridConfiguration} from '../../models/history.grid.configuration';
import {IGridConfiguration} from '../../models/grid.configuration';
import {StorageSubject} from './configuration.service.storage';

/**
 * This is the keys used for local storage
 */
export const StorageKeys = {
    CurrentDeviceGridView: 'dmux_device.grid.configuration',
    AvailableDeviceGridViews: 'dmux_device.grid.configurations',
    CurrentHistoryGridView: 'dmux_history.grid.configuration',
    AvailableHistoryGridViews: 'dmux_history.grid.configurations'
};

/**
 * This represents the user's configuration state; while they are currently stored in local storage, they could be pushed out to a service without changing the schema
 */
@Injectable()
export class ConfigurationService {
    /**
     * This represents the current configuration of the view for devices
     */
    public CurrentDeviceGridView: BehaviorSubject<IGridConfiguration>
        = new StorageSubject<IGridConfiguration>(sessionStorage, StorageKeys.CurrentDeviceGridView, DefaultDeviceGridConfiguration);
    /**
     * This represents the available configurations of the view for devices
     */
    public AvailableDeviceGridViews: BehaviorSubject<IGridConfiguration[]>
        = new StorageSubject<IGridConfiguration[]>(localStorage, StorageKeys.AvailableDeviceGridViews, [DefaultDeviceGridConfiguration]);
    /**
    * This represents the current configuration of the view for history of jobs
    */
    public CurrentHistoryGridView: BehaviorSubject<IGridConfiguration>
        = new StorageSubject<IGridConfiguration>(sessionStorage, StorageKeys.CurrentHistoryGridView, DefaultHistoryGridConfiguration);
    /**
     * This represents the available configurations of the view for history of jobs
     */
    public AvailableHistoryGridViews: BehaviorSubject<IGridConfiguration[]>
        = new StorageSubject<IGridConfiguration[]>(localStorage, StorageKeys.AvailableHistoryGridViews, [DefaultHistoryGridConfiguration]);
}