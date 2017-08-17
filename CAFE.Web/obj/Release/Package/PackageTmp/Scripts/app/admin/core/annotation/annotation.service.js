(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('admin.core.annotation')
        .factory('AnnotationItemProvider', ['$http', function ($http) {

            var AnnotationItemProvider = function () {

                AnnotationItemProvider.prototype.get = function () {
                    return $http.get('api/AnnotationItem/GetAllAnnotationItems');
                }
                AnnotationItemProvider.prototype.changeOwner = function (newOwnerId, annotationIds) {
                    return $http.post('api/AnnotationItem/ChangeAnnotationItemOwner?ownerId=' + newOwnerId, annotationIds);
                }
                AnnotationItemProvider.prototype.delete = function (models) {
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
            };
            return new AnnotationItemProvider();
        }]);

})();