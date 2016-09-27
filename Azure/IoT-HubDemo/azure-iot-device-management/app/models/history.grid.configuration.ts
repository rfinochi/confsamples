/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {IGridColumn, IGridFilter, IGridConfiguration} from './grid.configuration';
import {Resources} from '../core/resources';

/**
 * This is the default history grid configuration
 */
export const DefaultHistoryGridConfiguration: IGridConfiguration = {
    'name': 'Default',
    'columns': [
        {
            'name': Resources.JobProperties.StartTime,
            'model': 'default',
            'key': 'startTimeUtc',
            'indexed': true
        },
        {
            'name': Resources.JobProperties.ActionID,
            'model': 'default',
            'key': 'jobId',
            'indexed': true
        },
        {
            'name': Resources.JobProperties.ParentChild,
            'model': 'calculated',
            'key': 'parent-child',
            'indexed': false
        },
        {
            'name': Resources.JobProperties.Status,
            'model': 'default',
            'key': 'status',
            'indexed': true
        },
        {
            'name': Resources.JobProperties.DeviceID,
            'model': 'default',
            'key': 'deviceId',
            'indexed': true
        }
    ],
    'filters': [
    ]
};

/**
 * These are the available history grid columns
 */
export const HistoryGridAvailableColumns: IGridColumn[] = [
    {
        'name': Resources.JobProperties.ActionID,
        'model': 'default',
        'key': 'jobId',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.StartTime,
        'model': 'default',
        'key': 'startTimeUtc',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.EndTime,
        'model': 'default',
        'key': 'endTimeUtc',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.Status,
        'model': 'default',
        'key': 'status',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.Type,
        'model': 'default',
        'key': 'type',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.FailureReason,
        'model': 'default',
        'key': 'failureReason',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.StatusMessage,
        'model': 'default',
        'key': 'statusMessage',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.DeviceID,
        'model': 'default',
        'key': 'deviceId',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.ParentActionID,
        'model': 'default',
        'key': 'parentJobId',
        'indexed': true
    },
    {
        'name': Resources.JobProperties.ParentChild,
        'model': 'calculated',
        'key': 'parent-child',
        'indexed': false
    }
];

/**
 * These are the available history grid filters
 */
export const HistoryGridAvailableFilters: IGridFilter[] = [
    {
        'name': Resources.JobProperties.ActionID,
        'model': 'default',
        'key': 'jobId',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.StartTime,
        'model': 'default',
        'key': 'startTimeUtc',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.EndTime,
        'model': 'default',
        'key': 'endTimeUtc',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.Status,
        'model': 'default',
        'key': 'status',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.Type,
        'model': 'default',
        'key': 'type',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.FailureReason,
        'model': 'default',
        'key': 'failureReason',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.StatusMessage,
        'model': 'default',
        'key': 'statusMessage',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.DeviceID,
        'model': 'default',
        'key': 'deviceId',
        'in': [],
        'isArray': false
    },
    {
        'name': Resources.JobProperties.ParentActionID,
        'model': 'default',
        'key': 'parentJobId',
        'in': [],
        'isArray': false
    }
];
