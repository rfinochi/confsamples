/* Copyright (c) Microsoft Corporation. All Rights Reserved. */

(function () {
    var map = {
        'ng2-bootstrap': 'node_modules/ng2-bootstrap',
        'moment': 'node_modules/moment',
        '@angular': 'node_modules/@angular',
        'rxjs': 'node_modules/rxjs',
        'ng2-file-upload': 'node_modules/ng2-file-upload'
    };

    var packages = {};

    var packageNames = [
        '@angular/common',
        '@angular/compiler',
        '@angular/core',
        '@angular/http',
        '@angular/platform-browser',
        '@angular/platform-browser-dynamic',
        '@angular/router',
        '@angular/router-deprecated'
    ];

    packageNames.forEach(function (pkgName) {
        packages[pkgName] = { main: 'index.js', defaultExtension: 'js' };
    });
    packages['ng2-bootstrap'] = { main: 'ng2-bootstrap', defaultExtension: 'js' };
    packages['ng2-file-upload'] = { main: 'ng2-file-upload', defaultExtension: 'js' };
    packages['dist/app'] = { format: 'register', defaultExtension: 'js' };
    packages['moment'] = { main: 'moment', defaultExtension: 'js' };
    packages['@azure-iot/common-ux/modal'] = { format: 'register', defaultExtension: false };
    packages['@azure-iot/common-ux/grid'] = { format: 'register', defaultExtension: false };
    packages['@azure-iot/common-ux/applicationLoadingBar'] = { format: 'register', defaultExtension: false };
    packages['@azure-iot/common-ux/set-editor'] = { format: 'register', defaultExtension: false };

    var config = {
        defaultJSExtensions: true,
        meta: {
            '@azure-iot/common-ux/modal': {
                build: false
            },
            '@azure-iot/common-ux/applicationLoadingBar': {
                build: false
            },
            '@azure-iot/common-ux/grid': {
                build: false
            },
            '@azure-iot/common-ux/set-editor': {
                build: false
            },
            '@azure-iot/logging/client.js': {
                build: false
            }
        },
        map: map,
        packages: packages
    };

    System.config(config);

})(this);