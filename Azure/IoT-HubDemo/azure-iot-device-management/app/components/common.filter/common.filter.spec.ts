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

import {BehaviorSubject} from 'rxjs/Rx';
import {FilterEditor, FilterValue} from './common.filter';
import {IGridFilter} from '../../models/index';


describe('Column Filter Tests', () => {

    let deviceFilter = {
        option: {
            in: ['deviceId1'],
            isArray: false,
            key: 'deviceId',
            model: 'default',
            name: 'Device ID'
        },
        value: 'deviceId1'
    };

    let tagFilter = {
        option: {
            in: ['tag1'],
            isArray: true,
            key: 'tags',
            model: 'service',
            name: 'Tags'
        },
        value: 'tag1'
    };

    let tagFilter2 = {
        option: {
            in: ['tag2'],
            isArray: true,
            key: 'tags',
            model: 'service',
            name: 'Tags'
        },
        value: 'tag2'
    };

    let filter: FilterEditor<Object>;

    beforeEach(() => {
        filter = new FilterEditor();
        filter.currentFilters = new BehaviorSubject([deviceFilter, tagFilter]);
        filter.getId = function (column: IGridFilter) {
            return column.model + '.' + column.key;
        };
    });

    it('Should Construct', () => {
        expect(filter).toBeDefined();
    });

    it('Should subscribe to filters on ngOnInit', () => {
        spyOn(filter.currentFilters, 'subscribe');
        filter.ngOnInit();
        expect(filter.currentFilters.subscribe).toHaveBeenCalled();
    });

    it('Should set allFiltersRemoved to false when current filters change', () => {
        filter.currentFilters.next([deviceFilter]);
        expect(filter.allFiltersRemoved).toEqual(false);
        filter.currentFilters.next([deviceFilter, tagFilter]);
    });

    it('Should add', () => {
        spyOn(filter.currentFilters, 'next');
        filter.add();
        expect(filter.currentFilters.next).toHaveBeenCalled();
        expect(filter.allFiltersRemoved).toEqual(false);
    });

    it('Should remove', () => {
        spyOn(filter.currentFilters, 'next');
        let length = filter.currentFilters.value.length;
        filter.remove(0);
        expect(filter.currentFilters.value.length).toEqual(length - 1);
        expect(filter.currentFilters.value[0].option['key']).not.toEqual('deviceId');
        expect(filter.currentFilters.value[0].option['key']).toEqual('tags');
        expect(filter.currentFilters.next).toHaveBeenCalledWith(filter.currentFilters.value);
        expect(filter.allFiltersRemoved).toEqual(false);
    });

    it('Should set filter with null id', () => {
        filter.setFilter(tagFilter2, null);
        expect(tagFilter2.option).toEqual(null);
        expect(filter).toBeDefined();
    });

    it('Should set filter with id', () => {
        filter.options = [{
            in: ['tag2'],
            isArray: true,
            key: 'tags',
            model: 'service',
            name: 'Tags'
        }];
        filter.setFilter(tagFilter2, 'service.tags');
        expect(tagFilter2.option).toBeDefined(filter.options[0]);
    });
});
