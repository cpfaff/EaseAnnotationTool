(function () {
    'use strict';
    
    angular.module('annotationItem.time', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core'])
        .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('annotationTimeController', function (TimeProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider)
        {
            $scope.errorMessage = null;
            $scope.timeModule = {
                timeRanges: {
                    timeRange:
                        {
                            rangeStart:
                            {
                                dateTime:
                                {
                                }
                            },
                            rangeEnd:
                            {
                                dateTime:
                                {
                                }
                            },
                        }
                },
                timePeriods: { timePeriod: [] },
                temporalResolutions: []
            }

            $scope.timeRange = {
                rangeStart: {},
                rangeEnd: {}
            };

            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
            $scope.autocompleteFields = {};

            $scope.geologicalTimePeriodsModel = {};
            $scope.resolutionsModel = {};


            TimeProvider.get().then(function (response) {
                $scope.vocabularies = response.data;

                if ($.isEmptyObject(actualAnnotationModel.annotationItem.object.contexts[0].timeContext)) {
                    actualAnnotationModel.annotationItem.object.contexts[0].timeContext = $scope.timeModule;

                    var localTimeZone = GetCurrentTimeZone();
                    var selectedTimeZoneName;

                    $scope.vocabularies.timeZonesVocabulary.every(function (item)
                    {
                        if ($scope.GetNormalizedTimeZoneNames(item.value).indexOf(localTimeZone) > -1) {
                            selectedTimeZoneName = item;
                            return false;
                        }
                        return true;
                    });

                    if (selectedTimeZoneName) {
                        $scope.timeRange.rangeEnd.timezone = selectedTimeZoneName;
                        $scope.timeRange.rangeStart.timezone = selectedTimeZoneName;
                    }
                }
                else {
                    // Restore saved values
                    $scope.timeModule = actualAnnotationModel.annotationItem.object.contexts[0].timeContext;

                    if ($scope.timeModule.timeRanges)
                        for (var type in $scope.timeRange) {
                            if ($scope.timeModule.timeRanges.timeRange[type].dateTime.date) {
                                var dateTime = new Date($scope.timeModule.timeRanges.timeRange[type].dateTime.date);
                                $scope.timeRange[type].dateTime = dateTime;

                                if ($scope.timeModule.timeRanges.timeRange[type].dateTime.time) {
                                    var time = $scope.timeModule.timeRanges.timeRange[type].dateTime.time.split(':');
                                    $scope.timeRange[type].dateTime.setHours(time[0]);
                                    $scope.timeRange[type].dateTime.setMinutes(time[1]);
                                    $scope.timeRange[type].dateTime.setSeconds(time[2]);
                                }
                            }
                            if ($scope.timeModule.timeRanges.timeRange[type].dateTime.timezone)
                                $scope.autocompleteFields[type] = $scope.GetNormalizedTimeZoneNames($scope.timeModule.timeRanges.timeRange[type].dateTime.timezone.value);
                        }
                }
            });

            function createFilterFor(query) {
                return function filterFn(item) {
                    return ($scope.GetNormalizedTimeZoneNames(item.value).toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            function GetCurrentTimeZone()
            {
                var currentTime = new Date();
                var currentTimezone = currentTime.getTimezoneOffset();
                currentTimezone = (currentTimezone / 60) * -1;
                var gmt = 'GMT';
                if (currentTimezone !== 0) {
                    gmt += currentTimezone > 0 ? '+' : '';
                    gmt += currentTimezone;
                }

                return gmt; 
            }

            $scope.querySearch = function (query, vocabulary) {
                var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;
                return results;
            }

            $scope.ClearModel = function (mainModel) {
                for (var item in mainModel)
                    mainModel[item] = null;
            }

            $scope.GetNormalizedTimeZoneNames = function (name) {
                
                var newstr =
                    name.replace(/Slash/g, " ").
                        replace(/_/g, " ").
                        replace(/([A-Z])/g, '$1').trim();

                return newstr;
            }  

            $scope.GetNormalizedName = function(name)
            {
                var re = /^([a-z])(.*)([A-Z][a-z]*)/;
                var newstr = name.replace(re, function (string, $1, $2, $3) {
                    return ($1.toUpperCase() + $2 + ' ' + $3);
                });

                return newstr;
            }

            $scope.TimeRangeChanged = function(type)
            {
                var dateTime = $scope.timeRange[type].dateTime;
                if (dateTime) {

                    if (!$scope.timeModule.timeRanges)
                    {
                        $scope.timeModule.timeRanges = {
                            timeRange:
                                {
                                    rangeStart:
                                    {
                                        dateTime:
                                        {
                                        }
                                    },
                                    rangeEnd:
                                    {
                                        dateTime:
                                        {
                                        }
                                    },
                                }
                        };
                    }

                    $scope.timeModule.timeRanges.timeRange[type].dateTime.date = dateTime.toDateString();
                    $scope.timeModule.timeRanges.timeRange[type].dateTime.time = dateTime.getHours() + ':' + dateTime.getMinutes() + ':' + dateTime.getSeconds();

                    if ('rangeStart' == type)
                    {
                        if (!$scope.timeRange['rangeEnd'].dateTime)
                        {
                            var copiedDateTime = new Date(dateTime.getTime());
                            copiedDateTime.setHours(23);
                            copiedDateTime.setMinutes(59);
                            copiedDateTime.setSeconds(59);
                            $scope.timeRange['rangeEnd'].dateTime = copiedDateTime;

                            $scope.timeModule.timeRanges.timeRange['rangeEnd'].dateTime.date = copiedDateTime.toDateString();
                            $scope.timeModule.timeRanges.timeRange['rangeEnd'].dateTime.time = '23:59:59';
                            $scope.timeModule.timeRanges.timeRange['rangeEnd'].dateTime.timezone = $scope.timeRange[type].timezone;
                        }
                    }
                }

                if ($scope.timeModule.timeRanges)
                    $scope.timeModule.timeRanges.timeRange[type].dateTime.timezone = $scope.timeRange[type].timezone;
            }

            $scope.addGeologicItem = function()
            {
                $scope.errorMessage = null;
                var copiedObject = jQuery.extend(true, {}, $scope.geologicalTimePeriodsModel);
                
                if (!$scope.timeModule.timePeriods)
                    $scope.timeModule.timePeriods = { timePeriod: [] }

                if (!$scope.geologicalTimePeriodsModel.geologicalEon || $scope.geologicalTimePeriodsModel.geologicalEon.length < 1 || 
                    !$scope.geologicalTimePeriodsModel.geologicalEra || $scope.geologicalTimePeriodsModel.geologicalEra.length < 1)
                {
                    $scope.errorMessage = "Geological Eon and Geological Era are required fields.";
                    return false;
                }

                $scope.timeModule.timePeriods.timePeriod.push(copiedObject);
                $scope.ClearModel($scope.geologicalTimePeriodsModel);
            }

            $scope.addResolution = function()
            {
                var copiedObject = jQuery.extend(true, {}, $scope.resolutionsModel);

                if (!$scope.timeModule.temporalResolutions)
                    $scope.timeModule.temporalResolutions = [];

                $scope.timeModule.temporalResolutions.push(copiedObject);
                $scope.ClearModel($scope.resolutionsModel);
            }
        });
})();