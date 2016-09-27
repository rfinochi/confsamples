/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {ControlGroup, Control, FormBuilder, FORM_DIRECTIVES, FORM_BINDINGS, Validators} from '@angular/common';
import {Component, Input, Output, EventEmitter, OnInit, ChangeDetectionStrategy, ViewChild, OnChanges, SimpleChange} from '@angular/core';
import {GlobalContext, clone} from '../../core/index';
import {Device} from '../../models/index';
import {TagInput} from '../common.tag.input/common.tag.input';
import {KeyValueInput} from '../common.keyvalue.input/common.keyvalue.input';
import {ToggleInput} from '../common.toggle.input/common.toggle.input';

@Component({
    selector: 'service-properties-form',
    styleUrls: ['dist/app/components/device.service.properties.form/device.service.properties.form.css'],
    templateUrl: 'dist/app/components/device.service.properties.form/device.service.properties.form.html',
    directives: [TagInput, KeyValueInput, ToggleInput]
})
export class ServicePropertiesForm extends GlobalContext implements OnInit, OnChanges {
    @Input() device: Device;
    @Input() title: string;
    @Input() deviceConnectionString: string;

    @ViewChild(TagInput) tagControl: TagInput;
    @ViewChild(KeyValueInput) propertiesValueControl: KeyValueInput;
    @ViewChild(ToggleInput) enabledControl: ToggleInput;

    public form: ControlGroup;
    public isReadonly: boolean;
    public isOpen: boolean;

    constructor(private formBuilder: FormBuilder) {
        super();

        this.isReadonly = false;
        this.isOpen = true;
    }

    public ngOnInit() {
        this.form = this.formBuilder.group({
            'deviceId': new Control(this.deviceId()),
            'connectionString': new Control(this.connectionString()),
            'primaryKey': new Control(this.primaryKey()),
            'secondaryKey': new Control(this.secondaryKey())
        });
    }

    public ngOnChanges(changes: {[propName: string]: SimpleChange}) {
        // update device form info
        if (changes['device']) {
            let oldDevice = changes['device'].previousValue; 

            if (oldDevice) {
                // truthy -> truthy = updated device (including before ngOnInit) 
                if (this.device) {
                    // keep our own copy
                    this.device = clone(this.device);
                    return;
                }

                // truthy device -> falsy device = resetting add device form
                if (this.form) {
                    this.deviceConnectionString = null;
                    this.enabledControl.value = this.enabled();
                    this.deviceIdControl().updateValue(this.deviceId());
                    this.connectionStringControl().updateValue(this.connectionString());
                    this.primaryKeyControl().updateValue(this.primaryKey());
                    this.secondaryKeyControl().updateValue(this.secondaryKey());

                    this.isReadonly = false;
                }
            } else {
                // falsy device -> truthy device = going from add form to readonly version
                if (this.device) {
                    this.enabledControl.value = this.enabled();
                    this.deviceIdControl().updateValue(this.deviceId());
                    this.connectionStringControl().updateValue(this.connectionString());
                    this.primaryKeyControl().updateValue(this.primaryKey());
                    this.secondaryKeyControl().updateValue(this.secondaryKey());
                    this.isReadonly = true;
                }
            }
        }
    }

    public toDevice(): Device {
        let device = this.device ? clone(this.device) : new Device();

        device.deviceId = this.deviceIdControl().value;
        device.authentication.symmetricKey.primaryKey = this.primaryKeyControl().value;
        device.authentication.symmetricKey.secondaryKey = this.secondaryKeyControl().value;

        device.serviceProperties.properties = clone(this.propertiesValueControl.keyValueMap);

        device.serviceProperties.tags = this.tagControl.toTags();

        device.status = this.enabledControl.value ? 'enabled' : 'disabled'; 

        return device;
    }

    public deviceIdControl(): Control {
        return <Control>this.form.controls['deviceId'];
    }

    public primaryKeyControl(): Control {
        return <Control>this.form.controls['primaryKey'];
    }

    public secondaryKeyControl(): Control {
        return <Control>this.form.controls['secondaryKey'];
    }

    public connectionStringControl(): Control {
        return <Control>this.form.controls['connectionString'];
    }
    
    public deviceId(): string {
        return this.device ? this.device.deviceId || '' : '';
    }

    public primaryKey(): string {
        return this.device 
            && this.device.authentication
            && this.device.authentication.symmetricKey ? this.device.authentication.symmetricKey.primaryKey || '' : '';
    }

    public secondaryKey(): string {
        return this.device 
            && this.device.authentication
            && this.device.authentication.symmetricKey ? this.device.authentication.symmetricKey.secondaryKey || '' : '';
    }

    public tags(): string[] {
        return this.device 
            && this.device.serviceProperties ? this.device.serviceProperties.tags || null : null;
    }

    public properties(): Object {
        return this.device 
            && this.device.serviceProperties ? this.device.serviceProperties.properties || null : null;
    }

    public enabled(): boolean {
        // default new devices to be enabled 
        return this.device ? this.device.status.toLowerCase() === 'enabled' : true;
    }

    public connectionString(): string {
        return this.deviceConnectionString || '';
    }

    public copyToClipboard(text): void {
        window.prompt(this.Resources.Common.copyToClipboard, text);
    }
}