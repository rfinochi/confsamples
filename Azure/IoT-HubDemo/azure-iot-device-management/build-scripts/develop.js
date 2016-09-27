/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Development Tasks: Used to work inside your development environment

*/

// Dependencies
var gulp        = require('gulp'),
    opener      = require('opener'),
    livereload  = require('live-reload'),
    spawn        = require('child_process').spawn,
    istanbul    = require('gulp-istanbul'),
    jasmine     = require('gulp-jasmine'),
    extend      = require('util')._extend,
    build       = require('./build.js'),
    config      = require('./config.js').config;

// makes development easier
gulp.task('open:chrome:app', build, () => opener(['chrome', `http://${config.hostname}:${require('../user-config.json').Port || config.serverPort}`, '--remote-debugging-port=' + config.serverDebuggingPort]));
gulp.task('livereload', build, () => livereload({ _: [config.serverDest + '/', config.clientDest + '/'], port: config.liveReloadPort }));

// start and restart the server
var runningServer = null;
var restarting  = false;
var restartServer = () => {
    if (runningServer) {
        console.log('Restarting service...');
        restarting = true;
        runningServer.kill();
    } else {
        console.log('Starting service...');
    };
    updatedEnv = extend({}, process.env);
    updatedEnv.LIVE_RELOAD = true;
    runningServer = spawn('node', ['./' + config.serverDest + '/start'], {
        env: updatedEnv
    });
    
    runningServer.stdout.on('data', (output) => console.log(output.toString()));
    runningServer.stderr.on('data', (output) => console.log(output.toString()));

    runningServer.on('exit', () => {
        if(!restarting) {
            console.log('Server has crashed!');
            process.exit(-1);
        }

        restarting = false;
    });
}
gulp.task('start:express', build, restartServer);
gulp.task('restart:express', ['partial:build:typescript:common:server'], restartServer);

// start up a developing environment
gulp.task('develop', build.concat(['open:chrome:app', 'livereload', 'start:express']), () => {
    gulp.watch(config.serverDir + '.ts', ['restart:express']);
    gulp.watch(config.clientDir + '.ts', ['partial:build:typescript:common:client']);
    gulp.watch([config.clientDir + '.scss', config.clientDir + '.css'], ['partial:build:sass']);
    gulp.watch(config.clientDir + '.html', ['partial:build:html']);
});

module.exports = ['develop'];
