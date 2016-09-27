/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component} from '@angular/core';
import {RouteConfig, ROUTER_DIRECTIVES, ROUTER_PROVIDERS} from '@angular/router-deprecated';
import {GlobalContext} from '../../core/index';
import {ApplicationLoadingBar} from '@azure-iot/common-ux/applicationLoadingBar';
import {DataService} from '../../services/index';

@Component({
    selector: 'app-header',
    templateUrl: 'dist/app/components/application.header/application.header.html',
    styleUrls: ['dist/app/components/application.header/application.header.css'],
    directives: [ROUTER_DIRECTIVES, ApplicationLoadingBar]
})

export class AppHeader extends GlobalContext {
    progress: boolean;

    constructor(public dataService: DataService) {
        super();
    }

    public ngOnInit() {
        this.dataService.requestsInFlight.subscribe(request => {
            if (request > 0) {
                this.progress = true;
            }
            else {
                this.progress = false;
            }
        });
    }
}
