(function () {
    'use strict';

    angular.module('admin.masterdata', [
        'ngMaterial', 
        'md.data.table',
        'ui.grid',
        'ui.grid.grouping',
        'ui.grid.selection',
        'ui.grid.edit',
        'ui.grid.rowEdit',
        'ui.grid.cellNav',
        'ui.select',
        'admin.core'])
            .directive('uiSelectWrap', uiSelectWrap)
            .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
                'use strict';

                $compileProvider.debugInfoEnabled(false);

                $mdThemingProvider.theme('default')
                  .primaryPalette('blue')
                  .accentPalette('pink');
            }]);
    
    uiSelectWrap.$inject = ['$document', 'uiGridEditConstants'];
    function uiSelectWrap($document, uiGridEditConstants, $timeout) {
        return function link($scope, $elm, $attr, $timeout) {
            $scope.itemChanged = function (item)
            {
                if(item != null)
                {
                    setTimeout(function () {
                        $scope.$emit(uiGridEditConstants.events.END_CELL_EDIT);
                    }, 100);
                }
            }
        };
    }
    
})();