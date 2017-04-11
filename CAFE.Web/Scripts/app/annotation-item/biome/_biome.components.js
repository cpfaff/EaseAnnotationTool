(function () {
    'use strict';
    angular
        .module('annotationItem.biome')
        .component('biomeComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/biome/biome-main.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog, $controller) {

            }
        }).component('physiognomyComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/physiognomy/_physiognomy-main.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog, $controller) {

            }
        }).component('landuseComponent', {
            templateUrl: 'Scripts/app/annotation-item/biome/templates/landuse.template.html',
            controller: function (BiomeProvider, $scope, $mdDialog, $controller) {
                $scope.dictionaries = $scope.$parent.mainBiomeDictionary.PhysiognomyVocabulary;
            }
        });
})();

