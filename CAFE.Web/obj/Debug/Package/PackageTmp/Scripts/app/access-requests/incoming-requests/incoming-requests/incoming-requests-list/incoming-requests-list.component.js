(function () {
    'use strict';

    angular
        .module('incoming-requests.incoming-requests-list')
        .component('incomingRequests', {
            templateUrl: '/Scripts/app/access-requests/incoming-requests/incoming-requests/incoming-requests-list/incoming-requests-list.template.html',
            controller: ['IncomingRequestsProvider', '$scope', '$mdDialog',
              function (IncomingRequestsProvider, $scope, $mdDialog) {

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
                      limit: 50,
                      order: 'creationDate',
                      page: 1
                  };

                  $scope.showMessages = function (event, request) {
                      IncomingRequestsProvider.getRequestMessages(request.id).then(function (response) {
                          $mdDialog.show({
                              clickOutsideToClose: true,
                              controller: 'messagesController',
                              controllerAs: 'ctrl',
                              focusOnOpen: false,
                              targetEvent: event,
                              locals: {
                                  messages: response.data,
                                  subject: request.subject,
                                  conversationId: request.id,
                                  requestId: request.requestId,
                                  userName: request.requester,
                                  resources: request.resources,
                                  reloadFunction: $scope.getRequests
                              },
                              templateUrl: '/Scripts/app/access-requests/incoming-requests/incoming-requests/incoming-requests-list/incoming-requests-messages.template.html',
                          }).then($scope.getRequests);
                      });

                  };

                  $scope.getRequests = function () {
                      var prom = IncomingRequestsProvider.get($scope.query).then(function (response) {
                          $scope.requests = response.data;
                      });
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

                      $scope.getRequests();
                  });

              }
            ]

        });

    angular.
        module('incoming-requests.incoming-requests-list').
        controller('messagesController', function (IncomingRequestsProvider, $scope, $mdDialog, messages, subject, conversationId, requestId, userName, resources, reloadFunction) {
            'use strict';

            $scope.subject = subject;
            $scope.messages = messages;
            $scope.resources = resources;
            this.Cancel = $mdDialog.cancel;
            
            this.Accept = function () {
                var prom = IncomingRequestsProvider.acceptRequest(conversationId);

                prom.then(function (result) {
                    $mdDialog.cancel();
                    reloadFunction();
                });
            }

            this.Decline = function () {
                var prom = IncomingRequestsProvider.declineRequest({ ConversationId: conversationId, Reason: $scope.message });

                prom.then(function (result) {
                    $mdDialog.cancel();
                    reloadFunction();
                });
            }

            this.SendMessage = function () {
                var prom = IncomingRequestsProvider.sendMessage({ text: $scope.message, conversationId: conversationId });

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
        });
})();