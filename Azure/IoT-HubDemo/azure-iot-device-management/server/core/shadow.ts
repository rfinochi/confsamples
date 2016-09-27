/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

export type ShadowFunction = (instance: any, prop: string, target: any, argumentList: any[]) => any;

export function shadow<T>(instance: T, shouldProxy: (prop: string) => boolean, extraLogic: ShadowFunction): T {
    return <T> new Proxy({}, {
        // runs ANYTIME a property is accessed including immediately preceding function calls
        // ex. foo.myMethod() would call the getter to get 'myMethod' before calling myMethod.apply(foo, arguments) on it
        get: function(target, prop: string, receiver) {
            // this grabs the latest value of the prop in case a method call that isn't
            // proxied redefines one that is
            var base: any = instance[prop];
            
            // functions must be treated differently as they have both a value and a context to
            // worry about
            if (base instanceof Function) {
                if (shouldProxy(prop)) {
                    // return a function proxying the original 
                    return function(...args: any[]) {
                        return extraLogic(instance, prop, base, args);
                    };
                } else {
                    // return the original function forcibly bound to the original instance
                    return function() { return base.apply(instance, arguments); };
                }
            } else {
                // return value off the instance
                return base;
            }
        },
        set: function(target, prop, value, receiver) {
            // all sets go directly to the object
            instance[prop] = value;
            return true;
        }
    });
}