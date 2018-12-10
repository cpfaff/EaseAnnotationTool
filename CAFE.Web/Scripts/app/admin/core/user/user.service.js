(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('admin.core.user')
        .factory('UserProvider', ['$http', function($http) {

            var UserProvider = function() {

                var User = function() {
                    this.id = '';
                    this.email = '';
                    this.phoneNumber = '';
                    this.userName = '';
                    this.name = '';
                    this.surname = '';
                    this.postalAddress = '';
                    this.isActive = true;
                    this.role = "";
                };

                UserProvider.prototype.new = function() {
                    return new User();
                }

                UserProvider.prototype.get = function () {
                    return $http.get('api/Users/GetActiveUserList');
                }

                UserProvider.prototype.update = function(user) {
                    return $http.post('api/Users/UpdateUser', user);
                }

                UserProvider.prototype.deleteUsers = function (ids) {
                    return $http.post('Api/Users/DeleteUsers', ids);
                }

                UserProvider.prototype.deleteUserAcceptances = function (ids) {
                    return $http.post('Api/Users/DeleteUserAcceptances', ids);
                }


                UserProvider.prototype.getUserAcceptance = function() {
                    return $http.post('api/Users/GetUnnacceptedUsersList');
                }

                UserProvider.prototype.acceptUser = function (userId) {
                    return $http.post('api/Users/AcceptUser',  JSON.stringify(userId));
                }

                UserProvider.prototype.getUserFiles = function (userId) {
                    return $http({
                        method: 'GET',
                        url: "api/UserFiles/GetUserFilesList",
                        params: { userId: userId },
                        headers: { 'Content-Type': 'application/json' }
                    });
                }

                UserProvider.prototype.getUserAI = function (userId) {
                    return $http({
                        method: 'GET',
                        url: "api/AnnotationItem/GetUserAnnotationItems",
                        params: { userId: userId },
                        headers: { 'Content-Type': 'application/json' }
                    });
                }

                UserProvider.prototype.searchUsers = function (user) {
                    return $http.post('api/Groups/SearchUsers', user);
                }

                 UserProvider.prototype.switchUserFileOwner = function (fileId, newUserId) {
                    return $http.post('api/UserFiles/SwitchUserFileOwner', {UserFileId: fileId, UserFileNewUserId: newUserId});
                }

                 UserProvider.prototype.deleteUserFile = function (filesModel) {
                     /*
                     return $http({
                        method: 'DELETE',
                        url: "api/UserFiles/DeleteUserFiles",
                        data: filesModel,
                        headers: { 'Content-Type': 'application/json' }
                    });
                    */
                     return $http.post('api/UserFiles/UserFilesDelete', { UserFileId: fileId, UserFileNewUserId: newUserId });
                 }

                 UserProvider.prototype.setFilesAccessMode = function (fileModels) {
                     return $http.post('api/UserFiles/SetFilesAccessMode', fileModels);
                 }

                 UserProvider.prototype.setAIAccessMode = function (models) {
                     return $http.post('api/AnnotationItem/SetAnnotationItemsAccessMode', models);
                 }


                 UserProvider.prototype.searchUsersAndGroups = function (keyWord) {
                     return $http.post('api/UserFiles/SearchUsersAndGroups', { KeyWord: keyWord }, { headers: { 'Content-Type': 'application/json' } });
                 }
            };
            return new UserProvider();
        }]);

})();