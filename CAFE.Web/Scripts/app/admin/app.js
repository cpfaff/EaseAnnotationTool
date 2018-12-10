(function () {
    'use strict';

    var app = angular.module('admin', [
      'ngRoute',
      'ui.router',
      'ngAnimate',
      'ngMaterial',
      'ngSanitize',
      'admin.userList',
      'admin.annotationList',
      'admin.userAcceptanceList',
      'admin.groupList',
      'admin.masterdata',
      'admin.gfbio'
    ]);

    app.config(function ($stateProvider, $urlRouterProvider) {
        //
        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/security");

        $stateProvider
            .state('admin', {
                abstract: true,
                templateUrl: "Scripts/app/admin/views/common.html"
            });
        //
        // Now set up the states
        $stateProvider
            .state('admin.security', {
                url: "/security",
                templateUrl: "Scripts/app/admin/views/security.html"
            })
            .state('admin.acceptance', {
                url: "/acceptance",
                templateUrl: "Scripts/app/admin/views/acceptance.html"
            }).state('admin.masterdata', {
                url: "/masterdata",
                templateUrl: "Scripts/app/admin/views/masterdata.html"
            }).state('admin.annotation', {
                url: "/annotation",
                templateUrl: "Scripts/app/admin/views/annotation.html"
            }).state('admin.gfbio', {
                url: "/gfbio",
                templateUrl: "Scripts/app/admin/views/gfbio.html"
            });
    });

    app.controller('adminController', [
        '$scope', '$state', function ($scope, $state) {
            $scope.navigateTo = function(path, e) {
                $state.go(path);
            }

            $scope.activeMenuItem = $state;
        }
    ]);
})();