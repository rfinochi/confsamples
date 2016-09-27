/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {IGridColumn, IGridFilter, IGridConfiguration} from './grid.configuration';
import {Resources} from '../core/resources';

/**
 * This is the default device grid configuration
 */
export const DefaultDeviceGridConfiguration: IGridConfiguration = {
    'name': 'Default',
    'columns': [
        {
            'name': Resources.DeviceBaseProperties.DeviceID,
            'model': 'default',
            'key': 'deviceId',
            'indexed': true
        },
        {
            'name': Resources.DeviceBaseProperties.Status,
            'model': 'default',
            'key': 'status',
            'indexed': false
        },
        {
            'name': Resources.DeviceBaseProperties.Tags,
            'model': 'service',
            'key': 'tags',
            'indexed': true
        }
    ],
    'filters': [
    ]
};

/**
 * These are the available device grid columns
 */
export const DeviceGridAvailableColumns: IGridColumn[] = [
    {
        'name': Resources.DeviceBaseProperties.DeviceID,
        'model': 'default',
        'key': 'deviceId',
        'indexed': true
    },
    {
        'name': Resources.DeviceBaseProperties.Status,
        'model': 'default',
        'key': 'status',
        'indexed': false
    },
    {
        'name': Resources.DeviceBaseProperties.ConnectionState,
        'model': 'default',
        'key': 'connectionState',
        'indexed': false
    },
    {
        'name': Resources.DeviceBaseProperties.Tags,
        'model': 'service',
        'key': 'tags',
        'indexed': true
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.Manufacturer,
        'model': 'system',
        'key': 'Manufacturer',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.Description,
        'model': 'system',
        'key': 'DeviceDescription',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.ModelNumber,
        'model': 'system',
        'key': 'ModelNumber',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.SerialNumber,
        'model': 'system',
        'key': 'SerialNumber',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.HardwareVersion,
        'model': 'system',
        'key': 'HardwareVersion',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.FirmwareVersion,
        'model': 'system',
        'key': 'FirmwareVersion',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.BatteryLevel,
        'model': 'system',
        'key': 'BatteryLevel',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.BatteryStatus,
        'model': 'system',
        'key': 'BatteryStatus',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.MemoryFree,
        'model': 'system',
        'key': 'MemoryFree',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.MemoryTotal,
        'model': 'system',
        'key': 'MemoryTotal',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.Write.CurrentTime,
        'model': 'system',
        'key': 'CurrentTime',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.Write.UTCOffset,
        'model': 'system',
        'key': 'UtcOffset',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.Write.Timezone,
        'model': 'system',
        'key': 'Timezone',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.DefMinPeriod,
        'model': 'system',
        'key': 'DefaultMinPeriod',
        'indexed': false
    },
    {
        'name': Resources.DeviceProperties.ReadOnly.DefMaxPeriod,
        'model': 'system',
        'key': 'DefaultMaxPeriod',
        'indexed': false
    }
];

/**
 * These are the available device grid filters
 */
export const DeviceGridAvailableFilters: IGridFilter[] = [
    {
        'name': Resources.DeviceBaseProperties.DeviceID,
        'model': 'default',
        'key': 'deviceId',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.DeviceBaseProperties.Tags,
        'model': 'service',
        'key': 'tags',
        'in': [],
        'isArray': true
    }
];