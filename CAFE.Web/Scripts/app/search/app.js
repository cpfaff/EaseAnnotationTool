(function () {
    'use strict';

    var app = angular.module('search', [
      'ngRoute',
      'ui.bootstrap',
      'ui.select',
      'ngSanitize',
      'ngAnimate',
      'ngMaterial',
      'ng-pros.directive.autocomplete',
      'search.accessRequestDialog'
    ]);

    app.controller('SearchController', [
        '$scope', '$http', '$filter', '$mdDialog', function ($scope, $http, $filter, $mdDialog) {
            $scope.definedProperties = [];
            $scope.properties = [];

            $scope.filters = [];
            $scope.preparedFilters = '';

            $scope.searchBy = 'AnnotationItem';

            $scope.dateOptions = {
                //dateDisabled: disabled,
                formatYear: 'yy',
                maxDate: new Date(2020, 5, 22),
                minDate: new Date(2000, 1, 1),
                startingDay: 1
            };

            $scope.orderBy = "-CreationDate";
            $scope.isLoading = false;
            $scope.isLoadingFirst = true;
            $scope.items = [];
            $scope.search = function () {
                $scope.items = [];
                $scope.isLoading = true;
                $scope.isLoadingFirst = true;
                $http.get('/api/Search/SearchItems?itemsType=' + $scope.searchBy + '&orderBy=' + $scope.orderBy + '&searchText=' + $scope.npSearchString + '&filters=' + $scope.preparedFilters).then(function (response) {
                    $scope.isLoading = false;
                    $scope.isLoadingFirst = false;
                    $scope.items = response.data;
                    angular.forEach($scope.items, function (value, key) {
                        value.selected = false;
                    });
                });
            };
            $scope.anySelected = function() {
                return $filter('filter')($scope.items, { selected: true }).length > 0;
            };

            $scope.npAutocompleteOptions = {
                url: '/api/Search/SearchForAutocomplete',
                delay: 1000,
                //nameAttr: 'name',
                minlength: 1,
                //dataHolder: 'items',
                searchParam: 'q',
                highlightExactSearch: 'true',
                onSelect: function(item) {
                    //console.log(item);
                    $scope.npSearchString = item.name;
                    $scope.search();
                }
                //itemTemplateUrl: 'autocompleteSearchItem.html'
            };

            $scope.npSearchString = '';

            $scope.requestAccess = function(event) {
                event.stopPropagation();
                $mdDialog.show({
                    clickOutsideToClose: true,
                    controller: 'AccessRequestDialogCtrl',
                    controllerAs: 'ctrl',
                    focusOnOpen: false,
                    targetEvent: event,
                    windowClass: 'large-Modal',
                    locals: { seleted: $filter('filter')($scope.items, { selected: true }) },
                    templateUrl: 'Scripts/app/search/access-requests/access-request-dialog/access-request-dialog.template.html'
                }).then($scope.clearSelection);
            };

            $scope.requestAccessPredefined = function (event) {
                event.stopPropagation();
                $mdDialog.show({
                    clickOutsideToClose: true,
                    controller: 'AccessRequestDialogCtrl',
                    controllerAs: 'ctrl',
                    focusOnOpen: false,
                    targetEvent: event,
                    windowClass: 'large-Modal',
                    locals: { seleted: [predefinedARItem] },
                    templateUrl: '/Scripts/app/search/access-requests/access-request-dialog/access-request-dialog.template.html'
                }).then($scope.clearSelection);
            };
            
            $scope.clearSelection = function() {
                angular.forEach($scope.items, function (value, key) {
                    value.selected = false;
                });
            };

            $scope.addFilter = function() {
                var filter = {};
                $scope.filters.push(filter);
            };

            $scope.removeFilter = function(filter) {
                $scope.filters.splice($scope.filters.indexOf(filter), 1);
                $scope.properties.push(filter.property);
            };

            $scope.clearFilters = function() {
                $scope.filters = [];
                $http.get('/api/Search/GetFilters?itemsType=' + $scope.searchBy).then(function (response) {
                    $scope.properties = response.data;
                });
                $scope.preparedFilters = '';
                $scope.search();
            };

            $scope.onFilterParameterSelect = function (item) {
                
                if (item.property.filterType == "Select") {
                    if (item.property.value == undefined) {
                        item.property.value = '';
                    }

                    $http.post('/api/Search/GetSelectValuesForFilter', item.property).then(function(response) {
                        item.filterSelectionItems = response.data;
                    });
                }
                else if (item.property.filterType == "DateRange") {
                    item.property.popup1 = { opened: false };
                    item.property.popup2 = { opened: false };
                }
                $scope.properties.splice($scope.properties.indexOf(item.property), 1);

                //fill related properties
                for (var i = 0; i < item.property.relatedFilterModels.length; i++) {
                    var relatedItem = item.property.relatedFilterModels[i];

                    var filter = { property: relatedItem };
                    $scope.filters.push(filter);

                    $scope.onFilterParameterSelect(filter);
                }
            };

            $scope.applyFilters = function () {
                var filters = $scope.filters;
                $scope.preparedFilters = '';
                for (var i in filters) {
                    var value = filters[i];
                    //if (value.property.value == undefined) return;
                    $scope.preparedFilters += value.property.name + '=';
                    if (value.property.filterType == 'DateRange') {
                        if (value.property.min instanceof Date && value.property.max instanceof Date) {

                            var min = value.property.min.getDate() + "." + (value.property.min.getMonth() + 1) + "." + value.property.min.getFullYear() + " " +
                                value.property.min.getHours() + ":" + value.property.min.getMinutes() + ":" + value.property.min.getSeconds();
                            var max = value.property.max.getDate() + "." + (value.property.max.getMonth() + 1) + "." + value.property.max.getFullYear() + " " +
                                value.property.max.getHours() + ":" + value.property.max.getMinutes() + ":" + value.property.max.getSeconds();
                            $scope.preparedFilters += min + '-' + max;

                        }
                    }
                    else if (value.property.filterType == 'DigitalRange') {
                        $scope.preparedFilters += value.property.min + '-' + value.property.max;
                    }
                    else if (value.property.filterType == 'Select') {
                        if (value.property.value == undefined) {
                            $scope.errorMessage = "You must fill all filter's fields or remove some filters.";
                            $("#errorModal").modal("show");
                            return;
                        }
                        $scope.preparedFilters += value.property.value.value;
                    } else {
                        if (value.property.value == undefined) {
                            $scope.errorMessage = "You must fill all filter's fields or remove some filters.";
                            $("#errorModal").modal("show");
                            return;
                        }
                        $scope.preparedFilters += value.property.value;
                    }
                    $scope.preparedFilters += ',';
                }
                //angular.forEach(filters, function (value, key) {
                //    if (value.property.value == undefined) return;
                //    $scope.preparedFilters += value.property.name + '=';
                //    if (value.property.filterType == 'DateRange') {
                //        if (value.property.min instanceof Date && value.property.max instanceof Date) {

                //            var min = value.property.min.getDate() + "." + (value.property.min.getMonth() + 1) + "." + value.property.min.getFullYear() + " " +
                //                value.property.min.getHours() + ":" + value.property.min.getMinutes() + ":" + value.property.min.getSeconds();
                //            var max = value.property.max.getDate() + "." + (value.property.max.getMonth() + 1) + "." + value.property.max.getFullYear() + " " +
                //                value.property.max.getHours() + ":" + value.property.max.getMinutes() + ":" + value.property.max.getSeconds();
                //            $scope.preparedFilters += min + '-' + max;

                //        }
                //    } else if (value.property.filterType == 'Select') {
                //        $scope.preparedFilters += value.property.value.value;
                //    } else {
                //        $scope.preparedFilters += value.property.value;
                //    }
                //    if (key < filters.length) {
                //        $scope.preparedFilters += ',';
                //    }
                //});

                $scope.search();
            };

            $scope.openDate = function (item, whatExactly) {
                if (whatExactly == 1) {
                    item.property.popup1.opened = true;
                } else {
                    item.property.popup2.opened = true;
                }
            };

            $http.get('/api/Search/GetFilters?itemsType=' + $scope.searchBy).then(function (response) {
                $scope.properties = response.data;
            });

            //$scope.alerts = [];

            $scope.haveAccess = function (item, $event) {
                console.log("request check access");
                var result = item.isAccessible;
                if (!result) {
                    //item.link = "";
                    $event.preventDefault();
                    if (!isUserAuthenticated)
                        $scope.addAlert(item, "Access to this item was restricted by its owner. Please log in to get access to it.");
                    else
                        $scope.addAlert(item, "Access to this item was restricted by its owner. You can select items of interest and click \"Request access\" to contact the item's owner.");
                }
                return result;
                //if (__user_have_search_access) return true;
                //return false;
            };

            $scope.addAlert = function (item, message) {
                //$scope.alerts.push({ msg: message, type: tp });
                item.errmsg = message;
            };

            $scope.closeAlert = function (item) {
                //$scope.alerts.splice(index, 1);
                item.errmsg = undefined;
            };
        }
    ]);
})();