(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('annotationItem.core.vocabularies')
        .factory('VocabulariesProvider', ['$http', function ($http) {
            var VocabulariesProvider = function () {
            };

            VocabulariesProvider.prototype.search = function (searchToken, elementId) {
                return $http.get('Api/AnnotationItem/SearchVocabularies?searchToken=' + searchToken + '&elementId=' + elementId);
            }
            return new VocabulariesProvider();
        }]);

})();