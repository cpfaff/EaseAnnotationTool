(function () {
    'use strict';

    angular.module('annotationItem.method', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core']).
        config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('annotationMethodController', function (MethodProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider) {
            $scope.methodContext = actualAnnotationModel.annotationItem.object.contexts[0].methodContext;
            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
            $scope.approachModel = {};
            $scope.factorModel = {};

            if ($.isEmptyObject($scope.methodContext))
            {
                $scope.methodContext = {
                    approaches: [],
                    factors: [],
                    dataFormats: []
                };

                actualAnnotationModel.annotationItem.object.contexts[0].methodContext = $scope.methodContext;
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

            $scope.AddFactor = function () {
                var factorModel = jQuery.extend(true, {}, $scope.factorModel);

                if (!$scope.methodContext.factors)
                    $scope.methodContext.factors = [];

                $scope.methodContext.factors.push(factorModel);

                $scope.factorModel = {};
            }

            $scope.AddApproach = function () {
                var approachModel = jQuery.extend(true, {}, $scope.approachModel);

                if (!$scope.methodContext.approaches)
                    $scope.methodContext.approaches = [];

                $scope.methodContext.approaches.push(approachModel);

                $scope.approachModel = {};
            }
            
            MethodProvider.get().then(function (result) {
                $scope.methodDictionaries = jQuery.extend(true, {}, result.data);
            });
        });
})();