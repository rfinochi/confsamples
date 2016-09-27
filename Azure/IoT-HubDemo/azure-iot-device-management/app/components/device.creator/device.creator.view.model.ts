/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {ControlGroup} from '@angular/common';
import {FileUploader} from 'ng2-file-upload';

export class DeviceViewModel {
    singleAddForm: ControlGroup;
    singleAddTags: string[];
    singleAddIsReadonly: boolean;
    singleAddEnabledDeviceTypes: string[];
    singleAddUrl: string;
    bulkAddFileUploader: FileUploader;
    bulkAddFileName: string;
    bulkAddIsDisabled: boolean;
    bulkAddIsLoading: boolean;
    bulkAddIsFileValid: boolean;
}