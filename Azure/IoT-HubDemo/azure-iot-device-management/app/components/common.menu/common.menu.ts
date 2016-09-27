/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, Output, EventEmitter, Input} from '@angular/core';
import {NgClass} from '@angular/common';
import {Button, HalButton, HalLinks} from '../../models/index';
import {DataService} from '../../services/index';

@Component({
    selector: 'common-menu',
    templateUrl: 'dist/app/components/common.menu/common.menu.html',
    styleUrls: ['dist/app/components/common.menu/common.menu.css'],
    directives: [NgClass]
})

// refactor:  this should be renamed to hal-common-menu
export class CommonMenu {
    /**
     * This is the array (key-value pairs) of titles of nav buttons 
     */
    @Input() public buttons: HalButton<any>[];

    /**
     * The rels that drive what buttons are enabled
     */
    @Input() public availableRels: HalLinks;
    
    /**
     * This is the current active rel.
     */
    @Input() public activeButton: string;

    /**
     * This is the event fired when we have clicked an item
     */
    @Output() public onClick: EventEmitter<any> = new EventEmitter();

    public clickHandler = (item: HalButton<any>) => {
        this.onClick.emit(item);
    };

    public isButtonDisabled = (rel: string) => {
        return !this.availableRels || !this.availableRels[rel];
    };    
}
