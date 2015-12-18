'use strict';

module.exports = function($scope, $uibModalInstance) {
    console.log('aboutresult.js');

    $scope.dismiss = function () {
        console.log('dialog dismiss');
        $uibModalInstance.dismiss();
    };
};
