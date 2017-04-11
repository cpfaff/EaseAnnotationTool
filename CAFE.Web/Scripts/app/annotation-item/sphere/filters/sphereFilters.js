(function () {
    'use strict';

    var app = angular.module('annotationItem.sphere');

    app.filter('sphereAtmosphereNamedLayersFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function(item) {
                if (item.atmosphereLayerName.value === name) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });

    app.filter('sphereAtmosphereNumericLayersFilter', function () {
        return function (items, min, max, uom) {

            var result = null;
            items.forEach(function (item) {
                if (item.minimumAtmosphereHeight === min && item.maximumAtmosphereHeight === max
                    && item.minimumAtmosphereHeightUnit === uom && item.maximumAtmosphereHeightUnit) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });

    app.filter('sphereBiosphereNamedLayersFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function (item) {
                if (item.ecosphereLayerName.value === name) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });

    app.filter('sphereBiosphereOrganizationalFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function (item) {
                if (item.organizationHierarchyName.value === name) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });

    app.filter('sphereHydrosphereRiverFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function (item) {
                if (item.ecosphereLayerName.value === name) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });
    app.filter('spherePedosphereNamedLayersFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function (item) {
                if (item.soilHorizon.value === name) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });
    app.filter('spherePedosphereAciditiesFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function (item) {
                if (item.soilAcidityType.value === name) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });
    app.filter('spherePedosphereMorphologyFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function (item) {
                if (item.soilMorphologyType.value === name) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });

    app.filter('sphereUniversalHydrosphereFilter', function () {
        return function (items, name) {

            var result = null;
            items.forEach(function (item) {
                if (item[name.property].value === name.value) {
                    result = item;
                    return result;
                }
            });

            return result;
        };
    });


})();