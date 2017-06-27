(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('annotationItem.core.chemical')
        .factory('ChemicalProvider', ['$http', function($http) {
            var ChemicalProvider = function () {};

            ChemicalProvider.prototype.get = function () {
                return $http.get('Api/AnnotationItem/GetChemicalVocabularies');
            }

            return new ChemicalProvider();
        }]);

})();