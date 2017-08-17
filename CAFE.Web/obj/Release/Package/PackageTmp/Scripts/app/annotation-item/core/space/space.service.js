(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('annotationItem.core.space')
        .factory('SpaceProvider', ['$http', function($http) {
            var SpaceProvider = function () {
            };

            SpaceProvider.prototype.get = function () {
                return $http.get('Api/AnnotationItem/GetSpaceVocabularies');
            }

            SpaceProvider.prototype.getCoordinatesFromCSV = function (data) {
                return $http.post('/Api/AnnotationItem/GetAIDataArrayFromCSV', data);
            };

            SpaceProvider.prototype.getLocations = function (data) {
                return $http({
                    url: '/Api/AnnotationItem/GetGeoLocations', 
                    method: "GET",
                    params: data
                });
            };

            SpaceProvider.prototype.getCSVHeaders = function (data) {
                return $http.post('/Api/AnnotationItem/CreateCSVHeadersFile', data);

            };

            return new SpaceProvider();
        }]);

})();