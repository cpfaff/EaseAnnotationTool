(function () {
    'use strict';

    angular.module('annotationItem.space')
        .controller('annotationSpaceController', function ($scope, $mdDialog, AnnotationItemProvider, SpaceProvider, leafletData, actualAnnotationModel, $timeout) {

            var _mainMap;
            $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
            $scope.settings = { elevationEnabled: false };
            $scope.autocompleteFields = {};

            function createFilterFor(query) {
                return function filterFn(item) {
                    return (item.value.toLowerCase().indexOf(query.toLowerCase()) != -1);
                };
            }

            $scope.querySearch = function (query, vocabulary) {
                var results = query ? vocabulary.filter(createFilterFor(query)) : vocabulary;

                return results;
            }

            $scope.queryRequestSearch = function (search, type) {
                return SpaceProvider.getLocations({ type: type, search: search }).then(function (response) {

                    return response.data;
                });
            }

            $scope.ClearModel = function (mainModel) {
                for (var item in mainModel) {
                    if (Object.prototype.toString.call(mainModel[item]) === '[object Array]')
                        mainModel[item] = [];
                    else if (typeof mainModel[item] == 'object') {
                        $scope.ClearModel(mainModel[item]);
                    }
                    else
                        mainModel[item] = null;
                }
            }

            $scope.CheckText = function(model, fieldName)
            {
                if (!model)
                    $scope.autocompleteFields[fieldName] = null;
            }

            $scope.CheckNotSpecifiedArrays = function(object)
            {
                for(var item in object)
                {
                    if (-1 != item.indexOf('Specified') && object[item] == false)
                    {
                        var arrayName = item.split("Specified")[0];
                        object[arrayName] = [];
                    }
                }
            }

            $scope.editModel = {
                boundingBox: {
                    northBoundingCoordinate: 25,
                    southBoundingCoordinate: -25,
                    eastBoundingCoordinate: 25,
                    westBoundingCoordinate: -25
                },
                elevation: {
                    maximumElevation: 0,
                    maximumElevationUnit: { value: null, uri: '' },
                    minimumElevation: 0,
                    minimumElevationUnit: { value: null, uri: '' },
                    elevationDatum: { value: null, uri: '' }
                }
            }
            $scope.spaceContext = {
                boundingBoxes: [],
                elevations: [],
                coordinates: [],
                locations: [],
                spatialResolutions:
                [
                    {
                        spatialExtentType: null,
                        spatialResolutionType: null
                    }
                ]
                //spatialResolutions:
                //{
                //    spatialResolution:
                //    {
                //        spatialExtentType: null,
                //        spatialResolutionType: null
                //    }
                //}
            };

            $scope.coordinatesModel = {
                utmCoordinate: {
                    utmCoordinateZone: null,
                    utmCoordinateSubZone: null,
                    utmCoordinateHemisphere: null,
                    utmCoordinateEasting: null,
                    utmCoordinateEastingUnit: null,
                    utmCoordinateNorthing: null,
                    utmCoordinateNorthingUnit: null,
                    utmCoordinateGeodeticDatum: null
                }
            };

            $scope.locationsModel = {
                locationName: null,
                locationType: null,
                countryName: null,
                continentName: null
            };

            $scope.vocabularies = {uoms: []};

            SpaceProvider.get().then(function (response) {
                $scope.vocabularies = response.data;
            });

            function UpdateModel(newModel) {
                $scope.coordinatesModel.utmCoordinate = newModel;
            }

            $scope.AddCoordinate = function (model) {
                if (!model)
                {
                    model = $scope.coordinatesModel;
                    if(!$scope.AddNewCoordinatesToMap([model]))
                    {
                        $("#coordinatesError_Modal").modal("show");
                        return false;
                    } 
                }

                var model_copy = jQuery.extend(true, {}, model);
                if (!$scope.spaceContext.coordinates)
                    $scope.spaceContext.coordinates = [];

                if (!model_copy.utmCoordinate.utmCoordinateEasting)
                    model_copy.utmCoordinate.utmCoordinateEasting = 0.0;

                if (!model_copy.utmCoordinate.utmCoordinateNorthing)
                    model_copy.utmCoordinate.utmCoordinateNorthing = 0.0;

                $scope.spaceContext.coordinates.push(model_copy);

                $scope.coordinatesModel = {
                    utmCoordinate: {
                        utmCoordinateZone: null,
                        utmCoordinateSubZone: null,
                        utmCoordinateHemisphere: null,
                        utmCoordinateEasting: null,
                        utmCoordinateEastingUnit: null,
                        utmCoordinateNorthing: null,
                        utmCoordinateNorthingUnit: null,
                        utmCoordinateGeodeticDatum: null
                    }
                };
            }

            $scope.AddLocation = function () {
                var model_copy = jQuery.extend(true, {}, $scope.locationsModel);

                if (!$scope.spaceContext.locations)
                    $scope.spaceContext.locations = [];

                if (model_copy.locationName)
                    model_copy.locationName = model_copy.locationName.value;

                $scope.spaceContext.locations.push(model_copy);
                $scope.locationsModel = {};
            }

            $scope.onbbdded = function () {
                $scope.spaceContext.boundingBoxes.push(angular.copy($scope.editModel.boundingBox));
            };
            $scope.onbbremove = function (bb) {
                var index = $scope.spaceContext.boundingBoxes.indexOf(bb);
                $scope.spaceContext.boundingBoxes.splice(index, 1);
            };

            $scope.onelevationdded = function () {
                
                var cp = angular.copy($scope.editModel.elevation);
                cp.maximumElevationUnit = { value: $scope.elevationUom.uom, uri: '' };
                cp.minimumElevationUnit = { value: $scope.elevationUom.uom, uri: '' };
                cp.elevationDatum = { value: $scope.elevationUom.datum, uri: '' };

                if (!$scope.spaceContext.elevations)
                    $scope.spaceContext.elevations = [];

                $scope.spaceContext.elevations.push(cp);
            };
            $scope.onelevationremove = function (elevation) {
                var index = $scope.spaceContext.elevations.indexOf(elevation);
                $scope.spaceContext.elevations.splice(index, 1);
            };

            function FindObjectInArrayByProperty(objectArray, property, value, subProperty) {
                var foundIndex = -1;
                objectArray.every(function (item, i) {
                    if (subProperty) {
                        if (item[subProperty][property] == value) {
                            foundIndex = i;
                            return false;
                        }
                    }
                    else if (item[property] == value) {
                        foundIndex = i;
                        return false;
                    }
                    return true;
                });

                return foundIndex;
            }

            $scope.AddNewCoordinatesToMap = function (array) {
                var error;
                array.every(function (item) {
                    var currentModel = item.utmCoordinate;

                    var markerCoords = UTMtoLL(
                        {
                            northing: currentModel.utmCoordinateNorthing,
                            easting: currentModel.utmCoordinateEasting,
                            zoneLetter: currentModel.utmCoordinateSubZone,
                            zoneNumber: currentModel.utmCoordinateZone
                        }
                    );

                    if (null == markerCoords) {
                        error = true;
                        return false;
                    }

                    var marker = $scope.AddNewMarker(_mainMap, markerCoords);
                    item.utmCoordinate.markerid = marker._leaflet_id;
                    return true;
                });

                return !error;
            }

            $scope.DeleteCoordinateByMarkerId = function (id) {
                var indexInMap = FindObjectInArrayByProperty(markers, "_leaflet_id", id);
                var indexInAI = FindObjectInArrayByProperty($scope.spaceContext.coordinates, "markerid", id, "utmCoordinate");

                if (markers[indexInMap]) {
                    _mainMap.removeLayer(markers[indexInMap]);
                    markers.splice(indexInMap, 1);
                }

                if (indexInAI != -1)
                    $scope.spaceContext.coordinates.splice(indexInAI, 1);
            }


            $scope.AddNewMarker = function (map, latlng, addToAIModel) {
                var markerCoords = latlng;
                //popup.append($("<div>(" + markerCoords.lat.toFixed(5) + " ; " + markerCoords.lng.toFixed(5) + ")<br></div>"));

                var marker =
                 L
                .marker(markerCoords, { draggable: true })
                .addTo(map)
                .on('dragend', function (e) {
                    var id = e.target._leaflet_id;
                    var crd = marker.getLatLng();
                    var utm = coordinatesToUTM([crd.lat, crd.lng]);

                    var newmodel = {
                        utmCoordinateZone: utm.zoneNumber,
                        utmCoordinateSubZone: utm.zoneLetter,
                        utmCoordinateHemisphere: { value: utm.hemishpere, uri: "" },
                        utmCoordinateEasting: utm.easting,
                        utmCoordinateNorthing: utm.northing,
                        utmCoordinateGeodeticDatum: { value: "Wgs84", uri: "" },
                        utmCoordinateEastingUnit: { value: "Metre", uri: '' },
                        utmCoordinateNorthingUnit: { value: "Metre", uri: '' },
                        markerid: id
                    };

                    var indexInAI = FindObjectInArrayByProperty($scope.spaceContext.coordinates, "markerid", id, "utmCoordinate");

                    $scope.spaceContext.coordinates[indexInAI].utmCoordinate = newmodel;
                    //marker.setPopupContent("(" + crd.lat.toFixed(5) + " ; " + crd.lng.toFixed(5) + ")");

                });

                var crd = marker.getLatLng();
                var utm = coordinatesToUTM([crd.lat, crd.lng]);
                var newmodel = {
                    utmCoordinate: {
                        utmCoordinateZone: utm.zoneNumber,
                        utmCoordinateSubZone: utm.zoneLetter,
                        utmCoordinateHemisphere: { value: utm.hemishpere, uri: "" },
                        utmCoordinateEasting: utm.easting,
                        utmCoordinateNorthing: utm.northing,
                        utmCoordinateGeodeticDatum: { value: "Wgs84", uri: "" },
                        utmCoordinateEastingUnit: { value: "Metre", uri: '' },
                        utmCoordinateNorthingUnit: { value: "Metre", uri: '' },
                        markerid: marker._leaflet_id
                    }
                };

                var popupButton = $("<button id='marker_" + marker._leaflet_id + "'>Delete</button>").click(function (e) {
                    var id = e.currentTarget.id.split("_")[1];
                    $scope.DeleteCoordinateByMarkerId(id);
                })[0];

                marker.bindPopup(popupButton)
                markers.push(marker);

                if (addToAIModel)
                    $scope.AddCoordinate(newmodel);

                return marker;
            }

            var markers = [];
            leafletData.getMap('mainMap').then(function (map) {
                _mainMap = map;
                var boundingBoxCoordinates = [[-25, -25], [25, 25]];

                if ($.isEmptyObject(actualAnnotationModel.annotationItem.object.contexts[0].spaceContext)) {
                    $scope.editModel.boundingBox =
                    {
                        northBoundingCoordinate: 25,
                        southBoundingCoordinate: -25,
                        eastBoundingCoordinate: 25,
                        westBoundingCoordinate: -25
                    };

                    actualAnnotationModel.annotationItem.object.contexts[0].spaceContext = $scope.spaceContext;
                }
                else {
                    $scope.spaceContext = actualAnnotationModel.annotationItem.object.contexts[0].spaceContext;
                    $scope.CheckNotSpecifiedArrays($scope.spaceContext);

                    if ($scope.editModel.boundingBox.southBoundingCoordinate != 0 || $scope.editModel.boundingBox.westBoundingCoordinate != 0 ||
                        $scope.editModel.boundingBox.northBoundingCoordinate != 0 || $scope.editModel.boundingBox.eastBoundingCoordinate != 0)
                        boundingBoxCoordinates = [
                            [$scope.editModel.boundingBox.southBoundingCoordinate, $scope.editModel.boundingBox.westBoundingCoordinate],
                            [$scope.editModel.boundingBox.northBoundingCoordinate, $scope.editModel.boundingBox.eastBoundingCoordinate]
                        ];

                    if ($scope.spaceContext.elevations && $scope.spaceContext.elevations.length > 0)
                        $scope.settings.elevationEnabled = true;

                    $scope.elevation.minValue = $scope.editModel.elevation.minimumElevation;
                    $scope.elevation.maxValue = $scope.editModel.elevation.maximumElevation;

                    if ($scope.spaceContext.coordinates)
                        $scope.AddNewCoordinatesToMap($scope.spaceContext.coordinates);
                }

                angular.extend(map, {
                    editable: true
                });

                var rec = L.rectangle(boundingBoxCoordinates).addTo(map);
                if ($scope.isAccessible)
                    rec.enableEdit();
                rec.
                    on('mouseup', SaveCoordinatesToSpaceContext).
                    on('drag', SaveCoordinatesToSpaceContext).
                    on('editable:editing', SaveCoordinatesToSpaceContext);

                map.on('click', function (e) { $scope.AddNewMarker(map, e.latlng, true); });

                function SaveCoordinatesToSpaceContext() {
                    var latLngs = rec.getLatLngs();

                    var north = latLngs[0][1].lat > latLngs[0][3].lat ? latLngs[0][1].lat : latLngs[0][3].lat;
                    var south = latLngs[0][1].lat < latLngs[0][3].lat ? latLngs[0][1].lat : latLngs[0][3].lat;
                    var west = latLngs[0][0].lng < latLngs[0][2].lng ? latLngs[0][0].lng : latLngs[0][2].lng;
                    var east = latLngs[0][0].lng > latLngs[0][2].lng ? latLngs[0][0].lng : latLngs[0][2].lng;

                    if (
                            west < -180 ||
                            east > 180
                        ) {
                        var westOld = $scope.editModel.boundingBox.westBoundingCoordinate;
                        var eastOld = $scope.editModel.boundingBox.eastBoundingCoordinate;

                        rec.setBounds(
                            L.latLngBounds(
                                    L.latLng(
                                        south,
                                        west < -180 ? -180 : westOld
                                    ),
                                    L.latLng(
                                        north,
                                        east > 180 ? 180 : eastOld
                                    )
                                )
                           );
                    }
                    else
                        $scope.editModel.boundingBox =
                        {
                            northBoundingCoordinate: north,
                            southBoundingCoordinate: south,
                            eastBoundingCoordinate: east,
                            westBoundingCoordinate: west
                        };
                }

                $scope.SetBoundingBoxPostion = function () {

                    if (null != $scope.editModel.boundingBox.southBoundingCoordinate && null != $scope.editModel.boundingBox.westBoundingCoordinate &&
                        null != $scope.editModel.boundingBox.northBoundingCoordinate && null != $scope.editModel.boundingBox.eastBoundingCoordinate)
                        rec.setBounds(
                            L.latLngBounds(
                                    L.latLng(
                                        $scope.editModel.boundingBox.southBoundingCoordinate,
                                        $scope.editModel.boundingBox.westBoundingCoordinate
                                    ),
                                    L.latLng(
                                        $scope.editModel.boundingBox.northBoundingCoordinate,
                                        $scope.editModel.boundingBox.eastBoundingCoordinate
                                    )
                                )
                           );

                    rec.disableEdit();
                    if ($scope.isAccessible)
                        rec.enableEdit();
                }

            });

            $scope.elevationChanged = function () {
                if ($scope.settings.elevationEnabled) {
                    $scope.editModel.elevation = {
                        maximumElevation: 800,
                        maximumElevationUnit: { value: null, uri: '' },
                        minimumElevation: -500,
                        minimumElevationUnit: { value: null, uri: '' },
                        elevationDatum: { value: null, uri: '' }
                    }
                    $timeout(function () { $scope.$broadcast('rzSliderForceRender'); });
                }
                else
                    $scope.editModel.elevation = null;
            }

            $scope.elevation = {
                minValue: -500,
                maxValue: 800,
                options: {
                    floor: -1000,
                    ceil: 1000,
                    step: 0.00001,
                    precision: 5,
                    vertical: true,
                    disabled: !$scope.isAccessible,
                    hidePointerLabels: true,
                    hideLimitLabels: true
                }
            };

            $scope.elevationUom = { uom: 'km', datum: 'sea level' };
            $scope.elevationUoms = ['km', 'm', 'sm'];
            $scope.datums = ['sea level'];

            if ($.isEmptyObject(actualAnnotationModel.annotationItem.object.contexts[0].spaceContext))
                actualAnnotationModel.annotationItem.object.contexts[0].spaceContext = $scope.spaceContext;
            else {
                $scope.spaceContext = actualAnnotationModel.annotationItem.object.contexts[0].spaceContext;
            }
            
            if ($scope.editModel.elevation != undefined) {
                if ($scope.editModel.elevation.maximumElevationUnit != undefined) {
                    if ($scope.editModel.elevation.maximumElevationUnit.value != undefined) {
                        $scope.settings.elevationEnabled = true;
                        $scope.elevationUom.uom = getUomElevMode($scope.editModel.elevation.maximumElevationUnit.value);
                    }
                }
                if ($scope.editModel.elevation.ElevationDatum != undefined) {
                    if ($scope.editModel.elevation.ElevationDatum.value != undefined) {
                        $scope.settings.elevationEnabled = true;
                        $scope.elevationUom.datum = getDatumElev($scope.editModel.elevation.ElevationDatum.value);
                    }
                }

            }
            $scope.$watch('elevationUom.uom', function () {
                $scope.editModel.elevation.maximumElevationUnit.value =
                    getElevModelUom($scope.elevationUom.uom);
                $scope.editModel.elevation.minimumElevationUnit.value =
                    getElevModelUom($scope.elevationUom.uom);
            });

            $scope.$watch('elevation.minValue', function () {
                $scope.editModel.elevation.minimumElevation =
                    $scope.elevation.minValue;
            });

            $scope.$watch('elevation.maxValue', function () {
                $scope.editModel.elevation.maximumElevation =
                    $scope.elevation.maxValue;
            });

            $scope.$watch('elevationUom.datum', function () {
                if ($scope.editModel.elevation.ElevationDatum == undefined) {
                    $scope.editModel.elevation.ElevationDatum = { value: '', uri: '' };
                }
                $scope.editModel.elevation.ElevationDatum.value = getElevDatum($scope.elevationUom.datum);
            });

            function getElevModelUom(uiUom) {
                switch (uiUom) {
                    case 'km':
                        return 'Kilometre';
                    case 'm':
                        return 'Metre';
                    case 'sm':
                        return 'Centimetre';
                    default:
                        return null;
                }
            }
            function getUomElevMode(elev) {
                switch (elev) {
                    case 'Kilometre':
                        return 'km';
                    case 'Metre':
                        return 'm';
                    case 'Centimetre':
                        return 'sm';
                    default:
                        return null;
                }
            }
            function getElevDatum(dat) {
                switch (dat) {
                    case 'sea level':
                        return 'Mean Sea Level';
                    default:
                        return null;
                }
            }

            function getDatumElev(elev) {
                switch (elev) {
                    case 'Mean Sea Level':
                        return 'sea level';
                    default:
                        return null;
                }
            }
        }).
        controller('mapDialogController', function ($scope, $mdDialog, leafletData, currentModel, UpdateFunction) {
            var marker;
            this.cancel = $mdDialog.cancel;
            this.save = function () {

                var crd = marker.getLatLng();
                var utm = coordinatesToUTM([crd.lat, crd.lng]);
                var newmodel = {
                    utmCoordinateZone: utm.zoneNumber,
                    utmCoordinateSubZone: utm.zoneLetter,
                    utmCoordinateHemisphere: { value: utm.hemishpere, uri: "" },
                    utmCoordinateEasting: utm.easting,
                    utmCoordinateNorthing: utm.northing,
                    utmCoordinateGeodeticDatum: { value: "Wgs84", uri: "" },
                    utmCoordinateEastingUnit: { value: "Metre", uri: '' },
                    utmCoordinateNorthingUnit: { value: "Metre", uri: '' }
                };
                UpdateFunction(newmodel);
                $mdDialog.cancel();
            }

            leafletData.getMap('dialogMap').then(function (map) {
                var markerCoords = { lat: 0, lon: 0 };

                if (
                    currentModel.utmCoordinateEasting &&
                    currentModel.utmCoordinateNorthing &&
                    currentModel.utmCoordinateZone &&
                    currentModel.utmCoordinateSubZone
                )
                    markerCoords = UTMtoLL(
                        {
                            northing: currentModel.utmCoordinateNorthing,
                            easting: currentModel.utmCoordinateEasting,
                            zoneLetter: currentModel.utmCoordinateSubZone,
                            zoneNumber: currentModel.utmCoordinateZone
                        }
                    );

                marker =
                 L
                .marker(markerCoords, { draggable: true })
                .addTo(map)
                .bindPopup("(" + markerCoords.lat.toFixed(5) + " ; " + markerCoords.lon.toFixed(5) + ")")
                .on('dragend', function () {
                    var crd = marker.getLatLng();
                    marker.setPopupContent("(" + crd.lat.toFixed(5) + " ; " + crd.lng.toFixed(5) + ")");
                });
            });

        }).
        config(function ($logProvider) {
            $logProvider.debugEnabled(false);
        });
})();
