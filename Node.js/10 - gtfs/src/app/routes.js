'use strict';

module.exports = function($routeProvider) {
    $routeProvider
        .when('/routes', {
            controller: require('./controllers/routes.js'),
            controllerAs: 'Routes',
            templateUrl: 'partials/routes.html'
        });
};
