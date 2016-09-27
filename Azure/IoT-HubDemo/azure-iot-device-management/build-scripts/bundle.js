/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

/*

Bundling Tasks

*/

const Builder = require('systemjs-builder');
const gulp = require('gulp');
const build = require('./build');

gulp.task('bundle', (done) => {
    let builder = new Builder('.', 'systemjs.config.js');
    let srcs = [
        'node_modules/underscore/underscore.js',
        'dist/app/boot.js'
    ].join(' + ');
    builder
        .bundle(srcs, 'dist/app/client.bundle.js')
        .then(() => console.log('Build successful'))
        .catch(err => {
            console.error('Build error');
            console.error(err);
        })
        .then(done);
});