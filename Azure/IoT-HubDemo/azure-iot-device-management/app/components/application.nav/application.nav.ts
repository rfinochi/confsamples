/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component} from '@angular/core';
import {NgClass} from '@angular/common';
import {Router} from '@angular/router-deprecated';
import {GlobalContext} from '../../core/index';

@Component({
    selector: 'app-nav',
    templateUrl: 'dist/app/components/application.nav/application.nav.html',
    styleUrls: ['dist/app/components/application.nav/application.nav.css'],
    directives: [NgClass]
})

export class AppNav extends GlobalContext {
    constructor(private router: Router) {
        super();
    }

    public navigateTo = (route) => {
        this.router.navigate([route]);
    };
}