/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Build Tasks: Used to create a working instance

*/

// Dependencies
var gulp        = require('gulp'),
    sourcemaps  = require('gulp-sourcemaps'),
    ts          = require('gulp-typescript'),
    sass        = require('gulp-sass'),
    addsrc      = require('gulp-add-src'),
    fs          = require('fs'),
    glob        = require('glob'),
    install     = require('./install.js'),
    config      = require('./config.js').config,
    copy        = require('./config.js').copy,
    taskFactory = require('./config.js').taskFactory;

// Apply copyright to files
var applyCopyright = (done) => (err, files) => {
    files.forEach(function (file) {
        console.log('applying copyright to ' + file);
        var content = fs.readFileSync(file, 'utf8');
        content = content.replace(/^\s*\/\*(\S|\n|.)*?\*\/\s*/gi, '');
        content = config.copyrightNotice + content;
        fs.writeFileSync(file, content, 'utf8');
    });
    
    done();
}

var applyCopyrightTasks = taskFactory({
    'apply:copyright:app': (done) => glob('app/**/*.ts', applyCopyright(done)),
    'apply:copyright:server': (done) => glob('server/**/*.ts', applyCopyright(done)),
    'apply:copyright:build': (done) => glob('build-scripts/**/*.js', applyCopyright(done)),
    'apply:copyright:gulpfile': (done) => glob('gulpfile.js', applyCopyright(done)),
    'apply:copyright:karma': (done) => glob('karma.conf.js', applyCopyright(done)),
});

gulp.task('apply:copyright', applyCopyrightTasks);

// pull typescript options out of a config
var getTsOptions = (project) => {
    var options = JSON.parse(fs.readFileSync(project, 'utf8')).compilerOptions;
    options.typescript = require('typescript');
    return options;
}

// these tasks actually do the build; they also create 'partial:' prefixed tasks that just do the build
var build = taskFactory({
    'build:typescript:client': () => {
        return gulp.src([config.clientDir + '.ts'])
            .pipe(sourcemaps.init())
            .pipe(ts(getTsOptions(config.clientTsConfig)))
            .js
            .pipe(sourcemaps.write('maps', { includeContent: false }))
            .pipe(gulp.dest(config.clientDest));
    },
    'build:typescript:server': () => {
        return gulp.src([config.serverDir + '.ts'])
            .pipe(sourcemaps.init())
            .pipe(ts(getTsOptions(config.serverTsConfig)))
            .js
            .pipe(sourcemaps.write('maps', { includeContent: false }))
            .pipe(gulp.dest(config.serverDest));
    },
    'build:html': () => 
        copy(config.clientDir + '.html', config.clientDest),
    'build:sass': () =>
        gulp.src(config.clientDir + '.scss')
            .pipe(sass({ includePaths: [config.themeDir, 'node_modules/@azure-iot/common-ux/theme'] }).on('error', sass.logError))
            .pipe(addsrc(config.clientDir + '.css'))
            .pipe(gulp.dest(config.clientDest))
}, install);

// builds everything under the common folder with the appropriate
// module system for app/server and overwrites their faux versions
var buildCommonServer = taskFactory({
    'build:typescript:common:server': () => {
        return gulp.src([config.commonDir + '.ts'])
            .pipe(sourcemaps.init('maps'))
            .pipe(ts(getTsOptions(config.serverTsConfig)))
            .js
            .pipe(sourcemaps.write('maps', { includeContent: false }))
            .pipe(gulp.dest(config.serverDest));
    }
}, ['build:typescript:server'], true);

var buildCommonClient = taskFactory({
    'build:typescript:common:client': () => {
        return gulp.src([config.commonDir + '.ts'])
            .pipe(sourcemaps.init('maps'))
            .pipe(ts(getTsOptions(config.clientTsConfig)))
            .js
            .pipe(sourcemaps.write('maps', { includeContent: false }))
            .pipe(gulp.dest(config.clientDest));
    }
}, ['build:typescript:client'], true);

// this just provides a shortcut to build everything
gulp.task('build', build.concat(buildCommonClient, buildCommonServer));

// export dependencies
module.exports = ['build'];