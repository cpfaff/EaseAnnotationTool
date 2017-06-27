(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('annotationItem.core.method')
        .factory('MethodProvider', ['$http', function ($http) {
            var MethodProvider = function () {};

            MethodProvider.prototype.get = function () {
                return $http.get('Api/AnnotationItem/GetMethodVocabularies');
            }

            return new MethodProvider();
        }]);

})();