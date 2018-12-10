(function () {
    'use strict';
    
    angular.module('annotationItem.organism', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core'])
        .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('annotationOrganismController', function (OrganismProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider, VocabulariesProvider)
        {
            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
            $scope.organismContext = actualAnnotationModel.annotationItem.object.contexts[0].organismContext;
            
            $scope.organismModel = {};
            $scope.autocompleteFields = {};

            $scope.autocompleteFieldsNames = ['kingdom', 'family', 'class', 'order', 'genus'];

            $scope.networkSearching = ['family', 'class', 'order', 'species'];

            $scope.requiredTaxonoyFields = ['class', 'division', 'domain', 'family', 'genus', 'order', 'species'];

            $scope.minScore = 98;
            $scope.coverage = 98;

            $scope.precachedSpecies = {
                data: []
            };

            if ($.isEmptyObject($scope.organismContext)) {
                actualAnnotationModel.annotationItem.object.contexts[0].organismContext = $scope.organismContext = {
                    organisms: []
                };
            }

            $scope.CapitalizeFirstLetter = function (string) {
                return string.charAt(0).toUpperCase() + string.slice(1);
            }

            function createFilterFor(query) {
                return function filterFn(item) {
                    return (item.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            function getInternalNameForTaxonomy(externalname)
            {
                switch (externalname) {
                case 'superkingdom':
                    return 'domain';
                case 'superfamily':
                    return 'domain';
                default:
                    return externalname;
                }
            }
            $scope.querySearch = function (query, key) {
                return VocabulariesProvider.search(query, key).then(function (response) {
                    return response.data;
                });

                //debugger;
                //if (-1 != $scope.networkSearching.indexOf(key))
                //{
                //    var data = {
                //        search: query,
                //        type: key
                //    };

                //    return OrganismProvider.getOrganismProperyValues(data).then(function (response) {
                //        return response.data;
                //    });
                //}
                //else {
                //    var vocabulary = $scope.organismVocabularies[key];
                //    var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;
                //    return results;
                //}
            }

            $scope.precachedCpecies = [];
            $scope.searchOrganisms = function (query) {
                var data = {
                    search: query,
                    minScore: $scope.minScore,
                    coverage: $scope.coverage,
                    import: $scope.appendFile
                };

                return OrganismProvider.getOrganisms(data).then(function (response) {
                    $scope.precachedOrganism =
                    {
                        data: response.data
                    };
                    return $scope.precachedOrganism.data;
                });
            }
            $scope.selectedSearchChange = function (item) {
                var foundItem;
                if ($scope.precachedOrganism) {
                    foundItem = $scope.precachedOrganism.data.filter(function (person) {
                        if (item != undefined)
                            return (person.$id == item.$id);
                    })[0];
                }
                if (foundItem) {
                    for (var i = 0; i < foundItem.names.length; i++) {

                        console.log(foundItem.names[i].rank + ":" + foundItem.names[i].value);

                        var internalTaxonomyName = getInternalNameForTaxonomy(foundItem.names[i].rank);
                        if ($scope.organismModel.taxonomy.hasOwnProperty(internalTaxonomyName)) {
                            if (internalTaxonomyName == 'species') {
                                $scope.organismModel.taxonomy[internalTaxonomyName] = foundItem.names[i].value;
                            } else {
                                $scope.organismModel.taxonomy[internalTaxonomyName] =
                                    { uri: '', value: foundItem.names[i].value };
                            }
                        }
                    }
                }
            }

            OrganismProvider.get().then(function (response) {
                $scope.organismVocabularies = jQuery.extend(true, {}, response.data.taxonomyDictionaries);
                $scope.organismModel = jQuery.extend(true, {}, response.data.taxonomyDictionaries);
                $scope.organismModel.taxonomy = response.data.taxonomyDictionaries;
                $scope.organismModel.dictionaries = response.data.taxonomyDictionaries;
                for (var item in $scope.organismModel.taxonomy) {
                    if (Object.prototype.toString.call($scope.organismModel.taxonomy[item]) === '[object Array]') {
                        $scope.organismModel.taxonomy[item] = null;
                    }
                }
                $scope.minScore = 98;
                $scope.coverage = 100;
            });


            $scope.timeRange = {
                rangeStart: {},
                rangeEnd: {}
            };

            $scope.ClearModel = function (mainModel) {
                for (var item in mainModel) {
                    if (Object.prototype.toString.call(mainModel[item]) === '[object Array]') 
                        mainModel[item] = [];
                    else if (typeof mainModel[item] == 'object') {
                        if (-1 != $scope.autocompleteFieldsNames.indexOf(item)) {
                            mainModel[item] = null;
                            $scope.autocompleteFields[item] = '';
                        }
                        else
                            $scope.ClearModel(mainModel[item]);
                    }
                    else
                        mainModel[item] = null;
                }
            }

            $scope.GetNormalizedName = function(name)
            {
                var re = /^([a-z])([a-z]+)([A-Z]?[a-z]*)/;
                var newstr = name.replace(re, function (string, $1, $2, $3) {
                    return ($1.toUpperCase() + $2 + ' ' + ($3 == undefined ? '' : $3));
                });

                return newstr;
            }

            $scope.GetNormalizedName2 = function (name) {
                var re = /([A-z]?)([a-z]+)/g;
                var newstr = name.replace(re, function (string, $1, $2, $3) {
                    return ($1.toLowerCase() + $2 + ' ');
                });
                return newstr;
            }

            $scope.AddOrganism = function()
            {
                var copiedObject = jQuery.extend(true, {}, $scope.organismModel.taxonomy);

                if (!$scope.organismContext.organisms)
                    $scope.organismContext.organisms = [];

                copiedObject.species = copiedObject.species.species;
                $scope.organismContext.organisms.push({ taxonomy: copiedObject });
                $scope.ClearModel($scope.organismModel.taxonomy);
            }

            $scope.appendFile;

            $scope.updateAfterFileSelected = function(data) {
                $scope.appendFile = data;
            };

            $scope.ImportTaxonomyDialog = function (event) {
                $mdDialog.show({
                    clickOutsideToClose: false,
                    controller: 'TaxonomyImportController',
                    controllerAs: 'ctrl',
                    focusOnOpen: false,
                    targetEvent: event,
                    locals: { UpdateFunction: $scope.updateAfterFileSelected },
                    templateUrl: 'Scripts/app/annotation-item/organism/templates/selectTaxonomySourceDialog.template.html'
                });
            }
        });


    angular.module('annotationItem.organism').
        controller('TaxonomyImportController', function (AnnotationItemProvider, $scope, $mdDialog, UpdateFunction, $mdToast) {

            'use strict';
            $scope.model = {};
            $scope.ctrl = { importType: 0, importType2: 0, itemName: '', itemDescription: '' };
            $scope.userFiles = [];

            $scope.isLoading = false;
            AnnotationItemProvider.getUserFiles().then(function (result) {
                $scope.userFiles = result.data;
                $scope.userFiles.forEach(function (item) {
                    item.downloadURL = window.location.origin + item.downloadURL;
                });
            });

            $scope.userFilesSearch = function (query) {
                var results = query ? $scope.userFiles.filter(createFilterFor(query)) : $scope.userFiles;
                return results;
            }

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

            function CheckFileExtension(inputText, validExtensions) {
                var fileExtension = inputText.split('.').pop();

                if (-1 != fileExtension.indexOf("?"))
                    fileExtension = fileExtension.split('?')[0];

                if (!validExtensions || -1 != validExtensions.indexOf(fileExtension.toLowerCase()))
                    return true;

                return false;
            }


            $scope.Import = function () {
                $scope.isLoading = true;
                $scope.ctrl.error = null;

                var data = {};

                function ImportSuccess(data) {
                    UpdateFunction(data);
                    $scope.isLoading = false;
                    $mdDialog.hide();
                }


                var validExtensions = ["csv"];

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
                            $scope.ctrl.error = "You must specify .csv file only.";
                            $scope.isLoading = false;
                            return false;
                        }
                        ImportSuccess(data);
                    }
                    else if (selectedData && isGuid(selectedData.id)) {
                        data.ExtendableData = selectedData.id;
                        data.UseTransormation = false;
                        data.Name = $scope.ctrl.itemName;
                        data.Description = $scope.ctrl.itemDescription;
                        data.ExtendableDataName = inputText;
                        data.DataType = 2;

                        if (!CheckFileExtension(inputText, validExtensions)) {
                            $scope.ctrl.error = "You must specify .csv file only.";
                            $scope.isLoading = false;
                            return false;
                        }
                        ImportSuccess(data);
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
                            $scope.ctrl.error = "You must specify .csv file only.";
                            $scope.isLoading = false;
                            return false;
                        }
                        ImportSuccess(data);

                    };
                    reader.readAsDataURL($scope.ctrl.files[0].lfFile);
                }
            }

            $scope.dialogCancel = $mdDialog.cancel;
        });

})();