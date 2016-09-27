/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, Input, Output, EventEmitter} from '@angular/core';
import {NgClass} from '@angular/common';
import {GlobalContext} from '../../core/index';
import {BehaviorSubject} from 'rxjs/Rx';

/**
 * This is a single filter value
 */
export interface FilterValue<T> {
    option: T;
    value: string;
}

/**
 * This gives a UX for editing a filter
 */
@Component({
    selector: 'filter-editor',
    templateUrl: 'dist/app/components/common.filter/common.filter.html',
    styleUrls: ['dist/app/components/common.filter/common.filter.css']
})
export class FilterEditor<T> extends GlobalContext {
    
    /**
     * This is used to get the id for each option, which is used to populate the option select
     */
    @Input() public getId: (T) => string;
    
    /**
     * This is used to get the label for each option, which is used to represent it in the option select
     */
    @Input() public getLabel: (T) => string;
    
    /**
     * These are the options that can be selected to filter on
     */
    @Input() public options: T[];
    
    /**
     * This is an observable of the current filters that is updated with each change
     */
    @Input() public currentFilters: BehaviorSubject<FilterValue<T>[]>;

    /**
     * This is used to display a message that all filters have been removed
     */
    public allFiltersRemoved: boolean = false;

    /**
     * On init, subscribe to current filters and use it to set all filters removed
     */
    public ngOnInit(): void {
        this.currentFilters.subscribe(() => this.allFiltersRemoved = false);
    }

    /**
     * Adds a filter
     */
    public add(): void {
        this.currentFilters.next(this.currentFilters.value.concat([{ option: null, value: '' }]));
        this.allFiltersRemoved = false;
    }

    /**
     * Removes a filter
     */
    public remove(index: number) {
        this.currentFilters.value.splice(index, 1);
        this.currentFilters.next(this.currentFilters.value);
        this.allFiltersRemoved = this.currentFilters.value.length === 0;
    }

    /**
     * Sets the option that the field is filtering on
     */
    public setFilter(filter: FilterValue<T>, id: string) {
        if (!id) {
            filter.option = null;
        } else {
            let [option] = this.options.filter(option => this.getId(option) === id);
            filter.option = option;
        }
    }
}