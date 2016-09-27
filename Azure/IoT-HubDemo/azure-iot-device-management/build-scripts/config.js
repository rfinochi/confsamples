/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Config: contains helper functions + configuration for entire application

*/

// Dependencies
var gulp = require('gulp');
var process = require('process');

// Helper Functions
var taskFactory = (description, before, partialDeps) => {
    var tasks = Object.keys(description);
    before = before || [],
    partialBefore = [];

    if (partialDeps) {
        partialBefore = before.map(dep => 'partial:' + dep);
    }

    tasks.forEach(task => {
        gulp.task('partial:' + task, partialBefore, description[task]);
        gulp.task(task, before, description[task]);
    })
    return tasks;
}

var copy = (src, dest) => gulp.src(src).pipe(gulp.dest(dest));

// Configuration
var config = {
    // install configuration
    cleanFolder: 'dist',
    installPaths: [
        './package.json', 
        './app/tsd.json', 
        './server/tsd.json'
    ],
    lintPaths: [
        './app/**/*.ts', 
        './server/**/*.ts', 
        '!./app/**/*.d.ts',
        '!./server/**/*.d.ts'
    ],
    
    // karma config file
    karmaPath:  '../karma.conf.js', 
    
    // client configuration
    clientDir: 'app/**/*',
    clientTsConfig: 'app/tsconfig.json',
    clientDest: 'dist/app',
    themeDir: 'app/theme',
    clientBasePath: 'app',
    
    // server configuration
    serverDir: 'server/**/*',
    serverTsConfig: 'server/tsconfig.json',
    serverDest: 'dist/server',
    serverTestOut: 'dist/test/html-report',
    serverBasePath: 'server',
    
    // common congiguration
    commonDir: 'common/**/*',
    commonTsConfig: 'common/tsconfig.json',
    commonDest: 'dist/common',

    // development configuration
    serverPort: process.env.PORT || 3003,
    serverDebuggingPort: 9222,
    liveReloadPort: 9091,
    hostname: '127.0.0.1',
    
    // test configuration
    testDest: 'dist/test',
    
    // copyright notice
    copyrightNotice: `/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

`
}

// public interface
module.exports = {
    taskFactory: taskFactory,
    copy: copy,
    config: config,
    defaultUserConfig: {
        IOTHUB_CONNECTION_STRING: '<YOUR CONNECTION STRING HERE>',
        CONSOLE_REPORTING: "both",
        LOG_LEVEL: "trace",
        PORT: '3003',
        CACHING_ENABLED: false
    }
}