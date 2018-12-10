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

                AnnotationItemsProvider.prototype.newWithFiles = function (data, mode, id) {
                    if (mode && mode == 'uploading') {
                        if (id) {
                            return $http({
                                method: 'POST',
                                url: "api/AnnotationItem/AddFromNewFilesAndExistingAnnotaions?id=" + id,
                                data: data,
                                transformRequest: angular.identity,
                                headers: { 'Content-Type': undefined }
                            });
                        } else {
                            return $http({
                                method: 'POST',
                                url: "api/AnnotationItem/AddFromNewFiles",
                                data: data,
                                transformRequest: angular.identity,
                                headers: { 'Content-Type': undefined }
                            });
                        }
                    } else if (mode && mode == 'selection') {
                        if (id) {
                            return $http({
                                method: 'POST',
                                url: "api/AnnotationItem/AddFromExistingFilesAndExistingAnnotaions",
                                data: { filesIds: data, annotationItemId: id }
                            });
                        } else {
                            return $http({
                                method: 'POST',
                                url: "api/AnnotationItem/AddFromExistingFiles",
                                data: { filesIds: data }
                            });
                        }
                    } else {
                        return null;
                    }
                }
                AnnotationItemsProvider.prototype.updateWithFiles = function (data, mode, id) {
                    if (mode && mode == 'uploading') {
                        return $http({
                            method: 'POST',
                            url: "api/AnnotationItem/UpdateFromNewFiles?id=" + id,
                            data: data,
                            transformRequest: angular.identity,
                            headers: { 'Content-Type': undefined }
                        });
                    } else if (mode && mode == 'selection') {
                        return $http({
                            method: 'POST',
                            url: "api/AnnotationItem/UpdateFromExistingFiles",
                            data: { filesIds: data, annotationItemId: id }
                        });
                    } else {
                        return null;
                    }
                }
            };
            return new AnnotationItemsProvider();
        }]);
})();