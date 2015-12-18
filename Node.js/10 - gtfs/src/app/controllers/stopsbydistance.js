'use strict';

module.exports = function($rootScope, $scope, $http, uiGmapIsReady, spinnerService) {
    console.log('stopsbydistance.js');
    spinnerService.show('stopsbydistance-spinner');

    uiGmapIsReady.promise(1).then(function(instances) {
        $rootScope.$watch('map.center', function(newValue, oldValue) {
            spinnerService.show('stopsbydistance-spinner');
            $scope.stops = [];

            var radius = $rootScope.map.circle.radius * 0.000621371;
            $http
                .get('getstopsbydistance/' + $rootScope.map.circle.center.latitude + '/' + $rootScope.map.circle.center.longitude + '/' + radius)
                .then(function(response) {
                    $scope.stops = response.data;
                    spinnerService.hide('stopsbydistance-spinner');
                });
        });

        $rootScope.$watch('map.circle.radius', function(newValue, oldValue) {
            spinnerService.show('stopsbydistance-spinner');
            $scope.stops = [];

            var radius = $rootScope.map.circle.radius * 0.000621371;
            $http
                .get('getstopsbydistance/' + $rootScope.map.circle.center.latitude + '/' + $rootScope.map.circle.center.longitude + '/' + radius)
                .then(function(response) {
                    $scope.stops = response.data;
                    spinnerService.hide('stopsbydistance-spinner');
                });
        });

        var radius = $rootScope.map.circle.radius * 0.000621371;
        $http
        .get('getstopsbydistance/' + $rootScope.map.circle.center.latitude + '/' + $rootScope.map.circle.center.longitude + '/' + radius)
        .then(function(response) {
            $scope.stops = response.data;
            spinnerService.hide('stopsbydistance-spinner');
        });
    });

    $scope.showOnMap = function(stop) {
        $rootScope.map.window.coords = { latitude: stop.stop_lat, longitude: stop.stop_lon };
        $rootScope.map.window.content = stop.stop_name;
        $rootScope.map.window.show = true;
    };

    $scope.hideOnMap = function() {
        $rootScope.map.window.show = false;
    };
};
