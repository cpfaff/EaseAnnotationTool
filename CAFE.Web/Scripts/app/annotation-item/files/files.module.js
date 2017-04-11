(function () {
    'use strict';

    angular.module('annotationItem.files', ['ngMaterial', 'md.data.table', 'mdPickers', 'ui.tree', 'annotationItem.core']).
        config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]).
        controller('annotationFilesController', function (FilesProvider, $scope, $mdDialog, actualAnnotationModel, AnnotationItemProvider) {
            $scope.resources = actualAnnotationModel.annotationItem.object.resources[0].onlineResources[0].uris;

            if ($scope.resources.length)
            {
                $scope.resources.forEach(function (item) {
                    if (!item.name) {
                        debugger
                        var uriExploded = item.downloadUrl.split('/');
                        var fileId = uriExploded[uriExploded.length - 1];
                        item.name = actualAnnotationModel.filesNames[fileId];
                    }
                });
            }

            function createFilterFor(query) {
                return function filterFn(state) {
                    if (state.name != undefined)
                        return (state.name.toLowerCase().indexOf(query.toLowerCase()) != -1);
                    else
                        return (state.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            $scope.querySearch = function (query, vocabulary) {
                $scope.chemicalContext
                var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;
                return results;
            }

            FilesProvider.get().then(function (result)
            {
                $scope.filesList = result.data;
            });
        });
})();