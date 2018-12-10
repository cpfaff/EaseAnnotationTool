(function () {
    'use strict';

    /**
     * 
     */
    angular
        .module('admin.core.gfbio')
        .factory('UIElementsProvider', ['$http', function ($http) {

            var UIElementsProvider = function () {

                UIElementsProvider.prototype.get = function () {
                    return $http.get('api/UIElements/Get');
                }
                UIElementsProvider.prototype.add = function (models) {
                    return $http.post('api/UIElements/Add', models);
                }
                UIElementsProvider.prototype.update = function (models) {
                    return $http.post('api/UIElements/Update', models);
                }
                UIElementsProvider.prototype.delete = function (models) {
                    var mappedModel = models.reduce(function (map, item) {
                        var mappedData = {
                            id: item.id,
                            elementid: item.elementid,
                            url: item.url
                        };
                        return mappedData;
                    });
                    return $http.post('api/UIElements/DeleteElements', mappedModel);
                }
                UIElementsProvider.prototype.search = function (query) {
                    return $http.get('api/UIElements/Search?query=' + query);
                }
            };
            return new UIElementsProvider();
        }]);

})();