/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import {Config} from './config';
import * as App from './server';
import * as debug from 'debug';
import * as express from 'express';
import * as http from 'http';

export var getBinding = (port: string | number) => typeof port === 'string'
    ? 'Pipe ' + port
    : 'Port ' + port;

/**
 * This creates an on listening callback for use with the server
 */
export var onListening = (port: string | number) => () => {
    let bind = getBinding(port);
    console.log('Services started: listening on ' + bind);
};

/**
 * This creates an error callback for use with the server
 */
export var onStartupError = (port: string | number) => (error: NodeJS.ErrnoException) => {
    if (error.syscall !== 'listen') {
        throw error;
    }

    let bind = getBinding(port);

    // handle specific listen errors with friendly messages
    switch (error.code) {
        case 'EACCESS':
            console.error(bind + ' requires elevated privileges');
            process.exit(1);
            break;
        case 'EADDRINUSE':
            console.error(bind + ' is already in use');
            process.exit(1);
            break;
        default:
            throw error;
    }
};

/**
 * Normalize a port into a number, string, or false.
 */
export function normalizePort(val: any) {
    let port: number = parseInt(val, 10);

    if (isNaN(port)) {
        // named pipe
        return val;
    }

    if (port > 0 && port < 65536) {
        // port number
        return port;
    }

    console.error('Could not parse env variable $PORT. Expected number, Actual: \'{val}\'');
    process.exit(1);
    return false;
}

/**
 * This is the main function for the application
 */
export async function main() {    
    // initialize configuration:
    const config =  await Config.initialize();
    
    // get the port to listen on:
    const port = normalizePort(config.Port);
    
    // initialize the DM app:
    const app = await App.initialize();
    
    /**
     * Create HTTP server.
     */
    var server = http.createServer(app);

    /**
     * Listen on provided port, on all network interfaces.
     */
    server.listen(port);
    
    /**
     * Event listener for HTTP server "error" event.
     */
    server.on('error', onStartupError(port));
    
    /**
     * Event listener to log out what we're listening on
     */
    server.on('listening', onListening(port));

    /**
     * Notify users that we're running
     */
    console.log('Services starting...');
}

/**
 * Only execute the main function if we are the start of the application
 */
if (module === require.main) {
    main().catch(err => {
        console.error(err);
        process.exit(1);
    });
}