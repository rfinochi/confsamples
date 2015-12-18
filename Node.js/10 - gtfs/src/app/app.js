'use strict';

window._ = require('lodash');

var angular = require('angular');

require('angular-route')
require('angular-simple-logger');
require('angular-google-maps');
require('angular-ui-bootstrap');
require('angular-spinners');

angular.module('app', [
    'ngRoute',
    'nemLogging',
    'uiGmapgoogle-maps',
    'ui.bootstrap',
    'angularSpinners'])
    .config(require('./routes'), function(uiGmapGoogleMapApiProvider) {
        uiGmapGoogleMapApiProvider.configure({
            key: 'AIzaSyA1V-sf_PJokAEmaSuNUh6upnNuA5XaWBE',
            v: '3.22'
            //libraries: 'weather,geometry,visualization'
        });
    })

require('./controllers');
