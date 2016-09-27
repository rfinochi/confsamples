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

import {ToggleInput} from './common.toggle.input';

describe('ToggleInput Tests', () => {
    
    let toggleInput: ToggleInput;
    
    beforeEach(() => {
        toggleInput = new ToggleInput();
    });
    
    it('should construct', () => {
        expect(toggleInput).toBeDefined();
    });
    

    it('should change value when not disabled', () => {
        toggleInput.value = true;
        toggleInput.disabled = false;

        toggleInput.setValue(false);

        expect(toggleInput.value).toBe(false);
    });

    it('should not change value when disabled', () => {
        toggleInput.value = true;
        toggleInput.disabled = true;

        toggleInput.setValue(true);

        expect(toggleInput.value).toBe(true);
    });
});