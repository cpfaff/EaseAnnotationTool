(function () {
    'use strict';

    angular
        .module('admin.gfbio')
        .component('bindedcontrolsList',
        {
        	templateUrl: 'Scripts/app/admin/gfbio/bindedcontrols-list.template.html',
            controller: [
                'UIElementsProvider', '$scope', '$mdDialog', '$mdToast',
                function (UIElementsProvider, $scope, $mdDialog, $mdToast) {

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
                        $scope.uielements = response.data;
                    }

                    $scope.delete = function(item, event) {
                        var confirm = $mdDialog.confirm()
                            .title('Would you like to delete UI Elements?')
                            .textContent('Selected UI Elements will be deleted permanently.')
                             .ariaLabel('Lucky day')
                             .targetEvent(event)
                             .ok('Yes')
                             .cancel('No');

                        $mdDialog.show(confirm).then(function () {
                            delete item.$id;
                            UIElementsProvider.delete([item]).then(function (response) {
                                $scope.selected.forEach(function (resp) {
                                    $scope.uielements.splice($scope.uielements.indexOf(resp), 1);
                                    $scope.selected = [];
                                });
                                var pinTo = $scope.getToastPosition();
                                $mdToast.show(
                                    $mdToast.simple()
                                      .textContent("UI element item successfully deleted")
                                      .position(pinTo)
                                      .hideDelay(3000)
                                );
                                $scope.getUIElements();
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

                    $scope.getUIElements = function() {
                        var prom = UIElementsProvider.get($scope.query);
                        $scope.promise = prom.then(success);
                    };

                    $scope.showDetails = function (index, event) {
                        event.stopPropagation();
                        $mdDialog.show({
                            clickOutsideToClose: false,
                            controller: 'uiElementDetailsController',
                            controllerAs: 'ctrl',
                            focusOnOpen: false,
                            targetEvent: event,
                            locals: { uielement: $scope.uielements[index] },
                            templateUrl: 'Scripts/app/admin/gfbio/bindedcontrols-details.template.html'
                        }).then($scope.getUIElements);
                    };

                    $scope.addNew = function(event) {
                        event.stopPropagation();
                        $mdDialog.show({
                            clickOutsideToClose: false,
                            controller: 'uiElementDetailsController',
                            controllerAs: 'ctrl',
                            focusOnOpen: false,
                            targetEvent: event,
                            locals: {
                                uielement: {}
                            },
                            templateUrl: 'Scripts/app/admin/gfbio/bindedcontrols-details.template.html'
                        }).then($scope.getUIElements);
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

                            $scope.getUIElements();
                        });

                }
            ]
        });

/**
  * Edit controller. For details modal
  */
    angular
        .module('admin.gfbio').controller('uiElementDetailsController', ['UIElementsProvider', '$scope', '$mdDialog', 'uielement', '$filter',
            function (UIElementsProvider, $scope, $mdDialog, uielement, $filter) {
                'use strict';
                var bookmark;
                $scope.uielement = uielement;
                $scope.searchUIElementKey;
                $scope.query = {
                    filter: '',
                    limit: '10',
                    order: 'NameToLower',
                    page: 1
                };

                $scope.errorMessage;
                $scope.functionAutocomplete = {
                    elementSearch: '',
                    selectedItem: null
                };

                this.cancel = $mdDialog.cancel;

                if ($scope.uielement && $scope.uielement.elementid) {
                    UIElementsProvider.search('').then(function (response) {

                        var foundItem = $filter('filter')(response.data, { id: $scope.uielement.elementid })[0];                        
                        $scope.functionAutocomplete.selectedItem = foundItem;
                    });
                }

                $scope.searchUIElements = function (search) {
                    return UIElementsProvider.search(search).then(function (response) {
                        return response.data;
                    });
                }


                $scope.selectedUIElementChanged = function (item) {
                    $scope.uielement.elementid = item.id;
                    //console.log($scope.functionAutocomplete);
                }



                this.save = function () {
                    $scope.item.form.$setSubmitted();

                    if ($scope.item.form.$valid) {
                        UIElementsProvider.update($scope.uielement).then(function (response) {
                            $mdDialog.hide(response.data);
                        });
                    }
                };
            }]);

    angular
        .module('admin.gfbio')
        .controller('ToastCtrl', function ($scope, $mdToast) {
            $scope.closeToast = function () {
                $mdToast.hide();
            };
        });
})();