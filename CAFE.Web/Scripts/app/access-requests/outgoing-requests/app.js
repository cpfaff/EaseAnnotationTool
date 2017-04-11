(function () {
    'use strict';
    var app = angular.module('outgoingRequests', [
      'ngRoute',
      'ui.router',
      'ngAnimate',
      'ngMaterial',
      'outgoing-requests.outgoing-requests-list'
    ]);

    app.controller('outgoingRequestsController', [
        '$scope', '$state', function ($scope, $state) {
            
        }
    ]);
})();