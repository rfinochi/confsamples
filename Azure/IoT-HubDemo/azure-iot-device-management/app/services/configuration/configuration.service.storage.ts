/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {BehaviorSubject} from 'rxjs/Rx';

/**
 * This is a utility class that keeps track of a configuration value in storage
 */
export class StorageSubject<T> extends BehaviorSubject<T> {
    
    private key: string;
    
    private storage: Storage;
    
    constructor(storage: Storage, key: string, defaultValue: T) {
        var configString = storage.getItem(key);
        
        if (configString) {
            super(JSON.parse(configString));
        } else {
            super(defaultValue);
        }
        
        this.key = key;
        this.storage = storage;
    }
    
    next(config: T): void {
        this.storage.setItem(this.key, JSON.stringify(config));
        super.next(config);
    }
}