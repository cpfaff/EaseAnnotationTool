(function () {
    'use strict';

    angular.module('annotationItem.method', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core']).
        config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('annotationMethodController', function (MethodProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider, $filter, VocabulariesProvider) {
            $scope.methodContext = actualAnnotationModel.annotationItem.object.contexts[0].methodContext;
            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
            $scope.approachModel = {};
            $scope.factorModel = {};
            $scope.organizationalHierarchyModel = {
                bioma: {
                    checked: false
                },
                ecosystem: {
                    checked: false
                },
                population: {
                    checked: false
                },
                organizm: {
                    checked: false
                },
                systemOfOrgans: {
                    checked: false
                },
                organ: {
                    checked: false
                },
                tissue: {
                    checked: false
                },
                cell: {
                    checked: false
                },
                cellOrganelle: {
                    checked: false
                },
                molecule: {
                    checked: false
                },
                atom: {
                    checked: false
                }
            };

            if ($.isEmptyObject($scope.methodContext))
            {
                $scope.methodContext = {
                    approaches: [],
                    factors: [],
                    dataFormats: [],
                    organizationalHierarchies: []
                };

                actualAnnotationModel.annotationItem.object.contexts[0].methodContext = $scope.methodContext;


            }


            if (actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchiesSpecified
                || actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies) {
                for (var i = 0;
                    i <
                        actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies.length;
                    i++) {

                    var name = actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies[i].organizationHierarchyName.value;
                    switch (name) {
                        case 'Biome Level':
                            $scope.organizationalHierarchyModel.bioma.checked = true;
                            break;
                        case 'Ecosystem Level':
                            $scope.organizationalHierarchyModel.ecosystem.checked = true;
                            break;
                        case 'Population Level':
                            $scope.organizationalHierarchyModel.population.checked = true;
                            break;
                        case 'Organism Level':
                            $scope.organizationalHierarchyModel.organizm.checked = true;
                            break;
                        case 'System of Organs Level':
                            $scope.organizationalHierarchyModel.systemOfOrgans.checked = true;
                            break;
                        case 'Organ Level':
                            $scope.organizationalHierarchyModel.organ.checked = true;
                            break;
                        case 'Tissue Level':
                            $scope.organizationalHierarchyModel.tissue.checked = true;
                            break;
                        case 'Cell Level':
                            $scope.organizationalHierarchyModel.cell.checked = true;
                            break;
                        case 'Cell Organelle Level':
                            $scope.organizationalHierarchyModel.cellOrganelle.checked = true;
                            break;
                        case 'Molecule Level':
                            $scope.organizationalHierarchyModel.molecule.checked = true;
                            break;
                        case 'Atmom Level':
                            $scope.organizationalHierarchyModel.atom.checked = true;
                            break;
                    }

                }
            }
            if (!actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies)
                actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies = [];

            var addToOrganize = function (name) {
                var addedAtmL = {
                    organizationHierarchyName: {
                        value: name,
                        url: ''
                    }
                };

                var namedObj = $filter('sphereBiosphereOrganizationalFilter')
                    (actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies, name);
                if (!namedObj)
                    actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies.push(addedAtmL);
            };
            var removeFromBiosphereOrganize = function (name) {
                var namedObj = $filter('sphereBiosphereOrganizationalFilter')
                    (actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies, name);

                var namedIndex = actualAnnotationModel.annotationItem.object.contexts[0].methodContext.organizationalHierarchies.indexOf(namedObj);

                actualAnnotationModel.annotationItem.object.contexts[0].methodContext
                    .organizationalHierarchies.splice(namedIndex, 1);
            };

            function createFilterFor(query) {
                return function filterFn(item) {
                    return (item.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            $scope.querySearch = function (query, vocabulary) {
                return VocabulariesProvider.search(query, vocabulary).then(function (response) {
                    return response.data;
                });
            }

            $scope.AddFactor = function () {
                var factorModel = jQuery.extend(true, {}, $scope.factorModel);

                if (!$scope.methodContext.factors)
                    $scope.methodContext.factors = [];

                $scope.methodContext.factors.push(factorModel);

                $scope.factorModel = {};
            }

            $scope.AddApproach = function () {
                var approachModel = jQuery.extend(true, {}, $scope.approachModel);

                if (!$scope.methodContext.approaches)
                    $scope.methodContext.approaches = [];

                $scope.methodContext.approaches.push(approachModel);

                $scope.approachModel = {};
            }
            
            MethodProvider.get().then(function (result) {
                $scope.methodDictionaries = jQuery.extend(true, {}, result.data);
            });


            $scope.$watch('organizationalHierarchyModel.bioma.checked', function () {
                if ($scope.organizationalHierarchyModel.bioma.checked) {
                    addToOrganize('Biome Level');
                } else {
                    removeFromBiosphereOrganize('Biome Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.ecosystem.checked', function () {
                if ($scope.organizationalHierarchyModel.ecosystem.checked) {
                    addToOrganize('Ecosystem Level');
                } else {
                    removeFromBiosphereOrganize('Ecosystem Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.population.checked', function () {
                if ($scope.organizationalHierarchyModel.population.checked) {
                    addToOrganize('Population Level');
                } else {
                    removeFromBiosphereOrganize('Population Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.organizm.checked', function () {
                if ($scope.organizationalHierarchyModel.organizm.checked) {
                    addToOrganize('Organism Level');
                } else {
                    removeFromBiosphereOrganize('Organism Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.systemOfOrgans.checked', function () {
                if ($scope.organizationalHierarchyModel.systemOfOrgans.checked) {
                    addToOrganize('System of Organs Level');
                } else {
                    removeFromBiosphereOrganize('System of Organs Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.organ.checked', function () {
                if ($scope.organizationalHierarchyModel.organ.checked) {
                    addToOrganize('Organ Level');
                } else {
                    removeFromBiosphereOrganize('Organ Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.tissue.checked', function () {
                if ($scope.organizationalHierarchyModel.tissue.checked) {
                    addToOrganize('Tissue Level');
                } else {
                    removeFromBiosphereOrganize('Tissue Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.cell.checked', function () {
                if ($scope.organizationalHierarchyModel.cell.checked) {
                    addToOrganize('Cell Level');
                } else {
                    removeFromBiosphereOrganize('Cell Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.cellOrganelle.checked', function () {
                if ($scope.organizationalHierarchyModel.cellOrganelle.checked) {
                    addToOrganize('Cell Organelle Level');
                } else {
                    removeFromBiosphereOrganize('Cell Organelle Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.molecule.checked', function () {
                if ($scope.organizationalHierarchyModel.molecule.checked) {
                    addToOrganize('Molecule Level');
                } else {
                    removeFromBiosphereOrganize('Molecule Level');
                }
            });
            $scope.$watch('organizationalHierarchyModel.atom.checked', function () {
                if ($scope.organizationalHierarchyModel.atom.checked) {
                    addToOrganize('Atmom Level');
                } else {
                    removeFromBiosphereOrganize('Atmom Level');
                }
            });



        });
})();