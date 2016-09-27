/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {HalButton} from '../../models/index';
import {Resources} from '../../core/index';

export var GridNavConfigurationJobActions = {
    GridNavButtons: [
        new HalButton(Resources.GridNavLabels.details, null, 'jobs:get', 'GET')
    ]
};