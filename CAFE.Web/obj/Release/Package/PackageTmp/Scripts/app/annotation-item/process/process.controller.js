(function () {
    'use strict';

    angular.module('annotationItem.process')
        .controller('annotationProcessController', function ($scope, AnnotationItemProvider, actualAnnotationModel, $rootScope) {

            $scope.autocompleteFields = {};

            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();

            $scope.processContext = {};

            $scope.processNames = [];
            $scope.processObjects = [];
            $scope.interactionPartners = [];
            $scope.interactionDirections = [];
            $scope.interactionQualities = [];

            $scope.processModel = { processObject: [], processName: null };
            $scope.interactions = [];
            $scope.processObjectAutocomplete = { searchText: "" }

            $scope.interactionModel = { interactionName: '' };

            AnnotationItemProvider.getProcessVocabularies().then(function (response) {
                $scope.processNames = response.data.processes.processNames;
                $scope.processObjects = response.data.processes.processSubjects;
                $scope.interactionPartners = response.data.interactions.interactionsPartners;
                $scope.interactionDirections = response.data.interactions.interactionDirections;
                $scope.interactionQualities = response.data.interactions.interactionQualities;
            });

            $scope.clearModel = function (mainModel) {
                for (var item in mainModel)
                    mainModel[item] = null;
                $scope.processModel = { processObject: [], processName: null };
            }

            $scope.addProcess = function () {
                if (!$scope.processModel.processName || $scope.processModel.processObject.length == 0) return;
                var copiedObject = jQuery.extend(true, {}, $scope.processModel);
                $scope.processContext.processes.push(copiedObject);
                $scope.clearModel($scope.processModel);
            }
            $scope.transformChip = function(chip) {
                // If it is an object, it's already a known chip
                if (angular.isObject(chip)) {
                    return chip;
                }

                // Otherwise, create a new one
                return { value: chip}
            }
            $scope.addInteraction = function (form) {
                if (!$scope.interactionModel.interactionName || !$scope.interactionModel.interactionPartnerOne ||
                    !$scope.interactionModel.interactionDirection || !$scope.interactionModel.interactionQuality ||
                    !$scope.interactionModel.interactionPartnerTwo) {
                    
                    $scope.interactionModel.interactionFormRequired = true;
                    return;
                }
                var copiedObject = jQuery.extend(true, {}, $scope.interactionModel);
                $scope.processContext.interactions.push(copiedObject);
                $scope.clearModel($scope.interactionModel);
            }

            $scope.processContext = {
                processes: [],
                interactions: []
            };
            if ($.isEmptyObject(actualAnnotationModel.annotationItem.object.contexts[0].processContext))
                actualAnnotationModel.annotationItem.object.contexts[0].processContext = $scope.processContext;
            else {
                $scope.processContext = actualAnnotationModel.annotationItem.object.contexts[0].processContext;
                if (!$scope.processContext.hasOwnProperty("processes")) {
                    $scope.processContext.processes = [];
                }
                if (!$scope.processContext.hasOwnProperty("interactions")) {
                    $scope.processContext.interactions = [];
                }
            }

            function createFilterFor(query) {
                return function filterFn(state) {
                    if (state.name != undefined)
                        return (state.name.toLowerCase().indexOf(query.toLowerCase()) != -1);
                    else
                        return (state.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            function createFilterFor(query) {
                return function filterFn(item) {
                    return (item.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            $scope.querySearch = function (query, vocabulary) {
                var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;
                return results;
            }

            $scope.GetNormalizedName = function (name) {
                var re = /_([A-Z])/g;
                var newstr = name.value.replace(re, function ($1, $2) { return ' ' + $2.toLowerCase(); });
                return newstr;
            }
        });
})();