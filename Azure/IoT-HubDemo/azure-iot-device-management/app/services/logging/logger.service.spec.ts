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

import {LoggerService} from './logger.service';
import {Trace} from '@azure-iot/logging/client';

describe('Logger Service Tests', () => {
    let loggerService: LoggerService;

    beforeEach(() => {
        loggerService = new LoggerService();
        spyOn(loggerService, 'informational');
    });

    it('Should Construct', () => {
        expect(loggerService).toBeDefined();
    });

    it('Should log informational', () => {
        loggerService.informational(new Trace('hello'));
        expect(loggerService.informational).toHaveBeenCalled();
    });
});