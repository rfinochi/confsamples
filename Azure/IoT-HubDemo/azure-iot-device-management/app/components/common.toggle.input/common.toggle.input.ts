/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, Input} from '@angular/core';
import {GlobalContext} from '../../core/index';

@Component({
    selector: 'toggle-input',
    templateUrl: 'dist/app/components/common.toggle.input/common.toggle.input.html'
})
export class ToggleInput extends GlobalContext {
    @Input() value: boolean;
    @Input() disabled: boolean;

    @Input() onText: string;
    @Input() offText: string;

    constructor() {
        super();
        if (!this.onText) { this.onText = this.Resources.ToggleInput.enabled; }
        if (!this.offText) { this.offText = this.Resources.ToggleInput.disabled; }
    }

    setValue(newValue) {
        if (!this.disabled) {
            this.value = newValue;
        }
    }
}