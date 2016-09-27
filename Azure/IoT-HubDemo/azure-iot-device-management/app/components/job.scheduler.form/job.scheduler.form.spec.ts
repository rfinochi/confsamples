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

import {JobSchedulerForm} from './job.scheduler.form';
import {FormBuilder} from '@angular/common';

describe('JobSchedulerForm Tests', () => {
    beforeEachProviders(() => [
        FormBuilder
    ]);
    var jobForm: JobSchedulerForm;
    var fakeItems: any[] = [
        {
            label: 'Id',
            id: 'jobId',
            required: true
        },
        {
            label: 'Value',
            id: 'jobValue',
            required: false,
            defaultValue: 10
        }
    ];
    var fakeFormBuilder = {
        group: jasmine.createSpy('group')
    };
    var fakeValues = {
        'Id': 'new id',
        'Value': 'new value'
    };
        
    beforeEach(() => {
        jobForm = new JobSchedulerForm(<any>fakeFormBuilder);
        jobForm.items = fakeItems;
    });
    
    it('Should Construct', () => {
        expect(jobForm).toBeDefined();
    });
    
    it('on init, instantiates the form trying to construct a group of @angular controls', () => {
        jobForm.ngOnInit();
        expect(fakeFormBuilder.group).toHaveBeenCalled();
    });
    
    it('on submit, emits event with params', (done) => {
        jobForm.form = <any>{
            value: fakeValues
        };
        
        jobForm.submit.subscribe(next => {
            expect(next['jobId']).toEqual(fakeValues['Id']);
            expect(next['jobValue']).toEqual(fakeValues['Value']);
            done();
        });
        
        jobForm.onClickSubmit();
    });
    
    it('on cancel, emits event', (done) => {
        jobForm.cancel.subscribe(next => {
            done();
        });
        jobForm.onClickCancel();
    });
});
