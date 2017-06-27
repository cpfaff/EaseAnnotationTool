(function () {
    'use strict';

    angular.module('usersDashboard.unacceptedUsers', ['ngMaterial', 'md.data.table', 'usersDashboard.core'])
        .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';
            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]);
})();