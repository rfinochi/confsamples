/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {bootstrap} from '@angular/platform-browser-dynamic';
import {ROUTER_PROVIDERS} from '@angular/router-deprecated';
import {LocationStrategy, HashLocationStrategy} from '@angular/common';
import {HTTP_PROVIDERS} from '@angular/http';
import {Application} from './components/application/application';
import {provide, enableProdMode, ExceptionHandler} from '@angular/core';
import {SERVICE_PROVIDERS} from './services/index';
import {Resources, DMExceptionHandler} from './core/index';

enableProdMode();

document.title = Resources.DocumentTitle;

bootstrap(
    Application,
    [
        ROUTER_PROVIDERS,
        HTTP_PROVIDERS,
        SERVICE_PROVIDERS,
        provide(LocationStrategy, { useClass: HashLocationStrategy }),
        provide(ExceptionHandler, { useClass: DMExceptionHandler })
    ]
);