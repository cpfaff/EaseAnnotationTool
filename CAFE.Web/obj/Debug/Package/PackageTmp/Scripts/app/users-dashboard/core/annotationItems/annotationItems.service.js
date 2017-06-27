(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('usersDashboard.core.annotationItems')
        .factory('AnnotationItemsProvider', ['$http', function($http) {

            var AnnotationItemsProvider = function () {

                AnnotationItemsProvider.prototype.get = function () {
                    return $http.get('api/AnnotationItem/GetAnnotationItems');
                }

                AnnotationItemsProvider.prototype.delete = function (models) {
                    return $http.post('api/AnnotationItem/AnnotationItemsDelete', models);
                    /*
                    return $http({
                        method: 'DELETE',
                        url: "api/AnnotationItem/DeleteAnnotationItems",
                        data: models,
                        headers: { 'Content-Type': 'application/json' }
                    });
                    */
                }

                AnnotationItemsProvider.prototype.export = function (models) {
                    return $http({
                        method: 'POST',
                        url: "api/AnnotationItem/ExportAnnotationItems",
                        data: models,
                        headers: { 'Content-Type': 'application/json' }
                    });
                }

                AnnotationItemsProvider.prototype.setAIAccessMode = function (models) {
                    return $http.post('api/AnnotationItem/SetAnnotationItemsAccessMode', models);
                }

                AnnotationItemsProvider.prototype.import = function (model) {
                    return $http.post('api/AnnotationItem/Import', model);
                }


                AnnotationItemsProvider.prototype.getUserFiles = function () {
                    return $http.get('api/UserFiles/GetUserFilesList?userId=');
                }

                AnnotationItemsProvider.prototype.searchUsersAndGroups = function (keyWord) {
                    return $http.post('api/UserFiles/SearchUsersAndGroups', { KeyWord: keyWord }, { headers: { 'Content-Type': 'application/json' } });
                }

            };
            return new AnnotationItemsProvider();
        }]);
})();