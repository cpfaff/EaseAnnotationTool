(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('admin.core.masterdata')
        .factory('MasterdataProvider', ['$http', function($http) {

            var MasterdataProvider = function () {

                //MasterdataProvider.prototype.get = function () {
                //    return $http.get('api/AnnotationItem/GetUsersValues');
                //}
                MasterdataProvider.prototype.get = function () {
                    return $http.get('api/AnnotationItem/GetAllVocabularyValues');
                }
                MasterdataProvider.prototype.put = function (data) {
                    return $http.post('api/AnnotationItem/AcceptUserMetadata', data);
                }
                MasterdataProvider.prototype.saveGlobal = function (data) {
                    return $http.post('api/AnnotationItem/SaveGlobalVocabularyValue', data);
                }
                //MasterdataProvider.prototype.addGlobal = function (data) {
                //    return $http.post('api/AnnotationItem/AddGlobalVocabularyValue', data);
                //}
                //MasterdataProvider.prototype.updateGlobal = function (data) {
                //    return $http.post('api/AnnotationItem/UpdateGlobalVocabularyValue', data);
                //}
                MasterdataProvider.prototype.deleteGlobal = function (data) {
                    return $http.post('api/AnnotationItem/DeleteGlobalVocabularyValue', data);
                }
                MasterdataProvider.prototype.getVocabularyTypes = function () {
                    return $http.get('api/AnnotationItem/GetVocabularyTypes');
                }

                
            };

            return new MasterdataProvider();
        }]);

})();