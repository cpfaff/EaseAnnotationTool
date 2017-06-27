(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('annotationItem.core.biome')
        .factory('BiomeProvider', ['$http', function($http) {
            var BiomeProvider = function () {
            };

            BiomeProvider.prototype.get = function () {
                return $http.get('Api/AnnotationItem/GetBiomeVocabularies');
            }

            return new BiomeProvider();
        }]);

})();