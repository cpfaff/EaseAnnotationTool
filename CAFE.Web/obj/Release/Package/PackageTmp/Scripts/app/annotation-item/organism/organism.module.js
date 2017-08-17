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
        controller('annotationOrganismController', function (OrganismProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider)
        {
            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
            $scope.organismContext = actualAnnotationModel.annotationItem.object.contexts[0].organismContext;
            
            $scope.organismModel = {};
            $scope.autocompleteFields = {};

            $scope.autocompleteFieldsNames = ['kingdom', 'family', 'class', 'order'];

            $scope.networkSearching = ['family', 'class', 'order'];

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

            $scope.querySearch = function (query, key) {
                if (-1 != $scope.networkSearching.indexOf(key))
                {
                    var data = {
                        search: query,
                        type: key
                    };

                    return OrganismProvider.getOrganismProperyValues(data).then(function (response) {
                        return response.data;
                    });
                }
                else {
                    var vocabulary = $scope.organismVocabularies[key];
                    var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;
                    return results;
                }
            }

            OrganismProvider.get().then(function (response) {
                $scope.organismVocabularies = jQuery.extend(true, {}, response.data.taxonomyDictionaries);
                $scope.organismModel = jQuery.extend(true, {}, response.data.taxonomyDictionaries);
                $scope.organismModel.taxonomy = response.data.taxonomyDictionaries;
                $scope.organismModel.taxonomy.organismName.botanicalName.tradeDesignationNames = [];
                for (var item in $scope.organismModel.taxonomy) {
                    if (Object.prototype.toString.call($scope.organismModel.taxonomy[item]) === '[object Array]') {
                        $scope.organismModel.taxonomy[item] = null;
                    }
                }
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

                $scope.organismContext.organisms.push({ taxonomy: copiedObject });
                $scope.ClearModel($scope.organismModel.taxonomy);
            }
        });
})();