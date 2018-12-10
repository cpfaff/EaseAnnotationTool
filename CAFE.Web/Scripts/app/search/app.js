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
      'search.accessRequestDialog',
      'rzModule',
      'leaflet-directive'
    ]);

    app.controller('SearchController', [
        '$scope', '$http', '$filter', '$mdDialog', 'leafletData', '$timeout', function ($scope, $http, $filter, $mdDialog, leafletData, $timeout) {

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

            $scope.searchPost = function () {
                $scope.items = [];
                $scope.isLoading = true;
                $scope.isLoadingFirst = true;
                var data = {
                    searchItemsType: $scope.searchBy,
                    orderBy: $scope.orderBy,
                    searchText: $scope.npSearchString,
                    filters: $scope.filters.map(function (item) {
                        var prop = item.property;

                        prop.items = item.property.items.map(function (f) {
                            var f1 = f;

                            f1.values = f.values.map(function(v) {
                                var v1 = v;
                                delete v1['$id'];

                                return v1;
                            });

                            return f1;

                        });
                        return prop;
                    })
                }


                $http.post('/api/Search/SearchItems', data).then(function (response) {
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
                $('html, body').animate({ scrollTop: $("#filterBtn").offset().top }, 100); 
            };

            $scope.removeFilter = function(filter) {
                $scope.filters.splice($scope.filters.indexOf(filter), 1);
                $scope.properties.push(filter.property);
                $('html, body').animate({ scrollTop: $("#filterBtn").offset().top }, 100); 
            };

            $scope.clearFilters = function() {
                $scope.filters = [];
                $http.get('/api/Search/GetFilters?itemsType=' + $scope.searchBy).then(function (response) {
                    $scope.properties = response.data;
                });
                $scope.preparedFilters = '';
                $scope.search();
            };

            $scope.boundingBox =
            {
                northBoundingCoordinate: 25,
                southBoundingCoordinate: -25,
                eastBoundingCoordinate: 25,
                westBoundingCoordinate: -25
            };

            function InitLeaflet() {
                
                leafletData.getMap("mainMap").then(function (map) {
                    var boundingBoxCoordinates = [[-25, -25], [25, 25]];
                    angular.extend(map, { editable: true });
                    var rec = L.rectangle(boundingBoxCoordinates).addTo(map);
                    rec.enableEdit();
                    rec.
                        on('mouseup', SaveCoordinates).
                        on('drag', SaveCoordinates).
                        on('editable:editing', SaveCoordinates);

                    map._onResize(); 
                    function SaveFilter() {
                        $scope.filters.forEach(function (item) {
                            if (item.property.name == 'Space.Bounding Box') {
                                item.property.items.forEach(function (filterElement) {
                                    if (filterElement.description == 'North')
                                        filterElement.value = $scope.boundingBox.northBoundingCoordinate;

                                    if (filterElement.description == 'South')
                                        filterElement.value = $scope.boundingBox.southBoundingCoordinate;

                                    if (filterElement.description == 'East')
                                        filterElement.value = $scope.boundingBox.eastBoundingCoordinate;

                                    if (filterElement.description == 'West')
                                        filterElement.value = $scope.boundingBox.westBoundingCoordinate;
                                });
                            }
                        });
                    }

                    function SaveCoordinates() {
                        var latLngs = rec.getLatLngs();

                        var north = latLngs[0][1].lat > latLngs[0][3].lat ? latLngs[0][1].lat : latLngs[0][3].lat;
                        var south = latLngs[0][1].lat < latLngs[0][3].lat ? latLngs[0][1].lat : latLngs[0][3].lat;
                        var west = latLngs[0][0].lng < latLngs[0][2].lng ? latLngs[0][0].lng : latLngs[0][2].lng;
                        var east = latLngs[0][0].lng > latLngs[0][2].lng ? latLngs[0][0].lng : latLngs[0][2].lng;

                        if (
                            west < -180 ||
                            east > 180
                        ) {
                            var westOld = $scope.boundingBox.westBoundingCoordinate;
                            var eastOld = $scope.boundingBox.eastBoundingCoordinate;

                            rec.setBounds(
                                L.latLngBounds(
                                    L.latLng(
                                        south,
                                        west < -180 ? -180 : westOld
                                    ),
                                    L.latLng(
                                        north,
                                        east > 180 ? 180 : eastOld
                                    )
                                )
                            );
                        }
                        else
                            $scope.boundingBox =
                                {
                                    northBoundingCoordinate: north,
                                    southBoundingCoordinate: south,
                                    eastBoundingCoordinate: east,
                                    westBoundingCoordinate: west
                                };

                        SaveFilter();
                    }

                    $scope.SetBoundingBoxPostion = function () {
                        if (null != $scope.boundingBox.southBoundingCoordinate && null != $scope.boundingBox.westBoundingCoordinate &&
                            null != $scope.boundingBox.northBoundingCoordinate && null != $scope.boundingBox.eastBoundingCoordinate)
                            rec.setBounds(
                                L.latLngBounds(
                                    L.latLng(
                                        $scope.boundingBox.southBoundingCoordinate,
                                        $scope.boundingBox.westBoundingCoordinate
                                    ),
                                    L.latLng(
                                        $scope.boundingBox.northBoundingCoordinate,
                                        $scope.boundingBox.eastBoundingCoordinate
                                    )
                                )
                            );

                        SaveFilter();
                    }

                });
            }


            $scope.onFilterParameterSelect = function (item) {
                for (var i = 0; i < item.property.items.length; i++) {
                    let property = item.property.items[i];

                    if (property.filterType == "Select" || property.filterType == "ReferenceValue") {
                        if (property.value == undefined) {
                            property.value = '';
                        }

                        $http.post('/api/Search/GetSelectValuesForFilter', property).then(function (response) {
                            property.filterSelectionItems = response.data;
                        });
                    }
                    else if (property.filterType == "DateAndTime") {
                        property.popup1 = { opened: false };
                        //item.property.popup2 = { opened: false };
                    }
                }

                $scope.properties.splice($scope.properties.indexOf(item.property), 1);

                if (item.property.name == "Space.Bounding Box") {
                    InitLeaflet(); //$timeout(InitLeaflet, 2000); 
                }
            };

            $scope.applyFilters = function () {
                var filters = $scope.filters;
                //$scope.preparedFilters = '';
                //for (var i in filters) {
                //    var value = filters[i];
                //    //if (value.property.value == undefined) return;
                //    $scope.preparedFilters += value.property.name + '=';
                //    if (value.property.filterType == 'DateRange') {
                //        if (value.property.min instanceof Date && value.property.max instanceof Date) {

                //            var min = value.property.min.getDate() + "." + (value.property.min.getMonth() + 1) + "." + value.property.min.getFullYear() + " " +
                //                value.property.min.getHours() + ":" + value.property.min.getMinutes() + ":" + value.property.min.getSeconds();
                //            var max = value.property.max.getDate() + "." + (value.property.max.getMonth() + 1) + "." + value.property.max.getFullYear() + " " +
                //                value.property.max.getHours() + ":" + value.property.max.getMinutes() + ":" + value.property.max.getSeconds();
                //            $scope.preparedFilters += min + '-' + max;

                //        }
                //    }
                //    else if (value.property.filterType == 'DigitalRange') {
                //        $scope.preparedFilters += value.property.min + '-' + value.property.max;
                //    }
                //    else if (value.property.filterType == 'Select') {
                //        if (value.property.value == undefined) {
                //            $scope.errorMessage = "You must fill all filter's fields or remove some filters.";
                //            $("#errorModal").modal("show");
                //            return;
                //        }
                //        $scope.preparedFilters += value.property.value.value;
                //    } else {
                //        if (value.property.value == undefined) {
                //            $scope.errorMessage = "You must fill all filter's fields or remove some filters.";
                //            $("#errorModal").modal("show");
                //            return;
                //        }
                //        $scope.preparedFilters += value.property.value;
                //    }
                //    $scope.preparedFilters += ',';
                //}


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

                //$scope.search();
                $scope.searchPost();
            };

            $scope.openDate = function (item, whatExactly) {
                if (whatExactly == 1) {
                    item.popup1.opened = true;
                } else {
                    item.popup2.opened = true;
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