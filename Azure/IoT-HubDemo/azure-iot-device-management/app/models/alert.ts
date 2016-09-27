/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/**
 * Model to be used with ng2-bootstrap Alert component
 */
export const DefaultAlertTimeout: number = 30000; // in milliseconds

export class Alert {
    constructor(
        public type: string,
        public msg: string,
        public dismissible: boolean = true,
        public dismissOnTimeout: number = DefaultAlertTimeout
    ) {}
}

export const AlertType = {
    Success: 'success',
    Warning: 'warning',
    Info: 'info',
    Danger: 'danger'
};
