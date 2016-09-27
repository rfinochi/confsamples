import * as start from './start';

describe('startup functionality', () => {
    it('onListening logs the binding', () => {
        spyOn(console, 'log');
        start.onListening(3003)();
        expect(console.log).toHaveBeenCalledWith('Services started: listening on Port 3003');
    });
    
    it('getBinding returns Pipe on string', () => {
        expect(start.getBinding('test')).toEqual('Pipe test');
    });
    
    it('getBinding returns Port on number', () => {
        expect(start.getBinding(500)).toEqual('Port 500');
    });
    
    it('on EACCESS notifies user they require elevated privileges and exits', () => {
        spyOn(process, 'exit');
        spyOn(console, 'error');
        start.onStartupError(500)(<any>{
            syscall: 'listen',
            code: 'EACCESS'
        });
        expect(console.error).toHaveBeenCalledWith('Port 500 requires elevated privileges');
        expect(process.exit).toHaveBeenCalled();
    });
    
    it('on EADDRINUSE notifies user they require elevated privileges and exits', () => {
        spyOn(process, 'exit');
        spyOn(console, 'error');
        start.onStartupError(500)(<any>{
            syscall: 'listen',
            code: 'EADDRINUSE'
        });
        expect(console.error).toHaveBeenCalledWith('Port 500 is already in use');
        expect(process.exit).toHaveBeenCalled();
    });
    
    it('on non listen errors, rethrows', () => {
        expect(() => {
            start.onStartupError(500)(<any>{
                syscall: 'not-listen',
                code: 'EADDRINUSE'
            });
        }).toThrow();
    });
    
    it('on unexpected codes, rethrows', () => {
        expect(() => {
            start.onStartupError(500)(<any>{
                syscall: 'listen',
                code: 'unexpected'
            });
        }).toThrow();
    });
    
    it('normalizes pipes', () => {
        expect(start.normalizePort('test')).toEqual('test');
    });
    
    it('normalizes ports', () => {
        expect(start.normalizePort('500')).toEqual(500);
    });
    
    it('throws error on invalid port', () => {
        spyOn(process, 'exit');
        spyOn(console, 'error');
        expect(start.normalizePort('7000000')).toEqual(false);
        expect(process.exit).toHaveBeenCalled();
        expect(console.error).toHaveBeenCalled();
    });
});