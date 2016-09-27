/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Injectable} from '@angular/core';
import {BunyanLogger} from '@azure-iot/logging/client';
import {Common} from '@azure-iot/logging/client';

@Injectable()
export class LoggerService {
    private logger: BunyanLogger;
    private common: Common;
    public constructor() {
        this.logger = new BunyanLogger({ name: 'DMUXLogger', level: 'trace' });
    }

    public emergency = (log: Object) => {
        this.logger.emergency(log);
    };

    public alert = (log: Object) => {
        this.logger.alert(log);
    };

    public critical = (log: Object) => {
        this.logger.critical(log);
    };

    public error = (log: Object) => {
        this.logger.error(log);
    };

    public warning = (log: Object) => {
        this.logger.warning(log);
    };

    public notice = (log: Object) => {
        this.logger.notice(log);
    };

    public informational = (log: Object) => {
        this.logger.informational(log);
    };

    public debug = (log: Object) => {
        this.logger.debug(log);
    };

    private logWithCommon(log: Object, common: Common) {
        return { 'event': JSON.stringify(log), 'common': JSON.stringify(common) };
    }
}