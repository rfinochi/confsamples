/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component} from '@angular/core';
import {RouteConfig, RouterLink, ROUTER_DIRECTIVES, ROUTER_PROVIDERS} from '@angular/router-deprecated';
import {AppHeader} from '../application.header/application.header';
import {DeviceGrid} from '../device.grid/device.grid';
import {Device} from '../device.creator/device.creator';
import {HistoryGrid} from '../job.history.grid/job.history.grid';

@Component({
    selector: 'application',
    template: `<app-header></app-header>
               <router-outlet></router-outlet>`,

    directives: [AppHeader, RouterLink, ROUTER_DIRECTIVES]
})

@RouteConfig([
    { path: '/', name: 'DeviceGrid', component: DeviceGrid, useAsDefault: true },
    { path: '/edit/:deviceId', name: 'Edit', component: DeviceGrid },
    { path: '/job/:jobId', name: 'Job', component: HistoryGrid },
    { path: '/device', name: 'Device', component: Device },
    { path: '/history', name: 'HistoryGrid', component: HistoryGrid }
])

export class Application {

}
