(function () {
    'use strict';
    var app = angular.module('usersDashboard', [
      'ngRoute',
      'ui.router',
      'ngAnimate',
      'ngMaterial',
      "ngTable",
      'lfNgMdFileInput',
      'ngTagsInput',
      'usersDashboard.acceptRequests',
      'usersDashboard.filesList',
      'usersDashboard.unacceptedUsers',
      'usersDashboard.annotationItems',
      'mgo-angular-wizard'
    ]);

    app.controller('userDashboardController', [
        '$scope', '$state', '$http', function ($scope, $state, $http) {
            $scope.openedCommunications = false;
        }
    ]);
})();