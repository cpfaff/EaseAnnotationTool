(function () {
    'use strict';

    angular
        .module('usersDashboard.filesList')
        .component('filesList', {
            templateUrl: 'Scripts/app/users-dashboard/files/files-list/files-list.template.html',
            controller: ['FilesProvider', '$scope', '$mdDialog',
              function (FilesProvider, $scope, $mdDialog) {

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
                      limit: 10,
                      order: 'name',
                      page: 1
                  };

                  $scope.newAICreation = { type: undefined };

                  $scope.AccessLevelDialog = function (event, items) {
                      $mdDialog.show({
                          clickOutsideToClose: false,
                          controller: 'FilesAccessModeChangeController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { itemsList: items, deselectFunction: deselectAll },
                          templateUrl: 'Scripts/app/users-dashboard/files/files-list/accessModeDialog.template.html'
                      });
                  }

                  $scope.CreateAIWithFiles = function (event) {
                      if ($scope.newAICreation.type == 1) {
                          var form = $("#createAIWithFiles");
                          $scope.selected.forEach(function (item) {
                              form.append("<input type='hidden' name='filesId[]' value='" + item.id + "'>")
                          });

                          form.submit();
                      }
                      else
                          $mdDialog.show({
                              clickOutsideToClose: false,
                              controller: 'AISearchingController',
                              controllerAs: 'ctrl',
                              focusOnOpen: false,
                              targetEvent: event,
                              locals: { selectedFiles: $scope.selected },
                              templateUrl: 'Scripts/app/users-dashboard/files/files-list/aiSearchingDialog.template.html'
                          });
                  }
                  $scope.GetNormalizedDate = function (dateString) {
                      var date = new Date(dateString);
                      return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
                  }

                  function deselectAll() {
                      $scope.selected = [];
                  }

                  function success(response) {
                      $scope.files = response.data;
                  }

                  $scope.addItem = function (event) {
                      $mdDialog.show({
                          clickOutsideToClose: true,
                          controller: 'newFileController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { updateFilesFunction: $scope.getFiles, fileInfo: null },
                          templateUrl: 'Scripts/app/users-dashboard/files/files-list/files-list-add-file.template.html',
                      }).then($scope.getFiles);
                  };

                  $scope.getFiles = function () {
                      deselectAll();
                      var prom = FilesProvider.get($scope.query);
                      $scope.promise = prom.then(success);
                  };

                  $scope.delete = function (event) {
                      var confirm = $mdDialog.confirm()
                          .title('Would you like to delete files?')
                          .textContent('All selected files will be deleted permanently.')
                          .ariaLabel('Lucky day')
                          .targetEvent(event)
                          .ok('Yes')
                          .cancel('No');

                      $mdDialog.show(confirm).then(function () {
                          var ids = [];
                          $scope.selected.forEach(function (item) {
                              ids.push(item.id);
                          });

                          var datas = ids.join();

                          var prom = FilesProvider.delete($scope.selected);
                          $scope.errorMessage = null;
                          prom.then(function (response) {
                              if (response.data.success == false)
                                  $scope.errorMessage = "Sorry. You cannot delete following files as long as they are referenced in Annotation Items: " + response.data.attachedToAIFiles;
                              else {
                                  $scope.selected = [];
                                  $scope.getFiles();
                              }
                          });
                      }, function () { });
                  }

                  $scope.edit = function (event, file) {
                      $mdDialog.show({
                          clickOutsideToClose: true,
                          controller: 'newFileController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { updateFilesFunction: $scope.getFiles, fileInfo: file },
                          templateUrl: 'Scripts/app/users-dashboard/files/files-list/files-list-add-file.template.html',
                      }).then($scope.getFiles);
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

                      $scope.getFiles();
                  });

              }
            ]

        });
    angular.module('usersDashboard.filesList').
    controller('FilesAccessModeChangeController', function (FilesProvider, $scope, $mdDialog, itemsList, deselectFunction) {

        'use strict';

        $scope.usersAndGroupsArray = [];
        $scope.functionAutocomplete = {};
        $scope.updateTagsList = function () {
            $scope.usersAndGroupsArray = JSON.stringify($scope.selectedUserAndGroups);
        }

        $scope.filesAccesModes = [
           'Private',
           'Explicit',
           'Internal',
           'Public'
        ];

        $scope.loadTags = function (query) {
            var prom = FilesProvider.searchUsersAndGroups(query);
            return prom.then(function (response) {
                return response.data;
            });
        };

        $scope.model = { SelectedUsersAndGroups: [] };

        this.cancel = $mdDialog.cancel;

        this.save = function () {
            itemsList.forEach(function (item) {
                item.accessMode = $scope.model.AccessMode;
                item.UsersAndGroups = $scope.model.SelectedUsersAndGroups;
                item.selectedUsersAndGroups = $scope.model.SelectedUsersAndGroups;
            });

            FilesProvider.setFilesAccessMode(itemsList).then(function (result) {
                deselectFunction();
                $mdDialog.cancel();
            });
        };
    }).
    controller('AISearchingController', function (FilesProvider, $scope, $timeout, $mdDialog, selectedFiles) {
        'use strict';
        $scope.model = { minLength: 1 };

        $scope.querySearch = function (query) {
            var prom = FilesProvider.searchAI(query);
            return prom.then(function (response) {
                if ($scope.model.minLength == 0 && query.length > 0)
                    $scope.model.minLength = 1;

                return response.data;
            });
        }

        $scope.showAllAIs = function () {
            $scope.model.minLength = 0;
            $("#autocompleteSeacrh").blur();
            $timeout(function () {
                $("#autocompleteSeacrh").children().children().focus();
            }, 100);
        }

        this.cancel = $mdDialog.cancel;
        this.save = function () {
            var form = $("#createAIWithFiles");
            selectedFiles.forEach(function (item) {
                form.append("<input type='hidden' name='filesId[]' value='" + item.id + "'>")
            });

            form.append("<input type='hidden' name='cloningId' value='" + $scope.model.SelectedAI.id + "'>")
            form.submit();
        };
    }).
    controller('newFileController', function (FilesProvider, $scope, $mdDialog, updateFilesFunction, fileInfo) {

        'use strict';

        this.cancel = $mdDialog.cancel;

        $scope.errorMessage = null;
        $scope.selectedUserAndGroups = [];
        $scope.usersAndGroupsArray = '';
        $scope.editingMode = (fileInfo != null);

        if (fileInfo) {
            $scope.newFile = {
                Id: fileInfo.id,
                Description: fileInfo.description,
                AccessMode: fileInfo.accessMode,
                Name: fileInfo.name,
                SelectedUsersAndGroups: fileInfo.selectedUsersAndGroups
            };
        }
        else
            $scope.newFile = { AccessMode: 0 };

        $scope.updateTagsList = function () {
            $scope.usersAndGroupsArray = JSON.stringify($scope.selectedUserAndGroups);
        }

        $scope.filesAccesModes = [
           'Private',
           'Explicit',
           'Internal',
           'Public'
        ];

        $scope.loadTags = function (query) {
            var prom = FilesProvider.searchUsersAndGroups(query);
            return prom.then(function (response) {
                return response.data;
            });
        };


        this.save = function () {
            var files;
            var prom;
            $scope.errorMessage = null;

            if ($scope.editingMode) {
                files = {
                    Id: $scope.newFile.Id,
                    Description: $scope.newFile.Description,
                    AccessMode: $scope.newFile.AccessMode,
                    UsersAndGroups: $scope.newFile.SelectedUsersAndGroups == undefined ? null : $scope.newFile.SelectedUsersAndGroups
                };
                prom = FilesProvider.update(files);
            }
            else {
                var largefileName;
                var maxFileSize = 1.0; // in GB
                files = new FormData();

                $scope.files.every(function (obj) {
                    var fileSize = ((obj.lfFile.size / 1024) / 1024 / 1024).toFixed(4); // GB
                    if (fileSize > maxFileSize) {
                        largefileName = obj.lfFile.name;
                        return false;
                    }

                    files.append('files[]', obj.lfFile);
                    return true;
                });

                if (largefileName) {
                    $scope.errorMessage = "File " + largefileName + " has size larger than 1GB. Please select file with less size.";
                }
                else {
                    files.append('Description', $scope.newFile.Description);
                    files.append('AccessMode', $scope.newFile.AccessMode);
                    files.append('UsersAndGroups', $scope.newFile.SelectedUsersAndGroups == undefined ? null : JSON.stringify($scope.newFile.SelectedUsersAndGroups));
                }
                prom = FilesProvider.add(files);
            }

            prom.then(function (result) {
                updateFilesFunction();
                $mdDialog.cancel();

            }, function (err) {

            });
        };
    });
})();