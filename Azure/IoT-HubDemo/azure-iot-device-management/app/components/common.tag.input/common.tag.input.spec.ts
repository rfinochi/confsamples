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

import {TagInput, MAX_ALLOWED_TAGS} from './common.tag.input';

describe('TagInput Tests', () => {
    
    let tagInput: TagInput;
    let enter: KeyboardEvent;
    
    beforeEach(() => {
        tagInput = new TagInput();
        enter = <any>{ preventDefault: () => { }, keyCode: 13 };
    });

    it('should construct', () => {
        expect(tagInput).toBeDefined();
    });

    it('should create tags array onChange if it does not exist', () => {
        tagInput.ngOnChanges();

        expect(tagInput.tags).toEqual([]);
    });

    describe('Given input tags', () => {
        beforeEach(() => {
            tagInput.tags = [];
        });

        it('should ignore empty input', () => {
            tagInput.value = '';
            tagInput.onKeyDown(enter);

            expect(tagInput.tags.length).toBeFalsy();
        });

        describe('state dependent actions', () => {

            let testTags = [];

            for (var i = 0; i < MAX_ALLOWED_TAGS - 1; ++i) {
                testTags.push(`${i}`);
            }

            beforeEach(() => {
                tagInput.tags.push(...testTags);
            });

            it('should ignore duplicates', () => {
                tagInput.value = `${MAX_ALLOWED_TAGS - 2}`;
                tagInput.onKeyDown(enter);

                expect(tagInput.tags.length).toBe(testTags.length);
            });

            it('should add new input when not at capacity', () => {
                tagInput.value = `${MAX_ALLOWED_TAGS - 1}`;
                tagInput.onKeyDown(enter);

                expect(tagInput.tags.length).toBe(testTags.length + 1);
            });

            it('should ignore new input when at capacity', () => {
                let key = tagInput.value = `${MAX_ALLOWED_TAGS - 1}`;
                tagInput.onKeyDown(enter);
                expect(tagInput.tags.indexOf(key)).not.toBe(-1);

                key = tagInput.value = `${MAX_ALLOWED_TAGS}`;
                tagInput.onKeyDown(enter);

                expect(tagInput.tags.length).toBe(testTags.length + 1);
                expect(tagInput.tags.indexOf(key)).toBe(-1);
            });

            it('should remove the right tags', () => {
                let key = '0';
                tagInput.removeTag(key);
                expect(tagInput.tags.indexOf(key)).toBe(-1);

                key = `${MAX_ALLOWED_TAGS - 2}`;
                tagInput.removeTag(key);
                expect(tagInput.tags.indexOf(key)).toBe(-1);
            });

            it('toTags() should return a copy of the tags array', () => {
                let copiedTags = tagInput.toTags();

                expect(copiedTags).toEqual(tagInput.tags);
                expect(copiedTags).not.toBe(tagInput.tags);
            });
        });
    });
});