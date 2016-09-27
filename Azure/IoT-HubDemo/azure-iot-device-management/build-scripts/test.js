/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Test Tasks: Used to test code

*/

// dependencies
var gulp = require('gulp'),
    opener = require('opener'),
    remap = require('remap-istanbul/lib/gulpRemapIstanbul'),
    istanbul = require('gulp-istanbul'),
    jasmine = require('gulp-jasmine'),
    build = require('./build.js'),
    config = require('./config.js').config,
    taskFactory = require('./config.js').taskFactory,
    path = require('path');

gulp.task('pre-test:server', build, () => {
    return gulp.src([`${config.serverDest}/**/*.js`, `!${config.serverDest}/**/*.spec.js`])
        .pipe(istanbul())
        .pipe(istanbul.hookRequire())
});

gulp.task('do-test:server', ['pre-test:server'], () => {
    return gulp.src([`${config.serverDest}/**/*.js`])
        .pipe(jasmine())
        .pipe(istanbul.writeReports({
            dir: `${config.testDest}/coverage-server`,
            reporters: ['json']
        }));
});

gulp.task('post-test:server', ['do-test:server'], () => {
    var remapAction = gulp.src(`${config.testDest}/coverage-server/coverage-final.json`)
        .pipe(remap({
            basePath: config.serverBasePath,
            reports: {
                'json': `${config.testDest}/coverage-server/coverage.json`,
                'html': `${config.testDest}/html-report-server`
            }
        }));

    remapAction.on('end', () => opener(['chrome', __dirname + `/../${config.testDest}/html-report-server/index.html`]));
    return remapAction;
});

gulp.task('server-watchers', ['post-test:server'], () => {
    gulp.watch(config.serverDir + '.ts', ['post-test:server']);
});

gulp.task('test:server', ['pre-test:server', 'do-test:server', 'post-test:server', 'server-watchers']);

var retestClient = taskFactory({
    'retest:client': (done) => {
        require('karma').Server.start({
            configFile: path.join(__dirname, config.karmaPath)
        }, 
        (exitCode) => {
            if (exitCode) {
                console.log("\n*****************************");
                console.log("*   FIX BROKEN UNIT TESTS   *");
                console.log("*****************************\n");
            }

            var remapAction = gulp.src(`${config.testDest}/coverage-client/coverage-final.json`)
                .pipe(remap({
                    basePath: config.clientBasePath,
                    reports: {
                        'json': `${config.testDest}/coverage-client/coverage.json`,
                        'html': `${config.testDest}/html-report-client`
                    }
                }));
            remapAction.on('finish', done);
        });
    }
}, ['build:typescript:common:client'], true);

// client tests depend on the server that was created by going over the logic inside server.ts
gulp.task('do-test:client', build.concat(retestClient, 'post-test:server'), () => {
    gulp.watch(config.clientDir + '.ts', ['partial:retest:client']);
});

gulp.task('post-test:client', ['do-test:client'], () => {
    opener(['chrome', __dirname + `/../${config.testDest}/html-report-client/index.html`]);
});

gulp.task('test:client', ['do-test:client', 'post-test:client']);

gulp.task('test', ['test:client', 'test:server']);

module.exports = ['test'];