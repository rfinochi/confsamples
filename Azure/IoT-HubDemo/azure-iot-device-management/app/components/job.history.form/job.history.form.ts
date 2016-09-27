/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, Input} from '@angular/core';
import {NgForm} from '@angular/common';
import {Job} from '../../models/index';
import {GlobalContext} from '../../core/index';

@Component({
    selector: 'history-form',
    templateUrl: 'dist/app/components/job.history.form/job.history.form.html',
    styleUrls: ['dist/app/components/job.history.form/job.history.form.css']
})

export class HistoryForm extends GlobalContext {
    @Input() public job: Job;
    
    constructor() {
        super();        
    } 
}
