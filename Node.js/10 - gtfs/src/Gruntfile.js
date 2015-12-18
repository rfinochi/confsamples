module.exports = function(grunt) {
    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        browserify: {
            js: {
                src: 'app/app.js',
                dest: 'public/js/app.js'
            }
        },
        copy: {
            all: {
                expand: true,
                cwd: 'app/',
                src: ['**/*.html', '**/*.css'],
                dest: 'public/'
            },
            boostrap: {
                src: ['node_modules/bootstrap/dist/css/bootstrap.min.css'],
                dest: 'public/ccs/bootstrap.min.css'
            }
        },
    });

    grunt.loadNpmTasks('grunt-browserify');
    grunt.loadNpmTasks('grunt-contrib-copy');

    grunt.registerTask('default', ['browserify', 'copy']);
};
