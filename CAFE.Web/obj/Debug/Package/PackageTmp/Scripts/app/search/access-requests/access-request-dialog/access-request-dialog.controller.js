(function() {
    angular.module('search.accessRequestDialog').controller('AccessRequestDialogCtrl',
        ['$scope', 'seleted', '$mdDialog', 'AccessRequestProvider',
        function ($scope, selected, $mdDialog, AccessRequestProvider) {
        
            'use strict';

            this.cancel = $mdDialog.cancel;

            $scope.query = {
                filter: '',
                limit: '10',
                order: 'nameToLower',
                page: 1
            };

            $scope.selected = selected;

            $scope.newAccessRequest = AccessRequestProvider.new();

            function success(response) {
                $mdDialog.hide(response.data);
            }

            this.save = function () {
                $scope.requestForm.$setSubmitted();

                if ($scope.requestForm.$valid) {
                    angular.forEach($scope.selected, function (value, key) {
                        var resource = AccessRequestProvider.newResource();
                        resource.kind = value.itemType;
                        resource.name = value.name;
                        resource.resourceId = value.itemId;
                        resource.ownerId = value.userId;

                        $scope.newAccessRequest.requestedResources.push(resource);
                    });
                    AccessRequestProvider.save($scope.newAccessRequest).then(success);
                }
            };
    }]);
})();