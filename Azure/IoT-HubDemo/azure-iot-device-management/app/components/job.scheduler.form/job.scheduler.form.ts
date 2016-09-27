/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, Output, EventEmitter, Input} from '@angular/core';
import {NgClass, FormBuilder, Control, FORM_DIRECTIVES, FORM_BINDINGS, Validators, ControlGroup} from '@angular/common';
import {GlobalContext} from '../../core/index';

@Component({
    selector: 'jobscheduler-form',
    templateUrl: 'dist/app/components/job.scheduler.form/job.scheduler.form.html',
    styleUrls: ['dist/app/components/job.scheduler.form/job.scheduler.form.css'],
    directives: [FORM_DIRECTIVES],
    viewBindings: [FORM_BINDINGS]
})

export class JobSchedulerForm extends GlobalContext {
    
    /**
    * This is the array of formitems 
    */
    @Input() public items: { label: string, id: string, required: boolean, defaultValue: any }[];
       
    /**
    * When form is submitted a submit event is send
    */
    @Output() public submit: EventEmitter<Object> = new EventEmitter();
    
    /**
    * When form is cancelled a cancel event is send
    */
    @Output() public cancel: EventEmitter<any> = new EventEmitter();

    public form: ControlGroup;
    
    private isValid: boolean = false;
    
    constructor(private formBuilder: FormBuilder) {
        super();
    }

    public ngOnInit() {
        let controls = {};
        for (let item of this.items) {
            if (item.required === false)
                controls[item.label] = new Control('');
            else
                controls[item.label] = new Control('', Validators.required);
        }
        this.form = this.formBuilder.group(controls);
    }

    public onClickSubmit = () => {
        let params = {};
        for (let item of this.items) {
            params[item.id] = this.form.value[item.label] ? this.form.value[item.label] : item.defaultValue;
        }
        this.submit.emit(params);
    };

    public onClickCancel = () => {
        this.cancel.emit(null);
    };
}