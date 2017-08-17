(function () {
    'use strict';

    angular.module('outgoing-requests.outgoing-requests-list', ['ngMaterial', 'md.data.table', 'outgoing-requests.core'])
        .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';

            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]);
})();