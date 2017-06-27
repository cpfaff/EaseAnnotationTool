(function () {
    'use strict';
    var app = angular.module('incomingRequests', [
      'ngRoute',
      'ui.router',
      'ngAnimate',
      'ngMaterial',
      'incoming-requests.incoming-requests-list'
    ]);

    app.controller('incomingRequestsController', [
        '$scope', '$state', function ($scope, $state) {
            
        }
    ]);
})();