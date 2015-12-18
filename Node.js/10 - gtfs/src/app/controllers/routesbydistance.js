'use strict';

module.exports = function($rootScope, $scope, $http, uiGmapIsReady, spinnerService) {
    console.log('routesbydistance.js');
    spinnerService.show('routesbydistance-spinner');

    uiGmapIsReady.promise(1).then(function(instances) {
        $rootScope.$watch('map.center', function(newValue, oldValue) {
            spinnerService.show('routesbydistance-spinner');
            $scope.routes = [];

            var radius = $rootScope.map.circle.radius * 0.000621371;
            $http
                .get('getroutesbydistance/' + $rootScope.map.circle.center.latitude + '/' + $rootScope.map.circle.center.longitude + '/' + radius)
                .then(function(response) {
                    $scope.routes = response.data;
                    spinnerService.hide('routesbydistance-spinner');
                });
        });

        $rootScope.$watch('map.circle.radius', function(newValue, oldValue) {
            spinnerService.show('routesbydistance-spinner');
            $scope.routes = [];

            var radius = $rootScope.map.circle.radius * 0.000621371;
            $http
                .get('getroutesbydistance/' + $rootScope.map.circle.center.latitude + '/' + $rootScope.map.circle.center.longitude + '/' + radius)
                .then(function(response) {
                    $scope.routes = response.data;
                    spinnerService.hide('routesbydistance-spinner');
                });
        });

        var radius = $rootScope.map.circle.radius * 0.000621371;
        $http
            .get('getroutesbydistance/' + $rootScope.map.circle.center.latitude + '/' + $rootScope.map.circle.center.longitude + '/' + radius)
            .then(function(response) {
                $scope.routes = response.data;
                spinnerService.hide('routesbydistance-spinner');
            });
    });

    $scope.showCoords = function(route) {
        spinnerService.show('stopsbyroute1-spinner');
        spinnerService.show('stopsbyroute2-spinner');
        $rootScope.currentRoute.info = route;
        $rootScope.currentRoute.stops1 = [];
        $rootScope.currentRoute.stops2 = [];

        $rootScope.tabs.t1 = false;
        $rootScope.tabs.t2 = false;
        $rootScope.tabs.t3 = true;
        $rootScope.tabs.t4 = false;

        $http
            .get('getcoordinatesbyroute/localAgency/' + route.route_id)
            .then(function(response) {
                $rootScope.map.polylines =[{
                    id: 1,
                    path: [],
                    stroke: {
                        color: '#FF0000',
                        weight: 2,
                        opacity: 1
                    },
                    visible: true
                },{
                    id: 2,
                    path: [],
                    stroke: {
                        color: '#0000FF',
                        weight: 2,
                        opacity: 1
                    },
                    visible: true
                }];

                for(var i = 0; i < response.data[0].length; i++)
                    $rootScope.map.polylines[0].path.push({ latitude: response.data[0][i][1], longitude: response.data[0][i][0] });

                for(var i = 0; i < response.data[1].length; i++)
                    $rootScope.map.polylines[1].path.push({ latitude: response.data[1][i][1], longitude: response.data[1][i][0] });
            });

        $http
            .get('getstopsbyroute/localAgency/' + route.route_id + '/0')
            .then(function(response) {
                $rootScope.currentRoute.stops1 = response.data;
                spinnerService.hide('stopsbyroute1-spinner');
            });

        $http
            .get('getstopsbyroute/localAgency/' + route.route_id + '/1')
            .then(function(response) {
                $rootScope.currentRoute.stops2 = response.data;
                spinnerService.hide('stopsbyroute2-spinner');
            });
    }
};
