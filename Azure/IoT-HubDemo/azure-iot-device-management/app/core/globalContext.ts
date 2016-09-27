/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Resources} from './resources';
import {GridBodyPresentation, PresentationOnBreakpoint} from '@azure-iot/common-ux/grid';
import {BehaviorSubject} from 'rxjs/Rx';
import {Alert} from '../models/index';

/**
 * This is the presentation changes for both grid
 */
const gridBreakpoints = PresentationOnBreakpoint([
    {
        breakpoint: 0,
        presentation: GridBodyPresentation.List
    },
    {
        breakpoint: 480,
        presentation: GridBodyPresentation.Rows
    }
]);

// Base class that allows for all of our components and thus
// their templates to access string resources in an ergonomic way
export class GlobalContext {
    public static Alerts: BehaviorSubject<Alert> = new BehaviorSubject<Alert>(null);
    
    public get alert(): Alert {
        return GlobalContext.Alerts.value;
    }
    
    public set alert(newValue: Alert) {
        GlobalContext.Alerts.next(newValue);
    }
    
    public Resources = Resources;
    
    public GridBreakpoints = gridBreakpoints;
}