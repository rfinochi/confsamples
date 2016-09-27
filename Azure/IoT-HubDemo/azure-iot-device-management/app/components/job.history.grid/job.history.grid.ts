/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, ElementRef} from '@angular/core';
import {CommonMenu} from '../common.menu/common.menu';
import {HistoryForm} from '../job.history.form/job.history.form';
import {Grid, SelectionStyle, GridConfiguration} from '@azure-iot/common-ux/grid';
import {SetEditor} from '@azure-iot/common-ux/set-editor';
import {HistoryGridSource} from './job.history.grid.source';
import {AppNav} from '../application.nav/application.nav';
import {ConfigurationService, DataService} from '../../services/index';
import {GlobalContext, gridConfigEquals, clone} from '../../core/index';
import {ComparisonOperatorType, ComparisonExpression, QueryExpression, QueryProperty, HalLinks, HalResponse, Job, HistoryGridAvailableFilters,
IGridFilter, IGridColumn, HistoryGridAvailableColumns, AlertType, Alert, IGridConfiguration, HalButton} from '../../models/index';
import {GridNavConfigurationJobActions} from './job.history.grid.nav.configuration';
import {FilterEditor, FilterValue} from '../common.filter/common.filter';
import {DROPDOWN_DIRECTIVES} from 'ng2-bootstrap/ng2-bootstrap';
import {BehaviorSubject, Observable} from 'rxjs/Rx';
import {RouteParams} from '@angular/router-deprecated';
import {Location} from '@angular/common';
import {Resources} from '../../core/resources';
import {Alert as AlertView} from '../common.alert/common.alert';

/**
 * Model Resolution Strategies
 * Note the memoized argument structure
 */
const modelHanders = {
    'default': key => datum => {
        let property = datum[key];
        return !!property ? property.toString() : '';
    },
    'calculated': key => datum => {
        // special calculated cases
        switch (key) {
            case 'parent-child': 
                return !datum.parentJobId
                    ? Resources.JobProperties.IsParent
                    : Resources.JobProperties.IsChild;
        }
        return '';
    }
};

@Component({
    selector: 'history-grid',
    templateUrl: 'dist/app/components/job.history.grid/job.history.grid.html',
    providers: [HistoryGridSource],
    directives: [AppNav, CommonMenu, SetEditor, Grid, HistoryForm, FilterEditor, DROPDOWN_DIRECTIVES, AlertView]
})

// refactor: this should be history-grid
export class HistoryGrid extends GlobalContext {

    public gridNavConfigurationJobActions: {};
    public currentJob: Job;

    public currentValidRels: HalLinks;

    public currentActiveNavButton: HalButton<any>;
    public currentSelectedGridItem: Job;
    public currentJobId: string;

    public hideDetails: boolean;
    public HistNavConfiguration: {};
    public editorVisible: boolean = false;
    
    // Grid configurations
    public gridConfiguration: GridConfiguration<Job>;
    public selectedConfiguration: BehaviorSubject<IGridConfiguration>;
    public availableConfigurations: IGridConfiguration[];

    public searchJob: string;
    
    public viewFilterUnsaved: boolean = false;

    public gridConfigurationUnsaved: boolean = false;
    
    public availableColumns: BehaviorSubject<IGridColumn[]> = new BehaviorSubject<IGridColumn[]>(HistoryGridAvailableColumns);
    public currentColumns: BehaviorSubject<IGridColumn[]> = new BehaviorSubject<IGridColumn[]>([]);

    public currentFilters: BehaviorSubject<FilterValue<IGridFilter>[]> = new BehaviorSubject([]);
    public availableFilters = HistoryGridAvailableFilters;
    
    constructor(
        public gridSource: HistoryGridSource,
        public gridState: ConfigurationService,
        public routeParams: RouteParams,
        public dataService: DataService,
        public location: Location
    ) {
        super();
        this.gridNavConfigurationJobActions = GridNavConfigurationJobActions;

        this.selectedConfiguration = new BehaviorSubject<IGridConfiguration>(this.gridState.CurrentHistoryGridView.value);

        this.gridState.AvailableHistoryGridViews.subscribe(availableGridViews => {
            this.onAvailableConfigurationsLoaded(availableGridViews);
        });
        
        this.gridState.CurrentHistoryGridView.subscribe(currentGridView => {
            this.selectConfiguration(currentGridView);
        });
        
        this.currentJob = new Job();
        this.hideDetails = true;
    }

    public ngOnInit() {
        this.alert = null;
        let jobId = this.routeParams.get('jobId');

        this.gridSource.currentValidRels.subscribe((halLinksObject: HalLinks) => {
            this.currentValidRels = halLinksObject;

            if (jobId) {
                this.forceOpenDetails(jobId);
                this.gridSource.select(jobId);
            }
        });
        this.currentColumns.skip(1).subscribe(columns => {
            this.selectedConfiguration.value.columns = columns;
            this.gridConfigurationUnsaved = true;
        });

        this.gridSource.subscribeTo(this.selectedConfiguration);
    }

    public findJob() {
        if (!this.searchJob) {
            this.resetDetails();
            this.alert = new Alert(AlertType.Danger, this.Resources.HistoryGrid.titleAlertEmptySearchError);
            return;
        }

        let jobId = this.searchJob.replace(new RegExp('\\s', 'g'), '');
        let job = new Job();
        job.jobId = jobId;
        this.onJobDoubleClick(job);
        this.gridSource.selection.next({});
        this.gridSource.select(jobId); 
        this.searchJob = '';
    }

    public updateSelection = (selected: Job[]) => {
        this.currentSelectedGridItem = selected[0];

        if (!this.hideDetails) {
            if (this.currentSelectedGridItem) {
                this.fetchCurrentJob(this.currentSelectedGridItem.jobId);
            } else {
                this.resetDetails();
            }
        }
    };

    public onJobDoubleClick(job: Job) {
        if (this.hideDetails) {
            this.forceOpenDetails(job.jobId);
        } else {
            this.fetchCurrentJob(job.jobId);
        }
    }

    public forceOpenDetails(jobId: string) {
        this.gridNavAction(this.gridNavConfigurationJobActions['GridNavButtons'][0]);
        this.fetchCurrentJob(jobId);
    }

    public fetchCurrentJob(jobId: string): void {
        this.currentJobId = jobId;
        this.queryJob(jobId, this.currentValidRels).subscribe(this.onQueryJobSuccess, this.onQueryJobFailure);
    }

    public queryJob(jobId: string, currentValidRels: HalLinks): Observable<HalResponse<Job[]>> {
        let query = this.constructQueryExpression(jobId);
        return this.dataService.getJobs(null, null, query, currentValidRels);
    }

    public constructQueryExpression(jobId: string): QueryExpression {
        let queryProp = new QueryProperty('jobId', 'default');
        let comparisonExpression = new ComparisonExpression(queryProp, jobId, ComparisonOperatorType.Equals);
        let query = new QueryExpression();
        query.filter = comparisonExpression;
        return query;
    }

    private onQueryJobSuccess = (res: HalResponse<Job[]>) => {
                        
        document.body.scrollIntoView();
        
        let job = res.data.filter(j => j.jobId === this.currentJobId)[0];

        if (!job || job.status === 'unknown') {
            this.alertOnJobNotFound();
            return;
        }

        this.hideDetails = false;
        this.alert = null;
        this.currentJob = job;
        this.location.go(`/job/${job.jobId}`);
    };

    private onQueryJobFailure = (res: any) => {
        this.resetDetails();
        let halResponse: HalResponse<any> = res.json();
        this.alert = new Alert(AlertType.Danger, halResponse._error.message);
    };

    private alertOnJobNotFound = () => {
        this.resetDetails();
        this.alert = new Alert(AlertType.Danger, this.Resources.HistoryGrid.jobFailureAlert);
    };
    
    public resetDetails() {
        this.hideDetails = true;
        this.currentActiveNavButton = null;
        this.currentJob = new Job();
        this.location.go('/history');
    }

    public gridNavAction = (selectedNavAction: HalButton<Job>) => {
        this.alert = null;
        switch (selectedNavAction.rel) {
            case 'jobs:get':
                if (this.currentActiveNavButton && this.currentActiveNavButton.rel === selectedNavAction.rel) {
                    this.resetDetails();
                } else {
                    // activate button
                    this.currentActiveNavButton = selectedNavAction;

                    if (this.currentSelectedGridItem) {
                        this.fetchCurrentJob(this.currentSelectedGridItem.jobId);
                    }
                }

            case 'jobs:cancel':
                break;
        }
    };

    private setColumnSelection() {
        this.currentColumns.next(this.selectedConfiguration.value.columns);
    }
    
    public getColumnLabel(column: IGridColumn | IGridFilter) {
        return column.name;
    }

    private setFilters(): void {
        let filters: FilterValue<IGridFilter>[] 
            = this.selectedConfiguration.value.filters.map((filter) => {
                // Note: we currently only support single values, not arrays
                let value = filter.in[0];
                let option = _.find(HistoryGridAvailableFilters, x => x.model === filter.model && x.key === filter.key);
                return { option: option, value: value };
            });
        this.currentFilters.next(filters);
    }

    private onAvailableConfigurationsLoaded(configurations: IGridConfiguration[]): void {
        this.availableConfigurations = configurations;
    }

    public setGridConfiguration(): void {
        this.gridConfiguration = {
            columns: this.selectedConfiguration.value.columns.map(configuredColumn => {
                return {
                    header: () => configuredColumn.name,
                    value: modelHanders[configuredColumn.model](configuredColumn.key),
                    width: configuredColumn.width,
                    sortable: configuredColumn.indexed,
                    key: configuredColumn.key
                };
            }),
            selectionStyle: SelectionStyle.SingleSelect
        };
    }
    
    public searchKeyPress($event: KeyboardEvent) {
        // enter key
        if ($event.keyCode === 13) {
            this.findJob();
        }
    }

    public toggleEditor(): void {
        this.editorVisible = !this.editorVisible;
    }

    public applyFilter(): void {
        let filters = this.currentFilters.value.map(filter => {
            if (!filter.option) return;
            filter.option.in = [filter.value];
            return clone(filter.option);
        }).filter(filter => !!filter);

        this.selectedConfiguration.value.filters = filters || null;

        if (!gridConfigEquals(this.gridState.CurrentHistoryGridView.value, this.selectedConfiguration.value)) {
            this.gridState.CurrentHistoryGridView.next(this.selectedConfiguration.value);
        }
    }

    public selectConfiguration(configuration: IGridConfiguration): void {
        this.selectedConfiguration.next(clone(configuration));

        this.setColumnSelection();
        this.setFilters();
        this.setGridConfiguration();
    }
    
    public onViewFilterChange() {
        this.gridConfigurationUnsaved = true;
    }

    public saveConfiguration(): void {
        var configuration = _.find(this.availableConfigurations, config => config.name === this.selectedConfiguration.value.name);

        if (configuration) {
            Object.assign(configuration, this.selectedConfiguration.value);
        }
        else {
            configuration = clone(this.selectedConfiguration.value);
            this.availableConfigurations.push(configuration);
        }
        this.applyFilter();
                
        this.gridState.AvailableHistoryGridViews.next(this.availableConfigurations);
        this.alert = new Alert(AlertType.Success, this.Resources.DeviceGrid.titleAlertViewSaved, true, 5000);
        this.gridConfigurationUnsaved = false;
    }
    
    public getColumnId(filter: IGridFilter) {
        return filter.model + '.' + filter.key;
    }
}
