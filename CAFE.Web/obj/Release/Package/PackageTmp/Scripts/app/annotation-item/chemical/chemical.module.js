(function () {
    'use strict';

    angular.module('annotationItem.chemical', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core']).
        config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('annotationChemicalController', function (ChemicalProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider) {
            $scope.chemicalContext = actualAnnotationModel.annotationItem.object.contexts[0].chemicalContext;
            
            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();

            if ($.isEmptyObject($scope.chemicalContext)) {
                $scope.chemicalContext = {
                    elements: [],
                    isotopes: [],
                    compounds: [],
                    functions: []
                };

                actualAnnotationModel.annotationItem.object.contexts[0].chemicalContext = $scope.chemicalContext;
            }

            $scope.compondModel = {
                compoundName: null,
                compoundClass: [],
                compoundType: []
            };

            $scope.GetNormalizedIsotopeName = function (name) {
                var re = /^Item/;
                var newstr = name.replace(re, '');
                return newstr;
            }

            $scope.GetNormalizedName = function (name) {
                var re = /_([A-Z])/g;
                var newstr = name.value.replace(re, function ($1, $2) { return ' ' + $2.toLowerCase(); });
                return newstr;
            }

            $scope.ClearModel = function (mainModel) {
                for (var item in mainModel) {
                    if (Array.isArray(mainModel[item]))
                        mainModel[item] = [];
                    else
                        mainModel[item] = null;
                }
            }

            $scope.AddCompound = function()
            {
                var copiedObject = jQuery.extend(true, {}, $scope.compondModel);
                if (!$scope.chemicalContext.compounds)
                    $scope.chemicalContext.compounds = [];

                $scope.chemicalContext.compounds.push(copiedObject);
                $scope.ClearModel($scope.compondModel);
            }

            function createFilterFor(query) {
                return function filterFn(state) {
                    if (state.name != undefined)
                        return (state.name.toLowerCase().indexOf(query.toLowerCase()) != -1);
                    else
                        return (state.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };

            }

            $scope.querySearch = function (query, vocabulary) {
                $scope.chemicalContext
                var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;
                return results;
            }

            $scope.autocompleteFields = { searchText: null }

            ChemicalProvider.get().then(function (result) {
                $scope.chemicalDictionaries = result.data;

                $scope.chemicalDictionaries.compoundClassVocabulary.forEach(function (item, i) {
                    $scope.chemicalDictionaries.compoundClassVocabulary[i] = { name: $scope.GetNormalizedName(item), value: item };
                });

                $scope.chemicalDictionaries.compoundTypeVocabulary.forEach(function (item, i) {
                    $scope.chemicalDictionaries.compoundTypeVocabulary[i] = { name: $scope.GetNormalizedName(item), value: item };
                });

                if (!$scope.chemicalContext.elements)
                    $scope.chemicalContext.elements = [];

                if (!$scope.chemicalContext.isotopes)
                    $scope.chemicalContext.isotopes = [];

                if (!$scope.chemicalContext.compounds)
                    $scope.chemicalContext.compounds = [];

                if (!$scope.chemicalContext.functions)
                    $scope.chemicalContext.functions = [];
            });
        });
})();