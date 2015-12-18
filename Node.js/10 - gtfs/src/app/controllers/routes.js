'use strict';

module.exports = function($rootScope, $scope, $http) {
    console.log("routes.js");

    $http
        .get("getroutesbyagency/localAgency")
        .then(function(response) {
            $scope.routes = response.data;
        });

    $scope.show = function() {
        $rootScope.nav  = {
            b1: false,
            b2: true,
            b3: false
        };
    };
};
