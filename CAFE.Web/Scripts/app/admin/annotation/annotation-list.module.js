(function () {
    'use strict';

    angular.module('admin.annotationList', ['ngMaterial', 'md.data.table', 'admin.core'])
            .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
                'use strict';

                $compileProvider.debugInfoEnabled(false);

                $mdThemingProvider.theme('default')
                  .primaryPalette('blue')
                  .accentPalette('pink');
            }]);
})();