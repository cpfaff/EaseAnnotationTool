(function () {
    'use strict';
    angular.module('annotationItem.core', [
        'annotationItem.core.time',
        'annotationItem.core.space',
        'annotationItem.core.biome', 
        'annotationItem.core.organism', 
        'annotationItem.core.chemical', 
        'annotationItem.core.method'
    ]);
})();