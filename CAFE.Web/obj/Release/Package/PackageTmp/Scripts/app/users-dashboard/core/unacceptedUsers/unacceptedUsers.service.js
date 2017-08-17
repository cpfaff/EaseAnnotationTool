(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('usersDashboard.core.unacceptedUsers')
        .factory('UnacceptedUsersProvider', ['$http', function($http) {
            var UnacceptedUsersProvider = function () {
                UnacceptedUsersProvider.prototype.get = function() {
                    return $http.post('api/Users/GetUnnacceptedUsersList');
                }
                UnacceptedUsersProvider.prototype.acceptUser = function (userId) {
                    return $http.post('api/Users/AcceptUser', JSON.stringify(userId));
                }

            };
            return new UnacceptedUsersProvider();
        }]);
})();