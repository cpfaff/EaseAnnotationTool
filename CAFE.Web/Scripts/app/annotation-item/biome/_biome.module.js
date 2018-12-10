(function () {
    'use strict';

    angular.module('annotationItem.biome', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core']).
        config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('BiomeRootControler', function (BiomeProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider, VocabulariesProvider) {

            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();

            $scope.accordionOpenStatuses2 = {
                aquatic: [],
                semiAquatic: [],
                terrestrial: []
            };
            $scope.accordionOpenStatuses = {};
            $scope.mainBiomeDictionary = null;

            function createFilterFor(query) {
                return function filterFn(item) {
                    return (item.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            $scope.querySearch = function (query, key) {
                return VocabulariesProvider.search(query, key).then(function (response) {
                    return response.data;
                });
            }

            $scope.physiognomyModel =
            {
                 physiognomyTypes:
                 [
                     {
                         terrestrialPhysiognomies: [],
                         semiAquaticPhysiognomies: [],
                         aquaticPhysiognomies: []
                     }
                 ]
             };

            $scope.zonoModel = {
                biomeType: null,
                biomeLatitudinalZone: null,
                biomeHumidityType: null,
                biomeContinentalityType: null,
                biomeHemisphere: null
            }

            $scope.biomeModel = {
                zonoBiomes: [],
                oroBiomes: [],
                pedoBiomes: [],
                physiognomies:
                [
                ],
                landUses: []
            }

            if ($.isEmptyObject(actualAnnotationModel.annotationItem.object.contexts[0].biomeContext))
                actualAnnotationModel.annotationItem.object.contexts[0].biomeContext = $scope.biomeModel;
            else {
                $scope.biomeModel = actualAnnotationModel.annotationItem.object.contexts[0].biomeContext;

                if (!$scope.biomeModel.oroBiomes)
                    $scope.biomeModel.oroBiomes = [];

                if (!$scope.biomeModel.pedoBiomes)
                    $scope.biomeModel.pedoBiomes = [];

                if (!$scope.biomeModel.hasOwnProperty("physiognomies")) {
                    $scope.biomeModel.physiognomies = [];
                }
            }
          
            $scope.aquaticModel = {
                plantCharacterizedAquaticPhysiognomyType: null,
                habitatCharacterizedAquaticPhysiognomy: null
            };

            $scope.landuseViewModel = {
                landUseType: null,
                landUseForm: null
            }

            BiomeProvider.get().then(function (result) {
                $scope.mainBiomeDictionary = result.data;

                
                $scope.oroDictionary = $scope.mainBiomeDictionary.biomeVocabularies.oroVocabulary.oroBiomeType;
                $scope.pedoDictionary = $scope.mainBiomeDictionary.biomeVocabularies.pedoVocabulary.pedoBiomeType;
                $scope.zonoDictionaries = $scope.mainBiomeDictionary.biomeVocabularies.zonoVocabulary;
                $scope.terrestrialDictionaries = $scope.mainBiomeDictionary.physiognomyVocabularies.terrestrialVocabulary.terrestrialPhysiognomyType;
                $scope.terrestrialRootElements = ['Forest', 'Open Woodland', 'Shrubland', 'Herbaceous System', 'Barren Land'];

                $scope.semiAquaticDictionary = $scope.mainBiomeDictionary.physiognomyVocabularies.semiAquaticVocabulary.semiAquaticPhysiognomyType;
                $scope.semiAquaticRootElements = ['Mire', 'Saltmarsh'];

                $scope.aquaticDictionaries = $scope.mainBiomeDictionary.physiognomyVocabularies.aquaticVocabulary;
                $scope.landuseDictionaries = $scope.mainBiomeDictionary.physiognomyVocabulary;
            });

            $scope.ClearModel = function (mainModel) {
                for (var item in mainModel)
                    mainModel[item] = null;
            }

            $scope.AddZono = function () {
                var copiedObject = jQuery.extend(true, {}, $scope.zonoModel);
                if (!$scope.biomeModel.zonoBiomes)
                    $scope.biomeModel.zonoBiomes = [];
                $scope.biomeModel.zonoBiomes.push(copiedObject);
                $scope.ClearModel($scope.zonoModel);
            }

            $scope.physiognomyComponents = [
                { name: 'terrestrial', title: 'Terrestrial', type: 'terrestrialPhysiognomyType' },
                { name: 'semi-aquatic', title: 'Semi Aquatic', type: 'semiAquaticPhysiognomyType' },
                { name: 'aquatic', title: 'Aquatic', type: 'aquaticPhysiognomyType' }
            ];
            

            $scope.addOrRemoveItem = function (item, array) {
                var indexInArray = array.indexOf(item);

                if (indexInArray == -1)
                    array.push(item);
                else
                    array.splice(indexInArray, 1);
            }

            $scope.addAquaticItem = function () {
                var copiedObject = jQuery.extend({}, $scope.aquaticModel);
                if (!$scope.physiognomyModel.physiognomyTypes[0].aquaticPhysiognomies)
                    $scope.physiognomyModel.physiognomyTypes[0].aquaticPhysiognomies = [];
                $scope.physiognomyModel.physiognomyTypes[0].aquaticPhysiognomies.push(copiedObject);
                $scope.ClearModel($scope.aquaticModel);
            }

            $scope.addLanduseItem = function () {
                var copiedObject = jQuery.extend(true, {}, $scope.landuseViewModel);
                if (!$scope.biomeModel.landUses)
                    $scope.biomeModel.landUses = [];
                $scope.biomeModel.landUses.push(copiedObject);
                $scope.ClearModel($scope.landuseViewModel);
            }

            $scope.AddPhysiognomy = function () {
                var copiedObject = jQuery.extend(true, {}, $scope.physiognomyModel);
                if (!$scope.biomeModel.physiognomies)
                    $scope.biomeModel.physiognomies = [];
                $scope.biomeModel.physiognomies.push(copiedObject);

                $scope.physiognomyModel.physiognomyTypes[0].terrestrialPhysiognomies = [];
                $scope.physiognomyModel.physiognomyTypes[0].semiAquaticPhysiognomies = [];
                $scope.physiognomyModel.physiognomyTypes[0].aquaticPhysiognomies = [];
                $scope.ClearModel($scope.aquaticModel);
            }
        });
})();