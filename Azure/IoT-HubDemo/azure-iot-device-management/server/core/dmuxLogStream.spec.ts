import {DMUXLogStream, ConsoleReporting} from './dmuxLogStream';

describe('DMUXLogStream', () => {
    it('is enabled if console reporting is both', () => {
        let logStream = new DMUXLogStream(ConsoleReporting[ConsoleReporting.both]);
        expect(logStream.isEnabled).toBeTruthy();
    });
    
    it('is enabled if console reporting is server', () => {
        let logStream = new DMUXLogStream(ConsoleReporting[ConsoleReporting.server]);
        expect(logStream.isEnabled).toBeTruthy();
    });
    
    it('is not enabled if console reporting is client', () => {
        let logStream = new DMUXLogStream(ConsoleReporting[ConsoleReporting.client]);
        expect(logStream.isEnabled).toBeFalsy();
    });
    
    it('is not enabled if console reporting is none', () => {
        let logStream = new DMUXLogStream(ConsoleReporting[ConsoleReporting.none]);
        expect(logStream.isEnabled).toBeFalsy();
    });
    
    describe('write', () => {
        let logStream: DMUXLogStream = null;
        
        beforeEach(() => {
            logStream = new DMUXLogStream(ConsoleReporting[ConsoleReporting.server]);
        });
        
        it('doesn\'t write when not enabled', () => {
            spyOn(console, 'log');
            logStream.isEnabled = false;
            logStream.write('test');
            expect(console.log).not.toHaveBeenCalled();
        });
        
        it('writes ISR when enabled', () => {
            spyOn(console, 'log');
            logStream.write('{"eventType":"IncomingServiceRequest", "requestMethod":"GET", "requestUri":"/", "operationName":"test","responseStatusCode":200,"latencyMs":200}');
            expect(console.log).toHaveBeenCalled();
        });
        
        it('writes OSR when enabled without hostname', () => {
            spyOn(console, 'log');
            logStream.write('{"eventType":"OutgoingServiceRequest", "hostname":"test.com", "context":"service", "operationName": "test", "succeeded":true, "latencyMs":200}');
            expect(console.log).toHaveBeenCalled();
        });
        
        it('writes OSR when enabled with hostname', () => {
            spyOn(console, 'log');
            logStream.write('{"eventType":"OutgoingServiceRequest", "hostname":"HostName=test.com", "context":"service", "operationName": "test", "succeeded":true, "latencyMs":200}');
            expect(console.log).toHaveBeenCalled();
        });
        
        it('writes Exception when enabled', () => {
            spyOn(console, 'log');
            logStream.write('{"eventType":"Exception", "errorDetails":"error"}');
            expect(console.log).toHaveBeenCalled();
        });
    });
});