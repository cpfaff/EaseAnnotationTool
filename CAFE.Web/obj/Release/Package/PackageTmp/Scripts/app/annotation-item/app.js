(function () {
    'use strict';

    var app = angular.module('annotation', [
      'ngRoute',
      'ui.router',
      'ngAnimate',
      'ngMaterial',
      'annotationItem.time',
      'annotationItem.biome',
      'ui.bootstrap',
      'lfNgMdFileInput',
      'annotationItem.model',
      'annotationItem.space',
      'annotationItem.sphere',
      'annotationItem.organism',
      'annotationItem.chemical',
      'annotationItem.method',
      'annotationItem.process',
      'annotationItem.time',
      'annotationItem.common'
    ]);

    app.run(['$document', '$http', function ($document, $http) {
        var toJsonReplacer = function (key, value) {
            var val = value;

            if (/^\$+/.test(key) && key !== '$oid') {
                val = undefined;
            } else if (value && value.document && value.location && value.alert && value.setInterval) {
                val = '$WINDOW';
            } else if (value && $document === value) {
                val = '$DOCUMENT';
            } else if (value && value.$evalAsync && value.$watch) {
                val = '$SCOPE';
            }
            return val;
        };

        angular.toJson = function (obj, pretty) {
            return JSON.stringify(obj, toJsonReplacer, pretty ? '  ' : null);
        };

        $http.defaults.transformRequest = function (d) {
            return angular.isObject(d) && !(angular.toString.apply(d) === '[object File]') ? angular.toJson(d) : d;
        };
    }]);

    app.config(function ($stateProvider, $urlRouterProvider) {
        //
        // For any unmatched url, redirect to /state1
        $urlRouterProvider.otherwise("/common");

        $stateProvider
            .state('annotation', {
                abstract: true,
                templateUrl: "Scripts/app/annotation-item/views/__main.html",
                controller: 'annotationController'
            });
        //
        // Now set up the states

        $stateProvider
            .state('annotation.time', {
                url: "/time",
                templateUrl: "Scripts/app/annotation-item/views/time.html",
                parent: 'annotation',
                controller: 'annotationTimeController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            })
            .state('annotation.sphere', {
                url: "/sphere",
                templateUrl: "Scripts/app/annotation-item/sphere/templates/sphere.template.html",
                parent: 'annotation',
                controller: 'annotationSphereController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            })
            .state('annotation.space', {
                url: "/space",
                templateUrl: "Scripts/app/annotation-item/views/space.html",
                parent: 'annotation',
                controller: 'annotationSpaceController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            })
            .state('annotation.biome', {
                url: "/biome",
                templateUrl: "Scripts/app/annotation-item/views/biome.html",
                parent: 'annotation',
                controller: 'BiomeRootControler',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            })
            .state('annotation.organism', {
                url: "/organism",
                templateUrl: "Scripts/app/annotation-item/views/organism.html",
                parent: 'annotation',
                controller: 'annotationOrganismController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            }).
            state('annotation.method', {
                url: "/method",
                templateUrl: "Scripts/app/annotation-item/views/method.html",
                parent: 'annotation',
                controller: 'annotationMethodController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            }).
            state('annotation.chemical', {
                url: "/chemical",
                templateUrl: "Scripts/app/annotation-item/views/chemical.html",
                parent: 'annotation',
                controller: 'annotationChemicalController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            }).
            state('annotation.common', {
                url: "/common",
                templateUrl: "Scripts/app/annotation-item/views/common.html",
                parent: 'annotation',
                controller: 'annotationCommonController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            })
            .state('annotation.process', {
                url: "/process",
                templateUrl: "Scripts/app/annotation-item/process/templates/process.template.html",
                parent: 'annotation',
                controller: 'annotationProcessController',
                resolve: {
                    actualAnnotationModel: function (AnnotationItemProvider) {
                        return AnnotationItemProvider.getActualModel();
                    }
                }
            });
    });

    app.controller('annotationController', [
        '$scope', '$rootScope', '$state', 'AnnotationItemProvider', '$mdDialog', '$location', '$timeout', function ($scope, $rootScope, $state, AnnotationItemProvider, $mdDialog, $location, $timeout) {

            $scope.mainMenuItems = ['Common', 'Time', 'Space', 'Sphere', 'Biome', 'Organism', 'Process', 'Chemical', 'Method'];
            $scope.errorsList = [];
            $scope.isAccessible = false;
            $rootScope.mdTabs = { selectedIndex: 0, selectedSubIndex: 0 };

            $scope.navigateTo = function (path, e, subTabIndex) {

                console.log(path);
                $state.go(path);

                if (undefined != subTabIndex)
                    $rootScope.mdTabs.selectedIndex = subTabIndex;
            }

            $scope.loading = false;
            $scope.activeMenuItem = $state;

            $scope.FindValueInVocabulary = function (vocabulary, value) {
                var foundValue;
                vocabulary.every(function (item) {
                    if (item.value == value) {
                        foundValue = item;
                        return false;
                    }

                    return true;
                });

                return foundValue;
            }

            $scope.RedirectToError = function (error) {
                $state.go('annotation.' + error.path);

                if (error.tab) {
                    $scope.mdTabs.selectedIndex = error.tab;
                    if (error.subTab) {
                        $scope.mdTabs.selectedSubIndex = error.subTab;
                    }
                }
            }

            //The AnnotationModel init here and after it should be visible in inherited scopes

            AnnotationItemProvider.initModel();

            AnnotationItemProvider.getSimpleTypesVocabularies().then(function (result) {
                $scope.simpleTypesVocabularies = result.data;
            });

            if ('' != annotationItemId) {

                if (!$scope.loading) { //!AnnotationItemProvider.getIsLoading()) {
                    $scope.loading = true;
                    if (!AnnotationItemProvider.getIsLoading()) {
                        AnnotationItemProvider.setIsLoading(true);
                        AnnotationItemProvider.getAnnotationItemById(annotationItemId).then(function (result) {
                            AnnotationItemProvider.setActualModel(result.data);
                            AnnotationItemProvider.setIsAccessible(result.data.isAccessible);
                            $scope.isAccessible = result.data.isAccessible;
                            annotationItemId = '';
                            if ($state.current.name != 'annotation.common') {
                                $state.transitionTo('annotation.common', { "uniqueRequestCacheBuster": null, reload: true });
                            }
                            else {
                                $state.reload();
                            }
                            AnnotationItemProvider.setIsLoading(false);
                            $scope.loading = false;
                        },
                        function (data) {
                            $scope.loading = false;
                            AnnotationItemProvider.setIsLoading(false);
                        });
                    }
                }
            } else if ('' != cloningItemId) {
                if (!$scope.loading) {
                    $scope.loading = true;
                    if (!AnnotationItemProvider.getIsLoading()) {
                        AnnotationItemProvider.setIsLoading(true);
                        AnnotationItemProvider.cloneAnnotationItemById(cloningItemId).then(function (result) {
                            result.data.annotationItem.id = null;
                            AnnotationItemProvider.setActualModel(result.data);
                            $scope.isAccessible = true;
                            AnnotationItemProvider.setIsAccessible($scope.isAccessible);
                            cloningItemId = '';
                            $state.reload($state.current.name);
                            $timeout(function () {
                                AnnotationItemProvider.setIsLoading(false);
                                $scope.loading = false;
                            }, 0);
                        },
                            function (data) {
                                $timeout(function () {
                                    $scope.loading = false;
                                    AnnotationItemProvider.setIsLoading(false);
                                }, 0);
                            });
                    }
                }
            } else {
                $scope.isAccessible = true;
                AnnotationItemProvider.setIsAccessible($scope.isAccessible);
            }

            $scope.ImportDialogOpen = function (elementName, array, event, selectingModel) {
                $mdDialog.show({
                    clickOutsideToClose: true,
                    controller: 'importDialogController',
                    controllerAs: 'ctrl',
                    focusOnOpen: false,
                    targetEvent: event,
                    locals: { elementName: elementName, array: array, selectingModel: selectingModel },
                    templateUrl: 'Scripts/app/annotation-item/views/importDialog.html',
                });
            };

            $scope.CollectionsImportDialogOpen = function (event, arrayToImport, objectWithHeaders, AIClassName, structureToWrite, updateFunction) {
                $mdDialog.show({
                    clickOutsideToClose: true,
                    controller: 'collectionsImportDialogController',
                    controllerAs: 'ctrl',
                    focusOnOpen: false,
                    targetEvent: event,
                    locals: { headersObject: objectWithHeaders, array: arrayToImport, structureToWrite: structureToWrite, AIClassName: AIClassName, updateFunction: updateFunction },
                    templateUrl: 'Scripts/app/annotation-item/views/collectionsImportDialog.html',
                });
            };

            $scope.copyToMyData = function () {
                var model = AnnotationItemProvider.getActualModel();
                $scope.loading = true;
                AnnotationItemProvider.copyToMyData(model.annotationItem.id).
                   success(function (result) {
                       $scope.loading = false;
                       window.location.replace("/AnnotationItem?id=" + result);
                   }).
                   error(function (result) {
                       $scope.loading = false;
                   });
            };

            $scope.saveAnnotationItem = function () {
                var model = AnnotationItemProvider.getActualModel();
                $scope.errorsList = [];
                $scope.successSaving = null;

                $scope.loading = true;
                
                AnnotationItemProvider.save(model).
                    success(function (result) {
                        AnnotationItemProvider.setActualModel(result);
                        model = result;
                        $scope.successSaving = true;
                        $scope.loading = false;
                        window.location.href = "/Dashboard";
                    }).
                    error(function (result) {
                        if (result.exceptionMessage) {
                            $scope.errorsList.push({
                                path: null,
                                tab: null,
                                subTab: null,
                                errorpath: null,
                                messages: result.exceptionMessage
                            });
                        }

                        else
                            for (var item in result.modelState) {
                                if ('$id' == item)
                                    continue;
                                var errorPath = item.split('.');
                                var tab = null;
                                var subTab = null;
                                if (errorPath[3] && 'contexts[0]' == errorPath[3].toLowerCase() && errorPath[4]) //context
                                {
                                    var context = errorPath[4] ? errorPath[4].toLowerCase() : null;
                                    var subContext = errorPath[5] ? errorPath[5].toLowerCase() : 0;
                                    var tabIndex;

                                    //Time
                                    if ('timeranges' == subContext)
                                        tab = 0;
                                    else if ('timeperiods' == subContext)
                                        tab = 1;
                                    else if ('temporalresolutions' == subContext)
                                        tab = 2;

                                        //Space
                                    else if ('boundingboxes' == subContext || 'elevations' == subContext)
                                        tab = 0;
                                    else if ('coordinates' == subContext)
                                        tab = 1;
                                    else if ('locations' == subContext)
                                        tab = 2;
                                    else if ('spatialresolutions' == subContext)
                                        tab = 3;

                                        //Biome
                                    else if (-1 != ['zonobiomes', 'orobiomes', 'pedobiomes'].indexOf(subContext)) {
                                        tab = 0;

                                        if ('zonobiomes' == subContext)
                                            subTab = 0;
                                        else if ('orobiomes' == subContext)
                                            subTab = 1;
                                        else if ('pedobiomes' == subContext)
                                            subTab = 2;
                                    }

                                    else if ('physiognomies' == subContext)
                                        tab = 1;

                                    else if ('landuses' == subContext)
                                        tab = 2;

                                        //Process
                                    else if ('processes' == subContext)
                                        tab = 0;

                                    else if ('interactions' == subContext)
                                        tab = 1;

                                        //Chemical
                                    else if (-1 != ['elements', 'isotopes'].indexOf(subContext))
                                        tab = 0;
                                    else if ('compounds' == subContext)
                                        tab = 1;
                                    else if ('functions' == subContext)
                                        tab = 2;

                                        //Method
                                    else if (-1 != ['approaches', 'factors'].indexOf(subContext))
                                        tab = 0;
                                    else if (-1 != ['dataformats', 'datasources'].indexOf(subContext))
                                        tab = 1;

                                    if (context)
                                        $scope.errorsList.push({
                                            path: context.replace("context", "").toLowerCase(),
                                            tab: tab,
                                            subTab: subTab,
                                            messages: result.modelState[item]
                                        });
                                }

                                else if (errorPath[3] && 'resources[0]' == errorPath[3].toLowerCase()) {
                                    $scope.errorsList.push({
                                        path: 'files',
                                        tab: null,
                                        subTab: null,
                                        messages: result.modelState[item]
                                    });
                                }
                                else if (errorPath[3] && 'references' == errorPath[3].toLowerCase()) {
                                    $scope.errorsList.push({
                                        path: 'common',
                                        tab: null,
                                        subTab: null,
                                        messages: result.modelState[item]
                                    });
                                }
                                else {
                                    $scope.errorsList.push({
                                        path: null,
                                        tab: null,
                                        subTab: null,
                                        errorpath: item,
                                        messages: result.modelState[item]
                                    });
                                }
                            }
                        $scope.loading = false;
                    });
            };
        }
    ]);

    app.controller('importDialogController', function ($scope, $state, $mdDialog, AnnotationItemProvider, elementName, array, selectingModel) {
        $scope.ctrl = { importType: 0 };
        function isUrl(s) {
            var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
            return regexp.test(s);
        }

        $scope.closeDialog = $mdDialog.cancel;
        $scope.importDictionary = elementName;
        $scope.errorMessage;

        $scope.AddNewValues = function () {
            var data = {
                DataType: 0,
                IsUrl: false,
                DictionaryName: elementName
            };

            if ('spatialExtentType' == elementName)
                data.DictionaryName = 'spatialResolutionType';

            $scope.errorMessage = null;
            function ExtendSuccess(response) {
                response.data.forEach(function (newValue) {
                    if (newValue == "$id")
                        return false;

                    var itemExists = false;
                    array.every(function (item) {
                        if (item.value == newValue) {
                            itemExists = true;
                            return false;
                        }
                        return true;
                    });

                    if (!itemExists)
                        array.push({ uri: '', value: newValue.value, description: newValue.description });
                });
                if (selectingModel && response.data.length == 1) {
                    if (Object.prototype.toString.call(selectingModel) === '[object Array]')
                    {
                        var objectToPush = {};

                        if ('compoundClass' == elementName || 'compoundType' == elementName)
                            objectToPush = response.data[0];
                        else
                            objectToPush[elementName] = response.data[0];

                        selectingModel.push(objectToPush);
                    }
                    else
                        selectingModel[elementName] = response.data[0];
                }
                $scope.closeDialog();
            }

            if ($scope.ctrl.importType == 0) {
                data.ExtendableData = $scope.ctrl.userValue;
                data.Description = $scope.ctrl.description;
                AnnotationItemProvider.extendDictionaries(data).then(ExtendSuccess);
            }
            else {
                var excelExtensions = ["xls", "xlt", "xlm", "xlsx", "xlsm", "xltx", "xltm", "xlsb", "xla", "xlam", "xll", "xlw"];
                if ($scope.ctrl.importType == 2) {
                    if (isUrl($scope.ctrl.userLink)) {
                        data.ExtendableData = $scope.ctrl.userLink;
                        data.IsUrl = true;

                        var fileExtension = $scope.ctrl.userLink.split('.').pop();

                        if (-1 != fileExtension.indexOf("?"))
                            fileExtension = fileExtension.split('?')[0];

                        if (-1 != excelExtensions.indexOf(fileExtension.toLowerCase()))
                            data.DataType = 1;
                        else if (fileExtension.toLowerCase() == "csv")
                            data.DataType = 2;
                        else {
                            return false;
                        }

                        AnnotationItemProvider.extendDictionaries(data).then(ExtendSuccess);
                    }
                }
                else {
                    var file = $scope.ctrl.files[0].lfFile;
                    var maxFileSize = 1.0; // in GB
                    var fileSize = ((file.size / 1024) / 1024 / 1024).toFixed(4); // GB
                    if (fileSize > maxFileSize) {
                        $scope.errorMessage = "This file has size larger than 1GB. Please select file with less size.";
                        return false;
                    }

                    var reader = new FileReader();
                    reader.onload = function (e) {
                        data.ExtendableData = e.target.result;

                        var fileExtension = $scope.ctrl.files[0].lfFileName.split('.').pop();

                        if (-1 != excelExtensions.indexOf(fileExtension.toLowerCase()))
                            data.DataType = 1;
                        else if (fileExtension.toLowerCase() == "csv")
                            data.DataType = 2;
                        else {
                            $scope.errorMessage = "Error: You must select .csv or excel files only.";
                            return false;
                        }

                        AnnotationItemProvider.extendDictionaries(data).then(ExtendSuccess);;
                    };
                    reader.readAsDataURL(file);
                }
            }
        };
    });
    app.controller('collectionsImportDialogController', function ($scope, $state, $mdDialog, AnnotationItemProvider, headersObject, array, AIClassName, structureToWrite, updateFunction) {
        $scope.ctrl = { importType: 0, error: null, files: [], userLink: "" };
        function isUrl(s) {
            var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/
            return regexp.test(s);
        }

        $scope.closeDialog = $mdDialog.cancel;
        $scope.headersArray = [];
        $scope.AIClassName = AIClassName;

        for (var item in headersObject)
            $scope.headersArray.push(item.charAt(0).toUpperCase() + item.slice(1));

        $scope.AddNewValues = function () {
            var data = {
                DataType: 0,
                IsUrl: false,
                AIClassName: AIClassName
            };
            $scope.ctrl.error = null;

            function ExtendSuccess(response) {
                if (!response.data || response.data.length == 0) {
                    $scope.ctrl.error = "Error: unable to find any row. Check CSV structure.";
                }
                else {
                    var newValues = [];
                    response.data.forEach(function (item) {
                        if (structureToWrite) {
                            var obj = {};
                            obj[structureToWrite] = item;
                            array.push(obj);
                            newValues.push(obj);
                        }
                        else {
                            array.push(item);
                            newValues.push(item);
                        }
                    });

                    if (updateFunction)
                        updateFunction(newValues);

                    $scope.closeDialog();
                }
            }

            function ExtendError(response) {
                $scope.ctrl.error = response.data.exceptionMessage;
            }

            var excelExtensions = ["xls", "xlt", "xlm", "xlsx", "xlsm", "xltx", "xltm", "xlsb", "xla", "xlam", "xll", "xlw"];
            if ($scope.ctrl.importType == 1) {
                if (isUrl($scope.ctrl.userLink)) {
                    data.ExtendableData = $scope.ctrl.userLink;
                    data.IsUrl = true;

                    var fileExtension = $scope.ctrl.userLink.split('.').pop();

                    if (-1 != fileExtension.indexOf("?"))
                        fileExtension = fileExtension.split('?')[0];

                    if (-1 != excelExtensions.indexOf(fileExtension.toLowerCase()))
                        data.DataType = 0;
                    else if (fileExtension.toLowerCase() == "csv")
                        data.DataType = 1;
                    else {
                        $scope.ctrl.error = "Error: You must select .csv or excel files only.";
                        return false;
                    }

                    AnnotationItemProvider.importCollection(data).then(ExtendSuccess, ExtendError);
                }
            }
            else {
                var file = $scope.ctrl.files[0].lfFile;
                var maxFileSize = 1.0; // in GB
                var fileSize = ((file.size / 1024) / 1024 / 1024).toFixed(4); // GB
                if (fileSize > maxFileSize) {
                    $scope.ctrl.error = "This file has size larger than 1GB. Please select file with less size.";
                    return false;
                }

                var reader = new FileReader();
                reader.onload = function (e) {
                    data.ExtendableData = e.target.result;

                    var fileExtension = $scope.ctrl.files[0].lfFileName.split('.').pop();

                    if (-1 != excelExtensions.indexOf(fileExtension.toLowerCase()))
                        data.DataType = 0;
                    else if (fileExtension.toLowerCase() == "csv")
                        data.DataType = 1;
                    else {
                        $scope.ctrl.error = "Error: You must select .csv or excel files only.";
                        return false;
                    }

                    AnnotationItemProvider.importCollection(data).then(ExtendSuccess, ExtendError);
                };
                reader.readAsDataURL(file);
            }
        };
    });
})();