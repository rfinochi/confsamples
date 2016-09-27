/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Button, HalButton} from '../../models/index';
import {Resources} from '../../core/index';

export var GridNavConfigurationDevice = {
    GridNavButtons: [
        new HalButton(Resources.GridNavLabels.edit, null, 'devices:edit', 'POST'),
        new HalButton(Resources.GridNavLabels.delete, null, 'devices:delete', 'POST'),
        new HalButton(Resources.GridNavLabels.export, null, 'devices:export', 'POST')
    ]
};

export var GridNavConfigurationDeviceActions = {
    GridNavButtons: [
        new HalButton(Resources.GridNavLabels.reboot, null, 'jobs:jobReboot', 'POST'),
        new HalButton(Resources.GridNavLabels.firmwareUpdate, null, 'jobs:jobFirmware', 'POST'),
        new HalButton(Resources.GridNavLabels.factoryReset, null, 'jobs:jobReset', 'POST')
    ]
};