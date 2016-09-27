/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {ExceptionHandler, Injectable, NgZone} from '@angular/core';
import {GlobalContext} from './globalContext';
import {Resources} from './resources';
import {Alert, AlertType} from '../models/index';

@Injectable()
export class DMExceptionHandler extends ExceptionHandler {
    constructor(public zone: NgZone) {
        super(null, false);
    }
    
    call(error, stackTrace = null, reason = null) {
        console.error(error);
        if (stackTrace) console.error(stackTrace);
        if (reason) console.error(reason);
        this.zone.run(() => {
            GlobalContext.Alerts.next(new Alert(AlertType.Danger, Resources.CriticalError));
        });
    }
}