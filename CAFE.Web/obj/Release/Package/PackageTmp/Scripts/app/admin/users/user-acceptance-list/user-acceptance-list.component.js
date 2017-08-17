(function () {
    'use strict';

    angular
        .module('admin.userAcceptanceList')
        .component('userAcceptanceList', {
            templateUrl: 'Scripts/app/admin/users/user-acceptance-list/user-acceptance-list.template.html',
            controller: ['UserProvider', '$scope', '$mdDialog', '$mdToast',
              function (UserProvider, $scope, $mdDialog, $mdToast) {

                  var bookmark;

                  $scope.filter = {
                      options: {
                          debounce: 500
                      }
                  };

                  var last = {
                      bottom: false,
                      top: true,
                      left: false,
                      right: true
                  };
                  $scope.toastPosition = angular.extend({}, last);
                  $scope.getToastPosition = function () {
                      sanitizePosition();

                      return Object.keys($scope.toastPosition)
                        .filter(function (pos) { return $scope.toastPosition[pos]; })
                        .join(' ');
                  };
                  function sanitizePosition() {
                      var current = $scope.toastPosition;

                      if (current.bottom && last.top) current.top = false;
                      if (current.top && last.bottom) current.bottom = false;
                      if (current.right && last.left) current.left = false;
                      if (current.left && last.right) current.right = false;

                      last = angular.extend({}, current);
                  }

                  $scope.query = {
                      filter: '',
                      limit: '10',
                      order: 'NameToLower',
                      page: 1
                  };


                  function success(response) {
                      $scope.users = response.data;
                  }


                  $scope.accept = function (user, event) {
                      var confirm = $mdDialog.confirm()
                            .title('Would you like to accept user?')
                            .textContent('User ' + user.name + '(' + user.email + ') will be accepted.')
                            .ariaLabel('Lucky day')
                            .targetEvent(event)
                            .ok('Accept')
                            .cancel('Cancel');

                      $mdDialog.show(confirm).then(function () {
                          UserProvider.acceptUser(user.userId).then($scope.getUsers);

                          var pinTo = $scope.getToastPosition();

                          $mdToast.show(
                            $mdToast.simple()
                              .textContent(user.name + ' ' + user.email + ' accepted')
                              .position(pinTo)
                              .hideDelay(3000)
                          );
                      }, function () {
                          
                      });
                  };


                  $scope.getUsers = function () {
                      var prom = UserProvider.getUserAcceptance($scope.query);
                      $scope.promise = prom.then(success);
                  };

                  $scope.removeFilter = function () {
                      $scope.filter.show = false;
                      $scope.query.filter = '';

                      if ($scope.filter.form.$dirty) {
                          $scope.filter.form.$setPristine();
                      }
                  };

                  $scope.$watch('query.filter', function (newValue, oldValue) {
                      if (!oldValue) {
                          bookmark = $scope.query.page;
                      }

                      if (newValue !== oldValue) {
                          $scope.query.page = 1;
                      }

                      if (!newValue) {
                          $scope.query.page = bookmark;
                      }

                      $scope.getUsers();
                  });

              }
            ]
        });


    angular
        .module('admin.userAcceptanceList')
        .controller('ToastCtrl', function ($scope, $mdToast) {
            $scope.closeToast = function() {
                $mdToast.hide();
            };
    });
})();