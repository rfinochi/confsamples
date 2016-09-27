/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {shadow, ShadowFunction} from './shadow'; 

class Foo {
    public first;
    private second;

    constructor() {
        this.first = 'A';
        this.second = 'B';
    }

    // public api -> property access
    concat = function() {
        return this.first + this.second;
    };
    
    // public api -> internal mutation
    //           -> private api -> public api
    resetFirst = function(val: string) {
        this.second = this._concat();
        this.first = val;
    };

    // public api -> internal mutation
    bar = function() {
        this.first = 'bar';
        this.second = 'bar';
    };

    // private api -> public api -> property access
    _concat = function() {
        return '_' + this.concat();
    };

    // private api -> public api -> property access
    //             -> internal mutation
    _resetSecond = function(val: string) {
        this.first = this.concat();
        this.second = val;
    };

    // private api redefining public api
    _resetBar = function(val: string) {
        this.bar = () => {
            this.first = val;
            this.second = val;
        };
    };

    // private api redefining public api
    resetResetBar = function(val: string) {
        this._resetBar = () => {
            return val;
        };
    };

    boundMethod = () => {
        return this.concat() + this._concat();
    }

    _boundMethod = () => {
        return this.boundMethod();
    }
}

function propFilter(prop: string): boolean {
    return prop[0] !== '_';
}

function countSideEffects(map: Object) {
    return function(instance: any, methodName: string, originalMethod: any, args: any[]): any {
        if (!map[methodName]) {
            map[methodName] = 0;
        }

        map[methodName]++;

        return originalMethod.apply(instance, args);
    };
};

function resetSideEffects(sideEffects: Object) {
    Object.keys(sideEffects).forEach(key => delete sideEffects[key]);
}

const sideEffects = {};

class TestShadow {
    public foo: Foo;
    constructor() {
        this.foo = shadow(new Foo(), propFilter, countSideEffects(sideEffects));
    }
}

describe('Shadow Object Tests', () => {
    let testObj: { foo: Foo };

    beforeEach(() => {
        testObj = new TestShadow();
    });

    it('should log sideffects for each public function', () => {
        expect(testObj.foo.concat()).toEqual('AB');
        testObj.foo.bar();
        expect(testObj.foo.concat()).toEqual('barbar');
        testObj.foo.resetFirst('foofoo');
        expect(testObj.foo.concat()).toEqual('foofoo_barbar');

        expect(sideEffects['concat']).toBe(3);
        expect(sideEffects['bar']).toBe(1);
        expect(sideEffects['resetFirst']).toBe(1);
    });

    it('should not log side effects for private apis', () => {
        expect(testObj.foo._concat()).toEqual('_AB');
        testObj.foo._resetSecond('CD');
        expect(testObj.foo.concat()).toEqual('ABCD');

        expect(sideEffects['concat']).toBe(1);
    });

    it('should handle a private function resetting a public one', () => {
        testObj.foo._resetBar('reset');
        testObj.foo.bar();
        expect(testObj.foo.concat()).toEqual('resetreset');

        expect(sideEffects['concat']).toBe(1);
        expect(sideEffects['bar']).toBe(1);
    });

    it('should handle a public function resetting a private one', () => {
        testObj.foo.resetResetBar('reset');
        expect(testObj.foo._resetBar(null)).toEqual('reset');

        expect(sideEffects['resetResetBar']).toBe(1);
    });

    it('should handle bound public functions', () => {
        expect(testObj.foo.boundMethod()).toEqual('AB_AB');
        expect(sideEffects['boundMethod']).toBe(1);
    });

    it('should handle bound private functions', () => {
        expect(testObj.foo._boundMethod()).toEqual('AB_AB');
        expect(sideEffects['boundMethod']).toBeUndefined();
    });
    
    it('should return base properties', () => {
        expect(testObj.foo.first).toEqual('A');
    });
    
    it('should set base properties', () => {
        testObj.foo.first = 'C';
        expect(testObj.foo.first).toEqual('C');
    });

    afterEach(() => {
        let privateAPIs = Object.keys(testObj.foo).filter((prop) => {
            return testObj.foo[prop] instanceof Function && !propFilter(prop);
        });

        privateAPIs.forEach((prop) => {
            expect(sideEffects[prop]).toBeUndefined();
        });

        resetSideEffects(sideEffects);
    });
});