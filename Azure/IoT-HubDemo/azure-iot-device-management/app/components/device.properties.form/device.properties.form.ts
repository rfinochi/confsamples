/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {ControlGroup, Control, FormBuilder, FORM_DIRECTIVES, FORM_BINDINGS, Validators, NgClass} from '@angular/common';
import {Component, Input, Output, EventEmitter, OnInit, ChangeDetectionStrategy, ViewChild} from '@angular/core';
import {GlobalContext} from '../../core/index';
import {Device, DeviceProperties, Alert, AlertType} from '../../models/index';
import {Observable, Subject} from 'rxjs/Rx';

interface IDevicePropertyMapping {
    key: string;
    label: string; 
}

@Component({
    selector: 'device-properties-form',
    styleUrls: ['dist/app/components/device.properties.form/device.properties.form.css'],
    templateUrl: 'dist/app/components/device.properties.form/device.properties.form.html',
    directives: [NgClass]
})
export class DevicePropertiesForm extends GlobalContext implements OnInit {
    /**
     * The device that we are binding to
     */
    @Input() device: Device;
    
    /**
     * Toggle for expanding/collapsing the view
     */
    public isOpen: boolean = false;

    /**
     * The form of the device properties
     */
    public form: ControlGroup;
    
    public readonlyProperties: IDevicePropertyMapping[];
    
    public writeableProperties: IDevicePropertyMapping[];
    
    public controls: { [key: string]: Control };

    constructor(private formBuilder: FormBuilder) {
        super();

        this.readonlyProperties = [
            { key: 'Manufacturer', label: this.Resources.DeviceProperties.ReadOnly.Manufacturer },
            { key: 'ModelNumber', label: this.Resources.DeviceProperties.ReadOnly.ModelNumber },
            { key: 'SerialNumber', label: this.Resources.DeviceProperties.ReadOnly.SerialNumber },
            { key: 'DeviceDescription', label: this.Resources.DeviceProperties.ReadOnly.Description },
            { key: 'HardwareVersion', label: this.Resources.DeviceProperties.ReadOnly.HardwareVersion },
            { key: 'FirmwareVersion', label: this.Resources.DeviceProperties.ReadOnly.FirmwareVersion },
            { key: 'MemoryFree', label: this.Resources.DeviceProperties.ReadOnly.MemoryFree },
            { key: 'DefaultMinPeriod', label: this.Resources.DeviceProperties.ReadOnly.DefMinPeriod },
            { key: 'DefaultMaxPeriod', label: this.Resources.DeviceProperties.ReadOnly.DefMaxPeriod },
            { key: 'MemoryTotal', label: this.Resources.DeviceProperties.ReadOnly.MemoryTotal },
            { key: 'BatteryLevel', label: this.Resources.DeviceProperties.ReadOnly.BatteryLevel },
            { key: 'BatteryStatus', label: this.Resources.DeviceProperties.ReadOnly.BatteryStatus }
        ];

        this.writeableProperties = [
            { key: 'RegistrationLifetime', label: this.Resources.DeviceProperties.Write.RegLifetime },
            { key: 'CurrentTime', label: this.Resources.DeviceProperties.Write.CurrentTime },
            { key: 'Timezone', label: this.Resources.DeviceProperties.Write.Timezone },
            { key: 'UtcOffset', label: this.Resources.DeviceProperties.Write.UTCOffset }
        ];
        
        this.controls = {};
        
        [].concat(this.readonlyProperties, this.writeableProperties)
            .forEach((prop) => this.controls[prop.key] = new Control(''));

        this.form = this.formBuilder.group(this.controls);
    }

    /**
     * On init, bind our properties
     */
    public ngOnInit() {
        this.ngOnChanges();
    }
    
    /**
     * When our input changes, rebind our textboxes
     */
    public ngOnChanges() {
        // changes have occured; update properties
        Object.keys(this.controls)
            .forEach(this.updateProperty);
    }    

    /**
     * Updates a single property from device properties
     */
    public updateProperty = (property: string) => {
        let value = this.device 
            && this.device.deviceProperties 
            && this.device.deviceProperties[property] 
            && this.device.deviceProperties[property].value
            || '';
        this.controls[property].updateValue(value);
    }
    
    /**
     * Persists changes to form elements into the original device
     */
    public getChanges() {
        var changes = {
            device: null
        };
        
        this.writeableProperties.forEach(property => {
            if (this.controls[property.key].dirty) {
                var deviceProperties = changes.device || {};
                deviceProperties[property.key] = this.controls[property.key].value;
                changes.device = deviceProperties;
            }
        });
        
        return changes;
    }
    
    /**
     * Persists a single property
     */
    private persistProperty = (property: string) => {
        let properties = (this.device.deviceProperties || (this.device.deviceProperties = <any>{}));
        let reference = (properties[property] || (properties[property] = {}));
        let value = this.controls[property].value;
        reference.hasValue = !!value;
        reference.lastUpdatedTime = new Date().toString();
        reference.value = value;
    }
}