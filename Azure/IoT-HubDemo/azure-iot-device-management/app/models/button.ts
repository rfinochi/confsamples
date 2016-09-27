/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

export class Button<T> {
    public action: T;

    public label: string;

    constructor(label: string, action: T) {
        this.label = label;
        this.action = action;
    }
}

export class HalButton<T> extends Button<T> {
        
    public rel: string;
    
    public method: string;

    constructor(label: string, action: T, rel: string, method: string) {
        super(label, action);
        this.rel = rel;
        this.method = method;
    }
}