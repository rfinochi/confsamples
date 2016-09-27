/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {provide, Provider} from '@angular/core';
import {DataService} from './data/data.service';
import {ConfigurationService} from './configuration/configuration.service';
import {LoggerService} from './logging/logger.service';

// Export Default Provicders for Dependency Injection in boot.ts
export var SERVICE_PROVIDERS: Array<Provider> = [
    provide(DataService, { useClass: DataService }),
    provide(ConfigurationService, { useClass: ConfigurationService }),
    provide(LoggerService, { useClass: LoggerService })
];

// Export services from one location so they're easy to consume
export {DataService, ConfigurationService, LoggerService};
