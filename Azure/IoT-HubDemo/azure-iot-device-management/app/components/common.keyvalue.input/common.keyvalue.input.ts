/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {GlobalContext} from '../../core/index';
import {Component, Input, OnInit, OnChanges, SimpleChange} from '@angular/core';
import {ControlGroup, Control, FormBuilder, FORM_DIRECTIVES, CORE_DIRECTIVES} from '@angular/common';

@Component({
    selector: 'key-value-input',
    templateUrl: 'dist/app/components/common.keyvalue.input/common.keyvalue.input.html',
    styleUrls: ['dist/app/components/common.keyvalue.input/common.keyvalue.input.css'],
    directives: [FORM_DIRECTIVES, CORE_DIRECTIVES]
})
export class KeyValueInput extends GlobalContext implements OnInit, OnChanges {
    @Input() keyPlaceholder: string;
    @Input() valuePlaceholder: string;
    @Input() disabled: boolean;
    @Input() keyValueMap: { [key: string]: string};
    
    public form: ControlGroup;

    constructor(private formBuilder: FormBuilder) {
        super();
    }

    public ngOnInit() {
        this.form = this.formBuilder.group({
            'key': new Control(),
            'value': new Control()
        });
    }
    
    public ngOnChanges(changes: {[propName: string]: SimpleChange}) {
        if (!this.keyValueMap) {
            this.keyValueMap = {};
        }
    }

    public keys(): string[] {
        return Object.keys(this.keyValueMap);
    }

    public addPair() {
        let keyControl = this.keyControl(),
            valueControl = this.valueControl(),
            key = keyControl.value,
            value = valueControl.value;

        if (key) {
            this.keyValueMap[key] = value;
        }

        keyControl.updateValue(null);
        valueControl.updateValue(null);
    }

    public removePair(key: string, e: MouseEvent) {
        e.preventDefault();
        delete this.keyValueMap[key];
    }

    public keyControl(): Control {
        return <Control>this.form.controls['key'];
    }

    public valueControl(): Control {
        return <Control>this.form.controls['value'];
    }
}