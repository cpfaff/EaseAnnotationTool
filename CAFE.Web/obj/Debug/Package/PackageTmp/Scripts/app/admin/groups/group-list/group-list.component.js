(function() {
    'use strict';

    angular
        .module('admin.groupList')
        .component('groupList', {
            templateUrl: 'Scripts/app/admin/groups/group-list/group-list.template.html',
            controller: ['GroupProvider', '$scope', '$mdDialog',
              function (GroupProvider, $scope, $mdDialog) {

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


                  function success(response) {
                      $scope.groups = response.data;
                  }

                  $scope.addItem = function (event) {
                      $mdDialog.show({
                          clickOutsideToClose: true,
                          controller: 'newGroupController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          templateUrl: 'Scripts/app/admin/groups/group-list/group-list-add-group.template.html',
                      }).then($scope.getgroups);
                  };
                  
                  $scope.showDetails = function (index, event)
                  {
                      event.stopPropagation();
                      var users = [];

                       $scope.groups[index].users.forEach(function(item){
                           users.push(item);
                       })

                      $mdDialog.show({
                          clickOutsideToClose: false,
                          controller: 'groupDetailsController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { usersList: users, selectedGroup: $scope.groups[index], getGroupsFunction: $scope.getgroups },
                          templateUrl: 'Scripts/app/admin/groups/group-details/group-details.template.html'
                      });
                  };

                  /*
                  $scope.getUsers = function (groupIndex) {
                      var prom = GroupProvider.getUsers($scope.query);                
                      $scope.promise = prom.then(success);
                  };
                  */

                  $scope.getgroups = function () {
                      var prom = GroupProvider.get($scope.query);
                      $scope.promise = prom.then(success);
                  };

                  $scope.deleteGroup = function (groupIndex, event) {
                      event.stopPropagation();
                      var prom = GroupProvider.delete($scope.groups[groupIndex]);
                      $scope.promise = prom.then(function (response) {
                          $scope.groups.splice(groupIndex, 1);
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

                      $scope.getgroups();
                  });

              }
            ]
        });

          
    angular
        .module('admin.groupList').controller('groupDetailsController', ['GroupProvider', '$scope', '$mdDialog', 'usersList', 'selectedGroup', 'getGroupsFunction',
        function (GroupProvider, $scope, $mdDialog, usersList, selectedGroup, getGroupsFunction) {
            $scope.selectedGroupName = selectedGroup.name;
            $scope.searchUserKey = null;
            $scope.selectedUser = null;
            $scope.selected = [];
            $scope.usersList = usersList;
            $scope.addedUsers = [];
            $scope.deletedUsers = [];
            $scope.groupName = selectedGroup.name;
            this.cancel = $mdDialog.cancel;

            this.save = function() {
                var data = {
                    userAddedIds: $scope.addedUsers,
                    userDeletedIds: $scope.deletedUsers,
                    groupId: selectedGroup.id,
                    groupName: $scope.selectedGroupName
                };

                GroupProvider.updateGroup(data).then(function (response) {
                    getGroupsFunction();
                    $mdDialog.cancel();
                });
            };

            $scope.userLiveSearch = function (keyword) {
                return GroupProvider.searchUsers({ Name: keyword }).then(function (response) {
                    return response.data;
                });
            }

            $scope.FindValueInObjectCollection = function(objectCollection, property, value)
            {
                var foundIdex = -1;
                objectCollection.forEach(function(item, i){
                    if(item[property] == value)
                    {
                        foundIdex = i;
                        return false;
                    }
                });

                return foundIdex;
            }

            $scope.addUserToGroup = function (userModel)
            {
                if (userModel)
                {
                    var userId = userModel.id;
                    if (-1 == $scope.FindValueInObjectCollection($scope.usersList, 'id', userId)) {
                        $scope.usersList.push(userModel);
                        $scope.addedUsers.push(userModel.id);
                    }
                }
                $scope.searchUserKey = null;
                $scope.selectedUser = null;
            }

            $scope.deleteUserFromGroup = function (userIndex, event) {
                event.stopPropagation();
                var userId = $scope.usersList[userIndex].id;

                if (-1 == $scope.addedUsers.indexOf(userId))
                    $scope.deletedUsers.push(userId);
                else
                    $scope.addedUsers.splice($scope.addedUsers.indexOf(userId), 1);

                $scope.usersList.splice(userIndex, 1);
            };
        }]);

    angular
        .module('admin.groupList')
        .directive('groupsDetailsUsers', function() {
            return {
                templateUrl: 'Scripts/app/admin/groups/group-details/groups-details-users.template.html'
            };
        });

    angular
        .module('admin.groupList').controller('newGroupController', ['GroupProvider', '$scope', '$mdDialog',
            function (GroupProvider, $scope, $mdDialog) {

                'use strict';

                this.cancel = $mdDialog.cancel;

                function success(group) {
                    $mdDialog.hide(group);
                }

                this.save = function () 
                {
                    $scope.item.form.$setSubmitted();

                    if ($scope.item.form.$valid) {
                        var newGroup = GroupProvider.new();
                        newGroup.Name = $scope.newGroup.Name;

                        var prom = GroupProvider.add(newGroup);
                        $scope.promise = prom.then(success);
                    }
                };
            }]);
})();