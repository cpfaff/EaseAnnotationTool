(function () {
    'use strict';
    var app = angular.module('usersDashboard', [
      'ngRoute',
      'ui.router',
      'ngAnimate',
      'ngMaterial',
      'lfNgMdFileInput',
      'ngTagsInput',
      'usersDashboard.acceptRequests',
      'usersDashboard.filesList',
      'usersDashboard.unacceptedUsers',
      'usersDashboard.annotationItems'
    ]);

    app.controller('userDashboardController', [
        '$scope', '$state', '$http', function ($scope, $state, $http) {

        }
    ]);
})();