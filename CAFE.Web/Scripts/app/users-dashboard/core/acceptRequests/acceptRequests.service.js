(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('usersDashboard.core.acceptRequests')
        .factory('AcceptRequestsProvider', ['$http', function($http) {

            var AcceptRequestsProvider = function () {
                AcceptRequestsProvider.prototype.get = function () {
                    return $http.get('api/AccessResources/GetAllAccessRequests');
                }

                AcceptRequestsProvider.prototype.getMessages = function (conversationId) {
                    return $http.get('api/AccessResources/GetMessages?conversationId=' + conversationId);
                }

                AcceptRequestsProvider.prototype.sendMessage = function (requestMesageModel) {
                    return $http.post('/api/AccessResources/AddMessage', requestMesageModel);
                }

                AcceptRequestsProvider.prototype.acceptRequest = function (requestMesageModel) {
                    return $http.post('/api/AccessResources/AcceptRequest', requestMesageModel);
                }

                AcceptRequestsProvider.prototype.declineRequest = function (requestMesageModel) {
                    return $http.post('/api/AccessResources/DeclineRequest', requestMesageModel);
                }

            };
            return new AcceptRequestsProvider();
        }]);
})();