'use strict';

module.exports = function($rootScope, $scope, uiGmapGoogleMapApi, uiGmapIsReady) {
    console.log('home.js');

    $rootScope.nav  = {
        b1: true,
        b2: false,
        b3: false
    };

    $rootScope.tabs = {
        t1: true,
        t2: false,
        t3: false,
        t4: false
    };

    $rootScope.map = {
        bounds: {
            northeast: {},
            southwest: {}
        },
        circle: {
            radius: 500,
            fill: {
                color: '#000000',
                opacity: 0.25
            },
            stroke: { opacity: 0 },
        },
        polylines: [],
        window: {},
        events: {
            'click': function(maps, eventName, args) {
                $rootScope.map.center = { latitude: args[0].latLng.lat(), longitude: args[0].latLng.lng() };
                $rootScope.map.circle.center = { latitude: args[0].latLng.lat(), longitude: args[0].latLng.lng() };
                maps.setCenter(args[0].latLng);
            }
        }
    };

    $rootScope.currentRoute = {};

    if (navigator.geolocation) {
        navigator.geolocation.getCurrentPosition(function(position) {
            $rootScope.map.center = {
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            };

            $rootScope.map.circle.center = {
                latitude: position.coords.latitude,
                longitude: position.coords.longitude
            };
        });
    } else {
        $rootScope.map.center = {
            latitude: -33.43658,
            longitude: -70.6428761
        };

        $rootScope.map.circle.center = {
            latitude: -33.43658,
            longitude: -70.6428761
        };
    }

    uiGmapGoogleMapApi.then(function(maps) {
        console.log("uiGmapGoogleMapApi");
    });

    uiGmapIsReady.promise(1).then(function(instances) {
        console.log("uiGmapIsReady");
    });

    $scope.show = function() {
        $rootScope.nav  = {
            b1: true,
            b2: false,
            b3: false
        };
    };

    $scope.showOnMap = function(stop) {
        $rootScope.map.window.coords = { latitude: stop.stop_lat, longitude: stop.stop_lon };
        $rootScope.map.window.content = stop.stop_name;
        $rootScope.map.window.show = true;
    };

    $scope.hideOnMap = function() {
        $rootScope.map.window.show = false;
    };
};
