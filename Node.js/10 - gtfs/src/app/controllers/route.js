'use strict';

module.exports = function($rootScope, $scope, $routeParams, $http) {
    console.log("route.js");
    $http
        .get("getroutesbyid/localAgency/" + $routeParams.routeId)
        .then(function(response) {
            $scope.route = response.data;
        });
};
