/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {
beforeEach,
beforeEachProviders,
describe,
expect,
it,
inject,
injectAsync
} from '@angular/core/testing';

import {Alert} from './common.alert';

describe('Alert Tests', () => {
    
    let alert: Alert;
    
    beforeEach(() => {
        alert = new Alert();
    });

    it('should construct', () => {
        expect(alert).toBeDefined();
    });

    it('should reset the dismiss event when the dismissOnTimeout input changes', () => {
        spyOn(window, 'setTimeout').and.returnValue('test');
        spyOn(alert, 'clearDismissTimer').and.returnValue('test');

        alert.dismissOnTimeout = 123;
        alert.ngOnChanges(<any>{ dismissOnTimeout: {} });

        expect(alert.clearDismissTimer).toHaveBeenCalled();
        expect(window.setTimeout).toHaveBeenCalled();
        expect(alert.timer).toBe('test');
    });

    it('should not reset the dismiss event when the dismissOnTimeout input doesn\'t change', () => {
        spyOn(window, 'setTimeout').and.returnValue('test');
        spyOn(alert, 'clearDismissTimer').and.returnValue('test');

        alert.ngOnChanges({});

        expect(alert.clearDismissTimer).not.toHaveBeenCalled();
        expect(window.setTimeout).not.toHaveBeenCalled();
        expect(alert.timer).toBeUndefined();
    });

    it('should not cancel dismiss timeout if there isn\'t a timer running', () => {
        spyOn(window, 'clearTimeout');

        alert.clearDismissTimer();

        expect(window.clearTimeout).not.toHaveBeenCalled();
        expect(alert.timer).toBeUndefined();
    });

    it('should cancel dismiss timeout if there is a timer running', () => {
        spyOn(window, 'clearTimeout');

        alert.timer = 123;

        alert.clearDismissTimer();

        expect(window.clearTimeout).toHaveBeenCalledWith(123);
        expect(alert.timer).toBe(null);
    });

    it('dismiss event should clear the dismiss timer in case the dismiss was from user action', () => {
        spyOn(alert, 'clearDismissTimer');
        spyOn(alert.close, 'emit');

        alert.dismiss();

        expect(alert.clearDismissTimer).toHaveBeenCalled();
        expect(alert.close.emit).toHaveBeenCalled();
    });
});