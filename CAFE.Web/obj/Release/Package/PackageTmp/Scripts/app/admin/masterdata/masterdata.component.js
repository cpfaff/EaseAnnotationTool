(function () {
    'use strict';

    angular
        .module('admin.masterdata')
        .component('masterData', {
            templateUrl: 'Scripts/app/admin/masterdata/masterdata.template.html',
            controller: ['MasterdataProvider', '$scope', '$mdDialog', '$mdToast', 'uiGridGroupingConstants', '$timeout','$q',
              function (MasterdataProvider, $scope, $mdDialog, $mdToast, uiGridGroupingConstants, $timeout, $q) {
                  $scope.columnsForGrouping = [];
                  
                  $scope.vocabularyTypes = { types: [] };
                  $scope.actions = { enableGrid: false };

                  function createFilterFor(query) {
                      return function filterFn(state) {
                        return (state.toLowerCase().indexOf(query.toLowerCase()) != -1);
                      };
                  }

                  $scope.gridOptions = {
                      enableFiltering: true,
                      enableRowSelection: true,
                      treeRowHeaderAlwaysVisible: false,
                      columnDefs: [
                        {
                            displayName: 'Vocabulary',
                            name: 'type',
                            editableCellTemplate: 'uiSelect',
                            editDropdownOptionsFunction: function (item) {
                                var results = item ? $scope.vocabularyTypes.types.filter(createFilterFor(item)) : $scope.vocabularyTypes.types;
                                return results;
                            }
                        },
                        { displayName: 'Owner', name: 'owner', enableCellEdit: false },
                        { displayName: 'Creation date', name: 'creationDate', enableCellEdit: false },
                        { displayName: 'New value', name: 'value', sort: { priority: 1, direction: 'asc' } },
                        { displayName: 'Description', name: 'description' },
                        { displayName: 'Is global', name: 'isGlobal', enableCellEdit: false },
                      ],
                      onRegisterApi: function (gridApi) {
                          $scope.gridApi = gridApi;

                          gridApi.rowEdit.on.saveRow($scope, $scope.saveRow);
                          //gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                          //    //$scope.msg.lastCellEdited = 'edited row id:' + rowEntity.id + ' Column:' + colDef.name + ' newValue:' + newValue + ' oldValue:' + oldValue;

                          //    $scope.$apply();
                          //});
                      },
                      //enableVerticalScrollbar: 0
                  };

                  $scope.saveRow = function (rowEntity) {
                      // create a fake promise - normally you'd use the promise returned by $http or $resource
                      //var promise = $q.defer();

                      if (rowEntity.isGlobal) {
                          MasterdataProvider.saveGlobal(rowEntity);
                          var promise = $q.defer();
                          $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise.promise);
                          promise.resolve();
                      }

                      //// fake a delay of 3 seconds whilst the save occurs, return error if gender is "male"
                      //$interval(function () {
                      //    if (rowEntity.gender === 'male') {
                      //        promise.reject();
                      //    } else {
                      //        promise.resolve();
                      //    }
                      //}, 3000, 1);
                  };

                  function GetAllData() {
                      MasterdataProvider.get().then(function (response) {
                          $scope.gridOptions.data = response.data;
                      });
                  }

                  GetAllData();

                  MasterdataProvider.getVocabularyTypes().then(function (response) {
                      $scope.vocabularyTypes.types = response.data;
                      $scope.gridApi.core.refresh();
                  });
                  
                  $scope.gridOptions.data = [];

                  $scope.changeGrouping = function (item, list) {
                      var idx = list.indexOf(item);

                      if (idx > -1) 
                          list.splice(idx, 1);
                      else 
                          list.push(item);
                      
                      $scope.gridApi.grouping.clearGrouping();

                      list.forEach(function (item) {
                          $scope.gridApi.grouping.groupColumn(item);
                      });
                  };

                  $scope.AddSelelectionToVocabulary = function()
                  {
                      var selectedRows = $scope.gridApi.selection.getSelectedRows();

                      MasterdataProvider.put(selectedRows).then(function (response) {
                          GetAllData();
                      });
                  }

                  $scope.removeSelectedRows = function () {
                      var selectedRows = $scope.gridApi.selection.getSelectedRows();
                      MasterdataProvider.deleteGlobal(selectedRows).then(function (response) {
                          selectedRows.forEach(function (item) {
                              var index = $scope.gridOptions.data.indexOf(item);
                              $scope.gridOptions.data.splice(index, 1);
                          });
                      });
                  }

                  $scope.addNewValue = function () {
                      var data = {
                          "id": 0,
                          "value": 'new value',
                          "description": '',
                          "owner": '',
                          "creationDate": '',
                          "isGlobal": true
                      };
                      $scope.gridOptions.data.unshift(data);
                      
                      $timeout(function () {
                          //$scope.gridApi.selection.selectRow($scope.gridOptions.data[$scope.gridOptions.data.length - 1]);
                          $scope.gridApi.selection.selectRow(data);
                      },100);
                  }
              }
            ]
        });

})();