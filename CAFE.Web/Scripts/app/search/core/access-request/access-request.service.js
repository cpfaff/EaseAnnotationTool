(function () {
    angular.module('search.core.accessRequest', [])
            .factory('AccessRequestProvider', ['$http', function ($http) {

                var AccessRequestProvider = function () {

                    var AccessRequest = function () {
                        this.requestSubject = '';
                        this.requestMessage = '';
                        this.requestedResources = [];
                    };

                    var AccessibleResource = function() {
                        this.name = '';
                        this.resourceId = '';
                        this.ownerId = '';
                        this.kind = '';
                    };

                    AccessRequestProvider.prototype.new = function () {
                        return new AccessRequest();
                    }

                    AccessRequestProvider.prototype.newResource = function () {
                        return new AccessibleResource();
                    }

                    AccessRequestProvider.prototype.save = function (accessRequest) {
                        return $http.post('api/AccessResources/CreateRequest', accessRequest);
                    }

                };

                return new AccessRequestProvider();
            }]);

})();
