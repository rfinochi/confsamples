/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

// Karma configuration
// Generated on Tue Apr 05 2016 17:24:58 GMT-0700 (Pacific Daylight Time)

module.exports = function (config) {
    config.set({

        // base path that will be used to resolve all patterns (eg. files, exclude)
        basePath: './',

        // frameworks to use
        // available frameworks: https://npmjs.org/browse/keyword/karma-adapter
        frameworks: ['systemjs', 'jasmine'],

        // list of files / patterns to load in the browser   
        files: [
            'node_modules/reflect-metadata/Reflect.js',
            'node_modules/es5-shim/es5-shim.js',
            'node_modules/es6-shim/es6-shim.js',
            'node_modules/zone.js/dist/zone.js',
            'node_modules/systemjs/dist/system-polyfills.js',
            'node_modules/ng2-bootstrap/bundles/ng2-bootstrap.js',
            'node_modules/underscore/underscore.js',
            'node_modules/@azure-iot/common-ux/client.js',
            'node_modules/@azure-iot/logging/client/client.js',
            'node_modules/moment/moment.js',
            'node_modules/ng2-file-upload/ng2-file-upload.js',
            'node_modules/ng2-file-upload/components/file-upload/file-uploader.class.js',
            'node_modules/ng2-file-upload/components/file-upload/file-like-object.class.js',
            'node_modules/ng2-file-upload/components/file-upload/file-item.class.js',
            'node_modules/ng2-file-upload/components/file-upload/file-select.directive.js',
            'node_modules/ng2-file-upload/components/file-upload/file-drop.directive.js',
            'node_modules/ng2-file-upload/components/file-upload/file-type.class.js',

            // Include test files - these files will be included in the browser with <script> tag
            'dist/app/**/*.spec.js',

            // Include source files but exclude the spec files - these files will not be included in the browser but will be served by Karma's server
            { pattern: 'dist/app/**/!(*spec).js', included: false },
            { pattern: 'node_modules/@angular/**/*.js', included: false },
            { pattern: 'node_modules/rxjs/**/*.js', included: false },
        ],

        // test results reporter to use
        // possible values: 'dots', 'progress'
        // available reporters: https://npmjs.org/browse/keyword/karma-reporter
        reporters: ['spec', 'coverage'],

        // preprocess matching files before serving them to the browser
        // available preprocessors: https://npmjs.org/browse/keyword/karma-preprocessor
        preprocessors: {
            'dist/app/**/!(*spec).js': 'coverage'
        },

        coverageReporter: {
            type: 'json',
            dir: 'dist/test/coverage-client',
            subdir: '.',
            file: 'coverage-final.json'
        },

        // uses karma-spec-reporter to make errors readable
        specReporter: {
            maxLogLines: 1,         // limit number of lines logged per test
            suppressErrorSummary: true,  // do not print error summary
            suppressFailed: false,  // do not print information about failed tests
            suppressPassed: false,  // do not print information about passed tests
            suppressSkipped: false,  // do not print information about skipped tests
            showSpecTiming: true // print the time elapsed for each spec
        },

        // web server port
        port: 9876,

        // enable / disable colors in the output (reporters and logs)
        colors: true,

        // level of logging
        // possible values: config.LOG_DISABLE || config.LOG_ERROR || config.LOG_WARN || config.LOG_INFO || config.LOG_DEBUG
        // logLevel: config.LOG_INFO,

        // enable / disable watching file and executing tests whenever any file changes
        autoWatch: false,

        autoWatchBatchDelay: 1000,

        // start these browsers
        // available browser launchers: https://npmjs.org/browse/keyword/karma-launcher
        // To debug, use: browsers: ['Chrome'],
        browsers: ['PhantomJS'],
        browserNoActivityTimeout: 100000,

        // Continuous Integration mode
        // if true, Karma captures browsers, runs the tests and exits
        singleRun: true,

        // Concurrency level
        // how many browser should be started simultaneous
        concurrency: Infinity,

        systemjs: {
            config: {
                map: {
                    moment: 'node_modules/moment/moment',
                    'ng2-file-upload': 'node_modules/ng2-file-upload/ng2-file-upload',
                    '@angular': 'node_modules/@angular',
                    rxjs: 'node_modules/rxjs',
                    crypto: '@empty',
                },
                packages: {
                    '@angular/core': {
                        main: 'index',
                        defaultExtension: 'js'
                    },
                    '@angular/http': {
                        main: 'index',
                        defaultExtension: 'js'
                    },
                    '@angular/common': {
                        main: 'index',
                        defaultExtension: 'js'
                    },
                    '@angular/compiler': {
                        main: 'index',
                        defaultExtension: 'js'
                    },
                    '@angular/router-deprecated': {
                        main: 'index',
                        defaultExtension: 'js'
                    },
                    '@angular/platform-browser': {
                        main: 'index',
                        defaultExtension: 'js'
                    },
                    'rxjs': {
                        defaultExtension: 'js'
                    },
                    'node_modules/moment': {
                        defaultExtension: 'js'
                    },
                    'node_modules/ng2-file-upload': {
                        defaultExtension: 'js'
                    },
                    'dist/app': {
                        defaultExtension: 'js',
                        format: 'register'
                    }
                }
            }
        }
    });
};
