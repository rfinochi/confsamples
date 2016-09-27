/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {
    beforeEach,
    beforeEachProviders,
    describe,
    expect,
    it,
    inject,
    injectAsync,
    setBaseTestProviders
} from '@angular/core/testing';

import {FormBuilder} from '@angular/common';
import {KeyValueInput} from './common.keyvalue.input';

describe('KeyValueInput Tests', () => {
    
    beforeEachProviders(() => [
        FormBuilder
    ]);
  
    let keyValueInput: KeyValueInput;
    
    beforeEach(inject([FormBuilder], (formBuilder: FormBuilder) => {
        keyValueInput = new KeyValueInput(formBuilder);

        keyValueInput.ngOnInit();
    }));
    
    it('should construct', () => {
        expect(keyValueInput).toBeDefined();
    });

    it('should create key/value map onChange if it does not exist', () => {
        keyValueInput.ngOnChanges(<any>{});

        expect(keyValueInput.keyValueMap).toEqual({});
    });

    describe('Given input pairs', () => {

        beforeEach(() => {
            keyValueInput.keyValueMap = {};
        });

        it('should add input when the key is unique and key/val are both non empty', () => {
            keyValueInput.keyControl().updateValue('foo');
            keyValueInput.valueControl().updateValue('bar');

            keyValueInput.addPair();

            expect(Object.keys(keyValueInput.keyValueMap).length).toBe(1);
            expect(keyValueInput.keys()[0]).toBe('foo');
            expect(keyValueInput.keyValueMap['foo']).toBe('bar');
        });

        it('should ignore input if the key is empty', () => {
            keyValueInput.keyControl().updateValue('');
            keyValueInput.valueControl().updateValue('foo');

            keyValueInput.addPair();

            expect(keyValueInput.keys().length).toBe(0);
        });

        it('should allow input if the value is empty', () => {
            keyValueInput.keyControl().updateValue('foo');
            keyValueInput.valueControl().updateValue('');

            keyValueInput.addPair();

            expect(keyValueInput.keys().length).toBe(1);
        });

        it('should allow input if the key is a duplicate', () => {
            keyValueInput.keyControl().updateValue('foo');
            keyValueInput.valueControl().updateValue('bar');

            keyValueInput.addPair();

            keyValueInput.keyControl().updateValue('foo');
            keyValueInput.valueControl().updateValue('baz');

            keyValueInput.addPair();

            expect(keyValueInput.keys().length).toBe(1);
            expect(keyValueInput.keys()[0]).toBe('foo');
            expect(keyValueInput.keyValueMap['foo']).toBe('baz');
        });

        it('should clear the form whenever it attempts to add a pair', () => {
            keyValueInput.keyControl().updateValue('foo');
            keyValueInput.valueControl().updateValue('bar');

            keyValueInput.addPair();

            expect(keyValueInput.keyControl().value).toBe(null);
            expect(keyValueInput.valueControl().value).toBe(null);

            keyValueInput.keyControl().updateValue('');
            keyValueInput.valueControl().updateValue('');

            keyValueInput.addPair();

            expect(keyValueInput.keyControl().value).toBe(null);
            expect(keyValueInput.valueControl().value).toBe(null);
        });

        it('should remove the correct key value pair', () => {
            let pairs: { [key: string]: string} = {
                'a': '1',
                'b': '2',
                'c': '3',
                'd': '4',
                'e': '5'
            },
            event: any = {
                preventDefault: () => {}
            };

            keyValueInput.keyValueMap = pairs;

            keyValueInput.removePair('e', event);

            expect(keyValueInput.keys()).toEqual(['a', 'b', 'c', 'd']);
            expect(keyValueInput.keyValueMap['e']).toBeUndefined();

            keyValueInput.removePair('a', event);

            expect(keyValueInput.keys()).toEqual(['b', 'c', 'd']);
            expect(keyValueInput.keyValueMap['a']).toBeUndefined();

            keyValueInput.removePair('c', event);

            expect(keyValueInput.keys()).toEqual(['b', 'd']);
            expect(keyValueInput.keyValueMap['c']).toBeUndefined();
        });
    });
});