/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Install tasks

*/

// Dependencies
var gulp        = require('gulp'),
    rimraf      = require('rimraf'),
    tslint      = require('gulp-tslint'),
    fs          = require('fs'),
    install     = require('gulp-install'),
    config      = require('./config.js').config,
    copy        = require('./config.js').copy,
    defaultUser = require('./config.js').defaultUserConfig,
    taskFactory = require('./config.js').taskFactory;

// export a function to describe tasks and return the list of them

// this task has to happen before everything else
gulp.task('clean', () => rimraf.sync(config.cleanFolder));

var installTasks = config.installPaths.reduce((previous, path, index) => {      
    var taskName = 'install:' + index;        
    gulp.task(taskName, previous, () => gulp.src(path).pipe(install()));      
    return previous.concat([taskName]);       
}, ['clean']);        
      
gulp.task('install', installTasks)

// these tasks ensure that the code we have is valid without building it
// e.g. installing dependencies, doing fixups, linting, formatting
module.exports = taskFactory({
    'check-user-config': (done) => {
        if (process.env.PORT && process.env.CONFIG_URL) {
            console.log('-> Environment variables detected; not creating a user-config.json');
            done();
        } else {
            fs.stat('user-config.json', function (err, stat) {
                if (!stat) {
                    console.log('-> User config not found; writing out example that needs to be filled out');
                    fs.writeFile('user-config.json', JSON.stringify(defaultUser, null, '\t'), 'utf8', done);
                } else {
                    console.log('-> User config found');
                    done();
                }
            });
        }
    },
    'lint': () =>
        gulp.src(config.lintPaths)
            .pipe(tslint())
            .pipe(tslint.report("verbose")),
    'fixup': () => 
        copy('vendor/**/*', './node_modules/')
}, ['clean']);