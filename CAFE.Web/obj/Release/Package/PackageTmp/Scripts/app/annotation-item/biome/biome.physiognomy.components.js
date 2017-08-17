(function () {
    'use strict';
    angular
        .module('annotationItem.biome')
        .component('terrestrialComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/physiognomy/terrestrial.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog) {
                $scope.dictionary = $scope.$parent.$parent.mainBiomeDictionary.PhysiognomyVocabularies.TerrestrialVocabulary.TerrestrialPhysiognomyType;
                $scope.rootElements = ['Forest', 'Open Woodland', 'Shrubland', 'Herbaceous System', 'Barren Land'];
            }
        }).component('semiAquaticComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/physiognomy/semi-aquatic.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog) {
                $scope.dictionary = $scope.$parent.$parent.mainBiomeDictionary.PhysiognomyVocabularies.SemiAquaticVocabulary.SemiAquaticPhysiognomyType;
                $scope.rootElements = ['Mire', 'Saltmarsh'];
            }
        }).component('aquaticComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/physiognomy/aquatic.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog) {
                $scope.dictionaries = $scope.$parent.$parent.mainBiomeDictionary.PhysiognomyVocabularies.AquaticVocabulary;
            }
        });
})();
