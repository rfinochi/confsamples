/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Injectable} from '@angular/core';
import {GridSource, IGridSourceFilter} from '@azure-iot/common-ux/grid';
import {DataService} from '../../services/index';
import {Device} from '../../models/index';
import {QueryGridSource} from '../../core/index';
import {QueryExpression, HalLinks, HalResponse} from '../../models/index';
import {Observable} from 'rxjs/Rx';
/**
 * Querying devices without a sort expression returns devices sorted by deviceId in ascending order 
 * Change defaultSortOrder if this logic changes. 
 */
const defaultSortOrder: IGridSourceFilter = {
    sorted: {
        columnKey: 'deviceId',
        ascending: true
    }
};

interface IRegistryStatistics {
    totalDeviceCount: number;
    enabledDeviceCount: number;
    disabledDeviceCount: number;
}
    
/**
 * DeviceGridSource is an implemention of IGridSource<Device> that pulls data for devices from the services
 */
@Injectable()
export class DeviceGridSource extends QueryGridSource<Device> {

    public lastUpdated: Date;
    public registryStats: IRegistryStatistics;
    public visibleRowCount: number;

    constructor(private dataService: DataService) {
        super(defaultSortOrder);

        this.registryStats = {
            totalDeviceCount: 0,
            enabledDeviceCount: 0,
            disabledDeviceCount: 0
        };

        this.visibleRowCount = 0;

        this.lastUpdated = new Date();
    }

    /**
     * Gets the id for a device
     */
    public getId = (device: Device) => device.deviceId;

    public queryFunction = (skip?: number, count?: number, query?: QueryExpression, currentWorkspaceRels?: HalLinks) => {
        return this.dataService.getDevicesFromQuery(skip, count, query, currentWorkspaceRels)
            .do(this.updateStatistics)
            .do(this.updateLastUpdated);
    };

    private updateStatistics = (returnedRows: HalResponse<Device[]>) => {
        this.visibleRowCount = returnedRows.data.length;
        
        this.dataService.getRegistryStatistics()
            .subscribe((stats: HalResponse<any>) => {
                this.registryStats = stats.data;
            });
    };

    private updateLastUpdated = () => {
        this.lastUpdated = new Date();
    };
}