(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('incoming-requests.core.incoming-requests')
        .factory('IncomingRequestsProvider', ['$http', function($http) {

            var IncomingRequestsProvider = function () {

                IncomingRequestsProvider.prototype.get = function () {
                    return $http.get('/api/AccessResources/GetRequestsToMe');
                }

                IncomingRequestsProvider.prototype.getRequestMessages = function (conversationId) {
                    return $http.get('/api/AccessResources/GetMessages?conversationId=' + conversationId);
                }

                IncomingRequestsProvider.prototype.sendMessage = function (requestMesageModel) {
                    return $http.post('/api/AccessResources/AddMessage', requestMesageModel);
                }

                IncomingRequestsProvider.prototype.acceptRequest = function (requestMesageModel) {
                    return $http.post('/api/AccessResources/AcceptRequest', requestMesageModel);
                }

                IncomingRequestsProvider.prototype.declineRequest = function (requestMesageModel) {
                    return $http.post('/api/AccessResources/DeclineRequest', requestMesageModel);
                }
            };

            return new IncomingRequestsProvider();
        }]);
})();