/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

let colors = require('colors/safe');

export enum ConsoleReporting {
    none,
    both,
    server,
    client
}

// stolen mostly from sim
export class DMUXLogStream {
    public isEnabled: boolean;
    
    constructor(report: string) {
        this.isEnabled = (
            report === ConsoleReporting[ConsoleReporting.both] || 
            report === ConsoleReporting[ConsoleReporting.server]);
    }
    
    write(log: string) {
        if (this.isEnabled) {            
            let logJson = JSON.parse(log);
            let time = new Date(logJson.time);
            let timestamp = `${time.getHours()}:${time.getMinutes()}:${time.getSeconds()}.${time.getMilliseconds()}`;
            switch (logJson.eventType) {
                case 'IncomingServiceRequest':
                    console.log(`[${colors.gray(timestamp)}]: ${colors.magenta('Incoming')} (${colors.magenta(logJson.requestMethod + ':' + logJson.requestUri)}) -> ${colors.cyan(logJson.operationName)}: ${logJson.responseStatusCode >= 200 && logJson.responseStatusCode < 400 ? colors.green('SUCCESS') : colors.red('FAILURE')} (${logJson.latencyMs}ms)`);
                    break;
                case 'OutgoingServiceRequest':
                    let host: string = logJson.hostname;
                    if (host.includes('HostName')) {
                        // Otherwise use HostName for host if available
                        host = host
                            .split(';')
                            .filter((part: string): boolean => part.startsWith('HostName'))
                            .reduce((acc: string, val: string): string => val.split('=')[1], host);
                    }

                    console.log(`[${colors.gray(timestamp)}]: ${colors.magenta('Outgoing')} ${colors.magenta(logJson.context)} => (${colors.magenta(logJson.operationName)}) -> ${colors.cyan(host)}: ${logJson.succeeded ? colors.green('SUCCESS') : colors.red('FAILURE')} (${logJson.latencyMs}ms)`);
                    break;
                case 'Exception':
                    console.log(`[${colors.gray(timestamp)}]: ${colors.red('Exception')}:\n${colors.red(logJson.errorDetails)}`);
                    break;
            }
        }
    }
}