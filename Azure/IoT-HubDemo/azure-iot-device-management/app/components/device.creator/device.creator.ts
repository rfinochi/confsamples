/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, OnInit, ViewChild} from '@angular/core';
import {Http, Response, Headers} from '@angular/http';
import {FormBuilder, Control, FORM_BINDINGS, FORM_DIRECTIVES, Validators, CORE_DIRECTIVES, NgStyle, NgClass} from '@angular/common';
import {DeviceViewModel} from './device.creator.view.model';
import {AppNav} from '../application.nav/application.nav';
import {GlobalContext} from '../../core/index';
import {DataService} from '../../services/index';
import {HalButton, Device as DeviceModel, Alert, AlertType, DefaultAlertTimeout, HalLinks, HalResponse} from '../../models/index';
import {FILE_UPLOAD_DIRECTIVES, FileUploader} from 'ng2-file-upload';
import {Observable} from 'rxjs/Rx';
import {TagInput} from '../common.tag.input/common.tag.input';
import {ServicePropertiesForm} from '../device.service.properties.form/device.service.properties.form';
import {Alert as AlertView} from '../common.alert/common.alert';

/**
 * Create the example CSV data uri once
 */
let exampleBlob = new Blob([
`deviceId, status, serviceProperties.properties.EXAMPLEPROPERTY, serviceProperties.tags, authentication.symmetricKey.primaryKey, authentication.symmetricKey.secondaryKey
EXAMPLEID, disabled, EXAMPLEVALUE, [EXAMPLETAG;EXAMPLETAG1;EXAMPLETAG2;EXAMPLETAG3], , , `
], { type: 'text/csv' });
let exampleUrl = window.URL.createObjectURL(exampleBlob);

/**
 * 1 Kilobyte in Bytes
 * @type {Number}
 */
export const KB = 1024;

/**
 * Anything over 10kb should be comfortably over our 100 device bulk add limit
 * @type {Number}
 */
export const MAX_FILE_SIZE_BYTES = 10 * KB; // 10kb

/**
 * Currently only accepting .csv files 
 * @type {Array<String>}
 */
export const ACCEPTED_FILE_EXTENSIONS = ['.csv'];

@Component({
    selector: 'device-add',
    templateUrl: 'dist/app/components/device.creator/device.creator.html',
    styleUrls: ['dist/app/components/device.creator/device.creator.css'],
    viewBindings: [FORM_BINDINGS],
    directives: [FORM_DIRECTIVES, AppNav, TagInput, FILE_UPLOAD_DIRECTIVES, CORE_DIRECTIVES, NgClass, NgStyle, AlertView, ServicePropertiesForm]
})
export class Device extends GlobalContext implements OnInit {

    public model: DeviceViewModel;

    private currentValidRels: HalLinks;

    public creationAlert: Alert = null;
    public bulkAlert: Alert = null;
    public alert: Alert = null;
    
    public exampleCSVurl = exampleUrl;

    public createdDevice: DeviceModel;
    public deviceConnectionString: string;

    @ViewChild(ServicePropertiesForm) servicePropertiesForm: ServicePropertiesForm;

    constructor(private formBuilder: FormBuilder, private dataService: DataService) {
        super();

        this.model = new DeviceViewModel();

        this.model.bulkAddFileName = '';
        this.model.bulkAddFileUploader = null;
        this.model.bulkAddIsDisabled = true;
        this.model.bulkAddIsLoading = true;
        this.model.bulkAddIsFileValid = false;
    }

    public ngOnInit() {
        this.resetAlerts();

        this.dataService.discovery()
            .subscribe((val: HalLinks) => {

                // as i've come here to do 'device:new' i need to give the hateoas Api  
                // the follow on set of rels so it can do the lookup of the next 
                // state's href. for us this we are expecting to have 'devices:add'

                this.dataService.hateoasApi('devices:new', 'GET', null)
                    .subscribe((innerVal: HalResponse<any>) => {
                        this.currentValidRels = innerVal.links;
                    });

                // this time we are just using the href of known rel 'devices:newBulk' which
                // is a direct POST of the body.

                let urlForBulk = val['devices:newBulk']['href'];

                if (!this.model.bulkAddFileUploader) {
                    let fileUploader = new FileUploader({ url: urlForBulk });
                                        
                    fileUploader.onSuccessItem = this.onBulkAddSuccess;
                    fileUploader.onErrorItem = this.onBulkAddError;
                    fileUploader.onWhenAddingFileFailed = this.onAddingBulkAddFileFailed;
                    fileUploader.onAfterAddingFile = this.onAfterAddingBulkAddFile;
                    fileUploader.onBeforeUploadItem = this.onBeforeUploadingBulkAddFile;

                    // TODO - ng2-file-upload: Issue #220: Can't upload same file twice 
                    this.model.bulkAddFileUploader = fileUploader;

                    this.bulkAddLoaded();
                }
            });
    }

    public addDevices() {
        this.model.bulkAddFileUploader.uploadAll();
        this.bulkAddLoading();

        this.resetAlerts();
    }

    public updateBulkAdd() {
        if (this.model.bulkAddIsLoading || !this.model.bulkAddIsFileValid) {
            this.bulkAddDisabled();
        } else {
            this.bulkAddEnabled();
        }
    }

    public bulkAddEnabled() {
        this.model.bulkAddIsDisabled = false;
    }

    public bulkAddDisabled() {
        this.model.bulkAddIsDisabled = true;
    }

    public bulkAddLoading() {
        this.model.bulkAddIsLoading = true;
        this.updateBulkAdd();
    }

    public bulkAddLoaded() {
        this.model.bulkAddIsLoading = false;
        this.updateBulkAdd();
    }

    public bulkAddValidFile() {
        this.model.bulkAddIsFileValid = true;
        this.updateBulkAdd();
    }

    public bulkAddInvalidFile() {
        this.model.bulkAddIsFileValid = false;
        this.updateBulkAdd();
    }

    public alertBulkAddFileTypeError(fileName: string) {
        this.resetAlerts();

        let msg = this.Resources.Common.file
            + ' '
            + fileName
            + this.Resources.Device.titleAlertFileTypeFailure1
            + ACCEPTED_FILE_EXTENSIONS.toString()
            + this.Resources.Device.titleAlertFileTypeFailure2;
        this.bulkAlert = new Alert(AlertType.Danger, msg);
        this.bulkAddInvalidFile();
    }

    public alertBulkAddFileSizeError(fileName: string) {
        this.resetAlerts();

        let msg = this.Resources.Common.file
            + ' '
            + fileName
            + this.Resources.Device.titleAlertFileSizeFailure1
            + (MAX_FILE_SIZE_BYTES / KB)
            + this.Resources.Device.titleAlertFileSizeFailure2;
        this.bulkAlert = new Alert(AlertType.Danger, msg);
        this.bulkAddInvalidFile();
    }

    public onBulkAddSuccess = (item: any, response: any, status: any, headers: any) => {
        this.resetAlerts();
        this.dataService.markRequestEnd();

        let res = JSON.parse(response);

        if (res._import.error > 0) {
            let msg = this.Resources.Common.partialFailureMessage
                .replace('{action}', this.Resources.Common.add.toLowerCase())
                .replace('{total}', res._import.total)
                .replace('{error}', res._import.error)
                .replace('{success}', res._import.success);
            this.bulkAlert = new Alert(AlertType.Danger, msg);
        } else {
            let msg = this.Resources.Device.titleAlertAddDevicesSuccess + item.file.name;
            this.bulkAlert = new Alert(AlertType.Success, msg);
        }
        
        this.model.bulkAddFileName = '';

        this.bulkAddInvalidFile();
        this.bulkAddLoaded();
    };

    public onBulkAddError = (item: any, response: any, status: any, headers: any) => {
        this.resetAlerts();
        this.dataService.markRequestEnd();

        let msg: string = JSON.parse(response)._error.message;

        this.bulkAlert = new Alert(AlertType.Danger, msg);

        this.model.bulkAddFileName = '';

        this.bulkAddInvalidFile();
        this.bulkAddLoaded();
    };

    public onAddingBulkAddFileFailed = (item: any, filter: any, options: any) => {
        this.resetAlerts();

        if (filter.name === 'queueLimit') {
            this.model.bulkAddFileUploader.clearQueue();
            this.model.bulkAddFileUploader.addToQueue([item], null, null);
        } else if (filter.name === 'folder') {
            this.bulkAlert = new Alert(AlertType.Danger, this.Resources.Device.titleAlertFileUploadFolderFailure);
            this.bulkAddInvalidFile();
        } else {
            this.bulkAlert = new Alert(AlertType.Danger, this.Resources.Device.titleAlertFileUploadFailure);
            this.bulkAddInvalidFile();
        }
    };

    public onAfterAddingBulkAddFile = (item: any) => {
        this.resetAlerts();

        let fileSize: number = item.file.size,
            fileName: string = item.file.name;

        this.model.bulkAddFileName = fileName;

        if (fileSize > MAX_FILE_SIZE_BYTES) {
            this.alertBulkAddFileSizeError(fileName);
            return;
        }

        let validExt = false;

        for (let ext of ACCEPTED_FILE_EXTENSIONS) {
            if (fileName.endsWith(ext)) {
                validExt = true;
                break;
            }
        }

        if (!validExt) {
            this.alertBulkAddFileTypeError(fileName);
            return;
        }

        this.bulkAddValidFile();
    };

    public onBeforeUploadingBulkAddFile = () => {
        this.dataService.markRequestStart();
    };

    public addDevice() {
        let device = this.servicePropertiesForm.toDevice();

        this.dataService.addDevice(device, this.currentValidRels)
            .subscribe(this.onSingleAddSuccess, this.onSingleAddError);
    }

    public onSingleAddSuccess = (res: any) => {
        this.resetAlerts();
        this.creationAlert = new Alert(AlertType.Success, this.Resources.Device.titleAlertAddDeviceSuccess, true, null);

        this.createdDevice = res.data;
        this.deviceConnectionString = this.createdDevice['_deviceConnectionString'];

        // make sure we don't leak this later
        delete this.createdDevice['_deviceConnectionString'];
    };

    public onSingleAddError = (res: any) => {
        this.resetAlerts();

        let halResponse: HalResponse<any> = res.json();
        this.alert = new Alert(AlertType.Danger, halResponse._error.message);
    };

    public resetAddDeviceForm() {
        this.resetAlerts();
        this.createdDevice = null;
    }
    
    public resetAlerts() {
        this.alert = this.bulkAlert = this.creationAlert = null;
    }

    public addFormValid = (): boolean => {
        return this.servicePropertiesForm
            && !!this.servicePropertiesForm.deviceIdControl().value;
    }
}
