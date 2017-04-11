(function() {
    'use strict';

    angular
        .module('usersDashboard.acceptRequests')
        .component('accessRequestsList', {
            templateUrl: 'Scripts/app/users-dashboard/acceptRequests/accept-requests.template.html',
            controller: ['AcceptRequestsProvider', '$scope', '$mdDialog',
              function (AcceptRequestsProvider, $scope, $mdDialog) {
                  
                  var bookmark;
                  $scope.selected = [];
                  $scope.selectedGroupName = null;

                  $scope.filter = {};

                  $scope.query = {
                      filter: '',
                      limit: '10',
                      order: 'NameToLower',
                      page: 1
                  };

                $scope.GetNormalizedDate = function (dateString) {
                    var date = new Date(dateString);
                    return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
                }
                $scope.getAcceptRequsts = function () {
                    var prom = AcceptRequestsProvider.get();
                    $scope.promise = prom.then(function (response) {
                        $scope.requests = response.data;
                    });
                };

                $scope.showMessages = function (event, index) {
                    var currentrequest = $scope.requests[index];
                    var incomingTemplate = '/Scripts/app/access-requests/incoming-requests/incoming-requests/incoming-requests-list/incoming-requests-messages.template.html';
                    var outgoingTemplate = '/Scripts/app/access-requests/outgoing-requests/outgoing-requests/outgoing-requests-list/outgoing-requests-messages.template.html';
                    var template = (currentrequest.type == 0 ? incomingTemplate : outgoingTemplate);
                    AcceptRequestsProvider.getMessages(currentrequest.id).then(function (response) {
                        $mdDialog.show({
                            clickOutsideToClose: true,
                            controller: 'messagesController',
                            controllerAs: 'ctrl',
                            focusOnOpen: false,
                            targetEvent: event,
                            locals: {
                                messages: response.data,
                                subject: $scope.requests[index].subject,
                                conversationId: $scope.requests[index].id,
                                requestId: $scope.requests[index].requestId,
                                userName: $scope.requests[index].requester,
                                reloadFunction: $scope.getAcceptRequsts
                            },
                            templateUrl: template
                        }).then($scope.getAcceptRequsts);
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

                      $scope.getAcceptRequsts();
                  });
              }
            ]

        });
    
    angular.
       module('usersDashboard.acceptRequests').
       controller('messagesController', ['AcceptRequestsProvider', '$scope', '$mdDialog', 'messages', 'subject', 'conversationId', 'requestId', 'userName', 'reloadFunction',
       function (AcceptRequestsProvider, $scope, $mdDialog, messages, subject, conversationId, requestId, userName, reloadFunction) {
           'use strict';
           $scope.subject = subject;
           $scope.messages = messages;

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