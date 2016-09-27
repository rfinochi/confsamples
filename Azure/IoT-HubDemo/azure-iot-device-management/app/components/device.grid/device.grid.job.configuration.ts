/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Resources} from '../../core/index';

export var JobConfiguration = {};

JobConfiguration['jobs:jobFirmware'] = [
    { label: Resources.FirmwareUpdateLabels.packageUrl, id: Resources.FirmwareUpdateProperties.packageUrl, required: false },
    { label: Resources.FirmwareUpdateLabels.timeoutInMin, id: Resources.FirmwareUpdateProperties.timeoutInMin, required: false, defaultValue: 10 }
];
