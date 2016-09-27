/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {IGridConfiguration} from '../models/index';

/**
 *  Compares 2 grid configurations, returns true if they're equal. 
 *  This is not deep comparison - only works for simple JSON objects with no functions and ordering of properties matters.   
 */
export function gridConfigEquals(gridConfig1: IGridConfiguration, gridConfig2: IGridConfiguration) {
    return JSON.stringify(gridConfig1) === JSON.stringify(gridConfig2);
}