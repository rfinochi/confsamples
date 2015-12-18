'use strict';

module.exports = function($scope, $routeParams, $http) {
    console.log("coordinatesbyroute.js");
    $http
        .get("getcoordinatesbyroute/localAgency/" + $routeParams.routeId)
        .then(function(response) {
            $scope.coords = response.data;
        });
};
