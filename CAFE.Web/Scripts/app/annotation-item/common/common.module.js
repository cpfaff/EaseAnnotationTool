(function () {
    'use strict';

    angular.module('annotationItem.common', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core'])
        .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('annotationCommonController', function ($scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider) {
            $scope.commonModel = actualAnnotationModel;
            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
            $scope.functionAutocomplete = {};
            $scope.AIAccessModes = [
               'Private',
               'Explicit',
               'Internal',
               'Public'
            ];


            $scope.GetNormalizedName = function (name) {
                var re = /^([a-z])([a-z]*)([A-Z][a-z]*)*/;
                var newstr = name.replace(re, function (string, $1, $2, $3) {
                    return ($1.toUpperCase() + $2 + ($3 ? (' ' + $3.toLowerCase()) : '' ));
                });
                return newstr;
            }

            $scope.currentUserInfo = {};
            $scope.commonVocabularies = [];
            $scope.personToAddModel = {user:null};

            $scope.GetFileModel = function(chip)
            {
                var fileExtension = chip.name.split('.').pop();
                var mimeType = MimeType.get('.' + fileExtension);

                return {
                    filePath: chip.downloadURL, 
                    fileName: chip.name, 
                    dataFormat: { value: mimeType }
                };
            }

            $scope.personsTransform = function (object, user, withoutArrays) {
                if (withoutArrays)
                {
                    object.givenName = user.name;
                    object.surName = user.surname;
                    object.salutation = user.salutation;
                    object.emailAddress = user.email;
                    object.phoneNumber = user.phoneNumber;
                    return false;
                }

                object.givenName = [user.name];
                object.surName = [user.surname];
                object.salutation = [user.salutation];
                object.emailAddress = [user.email];
                object.phoneNumber = [user.phoneNumber];
            }

            $scope.querySearchStatic = function (query, vocabulary) {
                var results = query ? vocabulary.filter(createFilterForStandard(query)) : vocabulary;
                return results;
            }

            $scope.querySearch = function (query) {
                var prom = AnnotationItemProvider.searchUsersAndGroups(query);
                return prom.then(function (response) {
                    return response.data;
                });
            }

            $scope.queryActiveUsersSearch = function (query) {
                var prom = AnnotationItemProvider.getActiveUsers(query);
                return prom.then(function (response) {
                    return response.data;
                });
            }

            $scope.ClearModel = function (mainModel) {
                for (var item in mainModel) {
                    if (Object.prototype.toString.call(mainModel[item]) === '[object Array]')
                        mainModel[item] = [];
                    else if (typeof mainModel[item] == 'object') {
                        $scope.ClearModel(mainModel[item]);
                    }
                    else
                        mainModel[item] = null;
                }
            }

            $scope.resources = actualAnnotationModel.annotationItem.object.resources[0].offlineResources;

            if (!$scope.resources)
                $scope.resources = [];

            if (!$scope.commonModel.annotationItem.object.references.hosters)
                $scope.commonModel.annotationItem.object.references.hosters = [];

            if (!$scope.commonModel.annotationItem.object.references.persons)
                $scope.commonModel.annotationItem.object.references.persons = [];

            if (!$scope.commonModel.annotationItem.object.references.descriptions)
                $scope.commonModel.annotationItem.object.references.descriptions = [{ title: null, abstract: null }];

            if ($scope.resources.length) {
                $scope.resources.forEach(function (item) {
                    if (!item.fileName) {
                        var uriExploded = item.filePath.split('/');
                        var fileId = uriExploded[uriExploded.length - 1];
                        item.name = actualAnnotationModel.filesNames[fileId];
                    }
                });
            }
            
            function createFilterForStandard(query) {
                return function filterFn(state) {
                    if (state.value != undefined)
                        return (state.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                    else
                        return (state.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            function createFilterFor(query) {
                return function filterFn(state) {
                    if (state.name != undefined)
                        return (state.name.toLowerCase().indexOf(query.toLowerCase()) != -1);
                    else
                        return (state.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            $scope.queryFilesSearch = function (query, vocabulary) {
                var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;
                return results;
            }

            $scope.AddPerson = function()
            {
                var copiedObject = jQuery.extend(true, {}, $scope.commonVocabularies.persons);
                
                for(var item in copiedObject)
                    copiedObject[item] = [copiedObject[item]];

                $scope.commonModel.annotationItem.object.references.persons.push(copiedObject);

                $scope.ClearModel($scope.commonVocabularies.persons);
            }

            $scope.FillPersonModel = function () {
                var user = $scope.personToAddModel.user;
                if (user) {
                    $scope.personsTransform($scope.commonVocabularies.persons, user, true);
                    $scope.functionAutocomplete.personSearch = null;
                    user = null;
                }
            }

            AnnotationItemProvider.getUserFiles().then(function (result) {
                $scope.filesList = result.data;
                if ('' != fileIds)
                {
                    var filesArray = fileIds.split(',');
                    filesArray.forEach(function (fileId) {
                        $scope.filesList.every(function (file) {
                            if (file.id == fileId) {
                                $scope.resources.push($scope.GetFileModel(file));
                                return false;
                            }
                            return true;
                        });
                    });
                }
            });

            AnnotationItemProvider.getCommonVocabularies().then(function (result) {
                $scope.commonVocabularies = result.data;

                for (var item in $scope.commonVocabularies.persons)
                    $scope.commonVocabularies.persons[item] = 'Position' == item ? { value: null } : null;

                if ('' == annotationItemId)
                    AnnotationItemProvider.getCurrentUserInfo().then(function (result) {
                        $scope.currentUserInfo = result.data;
                        if (!$scope.commonModel.annotationItem.object.references.persons.length) {
                            $scope.personToAddModel.user = $scope.currentUserInfo;
                             $scope.FillPersonModel();
                        }
                    });
            });

        });
})();