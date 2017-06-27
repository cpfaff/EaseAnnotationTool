(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('annotationItem.core.time')
        .factory('TimeProvider', ['$http', function($http) {
            var TimeProvider = function () {
            };

            TimeProvider.prototype.get = function () {
                return $http.get('Api/AnnotationItem/GetTimeVocabularies');
            }
            TimeProvider.prototype.test = function () {
                return $http.get('Api/AnnotationItem/Test');
            }
            return new TimeProvider();
        }]);

})();