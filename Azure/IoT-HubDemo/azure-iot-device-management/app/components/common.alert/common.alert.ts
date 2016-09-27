/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, Input, Output, OnChanges, SimpleChange, EventEmitter} from '@angular/core';
import {Control, CORE_DIRECTIVES} from '@angular/common';

@Component({
    selector: 'alert',
    templateUrl: 'dist/app/components/common.alert/common.alert.html',
    styleUrls: ['dist/app/components/common.alert/common.alert.css'],
    directives: [CORE_DIRECTIVES]
})
export class Alert implements OnChanges {
    @Input() type: string;
    @Input() dismissible: boolean;
    @Input() dismissOnTimeout: number;

    @Output() close: EventEmitter<any>;

    public timer: number;

    constructor() {
        this.close = new EventEmitter();
    }

    public ngOnChanges(changes: {[propName: string]: SimpleChange}) {
        if (changes['dismissOnTimeout'] && this.dismissOnTimeout) {
            this.clearDismissTimer();
            this.timer = setTimeout(() => this.dismiss(), this.dismissOnTimeout);
        }
    }
    
    public ngOnDestroy() {
        this.clearDismissTimer();
    }

    public clearDismissTimer() {
        if (this.timer) {
            clearTimeout(this.timer);
            this.timer = null;
        }
    }

    public dismiss() {
        this.clearDismissTimer();
        this.close.emit(null);
    }
}