/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

import * as express from 'express';
import * as path from 'path';
import * as fs from 'fs';

var app: express.Express = express();

/**
 * Static files to be exposed to the client
 * 1. First, we need to expose our dist public and app folders; to do this, we walk up the directory structure from the build output location to find our root.
 * 2. Next, we need to expose node_modules we depend on from the client. Since some packages (angular2) don't expose a main, we can't use require, so instead
 *    we perform the same lookup as node does. 
 */
var moduleDirectory: string = path.resolve(__dirname, '../../../');

var currentWorkingDirectory: string = process.cwd();
var moduleChecks = [];

// split, then push each segment + previous segements into module checks:
// after this, moduleChecks = ['/node_modules', '/git/node_modules', '/git/azure-iot-device-management/node_modules']
moduleDirectory.split(path.sep).reduce((prev, next) => {
    var fullPath = path.join(prev, next);
    moduleChecks.push(path.join(fullPath, 'node_modules'));
    return fullPath;
}, '');

// we reverse, so now moduleChecks = ['/git/azure-iot-device-management/node_modules', '/git/node_modules', '/node_modules']
moduleChecks.reverse();

// this is our lookup function, which checks each directory in moduleChecks for a package
function getModuleDirectory(moduleName) {
    for (var i = 0; i < moduleChecks.length; i++) {
        var dir = path.join(moduleChecks[i], moduleName);
        try {
            fs.statSync(dir);
            return dir;
        } catch (err) {
            // expected when the folder doesn't exist
        }
    }
    throw new Error('Unable to find ' + moduleName);
}

var index = fs.readFileSync('dist/app/index.html', 'utf8');

// check to see if bundle exists; if not, remove the script tag so we don't fetch it
try {
    fs.statSync('dist/app/client.bundle.js');
} catch (err) {
    // bundle doesn't exist, remove the tag
    index = index.replace('<script src="client.bundle.js"></script>', '');
}

if (!process.env.LIVE_RELOAD) {
    index = index.replace('<script src="//localhost:9091"></script>', '');
}

app.get('/', function (req, res) {
    res.status(200).send(index);
});

// now we actually expose the modules to be used by the client + our own static folders
app.use(express.static(path.join(moduleDirectory, 'dist/app')));
app.use('/dist/app', express.static(path.join(moduleDirectory, 'dist/app')));
app.use('/systemjs.config.js', express.static(path.join(moduleDirectory, '/systemjs.config.js')));

[
    'es5-shim',
    'es6-shim',
    '@angular',
    'systemjs',
    'rxjs',
    'zone.js',
    'bootstrap',
    'ng2-bootstrap',
    'underscore',
    'moment',
    'ng2-file-upload',
    'reflect-metadata',
    '@azure-iot'
].forEach(clientModule => {
    app.use('/node_modules/' + clientModule, express.static(getModuleDirectory(clientModule)));
});

export = app;
