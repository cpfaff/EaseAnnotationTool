(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('usersDashboard.core.files')
        .factory('FilesProvider', ['$http', function($http) {

            var FilesProvider = function () {

                var File = function() {
                    this.Description = '';
                    this.AccessMode = '';
                    this.UsersAndGroups = '';
                    this.Files = '';
                };

                FilesProvider.prototype.searchAI = function (keyWord) {
                    return $http({
                        method: 'POST',
                        url: "api/AnnotationItem/SearchAI",
                        data: JSON.stringify(keyWord),
                    });
                };

                FilesProvider.prototype.new = function () {
                    return new File();
                }

                FilesProvider.prototype.add = function (data) {
                   return $http.post('api/UserFiles/AddUserFile', data, {
                       transformRequest: angular.identity,
                       headers: { 'Content-Type': undefined }
                   });
                }

                FilesProvider.prototype.get = function () {
                    return $http.get('api/UserFiles/GetUserFilesList?userId=');
                }

                FilesProvider.prototype.update = function (fileModel) {
                    return $http.post('api/UserFiles/UpdateUserFile', fileModel);
                }

                FilesProvider.prototype.delete = function (filesModel) {
                    return $http.post('api/UserFiles/UserFilesDelete', filesModel);
                    /*
                    return $http({
                        method: 'DELETE',
                        url: "api/UserFiles/DeleteUserFiles",
                        data: filesModel,
                        headers: { 'Content-Type': 'application/json' }
                    });
                    */
                }

                FilesProvider.prototype.setFilesAccessMode = function (fileModels) {
                    return $http.post('api/UserFiles/SetFilesAccessMode', fileModels);
                }

                FilesProvider.prototype.searchUsersAndGroups = function (keyWord) {
                    return $http.post('api/UserFiles/SearchUsersAndGroups', { KeyWord: keyWord }, { headers: { 'Content-Type': 'application/json' } });
                }

            };

            return new FilesProvider();
        }]);
})();