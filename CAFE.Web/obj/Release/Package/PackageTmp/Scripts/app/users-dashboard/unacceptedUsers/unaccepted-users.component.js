(function() {
    'use strict';

    angular
        .module('usersDashboard.unacceptedUsers')
        .component('unacceptedUsersList', {
            templateUrl: 'Scripts/app/users-dashboard/unacceptedUsers/unaccepted-users.template.html',
            controller: ['UnacceptedUsersProvider', '$scope', '$mdDialog', '$mdToast', 'NgTableParams',
              function (UnacceptedUsersProvider, $scope, $mdDialog, $mdToast, NgTableParams) {
                  
                  var bookmark;
                  $scope.selected = [];
                  $scope.selectedGroupName = null;

                  $scope.filter = {
                      options: {
                          debounce: 500
                      }
                  };

                  $scope.query = {
                      filter: '',
                      limit: '10',
                      order: 'NameToLower',
                      page: 1
                  };
                  $scope.unacceptedUsers = null;
                  $scope.tableParams = new NgTableParams({ count: 10 }, {
                      counts: [],
                      paginationMaxBlocks: 13,
                      paginationMinBlocks: 2,
                      dataset: $scope.unacceptedUsers
                  });

                  $scope.getUnacceptedUsers = function () {
                      var prom = UnacceptedUsersProvider.get();
                      $scope.promise = prom.then(function (response) {
                          $scope.unacceptedUsers = Array.isArray(response.data) ? response.data : null;
                          $scope.tableParams.settings({
                              dataset: $scope.unacceptedUsers
                          });
                      });
                  };
                  $scope.checkUser = function (user) {
                      var ind = $scope.selected.indexOf(user);
                      if (ind == -1) {
                          $scope.selected.push(user);
                      } else {
                          $scope.selected.splice(ind, 1);
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


                  $scope.accept = function (userIndex, event) 
                  {
                      var user = $scope.unacceptedUsers[userIndex];
                      var confirm = $mdDialog.confirm()
                            .title('Would you like to accept user?')
                            .textContent('User ' + user.name + '(' + user.email + ') will be accepted.')
                            .ariaLabel('Lucky day')
                            .targetEvent(event)
                            .ok('Accept')
                            .cancel('Cancel');

                      $mdDialog.show(confirm).then(function () {
                          UnacceptedUsersProvider.acceptUser(user.userId).then($scope.getUnacceptedUsers);
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

                  $scope.removeFilter = function() {
                      $scope.filter.show = false;
                      $scope.query.filter = '';

                      if ($scope.filter.form.$dirty) {
                          $scope.filter.form.$setPristine();
                      }
                  };

                  $scope.$watch('query.filter', function(newValue, oldValue) {
                      if (!oldValue) {
                          bookmark = $scope.query.page;
                      }

                      if (newValue !== oldValue) {
                          $scope.query.page = 1;
                      }

                      if (!newValue) {
                          $scope.query.page = bookmark;
                      }

                      $scope.getUnacceptedUsers();
                  });
              }
            ]

        });
    
    angular.
       module('usersDashboard.acceptRequests').
       controller('messagesController', ['AcceptRequestsProvider', '$scope', '$mdDialog', 'messages', 'subject', 'conversationId', 'requestId', 'userName', 'resources', 'reloadFunction',
       function (AcceptRequestsProvider, $scope, $mdDialog, messages, subject, conversationId, requestId, userName, resources, reloadFunction) {
           'use strict';
           $scope.subject = subject;
           $scope.messages = messages;
           $scope.resources = resources;
           
           this.Cancel = $mdDialog.cancel;

           this.Accept = function () {
               var prom = AcceptRequestsProvider.acceptRequest(conversationId);

               prom.then(function (result) {
                   $mdDialog.cancel();
                   reloadFunction();
               });
           }

           this.Decline = function () {
               var prom = AcceptRequestsProvider.declineRequest({ ConversationId: conversationId, Reason: $scope.message });

               prom.then(function (result) {
                   $mdDialog.cancel();
                   reloadFunction();
               });
           }

           this.SendMessage = function () {
               var prom = AcceptRequestsProvider.sendMessage({ text: $scope.message, conversationId: conversationId });

               prom.then(function (result) {
                   $scope.message = null;
                   $scope.messageForm.$setUntouched();
                   $scope.messages.push({
                       sender: result.data.sender,
                       text: result.data.text,
                       creationDate: result.data.creationDate
                   });
               });
           }
       }]);
})();