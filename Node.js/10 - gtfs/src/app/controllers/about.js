'use strict';

module.exports = function($scope, $uibModal, $http) {
    console.log('about.js');

    $scope.open = function () {
        var modalInstance = $uibModal.open({
            scope: $scope,
            animation: true,
            templateUrl: 'partials/about.html',
            controller: 'AboutResult'
        });

        modalInstance.result.then(function () {
            console.log('Modal result at: ' + new Date());
        }, function () {
            console.log('Modal dismissed at: ' + new Date());
        });

        $http
            .get("getfeedinfo/localAgency")
            .then(function(response) {
                $scope.feedinfo = response.data;
            });
    };
};
