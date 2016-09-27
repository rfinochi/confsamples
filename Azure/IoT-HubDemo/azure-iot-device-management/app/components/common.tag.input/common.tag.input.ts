/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Component, Input, OnChanges} from '@angular/core';
import {GlobalContext} from '../../core/index';

export const MAX_ALLOWED_TAGS = 10;

@Component({
    selector: 'tag-input',
    templateUrl: 'dist/app/components/common.tag.input/common.tag.input.html',
    styleUrls: ['dist/app/components/common.tag.input/common.tag.input.css']
})
export class TagInput extends GlobalContext implements OnChanges {
    @Input() tags: string[];
    @Input() value: string;
    @Input() disabled: boolean;
    @Input() placeholderText: string;

    constructor() {
        super();
        this.value = '';
        this.onKeyDown = this.onKeyDown.bind(this);
        this.disabled = false;
    }

    public ngOnChanges() {
        if (!this.tags) {
            this.tags = [];
        }
    }

    public onKeyDown(event: KeyboardEvent) {
        event.preventDefault();
        if (event.keyCode === 13) {
            this.addTag();
        }
    }
    
    public addTag() {
        if (this.value.length > 0 
            && this.tags.indexOf(this.value) === -1
            && this.tags.length < MAX_ALLOWED_TAGS) {
            this.tags.push(this.value);
            this.value = '';
        }
    }

    public removeTag(tag: string): void {
        this.tags.splice(this.tags.indexOf(tag), 1);
    }

    public toTags(): string[] {
        return this.tags.slice();
    }
}