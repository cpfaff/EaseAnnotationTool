(function () {
    'use strict';

    angular.module('incoming-requests.incoming-requests-list', ['ngMaterial', 'md.data.table', 'incoming-requests.core'])
        .config(['$compileProvider', '$mdThemingProvider', function ($compileProvider, $mdThemingProvider) {
            'use strict';

            $compileProvider.debugInfoEnabled(false);

            $mdThemingProvider.theme('default')
              .primaryPalette('blue')
              .accentPalette('pink');
        }]);
})();