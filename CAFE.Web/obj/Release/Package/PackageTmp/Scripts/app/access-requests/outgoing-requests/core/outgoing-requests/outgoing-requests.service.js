(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('outgoing-requests.core.outgoing-requests')
        .factory('outgoingRequestsProvider', ['$http', function($http) {

            var outgoingRequestsProvider = function () {

                outgoingRequestsProvider.prototype.get = function () {
                    return $http.get('/api/AccessResources/GetRequestsFromMe');
                }

                outgoingRequestsProvider.prototype.getRequestMessages = function (conversationId) {
                    return $http.get('/api/AccessResources/GetMessages?conversationId=' + conversationId);
                }

                outgoingRequestsProvider.prototype.sendMessage = function (requestMesageModel) {
                    return $http.post('/api/AccessResources/AddMessage', requestMesageModel);
                }
            };

            return new outgoingRequestsProvider();
        }]);
})();