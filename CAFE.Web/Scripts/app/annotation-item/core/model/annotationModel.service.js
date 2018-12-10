(function () {
    'use strict';

    angular
        .module('annotationItem.model')
        .factory('AnnotationItemProvider', ['$http', function ($http) {
            var modelInstance = {};
            var isLoading = false;
            var isAccessible = false;

            var AnnotationItemProvider = function () {
            };

            AnnotationItemProvider.prototype.get = function () {
                return $http.get('/AnnotationItem/GetVocabularies');
            };
            
            AnnotationItemProvider.prototype.getActiveUsers = function (keyWord) {
                return $http.post('/api/AnnotationItem/SearchUsers', { name: keyWord });
            };

            AnnotationItemProvider.prototype.getUserFiles = function () {
                return $http.get('api/UserFiles/GetUserFilesList?userId=');
            }

            AnnotationItemProvider.prototype.getCommonVocabularies = function () {
                return $http.get('api/AnnotationItem/GetCommonVocabularies');
            }

            AnnotationItemProvider.prototype.getAnnotationItemById = function (id) {
                return $http({
                    url: '/Api/AnnotationItem/GetAnnotationItemById',
                    method: "GET",
                    params: { id: id }
                });
            };

            AnnotationItemProvider.prototype.cloneAnnotationItemById = function (id) {
                return $http({
                    url: '/Api/AnnotationItem/CloneAnnotationItemById',
                    method: "GET",
                    params: { id: id }
                });
            };

            AnnotationItemProvider.prototype.getProcessVocabularies = function () {
                return $http.get('/Api/AnnotationItem/GetProcessVocabularies');
            };

            AnnotationItemProvider.prototype.getCurrentUserInfo = function () {
                return $http.get('/Api/AnnotationItem/GetCurrentUserInfo');
            };
            
            AnnotationItemProvider.prototype.save = function (data) {
                return $http.post('/Api/AnnotationItem/' + (data.annotationItem.id == null ? 'Add' : 'Update') + 'AnnotationItem', data);
            };

            AnnotationItemProvider.prototype.extendDictionaries = function (data) {
                return $http.post('/Api/AnnotationItem/ExtendDictionaries', data);
            };

            AnnotationItemProvider.prototype.importCollection = function (data) {
                return $http.post('/Api/AnnotationItem/ImportCollection', data);
            };

            AnnotationItemProvider.prototype.getAIAccessibleUsersAndGroups = function (id) {
                return $http({
                    url: '/Api/AnnotationItem/GetAnnotationItemAccessibleUsersAndGroups',
                    method: "GET",
                    params: { id: id }
                });
            };

            AnnotationItemProvider.prototype.copyToMyData = function (id) {
                return $http({
                    url: '/Api/AnnotationItem/CopyAIToMyData',
                    method: "GET",
                    params: { id: id }
                });
            };

            AnnotationItemProvider.prototype.getSimpleTypesVocabularies = function () {
                return $http.get('/Api/AnnotationItem/GetSimpleTypesVocabularies');
            };

            AnnotationItemProvider.prototype.getUserHiddenHelpers = function () {
                return $http.get('/Api/AnnotationItem/GetUserHiddenHelpers');
            };

            AnnotationItemProvider.prototype.hideUserHelper = function (helperName) {
                return $http({
                    method: "POST",
                    url: '/Api/AnnotationItem/HideUserHelper',
                    data: JSON.stringify(helperName)
                });
            };

            AnnotationItemProvider.prototype.searchUsersAndGroups = function (keyWord) {
                return $http.post('api/UserFiles/SearchUsersAndGroups', { KeyWord: keyWord }, { headers: { 'Content-Type': 'application/json' } });
            }

            AnnotationItemProvider.prototype.initModel = function () {
                if (!modelInstance.annotationItem) {
                    modelInstance =
                    {
                        annotationItem:
                            {
                                object: {
                                    contexts: [
                                        {
                                            timeContext: {},
                                            spaceContext: {},
                                            sphereContext: {},
                                            biomeContext: {},
                                            organismContext: {},
                                            chemicalContext: {},
                                            methodContext: {}
                                        }
                                    ],
                                    references: {},
                                    resources: [{ offlineResources: [] }]
                                },
                                accessMode: 0,
                                name: null,
                                description: null
                            },
                        acceptedUsersAndGroups: []
                    }
                }

                return modelInstance;
            };

            AnnotationItemProvider.prototype.getActualModel = function () {
                return modelInstance;
            };

            AnnotationItemProvider.prototype.setActualModel = function (model) {
                modelInstance = model;
            };

            AnnotationItemProvider.prototype.getActualModel = function () {
                return modelInstance;
            };

            AnnotationItemProvider.prototype.setActualModel = function (model) {
                modelInstance = model;
            };

            AnnotationItemProvider.prototype.getIsLoading = function () {
                return isLoading;
            };

            AnnotationItemProvider.prototype.setIsLoading = function (ind) {
                isLoading = ind;
            };

            AnnotationItemProvider.prototype.getIsAccessible = function () {
                return isAccessible;
            };

            AnnotationItemProvider.prototype.setIsAccessible = function (access) {
                isAccessible = access;
            };
            return new AnnotationItemProvider();
        }]);

})();