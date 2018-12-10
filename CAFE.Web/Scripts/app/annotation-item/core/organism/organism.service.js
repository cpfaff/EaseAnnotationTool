(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('annotationItem.core.organism')
        .factory('OrganismProvider', ['$http', function ($http) {
            var OrganismProvider = function () {
            };

            OrganismProvider.prototype.get = function () {
                return $http.get('Api/AnnotationItem/GetOrganismVocabularies');
            }

            OrganismProvider.prototype.getOrganismProperyValues = function (params) {
                return $http.post('/Api/AnnotationItem/SearchOrganismProperyValues', params);
            }

            OrganismProvider.prototype.getOrganisms = function (params) {
                return $http.post('/Api/AnnotationItem/SearchOrganismSpecifies', params);
            }
            
            return new OrganismProvider();
        }]);

})();