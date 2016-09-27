/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Injectable} from '@angular/core';
import {IGridSourceFilter} from '@azure-iot/common-ux/grid';
import {QueryGridSource} from '../../core/index';
import {Job} from '../../models/index';
import {DataService} from '../../services/index';

/**
 * Querying jobs without a sort expression returns jobs sorted by jobId in ascending order 
 * Change defaultSortOrder if this logic changes. 
 */
const defaultSortOrder: IGridSourceFilter = {
    sorted: {
        columnKey: 'startTimeUtc',
        ascending: false
    }
};

/**
 * HistoryGridSource is an implemention of IGridSource<Job> that pulls data for jobs from the services
 */
@Injectable()
export class HistoryGridSource extends QueryGridSource<Job> {
    /**
     * Only thing to do in the constructor is to subscribe for updates to the grid configuration
     */
    constructor(private dataService: DataService) {
        super(defaultSortOrder);
    }

    /**
     * Gets the id for a job
     */
    public getId = (job: Job) => job.jobId;

    public queryFunction = this.dataService.getJobs;
}