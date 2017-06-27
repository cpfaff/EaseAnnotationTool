(function() {
    'use strict';

    angular
        .module('admin.userList')
        .component('userList', {
            templateUrl: 'Scripts/app/admin/users/user-list/user-list.template.html',
            controller: ['UserProvider', '$scope', '$mdDialog',
              function (UserProvider, $scope, $mdDialog) {

                  var bookmark;
                  $scope.selected = [];

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
                      $scope.users = response.data;
                  }

                  $scope.deleteUsersDialog = function (event) {
                    $mdDialog.show({
                        contentElement: '#myStaticDialog'
                    });
                  };

                  $scope.accountRemovingType;

                  $scope.DeleteUsers = function()
                  {
                      var usersIds = [];

                      $scope.selected.forEach(function (user) {
                          usersIds.push(user.id);
                      });

                      UserProvider.deleteUsers({
                          usersIds: usersIds,
                          removeOwnData: $scope.accountRemovingType
                      }).then(function (response) {
                          $scope.selected = [];
                          $scope.getUsers();
                          $scope.CloseDeleteUsersDialog();
                      });
                  }

                  $scope.CloseDeleteUsersDialog = function () {
                      $mdDialog.hide();
                  }

                  $scope.showDetails = function (index, event) {
                      event.stopPropagation();
                      $mdDialog.show({
                          clickOutsideToClose: false,
                          controller: 'userDetailsController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { user: $scope.users[index] },
                          templateUrl: 'Scripts/app/admin/users/user-details/user-details.template.html'
                      }).then($scope.getUsers);
                  };


                  $scope.getUsers = function () {
                      var prom = UserProvider.get($scope.query);
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

    /**
      * Edit controller. For details modal
      */
    angular
        .module('admin.userList').controller('userDetailsController', ['UserProvider', '$scope', '$mdDialog', 'user',
            function (UserProvider, $scope, $mdDialog, user) {
                'use strict';
                var bookmark;
                $scope.user = user;
                $scope.searchUserKey;
                $scope.newOwner;
                $scope.userFiles = [];
                $scope.userAI = [];
                $scope.userFilesFiltered = [];
                $scope.userAIFiltered = [];
                $scope.query = {
                    filter: '',
                    limit: '10',
                    order: 'NameToLower',
                    page: 1
                };

                $scope.AIquery = {
                    filter: '',
                    limit: 10,
                    order: 'name',
                    page: 1
                };

                $scope.errorMessage;

                $scope.selectedAI = [];
                $scope.selected = [];

                $scope.GetNormalizedDate = function (dateString) {
                    var date = new Date(dateString);
                    var normalized = date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
                    return normalized;
                }

                function deselectAll() {
                    $scope.selected = [];
                    $scope.selectedAI = [];
                }

                this.cancel = $mdDialog.cancel;

                $scope.AccessLevelDialog = function (event, isAI) {
                    $mdDialog.show({
                        clickOutsideToClose: false,
                        controller: 'FilesAccessModeChangeController',
                        controllerAs: 'ctrl',
                        focusOnOpen: false,
                        targetEvent: event,
                        locals: { itemsList: isAI ? $scope.selectedAI : $scope.selected , deselectFunction: deselectAll, isAI: isAI },
                        templateUrl: 'Scripts/app/admin/users/user-details/accessModeDialog.template.html'
                    });
                }


                $scope.fileAccessModes = [
                    'Private',
                    'Explicit',
                    'Internal',
                    'Public'
                ];

                $scope.deleteFile = function(index)
                {
                    $scope.errorMessage = null;
                    UserProvider.deleteUserFile([$scope.userFilesFiltered[index]]).
                        then(function (response)
                        {
                            if (response.data.success == false)
                                $scope.errorMessage = "Sorry. You cannot delete following files as long as they are referenced in Annotation Items: " + response.data.attachedToAIFiles;
                            else
                                $scope.getUserFiles();
                        });
                }

                $scope.switchOwner = function (event, index)
                {
                    $mdDialog.show({
                        clickOutsideToClose: false,
                        controller: 'userFilesSwitchOwnerController',
                        controllerAs: 'ctrl',
                        focusOnOpen: false,
                        targetEvent: event,
                        locals: { currentUser: user, file: $scope.userFilesFiltered[index], updateFunction: $scope.getUserFiles },
                        templateUrl: 'Scripts/app/admin/users/user-details/user-details.files.switch-owner.template.html'
                    });
                }

                $scope.getUserFiles = function () {
                    $scope.userFilesFiltered = [];
                    var prom = UserProvider.getUserFiles(user.id);
                    $scope.promise = prom.then(function (response) {
                        $scope.userFiles = response.data;

                        $scope.userFiles.forEach(function (file) {
                            file.accessModeName = $scope.fileAccessModes[file.accessMode];
                            $scope.userFilesFiltered.push(file);
                        });
                    });
                };

                $scope.getUserAnnotationItems = function () {
                    $scope.userAIFiltered = [];
                    var prom = UserProvider.getUserAI(user.id);
                    $scope.promise = prom.then(function (response) {
                        $scope.userAI = response.data;

                        $scope.userAI.forEach(function (AI) {
                            AI.accessModeName = $scope.fileAccessModes[AI.accessMode];
                            $scope.userAIFiltered.push(AI);
                        });
                    });
                };


                $scope.filter = {
                    options: {
                        debounce: 500
                    }
                };

                $scope.AIfilter = {
                    options: {
                        debounce: 500
                    }
                };

                $scope.clearFilter = function () {
                    $scope.query.filter = '';

                    if ($scope.filter.form.$dirty) {
                        $scope.filter.form.$setPristine();
                    }
                };

                $scope.clearAIFilter = function () {
                    $scope.AIquery.filter = '';

                    if ($scope.AIfilter.form.$dirty) {
                        $scope.AIfilter.form.$setPristine();
                    }
                };

                $scope.filterData = function(objectsArray, propertiesToFilter, keyToSearch)
                {
                    var newFilesList = [];
                    objectsArray.forEach(function (object) {
                        propertiesToFilter.every(function (property) {
                            if(-1 != object[property].indexOf(keyToSearch))
                            {
                                newFilesList.push(object);
                                return false;
                            }
                            return true;
                        });
                    });

                    return newFilesList;
                }

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

                    $scope.userFilesFiltered = (newValue && newValue.length > 0) ? $scope.filterData($scope.userFiles, ['name'], newValue) : $scope.userFiles;
                });

                $scope.$watch('AIquery.filter', function (newValue, oldValue) {
                    if (!oldValue) {
                        bookmark = $scope.AIquery.page;
                    }

                    if (newValue !== oldValue) {
                        $scope.AIquery.page = 1;
                    }

                    if (!newValue) {
                        $scope.AIquery.page = bookmark;
                    }

                    $scope.userAIFiltered = (newValue && newValue.length > 0) ? $scope.filterData($scope.userAI, ['name'], newValue) : $scope.userAI;
                });


                this.save = function () {
                    $scope.item.form.$setSubmitted();

                    if ($scope.item.form.$valid) {
                        UserProvider.update($scope.user).then(function(response) {
                            $mdDialog.hide(response.data);
                        });
                    }
                };
            }]);

    angular.module('admin.userList').
        controller('FilesAccessModeChangeController', function (UserProvider, $scope, $mdDialog, itemsList, deselectFunction, isAI) {
            'use strict';

            $scope.usersAndGroupsArray = [];

            $scope.filesAccesModes = [
               'Private',
               'Explicit',
               'Internal',
               'Public'
            ];

            $scope.querySearch = function (query) {
                var prom = UserProvider.searchUsersAndGroups(query);
                return prom.then(function (response) {
                    return response.data;
                });
            }

            $scope.model = {SelectedUsersAndGroups: []};

            this.cancel = $mdDialog.cancel;

            this.save = function () {

                if (isAI) {
                    
                    if (1 == $scope.model.AccessMode && !$scope.model.SelectedUsersAndGroups.length)
                        return false;

                    var modelToSave = {
                        ids: [],
                        usersAndGroups: $scope.model.SelectedUsersAndGroups,
                        accessMode: $scope.filesAccesModes[$scope.model.AccessMode]
                    };

                    itemsList.forEach(function (item) {
                        modelToSave.ids.push(item.id);
                    });

                    UserProvider.setAIAccessMode(modelToSave).then(function (result) {
                        deselectFunction();
                        $mdDialog.cancel();
                    });
                }
                else {

                    itemsList.forEach(function (item) {
                        item.accessMode = $scope.model.AccessMode;
                        item.UsersAndGroups = $scope.model.SelectedUsersAndGroups;
                    });

                    UserProvider.setFilesAccessMode(itemsList).then(function (result) {
                        deselectFunction();
                        $mdDialog.cancel();
                    });
                }
            };
        });

    angular
        .module('admin.userList').controller('userFilesSwitchOwnerController', ['UserProvider', '$scope', '$mdDialog', 'currentUser', 'file', 'updateFunction',
            function (UserProvider, $scope, $mdDialog, currentUser, file, updateFunction) {
                'use strict';
                $scope.currentOwner = currentUser;
                $scope.searchUserKey;
                $scope.newOwner;

                $scope.userLiveSearch = function (keyword) {
                    return UserProvider.searchUsers({ Name: keyword }).then(function (response) {
                        return response.data;
                    });
                }

                this.cancel = $mdDialog.cancel;

                this.save = function () {
                    $scope.item.form.$setSubmitted();
                    if ($scope.item.form.$valid) {
                        UserProvider.switchUserFileOwner(file.id, $scope.newOwner.id).then(function (response) {
                            $mdDialog.cancel();
                        });
                    }
                };
            }]);
})();