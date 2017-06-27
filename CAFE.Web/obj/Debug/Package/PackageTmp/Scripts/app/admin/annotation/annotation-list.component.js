(function () {
    'use strict';

    angular
        .module('admin.annotationList')
        .component('annotationList',
        {
            templateUrl: 'Scripts/app/admin/annotation/annotation-list.template.html',
            controller: [
                'AnnotationItemProvider', '$scope', '$mdDialog', '$mdToast',
                function(AnnotationItemProvider, $scope, $mdDialog, $mdToast) {

                    var bookmark;

                    $scope.filter = {
                        options: {
                            debounce: 500
                        }
                    };

                    var last = {
                        bottom: false,
                        top: true,
                        left: false,
                        right: true
                    };

                    $scope.selected = [];

                    $scope.toastPosition = angular.extend({}, last);
                    $scope.getToastPosition = function() {
                        sanitizePosition();

                        return Object.keys($scope.toastPosition)
                            .filter(function(pos) { return $scope.toastPosition[pos]; })
                            .join(' ');
                    };

                    function sanitizePosition() {
                        var current = $scope.toastPosition;

                        if (current.bottom && last.top) current.top = false;
                        if (current.top && last.bottom) current.bottom = false;
                        if (current.right && last.left) current.left = false;
                        if (current.left && last.right) current.right = false;

                        last = angular.extend({}, current);
                    }

                    $scope.query = {
                        filter: '',
                        limit: '10',
                        order: 'NameToLower',
                        page: 1
                    };


                    function success(response) {
                        $scope.annotations = response.data;
                    }

                    function updateAfterChange() {
                        var pinTo = $scope.getToastPosition();
                        $mdToast.show(
                            $mdToast.simple()
                              .textContent("Owner was changed successfully")
                              .position(pinTo)
                              .hideDelay(3000)
                        );

                        $scope.selected = [];
                        $scope.getAnnotations();
                    };

                    $scope.changeOwner = function(item, event) {
                        $mdDialog.show({
                            clickOutsideToClose: false,
                            controller: 'AIChangeOwnerController',
                            controllerAs: 'ctrl',
                            focusOnOpen: false,
                            targetEvent: event,
                            locals: { annotationItems: [item], updateFunction: updateAfterChange, getToastPosition: $scope.getToastPosition },
                            templateUrl: 'Scripts/app/admin/annotation/annotation-change-owner-dialog.html'
                        });
                    };

                    $scope.changeOwnerDialog =function(event) {
                        $mdDialog.show({
                            clickOutsideToClose: false,
                            controller: 'AIChangeOwnerController',
                            controllerAs: 'ctrl',
                            focusOnOpen: false,
                            targetEvent: event,
                            locals: { annotationItems: $scope.selected, updateFunction: updateAfterChange, getToastPosition: $scope.getToastPosition },
                            templateUrl: 'Scripts/app/admin/annotation/annotation-change-owner-dialog.html'
                        });
                    }
                    $scope.deleteAnnotationsDialog = function (event) {
                        var confirm = $mdDialog.confirm()
                             .title('Would you like to delete Annotaion Items?')
                             .textContent('All selected Annotaion Items will be deleted permanently.')
                             .ariaLabel('Lucky day')
                             .targetEvent(event)
                             .ok('Yes')
                             .cancel('No');

                        $mdDialog.show(confirm).then(function () {

                            $scope.selected.forEach(function (v) { delete v.$id });

                            AnnotationItemProvider.delete($scope.selected).then(function (response) {
                                $scope.selected.forEach(function (item) {
                                    $scope.annotations.splice($scope.annotations.indexOf(item), 1);
                                });
                                $scope.selected = [];
                                var pinTo = $scope.getToastPosition();
                                $mdToast.show(
                                    $mdToast.simple()
                                      .textContent("Annotation items successfully deleted")
                                      .position(pinTo)
                                      .hideDelay(3000)
                                );
                                $scope.getAnnotations();
                            });
                        }, function() {
                            //var pinTo = $scope.getToastPosition();
                            //$mdToast.show(
                            //    $mdToast.simple()
                            //      .textContent("Error occurs when trying delete annotation items")
                            //      .position(pinTo)
                            //      .hideDelay(3000)
                            //);
                        });
                    }
                    $scope.delete = function(item, event) {
                        var confirm = $mdDialog.confirm()
                             .title('Would you like to delete Annotaion Item?')
                             .textContent('Selected Annotaion Item will be deleted permanently.')
                             .ariaLabel('Lucky day')
                             .targetEvent(event)
                             .ok('Yes')
                             .cancel('No');

                        $mdDialog.show(confirm).then(function () {
                            delete item.$id;
                            AnnotationItemProvider.delete([item]).then(function (response) {
                                $scope.selected.forEach(function (resp) {
                                    $scope.annotations.splice($scope.annotations.indexOf(resp), 1);
                                });
                                var pinTo = $scope.getToastPosition();
                                $mdToast.show(
                                    $mdToast.simple()
                                      .textContent("Annotation item successfully deleted")
                                      .position(pinTo)
                                      .hideDelay(3000)
                                );
                                $scope.getAnnotations();
                            });
                        }, function () {
                            //var pinTo = $scope.getToastPosition();
                            //$mdToast.show(
                            //    $mdToast.simple()
                            //      .textContent("Error occurs when trying delete annotation items")
                            //      .position(pinTo)
                            //      .hideDelay(3000)
                            //);
                        });
                    };

                    $scope.getAnnotations = function() {
                        var prom = AnnotationItemProvider.get($scope.query);
                        $scope.promise = prom.then(success);
                    };

                    $scope.removeFilter = function() {
                        $scope.filter.show = false;
                        $scope.query.filter = '';

                        if ($scope.filter.form.$dirty) {
                            $scope.filter.form.$setPristine();
                        }
                    };

                    $scope.$watch('query.filter',
                        function(newValue, oldValue) {
                            if (!oldValue) {
                                bookmark = $scope.query.page;
                            }

                            if (newValue !== oldValue) {
                                $scope.query.page = 1;
                            }

                            if (!newValue) {
                                $scope.query.page = bookmark;
                            }

                            $scope.getAnnotations();
                        });

                }
            ]
        }).controller('AIChangeOwnerController',
            function (AnnotationItemProvider, $scope, $mdDialog, $mdToast, annotationItems, updateFunction, getToastPosition, GroupProvider) {
                'use strict';

                $scope.searchUserKey = null;
                $scope.selectedUser = null;

                $scope.userLiveSearch = function(keyword) {
                    return GroupProvider.searchUsers({ Name: keyword }).then(function(response) {
                        return response.data;
                    });
                }

                $scope.model = annotationItems;

                this.cancel = $mdDialog.cancel;

                this.save = function() {
                    AnnotationItemProvider.changeOwner($scope.selectedUser.id, $scope.model.map(function (a) { return a.id; })).then(function (result) {
                            updateFunction();
                            $mdDialog.cancel();
                        },
                        function (err) {

                            var pinTo = getToastPosition();
                            $mdToast.show(
                                $mdToast.simple()
                                  .textContent("Error occurs when trying change owner")
                                  .position(pinTo)
                                  .hideDelay(3000)
                            );
                        });
                };

            });



    angular
        .module('admin.annotationList')
        .controller('ToastCtrl', function ($scope, $mdToast) {
            $scope.closeToast = function () {
                $mdToast.hide();
            };
        });
})();