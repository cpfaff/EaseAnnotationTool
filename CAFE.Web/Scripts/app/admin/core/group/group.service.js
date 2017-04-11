(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('admin.core.group')
        .factory('GroupProvider', ['$http', function($http) {

            var GroupProvider = function () {

                var Group = function() {
                    this.Id = '';
                    this.Name = '';
                };

                GroupProvider.prototype.new = function () {
                    return new Group();
                }

                GroupProvider.prototype.add = function (group) {
                    return $http.post('api/Groups/AddGroup', group);
                }

                GroupProvider.prototype.get = function () {
                    return $http.get('api/Groups/GetGroupList');
                }

                GroupProvider.prototype.update = function (group) {
                    return $http.post('api/Groups/UpdateGroup', group);
                }

                 GroupProvider.prototype.delete = function (group) {
                    return $http.post('api/Groups/DeleteGroup', group);
                }

                GroupProvider.prototype.deleteUserFromGroup = function (user, group) {
                    return $http.post('api/Groups/DeleteUserFromGroup', { user: user, group: group });
                }

                GroupProvider.prototype.updateGroup = function (data) {
                    return $http.post('api/Groups/UpdateGroup', data);
                }

                 GroupProvider.prototype.searchUsers = function (user) {
                     return $http.post('api/Groups/SearchUsers', user);
                }

            };

            return new GroupProvider();
        }]);

})();