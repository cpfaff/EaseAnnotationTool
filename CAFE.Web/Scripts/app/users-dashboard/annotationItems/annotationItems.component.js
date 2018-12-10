(function () {
    'use strict';

    angular
        .module('usersDashboard.annotationItems')
        .component('annotationItemsList', {
            templateUrl: 'Scripts/app/users-dashboard/annotationItems/annotationItems.template.html',
            controller: ['AnnotationItemsProvider', '$scope', '$mdDialog', '$mdToast', 'NgTableParams', '$filter', '$timeout',
                function (AnnotationItemsProvider, $scope, $mdDialog, $mdToast, NgTableParams, $filter, $timeout) {
                  var bookmark;
                  $scope.selectedAnnotationItems = [];
                  $scope.loading = true;
                  $scope.additionalLoading = false;
                  $scope.notification;
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
                      limit: 10,
                      order: 'name',
                      page: 1
                  };

                  $scope.unacceptedUsers = null;

                  $scope.tableParams = new NgTableParams({ count: 10 }, {
                      counts: [],
                      paginationMaxBlocks: 13,
                      paginationMinBlocks: 2,
                      dataset: $scope.annotationItems
                  });
                  function DeSelectAllAI() {
                      $scope.notification = 'Access mode has been set';
                      $scope.selectedAnnotationItems = [];
                      angular.forEach($scope.annotationItems, function(value, key) {
                          value.selected = false;
                      });
                  }

                  $scope.GetNormalizedDate = function (dateString) {
                      var date = new Date(dateString);
                      return date.toLocaleDateString() + ' ' + date.toLocaleTimeString();
                  };

                  $scope.AccessLevelDialog = function (event, items) {
                      $mdDialog.show({
                          clickOutsideToClose: false,
                          controller: 'AIAccessModeChangeController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { itemsList: items, updateFunction: DeSelectAllAI },
                          templateUrl: 'Scripts/app/users-dashboard/annotationITems/accessModeDialog.template.html'
                      });
                  };

                  $scope.ImportDialog = function (event) {
                      $mdDialog.show({
                          clickOutsideToClose: false,
                          controller: 'AIImportController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { UpdateFunction: $scope.getAnnotationItems },
                          templateUrl: 'Scripts/app/users-dashboard/annotationItems/importDialog.template.html'
                      });
                  };


                  $scope.ImportWizardDialog = function (event) {
                      $mdDialog.show({
                          clickOutsideToClose: false,
                          controller: 'AIImportWizardController',
                          controllerAs: 'ctrl',
                          focusOnOpen: false,
                          targetEvent: event,
                          locals: { UpdateFunction: $scope.getAnnotationItems },
                          templateUrl: 'Scripts/app/users-dashboard/annotationItems/importWizardDialog.template.html'
                      });
                  };

                  $scope.CloneAnnotationItem = function (event, annotationItems) {
                      if (annotationItems.length > 1) {
                          var pinTo = $scope.getToastPosition();
                          $mdToast.show(
                              $mdToast.simple()
                                  .textContent("You can clone only one annotation item. Select one item")
                                  .position(pinTo)
                                  .hideDelay(3000)
                          );
                          return;
                      }

                      window.location.href = "/AnnotationItem/Clone?id=" + annotationItems[0].id;
                  };

                  $scope.getAnnotationItems = function () {
                      $scope.loading = true;
                      AnnotationItemsProvider.get().then(function (response) {
                          $scope.annotationItems = Array.isArray(response.data) ? response.data : null;
                          $scope.tableParams.settings({
                              dataset: $scope.annotationItems
                          });
                          $scope.loading = false;
                      }, function (response) {
                          $scope.loading = false;
                      });
                  };


                  $scope.removeFilter = function () {
                      $scope.filter.show = false;
                      $scope.query.filter = '';

                      if ($scope.filter.form.$dirty) {
                          $scope.filter.form.$setPristine();
                      }
                  };

                  $scope.DeleteAnnotationItems = function (event) {
                      var confirm = $mdDialog.confirm()
                           .title('Would you like to delete Annotaion Items?')
                           .textContent('All selected Annotaion Items will be deleted permanently.')
                           .ariaLabel('Lucky day')
                           .targetEvent(event)
                           .ok('Yes')
                           .cancel('No');

                      $mdDialog.show(confirm).then(function (e) {
                          AnnotationItemsProvider.delete($scope.selectedAnnotationItems).then(function (response) {

                              for (var k = 0; k < $scope.selectedAnnotationItems.length; k++) {
                                  //setTimeout(function () {
                                    var removingItem= $scope.selectedAnnotationItems[k];
                                    var originalItem = $filter('filter')($scope.annotationItems, { '$id': removingItem.$id })[0];
                                    var originalIndex = $scope.annotationItems.indexOf(originalItem);
                                  $scope.annotationItems.splice(originalIndex, 1);
                              }
                              $scope.tableParams.reload();
                                  $scope.selectedAnnotationItems = [];

                          });
                      }, function (e) { });
                  };

                  $scope.ExportAnnotationItems = function () {
                      $scope.additionalLoading = true;
                      AnnotationItemsProvider.export($scope.selectedAnnotationItems).then(function (response) {
                          $scope.additionalLoading = false;
                          //TODO: Start download
                          //response.data its url to download
                          window.location = response.data;
                      });
                  };

                  $scope.checkAnnotationItem = function(annotationItem) {
                      var ind = $scope.selectedAnnotationItems.indexOf(annotationItem);
                      if (ind == -1) {
                          $scope.selectedAnnotationItems.push(annotationItem);
                      } else {
                          $scope.selectedAnnotationItems.splice(ind, 1);
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

                      $scope.getAnnotationItems();
                  });
              }
            ]
        });

    angular.module('usersDashboard.annotationItems').
        controller('AIAccessModeChangeController', function (AnnotationItemsProvider, $scope, $mdDialog, itemsList, updateFunction) {
            'use strict';

            $scope.AIAccesModes = [
                'Private',
                'Explicit',
                'Internal',
                'Public'
            ];

            $scope.querySearch = function (query) {
                var prom = AnnotationItemsProvider.searchUsersAndGroups(query);
                return prom.then(function (response) {
                    return response.data;
                });
            };

            $scope.model = { SelectedUsersAndGroups: [] };

            this.cancel = $mdDialog.cancel;

            this.save = function () {
                var modelToSave = {
                    ids: [],
                    usersAndGroups: $scope.model.SelectedUsersAndGroups,
                    accessMode: $scope.AIAccesModes[$scope.model.AccessMode]
                };

                itemsList.forEach(function (item) {
                    modelToSave.ids.push(item.id);
                });

                AnnotationItemsProvider.setAIAccessMode(modelToSave).then(function (result) {
                    updateFunction();
                    $mdDialog.cancel();
                }, function (err) {

                });
            };
        });

    angular.module('usersDashboard.annotationItems').
        controller('AIImportController', function (AnnotationItemsProvider, $scope, $mdDialog, UpdateFunction, $mdToast) {

            'use strict';
            $scope.model = {};
            $scope.ctrl = { importType: 0, importType2: 0, itemName: '', itemDescription: '' };
            $scope.userFiles = [];

            $scope.isLoading = false;
            AnnotationItemsProvider.getUserFiles().then(function (result) {
                $scope.userFiles = result.data;
                $scope.userFiles.forEach(function (item) {
                    item.downloadURL = window.location.origin + item.downloadURL;
                });
            });

            $scope.userFilesSearch = function (query) {
                var results = query ? $scope.userFiles.filter(createFilterFor(query)) : $scope.userFiles;
                return results;
            };

            function createFilterFor(query) {
                return function filterFn(item) {
                    return (item.name.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            function isUrl(s) {
                var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
                return regexp.test(s);
            }

            function isGuid(value) {
                var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;
                var match = regex.exec(value);
                return match != null;
            }

            function ImportSuccess() {
                UpdateFunction();
                $scope.isLoading = false;
                $mdDialog.hide();
            }

            function ImportError(result) {
                $scope.isLoading = false;
                $scope.ctrl.error = result.data;
            }

            function CheckFileExtension(inputText, validExtensions) {
                var fileExtension = inputText.split('.').pop();

                if (-1 != fileExtension.indexOf("?"))
                    fileExtension = fileExtension.split('?')[0];

                if (!validExtensions || -1 != validExtensions.indexOf(fileExtension.toLowerCase()))
                    return true;

                return false;
            }

            function AttachTransformationFile(data, validExtensions) {
                var selectedData = $scope.ctrl.userLink2;
                var inputText = $scope.ctrl.selectedLink2;

                if ($scope.ctrl.importType == 0) {
                    if (isUrl(inputText)) {
                        data.UseTransormation = true;
                        data.TransformationData = inputText;

                        if (!CheckFileExtension(inputText, validExtensions)) {
                            $scope.ctrl.error = "You must specify for tranformation .xml file only.";
                            $scope.isLoading = false;
                            return false;
                        }

                        data.TransformatioDataName = inputText.substring(inputText.lastIndexOf('/') + 1);
                        AnnotationItemsProvider.import(data).then(ImportSuccess, ImportError);
                    }
                    else if (selectedData && isGuid(selectedData.id)) {
                        data.UseTransormation = true;
                        data.TransformationData = selectedData.id;
                        data.TransformatioDataName = inputText;
                        data.TransformationDataType = 2;

                        if (!CheckFileExtension(inputText, validExtensions)) {
                            $scope.ctrl.error = "You must specify for transformation .xml file only.";
                            $scope.isLoading = false;
                            return false;
                        }

                        AnnotationItemsProvider.import(data).then(ImportSuccess, ImportError);
                    }
                    else {
                        $scope.ctrl.error = "You must enter vaild link or select own file for transformation.";
                        $scope.isLoading = false;
                    }
                }
                else {
                    var reader = new FileReader();
                    reader.onload = function (e) {

                        data.TransformationData = e.target.result;
                        data.UseTransormation = true;

                        var fileExtension = $scope.ctrl.files2[0].lfFileName.split('.').pop();
                        if (-1 != validExtensions.indexOf(fileExtension.toLowerCase())) {
                            data.TransformationDataType = 0;
                            data.TransformatioDataName = $scope.ctrl.files2[0].lfFileName;
                        }
                        else {
                            $scope.ctrl.error = "You must specify .xml file only.";
                            $scope.isLoading = false;
                            return false;
                        }


                        AnnotationItemsProvider.import(data).then(ImportSuccess, ImportError);
                    };
                    reader.readAsDataURL($scope.ctrl.files2[0].lfFile);
                }
            }

            $scope.Import = function () {
                $scope.isLoading = true;
                $scope.ctrl.error = null;

                var data = {};

                function ImportSuccess(response) {
                    UpdateFunction();
                    $scope.isLoading = false;
                    $mdDialog.hide();
                }

                function ImportError(response) {
                    $scope.isLoading = false;
                    $scope.ctrl.error = response.data.message;
                }

                var validExtensions = ["xml"];

                data.SaveFileAfterUpload = $scope.ctrl.saveFilesAfterUpload;
                if ($scope.ctrl.importType == 0) {
                    var selectedData = $scope.ctrl.userLink;
                    var inputText = $scope.ctrl.selectedLink1;

                    if (isUrl(inputText)) {
                        data.ExtendableData = inputText;
                        data.UseTransormation = false;
                        data.Name = $scope.ctrl.itemName;
                        data.Description = $scope.ctrl.itemDescription;
                        data.ExtendableDataName = inputText.substring(inputText.lastIndexOf('/') + 1);
                        data.DataType = 1;

                        if (!CheckFileExtension(inputText, validExtensions)) {
                            $scope.ctrl.error = "You must specify .xml file only.";
                            $scope.isLoading = false;
                            return false;
                        }

                        if ($scope.ctrl.useTransformation) {
                            AttachTransformationFile(data, validExtensions);
                        } else
                            AnnotationItemsProvider.import(data).then(ImportSuccess, ImportError);
                    }
                    else if (selectedData && isGuid(selectedData.id)) {
                        data.ExtendableData = selectedData.id;
                        data.UseTransormation = false;
                        data.Name = $scope.ctrl.itemName;
                        data.Description = $scope.ctrl.itemDescription;
                        data.ExtendableDataName = inputText;
                        data.DataType = 2;

                        if (!CheckFileExtension(inputText, validExtensions)) {
                            $scope.ctrl.error = "You must specify .xml file only.";
                            $scope.isLoading = false;
                            return false;
                        }

                        if ($scope.ctrl.useTransformation) {
                            AttachTransformationFile(data, validExtensions);
                        } else
                            AnnotationItemsProvider.import(data).then(ImportSuccess, ImportError);
                    }
                    else {
                        $scope.ctrl.error = "You must enter vaild link or select own file";
                        $scope.isLoading = false;
                    }
                }
                else {
                    var reader = new FileReader();
                    reader.onload = function (e) {

                        data.ExtendableData = e.target.result;
                        data.UseTransormation = false;
                        data.Name = $scope.ctrl.itemName;
                        data.Description = $scope.ctrl.itemDescription;

                        var fileExtension = $scope.ctrl.files[0].lfFileName.split('.').pop();

                        if (-1 != validExtensions.indexOf(fileExtension.toLowerCase()))
                            data.DataType = 0;
                        else {
                            $scope.ctrl.error = "You must specify .xml file only.";
                            $scope.isLoading = false;
                            return false;
                        }

                        data.ExtendableDataName = $scope.ctrl.files[0].lfFileName;
                        if ($scope.ctrl.useTransformation) {
                            AttachTransformationFile(data, validExtensions);
                        }
                        else
                            AnnotationItemsProvider.import(data).then(ImportSuccess, ImportError);

                    };
                    reader.readAsDataURL($scope.ctrl.files[0].lfFile);
                }
            };

            $scope.dialogCancel = $mdDialog.cancel;
        });

    angular.module('usersDashboard.annotationItems').
        controller('AIImportWizardController', function (AnnotationItemsProvider, $scope, $mdDialog, UpdateFunction, $mdToast, WizardHandler, FilesProvider) {

            'use strict';
            $scope.model = {};
            $scope.ctrl = { importType: 0, importType2: 0, itemName: '', itemDescription: '', userLinks: [], files: [] };

            //WizardHandler.wizard().next();
            $scope.userFiles = [];
            $scope.importData = null;
            $scope.isLoading = false;

            $scope.errorMessage = null;
            $scope.selectedUserAndGroups = [];
            $scope.usersAndGroupsArray = '';

            $scope.newFile = { AccessMode: 0 };

            $scope.usersFilesData = null;
            $scope.usingFilesMode;

            $scope.updateTagsList = function () {
                $scope.usersAndGroupsArray = JSON.stringify($scope.selectedUserAndGroups);
            };

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


            AnnotationItemsProvider.getUserFiles().then(function (result) {
                $scope.userFiles = result.data;
                $scope.userFiles.forEach(function (item) {
                    item.downloadURL = window.location.origin + item.downloadURL;
                });
            });

            AnnotationItemsProvider.get().then(function (response) {
                $scope.annotationItems = Array.isArray(response.data) ? response.data : null;
                $scope.loading = false;
            }, function (response) {
                $scope.loading = false;
            });

            $scope.wizardUploadData = function () {
                WizardHandler.wizard('customOrderWizard').goTo(1);
            };

            $scope.uploadFileAndMoveNext = function () {
                var largefileName;
                var maxFileSize = 1.0; // in GB
                var files = new FormData();
                

                $scope.ctrl.files.every(function (obj) {
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

                $scope.usersFilesData = files;
                $scope.usingFilesMode = 'uploading';
                WizardHandler.wizard('customOrderWizard').goTo(2);
            };

            $scope.wizardSelectData = function () {

                var data = [];
                var selectedData = $scope.ctrl.userLinks;

                if (selectedData) {

                    selectedData.every(function (obj) {
                        data.push(obj.fileId);
                        return true;
                    });
                }
                else {
                    $scope.ctrl.error = "You must enter vaild link or select own file";
                    $scope.isLoading = false;
                }

                $scope.usersFilesData = data;
                $scope.usingFilesMode = 'selection';

                WizardHandler.wizard('customOrderWizard').goTo(2);
            };
       
            $scope.wizardWithoutData = function () {
                WizardHandler.wizard('customOrderWizard').finish();

                window.location.href = "/AnnotationItem";
            };

            $scope.createNewAIWithFileData = function () {
                performImport(0);
            };

            $scope.wizardReuseData = function () {
                performImport(1);
            };

            $scope.wizardUpdateData = function () {
                performImport(2);
            };

            function performImport(behavior) {
                var selectedItem = $scope.ctrl.selectedAnnotation;

                var success = function (data) {
                    if (data.data) {
                        window.location.href = data.data;
                    }
                    UpdateFunction();
                    $scope.isLoading = false;
                    WizardHandler.wizard('customOrderWizard').finish();
                    $mdDialog.hide();
                };

                var error = function() {
                    $scope.isLoading = false;
                    $scope.ctrl.error = result.data;
                    WizardHandler.wizard('customOrderWizard').cancel();
                };
                var payloadData;
                if ($scope.usingFilesMode == 'uploading') {
                    payloadData = $scope.usersFilesData;
                } else {
                    payloadData = $scope.usersFilesData;
                }

                switch (behavior) {
                    case 0:
                        AnnotationItemsProvider.newWithFiles(payloadData, $scope.usingFilesMode).then(success, error);
                        break;
                    case 1:
                        AnnotationItemsProvider.newWithFiles(payloadData, $scope.usingFilesMode, selectedItem.id).then(success, error);
                        break;
                    case 2:
                        AnnotationItemsProvider.updateWithFiles(payloadData, $scope.usingFilesMode, selectedItem.id).then(success, error);
                        break;
                    default:
                }
                
            }

            $scope.userFilesSearch = function (query) {
                var results = query ? $scope.userFiles.filter(createFilterFor(query)) : $scope.userFiles;
                return results;
            };

            $scope.userAnnotaionSearch = function (query) {
                var results = query ? $scope.annotationItems.filter(createFilterFor(query)) : $scope.annotationItems;
                return results;
            };

            function createFilterFor(query) {
                return function filterFn(item) {
                    return (item.name.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            function isUrl(s) {
                var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
                return regexp.test(s);
            }

            function isGuid(value) {
                var regex = /[a-f0-9]{8}(?:-[a-f0-9]{4}){3}-[a-f0-9]{12}/i;
                var match = regex.exec(value);
                return match != null;
            }

            $scope.dialogCancel = $mdDialog.cancel;
        });

})();