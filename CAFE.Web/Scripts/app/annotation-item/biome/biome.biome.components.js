(function () {
    'use strict';
    angular
        .module('annotationItem.biome')
        .component('oroComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/biome/oro.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog) {
                $scope.dictionaries = $scope.$parent.$parent.mainBiomeDictionary.BiomeVocabularies.OroVocabulary;
            }
        }).component('pedoComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/biome/pedo.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog) {
                $scope.dictionaries = $scope.$parent.$parent.mainBiomeDictionary.BiomeVocabularies.PedoVocabulary;
            }
        }).component('zonoComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/biome/zono.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog) {
                $scope.mainBiomeDictionary;
                
                $scope.dictionaries = $scope.$parent.$parent.mainBiomeDictionary.BiomeVocabularies.ZonoVocabulary;
                $scope.biomeZones = { biomeZone1: null, biomeZone2: null };
                $scope.zonoModel = {};
                $scope.zonoArray = [];
                $scope.ChangeBiomeZone = function (newZoneIndex, hemisphereIndex)
                {
                    $scope.biomeZones[(0 == hemisphereIndex ? 'biomeZone2' : 'biomeZone1')] = null;
                    $scope.zonoModel.BiomeLatitudinalZone = newZoneIndex;
                    $scope.zonoModel.BiomeHemisphere = hemisphereIndex;
                }
                
                $scope.ClearModel = function (mainModel)
                {
                    for (var item in mainModel)
                        mainModel[item] = null;
                }

                $scope.CopyModel = function (mainModel) {
                    var copiedModel = {};

                    for (var item in mainModel)
                        copiedModel[item] = mainModel[item];

                    return copiedModel;
                }

                $scope.AddZono = function ()
                {
                    $scope.zonoArray.push($scope.CopyModel($scope.zonoModel));
                    $scope.ClearModel($scope.zonoModel);
                    $scope.biomeZones = { biomeZone1: null, biomeZone2: null };
                }
            }
        });
})();

