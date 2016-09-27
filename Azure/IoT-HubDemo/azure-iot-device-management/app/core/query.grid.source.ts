/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Injectable} from '@angular/core';
import {GridSource, IGridSourceFilter, GridSourceView} from '@azure-iot/common-ux/grid';
import {DataService, ConfigurationService} from '../services/index';
import {Subject, BehaviorSubject} from 'rxjs/Rx';
import {HalLinks, HalResponse, IGridConfiguration, IGridFilter, LogicalOperatorType, LogicalExpression, ComparisonOperatorType, ComparisonExpression,
SortOrder, QueryExpression, ProjectionExpression, QueryProperty, SortExpression} from '../models/index';
import {Observable} from 'rxjs/Rx';

export type QueryFunction<T> = (skip?: number, count?: number, query?: QueryExpression, currentWorkspaceRels?: HalLinks) => Observable<HalResponse<T[]>>;

/**
 * QueryGridSource is an implemention of IGridSource<T> that pulls data from the services using IoTHub query expressions 
 */
@Injectable()
export class QueryGridSource<T> extends GridSource<T> {
    /**
     * This is how frequently we intend to poll for updates
     */
    public PollingFrequency: number = 150000;

    /**
    * QueryExpression to be send to IoT Hub for querying data
    */
    public query: QueryExpression = new QueryExpression();

    public currentValidRels: Subject<HalLinks>;
    
    /**
     * This is the interval variable used to close out the interval on clean up
     */
    private intervalId: number = null;

    /**
     * Query Function to be implemented by the subclassed QueryGridSource
     */
    public queryFunction: QueryFunction<T>;

    /**
     * Only thing to do in the constructor is to subscribe for updates to the grid configuration
     */
    constructor(defaultSortOrder: IGridSourceFilter) {
        super();

        this.currentValidRels = new Subject<HalLinks>();
        
        // set filter to default sorting strategy 
        this.filter.next(defaultSortOrder);
        
        // poll for updates
        this.poll(false);
    }
    
    // Selects any visible current values
    public select(id: string): void {
        this.openViews.forEach(view => {
            if (view) {
                view.first().subscribe(data => {
                    for (var i = 0; i < data.length; i++) {
                        if (this.getId(data[i]) === id) {
                            this.selection.value[id] = data[i];
                            this.selection.next(this.selection.value);
                            return;
                        }
                    }
                });
            }
        });
    }
    
    
    // Subscribes to the current selected configuration
    public subscribeTo = (selectedConfiguration: BehaviorSubject<IGridConfiguration>) => {
        
        // subscribe to selected configuration
        selectedConfiguration.subscribe(configuration => {
            this.query = new QueryExpression();
            this.constructProjectionExpression(configuration);
            this.constructFilterExpression(configuration);
            this.update();
        });

        // subscribe to filter 
        this.filter.subscribe(gridFilter => {
            this.sortColumn(gridFilter, selectedConfiguration.value);
        });
    };
    
    // Constructs the filter expression based on the filters of grid confguration
    private constructFilterExpression = (configuration: IGridConfiguration) => {
        if (configuration.filters.length > 0) {
            // construct root logical expression - and logical operator will be used for everything in this expression 
            let parentLogicalExpression = new LogicalExpression(LogicalOperatorType.and, []);
            configuration.filters.forEach(filter => {
                this.constructFiltersExpression(filter, parentLogicalExpression);
            });
            this.query.filter = parentLogicalExpression;
        }
    };

    // construct inner filter expression
    private constructFiltersExpression = (filter: IGridFilter, parentLogicalExpression: LogicalExpression) => {
        if (filter && filter.key) {
            let model = this.changeTagsModel(filter.key, filter.model);
            if (filter.isArray) {
                // if the column is an array, use all comparison operator and send the array as the value (filter.in) 
                let queryProp = new QueryProperty(filter.key, model);
                let comparisonExpression = new ComparisonExpression(queryProp, filter.in, ComparisonOperatorType.All);
                parentLogicalExpression.filters.push(comparisonExpression);
            } else {
                // if the column is not array generate a comparison expression for each value in the array - use or as the logical operator 
                let logicalExpression = new LogicalExpression(LogicalOperatorType.and, []);
                filter.in.forEach(value => {
                    let queryProp = new QueryProperty(filter.key, model);
                    let comparisonExpression = new ComparisonExpression(queryProp, value, ComparisonOperatorType.Equals);
                    logicalExpression.filters.push(comparisonExpression);
                });
                parentLogicalExpression.filters.push(logicalExpression);
            }
        }
    };

    // Constructs the projection expression based on the columns of grid confguration
    private constructProjectionExpression = (configuration: IGridConfiguration) => {
        if (configuration.columns && configuration.columns.length > 0) {
            // Use projection only when all projected fields are indexed 
            if (this.checkIfAllAreIndexed(configuration.columns)) {
                let queryProperties: QueryProperty[] = [];
                for (let column of configuration.columns) {
                    let key = column.key;

                    let model = this.changeTagsModel(column.key, column.model);
                    queryProperties.push(new QueryProperty(key, model));
                }
                this.query.project = new ProjectionExpression(false, queryProperties);
            }
        }
    };
    
    // Constructs query with sort expression and calls the API 
    private sortColumn = (gridFilter: IGridSourceFilter, gridConfiguration: IGridConfiguration) => {
        let sorted = gridFilter.sorted;
        if (sorted) {
            let order: string = this.getSortOrder(sorted.ascending);
            let model: string = this.getModel(sorted.columnKey, gridConfiguration);
            let property: QueryProperty = new QueryProperty(sorted.columnKey, model);
            this.query.sort = [new SortExpression(order, property)];
            this.update();
        }
    };
    
    // Gets the sort order (ascending/descending) of the query expression
    public getSortOrder = (asc: boolean): string => {
        if (asc)
            return SortOrder.asc;
        else
            return SortOrder.desc;
    };
    
    // Find the model of the given columnKey from the current configuration 
    public getModel = (columnKey: string, gridConfiguration: IGridConfiguration) => {
        let model: string;
        gridConfiguration.columns.forEach(column => {
            if (column.name === columnKey) {
                model = this.changeTagsModel(columnKey, column.model);
            }
        });
        return model;
    };
 
    // Query by projection doesn't let us use all the fields we want to project (ex: Status). 
    // This check can be removed when projection by all fields is allowed  
    public checkIfAllAreIndexed = (columns: any): boolean => {
        for (let column of columns) {
            if (column.indexed === false)
                return false;
        }
        return true;
    };

    // Although tags is under service properties, you have to send it as type = default in queries  
    // This can be removed once tags can be send with type 'service' in query 
    public changeTagsModel = (columnKey: string, columnModel: string): string => {
        let model = columnModel;
        if (columnKey === 'tags') {
            model = 'default';
        }
        return model;
    };

    /**
    * See common.grid.poll for more information on the signature
    */
    public poll = (shouldPoll?: boolean) => {
        if (shouldPoll !== undefined) {
            if (shouldPoll && this.intervalId == null) {
                this.intervalId = setInterval(() => this.update(), this.PollingFrequency);
            } else if (!shouldPoll && this.intervalId != null) {
                clearInterval(this.intervalId);
                this.intervalId = null;
            }
        }
        return !!this.intervalId;
    };
    
    /**
    * This will force an update to all open views. It is also called periodically.
    */
    public update = () => {
        this.openViews.forEach(gridView => {
            if (gridView) {
                var updatedSelection: { [key: string]: T } = null;
                this.queryFunction(gridView.viewSkip, gridView.viewCount, this.query)
                    .subscribe(updatedView => {
                        gridView.next(updatedView.data);
                        this.currentValidRels.next(updatedView.links);
                    });
            }
        });
    };
}