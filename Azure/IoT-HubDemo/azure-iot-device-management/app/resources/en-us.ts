/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

export var Resources = {
    Common: {
        save: 'Save',
        delete: 'Delete',
        add: 'Add',
        file: 'File',
        edit: 'Edit',
        browse: 'Browse',
        details: 'Details',
        simulatedDeviceTag: 'simulatedDevice',
        simulatedDeviceConfigField: 'simulatedDeviceConfig',
        mustSelectDevices: 'You must select at least one device for this job',
        allJobsScheduled: {
            actionStarted: 'Job for Device has started. Please check ',
            historyHref: 'Job History',
            trackProgress: ' to track progress.'
        },
        partialFailureMessage: 'Attempted to {action} {total} devices. {error} failed. {success} succeeded.',
        ok: 'Ok',
        submit: 'Submit',
        cancel: 'Cancel',
        export: 'Export',
        reboot: 'Reboot',
        firmwareUpdate: 'Firmware Update',
        factoryReset: 'Factory Reset',
        copyToClipboard: 'Use CTRL+C/Cmd+C to copy the hightlighted text to the clipboard',
        tagEnterText: 'Press enter to add a tag',
        connectionStringText: 'Device connection string generated on save',
        autoGenIfBlank: 'Leave blank to generate',
        deviceIdText: 'Enter a custom device id',
        browseForFile: 'Use Browse to select a local csv file and use Upload to submit',
        saveChanges: 'Save Changes'
    },
    DocumentTitle: 'DM - Azure IoT Solutions',
    AppHeader: {
        title: 'Azure IoT Solutions - Device Management'
    },
    Device: {
        titleBulkAdd: 'Add a set of devices (max 100)',
        titleCheckBoxSingleAddAutoGen: 'auto gen',
        titleTextBoxSingleAddDeviceId: 'Device ID',
        titleTextBoxSingleAddPrimaryKey: 'Primary Key',
        titleTextBoxSingleAddSecondaryKey: 'Secondary Key',
        titleTextBoxSingleAddTags: 'Tags',
        titleSimulatedAdd: 'Add a simulated device',
        titleTextBoxSimulatedAddDeviceSpec: 'Device Spec',
        titleTextBoxSimulatedAddCountField: 'Count',
        titleAlertAddDevicesSuccess: 'All devices successfully added from ',
        titleAlertAddDevicesFailure: 'Not all devices were successfully added from ',
        titleAlertAddDevicesStart: 'Starting bulk add of devices',
        titleAlertFileUploadFailure: 'Could not upload file',
        titleAlertFileUploadFolderFailure: 'Cannot add a folder',
        titleAlertFileSizeFailure1: ' exceeds the ',
        titleAlertFileSizeFailure2: 'kb size limit for bulk add',
        titleAlertFileTypeFailure1: ' is of the wrong type. Only files ending with ',
        titleAlertFileTypeFailure2: ' are allowed',
        titleAlertAddDeviceStart: 'Attempting to add device',
        titleAlertAddDeviceSuccess: 'Device successfully added.',
        titleClickHereToEdit: 'Click here to edit',
        titleButtonAddNewDevice: 'Add New Device',
        titleTextBoxBulkAddDevices: 'CSV file',
        titleButtonBulkAdd: 'Upload',
        titleButtonSingleSave: 'Save new device',
        titleStatus: 'Status',
        titleConnectionString: 'Connection String',
        titleAuthentication: 'Authentication',
        titleProperties: 'Properties',
        titlePropertyKeyPlaceholder: 'Property Name',
        titlePropertyValuePlaceholder: 'Property Value',
        titleDeviceProperties: 'Device Properties',
        titleConfiguration: 'Device Configuration',
        titleConfigurationChange: 'Edit Device Configuration',
        warningConfigurationAction: 'Updating the device configuration will start a new job and complete this device editing session. You must wait for this job to complete before performing another edit on this device. You can check the status of the job using the Job History page.'
    },
    DeviceGrid: {
        titleAlertDeviceDeletedSuccess: 'Delete completed',
        titleAlertViewSaved: 'View Saved',
        titleAlertExportSuccess: 'Devices exported succesfully',
        titleAlertEditSuccess: 'Device update successfully scheduled',
        titleAlertEditError: 'Device update could not be scheduled',
        titleAlertGetDeviceError: 'Device cannot be found.',
        titleAlertExportFailed: 'Error exporting devices. Please check Console Log',
        titleAlertTooManyDevicesError: 'Edit is only supported for one device at a time',
        titleEditDevice: 'Edit a Device Twin',
        labelRefreshDevice: 'Refresh',
        findDevicePlaceHolder: 'Find and edit a device',
        titleAlertEditNoChanges: 'Must change at least one property to update a device',
        titleAlertEditNoCanceled: 'Device edit cancelled',
        titleAlertEmptySearchError: 'Invalid Device ID. At least one character is required',
        labelLastUpdated: 'Last Updated',
        labelPoll: 'Poll',
        labelDisplayingDevices: 'Devices ; Displaying'
    },
    HistoryGrid: {
        findJobPlaceHolder: 'Find Job by ID',
        jobFailureAlert: 'Job cannot be found.',
        titleAlertEmptySearchError: 'Invalid Job ID. At least one character is required'
    },
    GridViewSelector: {
        titleInputViewFilter: 'View Filter',
        titleButtonConfigure: 'Configure',
        titleButtonSave: 'Save',
        headingColumns: 'Columns',
        headingFilter: 'Filter',
        captionFilterDropdownSelectColumn: '< Select column >'
    },
    ColumnSelector: {
        titleButtonRemove: 'Remove',
        titleButtonAdd: 'Add'
    },
    FilterEditor: {
        titleButtonAddFilter: 'Add a filter',
        titleButtonDeleteFilter: 'Delete',
        titleButtonApply: 'Apply',
        titleLabelColumn: 'Column',
        titleLabelValue: 'Value',
        titleLabelOperator: 'Operator',
        titleLabelOperatorValue: 'and',
        messageAllFiltersRemoved: 'All filters have been removed. Please Apply to reset data'
    },
    JobDetails: {
        titleJobId: 'Job ID',
        titleStartTime: 'Start Time',
        titleEndTime: 'End Time',
        titleJobType: 'Job Type',
        titleStatus: 'Status',
        titleProgress: 'Progress',
        titleFailureReason: 'Failure Reason',
        titleStatusMessage: 'Status Message',
        titleDeviceId: 'Device ID',
        titleParentJobId: 'Parent Job ID'
    },
    AppNavLabels: {
        deviceGrid: 'Devices',
        device: 'Add a Device',
        historyGrid: 'Job History'
    },
    AppNavRoutes: {
        deviceGrid: 'DeviceGrid',
        device: 'Device',
        historyGrid: 'HistoryGrid'
    },
    GridNavLabels: {
        edit: 'Edit',
        delete: 'Delete',
        export: 'Export',
        reboot: 'Reboot',
        firmwareUpdate: 'Firmware Update',
        factoryReset: 'Factory Reset',
        details: 'Details'
    },
    ModalPrompts: {
        delete: 'Are you sure you want to delete these devices?',
        firmwareUpdate: 'This action will create a job to update firmware on your device(s). Are you sure you want to proceed?',
        export: 'Are you sure you want to export these devices?',
        factoryReset: 'This action will create a job to factory reset your device(s). Are you sure you want to proceed?',
        reboot: 'This action will create a job to reboot your device(s). Are you sure you want to proceed?'
    },
    FirmwareUpdateLabels: {
        packageUrl: 'Package URI',
        timeoutInMin: 'Timeout (in minutes)'
    },
    FirmwareUpdateProperties: {
        packageUrl: 'packageUrl',
        timeoutInMin: 'timeOutInMin'
    },
    DeviceDevicePropertiesForm: {
        titleAlertRefreshSuccess: 'Device properties up to date',
        titleAlertRefreshFailure: 'Device properties could not be updated'
    },
    ToggleInput: {
        enabled: 'Enabled',
        disabled: 'Disabled'
    },
    Yes: 'Yes',
    No: 'No',
    ConfirmDeleteTitle: 'Delete',
    ConfirmFirmwareUpdateDevices: 'Update Firmware',
    ConfirmRebootDevices: 'Reboot',
    ConfirmFactoryResetDevices: 'Factory Reset',
    ConfirmExportDevices: 'Export Devices',
    LabelKeyValueName: 'Name',
    LabelKeyValueValue: 'Value',
    CriticalError: 'Unexpected error in application; check configuration and restart',
    ExampleFile: '(Example)',
    AddTag: 'Add Tag',
    LabelJobs: 'Device Jobs:',
    LabelSelectJob: '<Select a Job>',
    LabelExecuteJob: 'Start',
    DeviceBaseProperties: {
        DeviceID: 'Device ID',
        Status: 'Status',
        Tags: 'Tags',
        ConnectionState: 'Connection State'
    },
    DeviceProperties: {
        ReadOnly: {
            Manufacturer: 'Manufacturer',
            ModelNumber: 'Model Number',
            SerialNumber: 'Serial Number',
            Description: 'Description',
            HardwareVersion: 'Hardware Version',
            FirmwareVersion: 'Firmware Version',
            MemoryFree: 'Memory Free',
            DefMinPeriod: 'Def Min Period',
            DefMaxPeriod: 'Def Max Period',
            MemoryTotal: 'Memory Total',
            BatteryLevel: 'Battery Level',
            BatteryStatus: 'Battery Status'
        },
        Write: {
            RegLifetime: 'Reg Lifetime',
            CurrentTime: 'Current Time',
            Timezone: 'Timezone',
            UTCOffset: 'UTC Offset'
        }
    },
    JobProperties: {
        StartTime: 'Start Time',
        ActionID: 'Job ID',
        Status: 'Status',
        DeviceID: 'Device ID',
        EndTime: 'End Time',
        Type: 'Type',
        FailureReason: 'Failure Reason',
        StatusMessage: 'Status Message',
        ParentActionID: 'Parent Job ID',
        ParentChild: 'Parent/Child',
        IsParent: 'Parent',
        IsChild: 'Child'
    }
};
