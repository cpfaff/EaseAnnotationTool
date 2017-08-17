(function () {
    'use strict';

    angular.module('annotationItem.sphere')
        .controller('annotationSphereController', ['$scope', 'AnnotationItemProvider', '$timeout', '$filter', 'actualAnnotationModel',
            function ($scope, AnnotationItemProvider, $timeout, $filter, actualAnnotationModel) {

                $scope.isAccessible = AnnotationItemProvider.getIsAccessible();
                $scope.settings = { depthEnabled: false, soilTextureEnabled:false, heightEnabled: false };

                $scope.ReloadSliders = function () { $timeout(function () { $scope.$broadcast('rzSliderForceRender'); }); }

                $scope.depthChanged = function()
                {
                    if (!$scope.settings.depthEnabled)
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.numericSoilLayers = [];
                    else
                        $scope.ReloadSliders();
                }

                $scope.soilTextureChanged = function () {
                    if (!$scope.settings.soilTextureEnabled)
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilTextures = [];
                    else
                        $scope.ReloadSliders();
                }

                $scope.heightChanged = function () {
                    if (!$scope.settings.heightEnabled)
                        $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers = [];
                    else
                        $scope.ReloadSliders();
                }

                $scope.exctSpecify = {
                    minValue: 0,
                    maxValue: 10,
                    options: {
                        floor: 0,
                        ceil: 10,
                        step: 0.1,
                        vertical: false,
                        disabled: !$scope.isAccessible
                    }
                };

                $scope.sphereHeigh = {
                    minValue: 0,
                    maxValue: 100,
                    options: {
                        floor: 0,
                        ceil: 100,
                        step: 1,
                        vertical: true,
                        disabled: !$scope.isAccessible
                    }
                };

                $scope.biosphereVegetation = {
                    min: -1000,
                    max: 1000,
                    uom: 'Millimetre',
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
                $scope.pedosphereHeight = {
                    min: -1000,
                    max: 1000,
                    uom: 'Millimetre',
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
                $scope.vegetationUoms = ['Millimetre', 'Centimetre', 'Metre'];
                $scope.soilHorizonUoms = ['Millimetre', 'Centimetre', 'Metre'];

                $scope.soilMorphologyValues = [
                    'Histosols',
                    'Anthrosols',
                    'Technosols',
                    'Cryosols',
                    'Leptotols',
                    'Solonetz',
                    'Vertisols',
                    'Solonchaks',
                    'Gleysols',
                    'Andosols',
                    'Podzols',
                    'Plinthosols',
                    'Nitisols',
                    'Ferralsols',
                    'Planosols',
                    'Stagnosols',
                    'Chernozems',
                    'Kastanozems',
                    'Phaeozems',
                    'Umbrisols',
                    'Durisols',
                    'Gypsisols',
                    'Calcisols',
                    'Retisols',
                    'Acrisols',
                    'Lixisols',
                    'Alisols',
                    'Luvisols',
                    'Cambisols',
                    'Arenosols',
                    'Fluvisols',
                    'Regosols'
                ];

                $scope.sandySlider = {
                    value: 100,
                    options: {
                        floor: 0,
                        ceil: 100,
                        step: 1,
                        disabled: !$scope.isAccessible
                    }
                };
                $scope.siltySlider = {
                    value: 100,
                    options: {
                        floor: 0,
                        ceil: 100,
                        step: 1,
                        disabled: !$scope.isAccessible
                    }
                };
                $scope.loamySlider = {
                    value: 100,
                    options: {
                        floor: 0,
                        ceil: 100,
                        step: 1,
                        disabled: !$scope.isAccessible
                    }
                };
                var refreshSlider = function () {
                    $timeout(function () {
                        $scope.$broadcast('rzSliderForceRender');
                    });
                };

                $scope.atmosphereModel = {
                    exosphere: {
                        checked: false,
                        min: 550,
                        max: 600,
                        uom: 'Kilometre'
                    },
                    thermosphere: {
                        checked: false,
                        min: 95,
                        max: 95,
                        uom: 'Kilometre'
                    },
                    stratosphere: {
                        checked: false,
                        min: 48,
                        max: 48,
                        uom: 'Kilometre'
                    },
                    troposphere: {
                        checked: false,
                        min: 11,
                        max: 11,
                        uom: 'Kilometre'
                    }
                };

                $scope.biosphereModel = {
                    treeLayer: {
                        checked: false
                    },
                    shrubLayer: {
                        checked: false
                    },
                    herbLayer: {
                        checked: false
                    },
                    mossLayer: {
                        checked: false
                    }
                };
                $scope.soilQualityModel = {
                    acidic: {
                        checked: false
                    },
                    neutral: {
                        checked: false
                    },
                    alkaline: {
                        checked: false
                    }
                };

                $scope.organizationalModel = {
                    bioma: {
                        checked: false
                    },
                    ecosystem: {
                        checked: false
                    },
                    population: {
                        checked: false
                    },
                    organizm: {
                        checked: false
                    },
                    systemOfOrgans: {
                        checked: false
                    },
                    organ: {
                        checked: false
                    },
                    tissue: {
                        checked: false
                    },
                    cell: {
                        checked: false
                    },
                    cellOrganelle: {
                        checked: false
                    },
                    molecule: {
                        checked: false
                    },
                    atom: {
                        checked: false
                    }
                };

                $scope.hydrosphereModel = {
                    lake:{
                        litoral: {
                            checked: false
                        },
                        benthic: {
                            checked: false
                        },
                        empilimnion: {
                            checked: false
                        },
                        metalimnion: {
                            checked: false
                        },
                        hypolimnion: {
                            checked: false
                        },
                        profundal: {
                            checked: false
                        },
                        pelagic: {
                            checked: false
                        }
                    },
                    sea: {
                        litoral: {
                            checked: false
                        },
                        benthic: {
                            checked: false
                        },
                        neritic: {
                            checked: false
                        },
                        hadal: {
                            checked: false
                        },
                        bathyal: {
                            checked: false
                        },
                        abyssal: {
                            checked: false
                        },
                        epipelagic: {
                            checked: false
                        },
                        mesopelagic: {
                            checked: false
                        },
                        bathypelagic: {
                            checked: false
                        },
                        abyssopelagic: {
                            checked: false
                        },
                        hadopelagic: {
                            checked: false
                        },
                        pelagic: {
                            checked: false
                        }
                    },
                    river: {
                        crenon: {
                            checked: false
                        },
                        rhitron: {
                            checked: false
                        },
                        potamon: {
                            checked: false
                        },
                        floodplain: {
                            checked: false
                        },
                        riverBank: {
                            checked: false
                        },
                        riverWater: {
                            checked: false
                        },
                        riverBed: {
                            checked: false
                        }
                    }
                };

                $scope.perdoZoneModel = {
                    oHorizon: {
                        checked: false
                    },
                    aHorizon: {
                        checked: false
                    },
                    eHorizon: {
                        checked: false
                    },
                    bHorizon: {
                        checked: false
                    },
                    cHorizon: {
                        checked: false
                    },
                    rHorizon: {
                        checked: false
                    },
                    lHorizon: {
                        checked: false
                    }
                };

                $scope.soilMorphology = {
                    selected: ''
                };
                
                $scope.sphereContext = {
                    spheres: [{
                    atmosphere: {
                        namedAtmosphereLayers: [
                            //{
                            //     atmosphereLayerName: { value: '', url: '' }
                            //}
                        ],
                        numericAtmosphereLayers: [
                            //{
                            //    minimumAtmosphereHeight: 0,
                            //    minimumAtmosphereHeightUnit: { value: '', url: '' },
                            //    maximumAtmosphereHeight: 0,
                            //    maximumAtmosphereHeightUnit: { value: '', url: '' }
                            //}
                        ]
                    },
                    hydrosphere: {
                        hydrosphereCompartments: [
                            {
                                river: {
                                    namedRiverZones: [
                                        //{
                                        //    longitudinalRiverZone: { value: '', url: '' },
                                        //    verticalRiverZone: { value: '', url: '' }
                                        //}
                                    ]
                                },
                                lake: {
                                    namedLakeZones: [
                                        //{
                                            //benthicLakeZone: { value: '', url: '' },
                                            //pelagicLakeZone: { value: '', url: '' }
                                        //}
                                    ]
                                },
                                sea: {
                                    namedSeaZones: [
                                        //{
                                            //benthicSeaZone: {},
                                            //pelagicSeaZone: {}
                                        //}
                                    ]
                                }
                            }
                        ]
                    },
                    pedosphere: {
                        pedosphereCompartments: [
                            {
                                soil: {
                                    namedSoilLayers: [
                                        //{
                                        //    soilHorizon: { value: '', url: '' }
                                        //}
                                    ],
                                    numericSoilLayers: [
                                        //{
                                        //    minimumSoilDepth: 0,
                                        //    minimumSoilDepthUnit: { value: '', url: '' },
                                        //    maximumSoilDepth: 0,
                                        //    maximumSoilDepthUnit: { value: '', url: '' }
                                        //}
                                    ],
                                    soilTextures: [
                                        //{
                                        //    sandPercent: 0,
                                        //    siltPercent: 0,
                                        //    loamPercent: 0
                                        //}
                                    ],
                                    soilMorphologies: [
                                        //{
                                        //    soilMorphologyType: { value: '', url: '' }
                                        //}
                                    ],
                                    soilAcidities: [
                                        //{
                                        //    soilAcidityType: { value: '', url: '' }
                                        //}
                                    ]
                                }
                            }
                        ]
                    },
                    ecosphere: {
                        namedEcosphereLayers: [
                            //{
                            //    ecosphereLayerName: { value: '', url: '' }
                            //}
                        ],
                        numericEcosphereLayers: [
                            //{
                            //    minimumVegetationHeight: 0,
                            //    minimumVegetationHeightUnit: { value: '', url: '' },
                            //    maximumVegetationHeight: 0,
                            //    maximumVegetationHeightUnit: { value: '', url: '' }
                            //}
                        ],
                        organizationalHierarchies: [
                            //{
                                //organizationHierarchyName: { value: '', url: '' }
                            //}
                        ]
                    }
                    }]
                };

                var annotationItemModel = AnnotationItemProvider.getActualModel();

                if ($.isEmptyObject(annotationItemModel.annotationItem.object.contexts[0].sphereContext) || actualAnnotationModel.annotationItem.object.contexts[0].sphereContext.spheres.length == 0)
                    annotationItemModel.annotationItem.object.contexts[0].sphereContext = $scope.sphereContext;
                else {
                    $scope.sphereContext = annotationItemModel.annotationItem.object.contexts[0].sphereContext;
                }

                function fillExistData() {
                    //Fill atmosphere
                    var atmosphere = $scope.sphereContext.spheres[0].atmosphere;
                    if (atmosphere.namedAtmosphereLayersSpecified || atmosphere.namedAtmosphereLayers) {
                        for (var i = 0;
                            i <
                                atmosphere.namedAtmosphereLayers.length;
                            i++) {

                            var name = atmosphere.namedAtmosphereLayers[i].atmosphereLayerName.value;

                            var num = atmosphere.numericAtmosphereLayers[i];

                            switch (name) {
                            case 'Exosphere':
                                $scope.atmosphereModel.exosphere.checked = true;
                                //$scope.atmosphereModel.exosphere.min = num.minimumAtmosphereHeight;
                                //$scope.atmosphereModel.exosphere.max = num.maximumAtmosphereHeight;
                                //$scope.atmosphereModel.exosphere.uom = num.minimumAtmosphereHeightUnit.value;
                                break;
                            case 'Thermosphere':
                                $scope.atmosphereModel.thermosphere.checked = true;
                                //$scope.atmosphereModel.thermosphere.min = num.minimumAtmosphereHeight;
                                //$scope.atmosphereModel.thermosphere.max = num.maximumAtmosphereHeight;
                                //$scope.atmosphereModel.thermosphere.uom = num.minimumAtmosphereHeightUnit.value;
                                break;
                            case 'Stratosphere':
                                $scope.atmosphereModel.stratosphere.checked = true;
                                //$scope.atmosphereModel.stratosphere.min = num.minimumAtmosphereHeight;
                                //$scope.atmosphereModel.stratosphere.max = num.maximumAtmosphereHeight;
                                //$scope.atmosphereModel.stratosphere.uom = num.minimumAtmosphereHeightUnit.value;
                                break;
                            case 'Troposphere':
                                $scope.atmosphereModel.troposphere.checked = true;
                                //$scope.atmosphereModel.troposphere.min = num.minimumAtmosphereHeight;
                                //$scope.atmosphereModel.troposphere.max = num.maximumAtmosphereHeight;
                                //$scope.atmosphereModel.troposphere.uom = num.minimumAtmosphereHeightUnit.value;
                                break;
                            }
                        }
                    }
                    if (!atmosphere.namedAtmosphereLayers)
                        atmosphere.namedAtmosphereLayers = [];

                    //Fill biosphere
                    var ecosphere = $scope.sphereContext.spheres[0].ecosphere;
                    if (ecosphere.namedEcosphereLayersSpecified || ecosphere.namedEcosphereLayers) {
                        for (var i = 0;
                            i <
                                ecosphere.namedEcosphereLayers.length;
                            i++) {

                            var name = ecosphere.namedEcosphereLayers[i].ecosphereLayerName.value;
                            switch (name) {
                            case 'Tree Layer':
                                $scope.biosphereModel.treeLayer.checked = true;
                                break;
                            case 'Shrub Layer':
                                $scope.biosphereModel.shrubLayer.checked = true;
                                break;
                            case 'Herb Layer':
                                $scope.biosphereModel.herbLayer.checked = true;
                                break;
                            case 'Moss Layer':
                                $scope.biosphereModel.mossLayer.checked = true;
                                break;
                            }

                        }
                    }
                    if (ecosphere.numericEcosphereLayersSpecified || ecosphere.numericEcosphereLayers) {
                        for (var i = 0;
                            i <
                                ecosphere.numericEcosphereLayers.length;
                            i++) {

                            var num = ecosphere.numericEcosphereLayers[i];
                            $scope.biosphereVegetation.min = num.minimumVegetationHeight;
                            $scope.biosphereVegetation.max = num.maximumVegetationHeight;
                            $scope.biosphereVegetation.uom = num.minimumVegetationHeightUnit.value;

                        }

                        if (ecosphere.numericEcosphereLayers.length > 0) {
                            $scope.settings.heightEnabled = true;
                        }
                    }
                    if (ecosphere.organizationalHierarchiesSpecified || ecosphere.organizationalHierarchies) {
                        for (var i = 0;
                            i <
                                ecosphere.organizationalHierarchies.length;
                            i++) {

                            var name = ecosphere.organizationalHierarchies[i].organizationHierarchyName.value;
                            switch (name) {
                                case 'Biome Level':
                                    $scope.organizationalModel.bioma.checked = true;
                                    break;
                                case 'Ecosystem Level':
                                    $scope.organizationalModel.ecosystem.checked = true;
                                    break;
                                case 'Population Level':
                                    $scope.organizationalModel.population.checked = true;
                                    break;
                                case 'Organism Level':
                                    $scope.organizationalModel.organizm.checked = true;
                                    break;
                                case 'System of Organs Level':
                                    $scope.organizationalModel.systemOfOrgans.checked = true;
                                    break;
                                case 'Organ Level':
                                    $scope.organizationalModel.organ.checked = true;
                                    break;
                                case 'Tissue Level':
                                    $scope.organizationalModel.tissue.checked = true;
                                    break;
                                case 'Cell Level':
                                    $scope.organizationalModel.cell.checked = true;
                                    break;
                                case 'Cell Organelle Level':
                                    $scope.organizationalModel.cellOrganelle.checked = true;
                                    break;
                                case 'Molecule Level':
                                    $scope.organizationalModel.molecule.checked = true;
                                    break;
                                case 'Atmom Level':
                                    $scope.organizationalModel.atom.checked = true;
                                    break;
                            }

                        }
                    }

                    if (!ecosphere.namedEcosphereLayers)
                        ecosphere.namedEcosphereLayers = [];
                    if (!ecosphere.numericEcosphereLayers)
                        ecosphere.numericEcosphereLayers = [];
                    if (!ecosphere.organizationalHierarchies)
                        ecosphere.organizationalHierarchies = [];

                    //Fill pedosphere
                    var pedosphereSoil = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil;
                    if (pedosphereSoil.namedSoilLayersSpecified || pedosphereSoil.namedSoilLayers) {
                        for (var i = 0;
                            i <
                                pedosphereSoil.namedSoilLayers.length;
                            i++) {

                            var name = pedosphereSoil.namedSoilLayers[i].soilHorizon.value;

                            switch (name) {
                            case 'O Horizon':
                                $scope.perdoZoneModel.oHorizon.checked = true;
                                break;
                            case 'A Horizon':
                                $scope.perdoZoneModel.aHorizon.checked = true;
                                break;
                            case 'E Horizon':
                                $scope.perdoZoneModel.eHorizon.checked = true;
                                break;
                            case 'B Horizon':
                                $scope.perdoZoneModel.bHorizon.checked = true;
                                break;
                            case 'C Horizon':
                                $scope.perdoZoneModel.cHorizon.checked = true;
                                break;
                            case 'R Horizon':
                                $scope.perdoZoneModel.rHorizon.checked = true;
                                break;
                            case 'L Horizon':
                                $scope.perdoZoneModel.lHorizon.checked = true;
                                break;

                            }

                        }
                    }
                    if (pedosphereSoil.numericSoilLayersSpecified || pedosphereSoil.numericSoilLayers) {
                        for (var i = 0;
                            i <
                                pedosphereSoil.numericSoilLayers.length;
                            i++) {

                            var num = pedosphereSoil.numericSoilLayers[i];
                            $scope.pedosphereHeight.min = num.minimumSoilDepth;
                            $scope.pedosphereHeight.max = num.maximumSoilDepth;
                            $scope.pedosphereHeight.uom = num.minimumSoilDepthUnit.value;
                        }
                        if (pedosphereSoil.numericSoilLayers.length > 0) {
                            $scope.settings.depthEnabled = true;
                        }
                    }
                    if (pedosphereSoil.soilAciditiesSpecified || pedosphereSoil.soilAcidities) {
                        for (var i = 0;
                            i <
                                pedosphereSoil.soilAcidities.length;
                            i++) {

                            var name = pedosphereSoil.soilAcidities[i].soilAcidityType.value;
                            switch (name) {
                                case 'Acidic':
                                    $scope.soilQualityModel.acidic.checked = true;
                                    break;
                                case 'Neutral':
                                    $scope.soilQualityModel.neutral.checked = true;
                                    break;
                                case 'Alkaline':
                                    $scope.soilQualityModel.alkaline.checked = true;
                                    break;
                            }

                        }
                    }
                    if (pedosphereSoil.soilMorphologiesSpecified || pedosphereSoil.soilMorphologies) {
                        for (var i = 0;
                            i <
                                pedosphereSoil.soilMorphologies.length;
                            i++) {

                            var name = pedosphereSoil.soilMorphologies[i].soilMorphologyType.value;
                            $scope.soilMorphology.selected = name;
                            break;
                        }
                    }
                    if (pedosphereSoil.soilTexturesSpecified || pedosphereSoil.soilTextures) {
                        for (var i = 0;
                            i <
                                pedosphereSoil.soilTextures.length;
                            i++) {

                            var num = pedosphereSoil.soilTextures[i];
                            $scope.sandySlider.value = num.sandPercent;
                            $scope.siltySlider.value = num.siltPercent;
                            $scope.loamySlider.value = num.loamPercent;
                        }
                        if (pedosphereSoil.soilTextures.length > 0) {
                            $scope.settings.soilTextureEnabled = true;
                        }
                    }

                    if (!pedosphereSoil.namedSoilLayers)
                        pedosphereSoil.namedSoilLayers = [];
                    if (!pedosphereSoil.numericSoilLayers)
                        pedosphereSoil.numericSoilLayers = [];
                    if (!pedosphereSoil.soilAcidities)
                        pedosphereSoil.soilAcidities = [];
                    if (!pedosphereSoil.soilTextures)
                        pedosphereSoil.soilTextures = [];

                    
                    
                    //Fill hydrosphere
                    var hydrosphereRiver = $scope.sphereContext.spheres[0]
                        .hydrosphere.hydrosphereCompartments[0].river;
                    if (hydrosphereRiver.namedRiverZonesSpecified || hydrosphereRiver.namedRiverZones) {
                        for (var i = 0;
                            i <
                                hydrosphereRiver.namedRiverZones.length;
                            i++) {
                            var nameLat = hydrosphereRiver.namedRiverZones[i].longitudinalRiverZone.value;
                            switch (nameLat) {
                                case 'Crenon':
                                    $scope.hydrosphereModel.river.crenon.checked = true;
                                    break;
                                case 'Rhitron':
                                    $scope.hydrosphereModel.river.rhitron.checked = true;
                                    break;
                                case 'Potamon':
                                    $scope.hydrosphereModel.river.potamon.checked = true;
                                    break;
                            }
                            var nameVert = hydrosphereRiver.namedRiverZones[i].verticalRiverZone.value;
                            switch (nameVert) {
                                case 'Floodplain':
                                    $scope.hydrosphereModel.river.floodplain.checked = true;
                                    break;
                                case 'River Bank':
                                    $scope.hydrosphereModel.river.riverBank.checked = true;
                                    break;
                                case 'River Water Column':
                                    $scope.hydrosphereModel.river.riverWater.checked = true;
                                    break;
                                case 'River Bed':
                                    $scope.hydrosphereModel.river.riverBed.checked = true;
                                    break;
                            }
                        }
                    }

                    var hydrosphereLake = $scope.sphereContext.spheres[0]
                        .hydrosphere.hydrosphereCompartments[0].lake;
                    if (hydrosphereLake.namedLakeZonesSpecified || hydrosphereLake.namedLakeZones) {
                        for (var i = 0;
                            i <
                                hydrosphereLake.namedLakeZones.length;
                            i++) {
                            var nameBenth = hydrosphereLake.namedLakeZones[i].benthicLakeZone.value;
                            switch (nameBenth) {
                                case 'Litoral':
                                    $scope.hydrosphereModel.lake.litoral.checked = true;
                                    break;
                                case 'Benthic':
                                    $scope.hydrosphereModel.lake.benthic.checked = true;
                                    break;
                                case 'Profundal':
                                    $scope.hydrosphereModel.lake.profundal.checked = true;
                                    break;
                            }
                            var namePel = hydrosphereLake.namedLakeZones[i].pelagicLakeZone.value;
                            switch (namePel) {
                                case 'Pelagic':
                                    $scope.hydrosphereModel.lake.pelagic.checked = true;
                                    break;
                                case 'Empilimnion':
                                    $scope.hydrosphereModel.lake.empilimnion.checked = true;
                                    break;
                                case 'Metalimnion':
                                    $scope.hydrosphereModel.lake.metalimnion.checked = true;
                                    break;
                                case 'Hypolimnion':
                                    $scope.hydrosphereModel.lake.hypolimnion.checked = true;
                                    break;
                            }
                        }
                    }

                    var hydrosphereSea = $scope.sphereContext.spheres[0]
                        .hydrosphere.hydrosphereCompartments[0].sea;
                    if (hydrosphereSea.namedSeaZonesSpecified || hydrosphereSea.namedSeaZones) {
                        for (var i = 0;
                            i <
                                hydrosphereSea.namedSeaZones.length;
                            i++) {
                            var nameBenth = hydrosphereSea.namedSeaZones[i].benthicSeaZone.value;
                            switch (nameBenth) {
                                case 'Benthic':
                                    $scope.hydrosphereModel.sea.benthic.checked = true;
                                    break;
                                case 'Litoral':
                                    $scope.hydrosphereModel.sea.litoral.checked = true;
                                    break;
                                case 'Neritic':
                                    $scope.hydrosphereModel.sea.neritic.checked = true;
                                    break;
                                case 'Bathyal':
                                    $scope.hydrosphereModel.sea.bathyal.checked = true;
                                    break;
                                case 'Abyssal':
                                    $scope.hydrosphereModel.sea.abyssal.checked = true;
                                    break;
                                case 'Hadal':
                                    $scope.hydrosphereModel.sea.hadal.checked = true;
                                    break;
                            }
                            var namePel = hydrosphereSea.namedSeaZones[i].pelagicSeaZone.value;
                            switch (namePel) {
                                case 'Pelagic':
                                    $scope.hydrosphereModel.sea.pelagic.checked = true;
                                    break;
                                case 'Epipelagic':
                                    $scope.hydrosphereModel.sea.epipelagic.checked = true;
                                    break;
                                case 'Mesopilagic':
                                    $scope.hydrosphereModel.sea.mesopelagic.checked = true;
                                    break;
                                case 'Bathypelagic':
                                    $scope.hydrosphereModel.sea.bathypelagic.checked = true;
                                    break;
                                case 'Abyssopelagic':
                                    $scope.hydrosphereModel.sea.abyssopelagic.checked = true;
                                    break;
                                case 'Hadopelagic':
                                    $scope.hydrosphereModel.sea.hadopelagic.checked = true;
                                    break;
                            }
                        }
                    }

                    if (!hydrosphereRiver.namedRiverZones)
                        hydrosphereRiver.namedRiverZones = [];
                    if (!hydrosphereLake.namedLakeZones)
                        hydrosphereLake.namedLakeZones = [];
                    if (!hydrosphereSea.namedSeaZones)
                        hydrosphereSea.namedSeaZones = [];

                }


                /////////////////////////////////////////////////
                ////////////////// Atmosphere ///////////////////
                /////////////////////////////////////////////////

                //Add record (checked checkbox) to atmosphere model
                var addToAtmosphere = function (name, selectedmodel) {
                    var addedAtmL = {
                        atmosphereLayerName: {
                            value: name,
                            url: ''
                        }
                    };

                    var namedObj = $filter('sphereAtmosphereNamedLayersFilter')
                        ($scope.sphereContext.spheres[0]
                        .atmosphere.namedAtmosphereLayers, name);
                    if (!namedObj)
                        $scope.sphereContext.spheres[0].atmosphere.namedAtmosphereLayers.push(addedAtmL);

                    var addedAtmN = {
                        minimumAtmosphereHeight: selectedmodel.min,
                        minimumAtmosphereHeightUnit: { value: selectedmodel.uom, uri: '', id: 0 },
                        maximumAtmosphereHeight: selectedmodel.max,
                        maximumAtmosphereHeightUnit: { value: selectedmodel.uom, uri: '', id: 0 }
                    };

                    var numericObj = $filter('sphereAtmosphereNumericLayersFilter')
                        ($scope.sphereContext.spheres[0]
                        .atmosphere.numericAtmosphereLayers, selectedmodel.min,
                        selectedmodel.max, selectedmodel.uom);
                    if (!numericObj)
                        $scope.sphereContext.spheres[0].atmosphere.numericAtmosphereLayers.push(addedAtmN);
                };

                //Remove record from atmosphere model
                var removeFromAtmosphere = function(name, selectedmodel) {
                    var namedObj = $filter('sphereAtmosphereNamedLayersFilter')
                        ($scope.sphereContext.spheres[0]
                        .atmosphere.namedAtmosphereLayers, name);

                    var namedIndex = $scope.sphereContext.spheres[0]
                        .atmosphere.namedAtmosphereLayers.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].atmosphere
                        .namedAtmosphereLayers.splice(namedIndex, 1);

                    var numericObj = $filter('sphereAtmosphereNumericLayersFilter')
                        ($scope.sphereContext.spheres[0]
                        .atmosphere.numericAtmosphereLayers, selectedmodel.min,
                        selectedmodel.max, selectedmodel.uom);

                    var numericIndex = $scope.sphereContext.spheres[0]
                        .atmosphere.numericAtmosphereLayers.indexOf(numericObj);

                    $scope.sphereContext.spheres[0].atmosphere.numericAtmosphereLayers.splice(
                        numericIndex, 1);
                };

                $scope.$watch('atmosphereModel.exosphere.checked', function () {
                    if ($scope.atmosphereModel.exosphere.checked) {
                        addToAtmosphere('Exosphere', $scope.atmosphereModel.exosphere);
                    } else {
                        removeFromAtmosphere('Exosphere', $scope.atmosphereModel.exosphere);
                    }
                });
                $scope.$watch('atmosphereModel.thermosphere.checked', function () {
                    if ($scope.atmosphereModel.thermosphere.checked) {
                        addToAtmosphere('Thermosphere', $scope.atmosphereModel.thermosphere);
                    } else {
                        removeFromAtmosphere('Thermosphere', $scope.atmosphereModel.thermosphere);
                    }
                });
                $scope.$watch('atmosphereModel.stratosphere.checked', function () {
                    if ($scope.atmosphereModel.stratosphere.checked) {
                        addToAtmosphere('Stratosphere', $scope.atmosphereModel.stratosphere);
                    } else {
                        removeFromAtmosphere('Stratosphere', $scope.atmosphereModel.stratosphere);
                    }
                });
                $scope.$watch('atmosphereModel.troposphere.checked', function () {
                    if ($scope.atmosphereModel.troposphere.checked) {
                        addToAtmosphere('Troposphere', $scope.atmosphereModel.troposphere);
                    } else {
                        removeFromAtmosphere('Stratosphere', $scope.atmosphereModel.troposphere);
                    }
                });

                
                /////////////////////////////////////////////////
                /////////////////// Biosphere ///////////////////
                /////////////////////////////////////////////////

                //Add record (checked checkbox) to biosphere model
                var addToBiosphere = function (name) {
                    var addedAtmL = {
                        ecosphereLayerName: {
                            value: name,
                            url: ''
                        }
                    };

                    var existObj = $filter('sphereBiosphereNamedLayersFilter')
                        ($scope.sphereContext.spheres[0]
                        .ecosphere.namedEcosphereLayers, name);
                    if (!existObj)
                        $scope.sphereContext.spheres[0].ecosphere.namedEcosphereLayers.push(addedAtmL);

                    var addedAtmN = {
                        minimumVegetationHeight: $scope.biosphereVegetation.min,
                        minimumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0 },
                        maximumVegetationHeight: $scope.biosphereVegetation.max,
                        maximumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0}
                    };
                    //$scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.push(addedAtmN);
                    if ($scope.settings.heightEnabled)
                        $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers = [ addedAtmN ];
                };
                var addToBiosphereOrganize = function (name) {
                    var addedAtmL = {
                        organizationHierarchyName: {
                            value: name,
                            url: ''
                        }
                    };

                    var namedObj = $filter('sphereBiosphereOrganizationalFilter')
                        ($scope.sphereContext.spheres[0]
                        .ecosphere.organizationalHierarchies, name);
                    if (!namedObj)
                        $scope.sphereContext.spheres[0].ecosphere.organizationalHierarchies.push(addedAtmL);
                };

                //Remove record from biosphere model
                var removeFromBiosphere = function (name) {
                    var namedObj = $filter('sphereBiosphereNamedLayersFilter')
                        ($scope.sphereContext.spheres[0]
                        .ecosphere.namedEcosphereLayers, name);

                    var namedIndex = $scope.sphereContext.spheres[0]
                        .ecosphere.namedEcosphereLayers.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].ecosphere
                        .namedEcosphereLayers.splice(namedIndex, 1);

                    $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.splice(
                        0, 1);
                };
                var removeFromBiosphereOrganize = function (name) {
                    var namedObj = $filter('sphereBiosphereOrganizationalFilter')
                        ($scope.sphereContext.spheres[0]
                        .ecosphere.organizationalHierarchies, name);

                    var namedIndex = $scope.sphereContext.spheres[0]
                        .ecosphere.organizationalHierarchies.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].ecosphere
                        .organizationalHierarchies.splice(namedIndex, 1);
                };

                $scope.$watch('biosphereModel.treeLayer.checked', function () {
                    if ($scope.biosphereModel.treeLayer.checked) {
                        addToBiosphere('Tree Layer');
                    } else {
                        removeFromBiosphere('Tree Layer');
                    }
                });
                $scope.$watch('biosphereModel.shrubLayer.checked', function () {
                    if ($scope.biosphereModel.shrubLayer.checked) {
                        addToBiosphere('Shrub Layer');
                    } else {
                        removeFromBiosphere('Shrub Layer');
                    }
                });
                $scope.$watch('biosphereModel.herbLayer.checked', function () {
                    if ($scope.biosphereModel.herbLayer.checked) {
                        addToBiosphere('Herb Layer');
                    } else {
                        removeFromBiosphere('Herb Layer');
                    }
                });
                $scope.$watch('biosphereModel.mossLayer.checked', function () {
                    if ($scope.biosphereModel.mossLayer.checked) {
                        addToBiosphere('Moss Layer');
                    } else {
                        removeFromBiosphere('Moss Layer');
                    }
                });
                $scope.$watch('biosphereVegetation.min', function () {

                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.length == 0) {
                        foundRecord = {
                            minimumVegetationHeight: $scope.biosphereVegetation.min,
                            minimumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0 },
                            maximumVegetationHeight: $scope.biosphereVegetation.max,
                            maximumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0 }
                        };
                        $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers[0];
                        foundRecord.minimumVegetationHeight =  $scope.biosphereVegetation.min;
                        foundRecord.minimumVegetationHeightUnit = { value: $scope.biosphereVegetation.uom, uri: '', id: 0 };
                    }
                });
                $scope.$watch('biosphereVegetation.max', function () {

                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.length == 0) {
                        foundRecord = {
                            minimumVegetationHeight: $scope.biosphereVegetation.min,
                            minimumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0 },
                            maximumVegetationHeight: $scope.biosphereVegetation.max,
                            maximumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0 }
                        };
                        $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers[0];
                        foundRecord.maximumVegetationHeight = $scope.biosphereVegetation.max;
                        foundRecord.maximumVegetationHeightUnit = { value: $scope.biosphereVegetation.uom, uri: '', id: 0 };
                    }
                });
                $scope.$watch('biosphereVegetation.uom', function () {

                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.length == 0) {
                        foundRecord = {
                            minimumVegetationHeight: $scope.biosphereVegetation.min,
                            minimumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0 },
                            maximumVegetationHeight: $scope.biosphereVegetation.max,
                            maximumVegetationHeightUnit: { value: $scope.biosphereVegetation.uom, uri: '', id: 0 }
                        };
                        $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].ecosphere.numericEcosphereLayers[0];
                        foundRecord.maximumVegetationHeightUnit = { value: $scope.biosphereVegetation.uom, uri: '', id: 0 };
                        foundRecord.minimumVegetationHeightUnit = { value: $scope.biosphereVegetation.uom, uri: '', id: 0 };
                    }
                });

                $scope.$watch('organizationalModel.bioma.checked', function () {
                    if ($scope.organizationalModel.bioma.checked) {
                        addToBiosphereOrganize('Biome Level');
                    } else {
                        removeFromBiosphereOrganize('Biome Level');
                    }
                });
                $scope.$watch('organizationalModel.ecosystem.checked', function () {
                    if ($scope.organizationalModel.ecosystem.checked) {
                        addToBiosphereOrganize('Ecosystem Level');
                    } else {
                        removeFromBiosphereOrganize('Ecosystem Level');
                    }
                });
                $scope.$watch('organizationalModel.population.checked', function () {
                    if ($scope.organizationalModel.population.checked) {
                        addToBiosphereOrganize('Population Level');
                    } else {
                        removeFromBiosphereOrganize('Population Level');
                    }
                });
                $scope.$watch('organizationalModel.organizm.checked', function () {
                    if ($scope.organizationalModel.organizm.checked) {
                        addToBiosphereOrganize('Organism Level');
                    } else {
                        removeFromBiosphereOrganize('Organism Level');
                    }
                });
                $scope.$watch('organizationalModel.systemOfOrgans.checked', function () {
                    if ($scope.organizationalModel.systemOfOrgans.checked) {
                        addToBiosphereOrganize('System of Organs Level');
                    } else {
                        removeFromBiosphereOrganize('System of Organs Level');
                    }
                });
                $scope.$watch('organizationalModel.organ.checked', function () {
                    if ($scope.organizationalModel.organ.checked) {
                        addToBiosphereOrganize('Organ Level');
                    } else {
                        removeFromBiosphereOrganize('Organ Level');
                    }
                });
                $scope.$watch('organizationalModel.tissue.checked', function () {
                    if ($scope.organizationalModel.tissue.checked) {
                        addToBiosphereOrganize('Tissue Level');
                    } else {
                        removeFromBiosphereOrganize('Tissue Level');
                    }
                });
                $scope.$watch('organizationalModel.cell.checked', function () {
                    if ($scope.organizationalModel.cell.checked) {
                        addToBiosphereOrganize('Cell Level');
                    } else {
                        removeFromBiosphereOrganize('Cell Level');
                    }
                });
                $scope.$watch('organizationalModel.cellOrganelle.checked', function () {
                    if ($scope.organizationalModel.cellOrganelle.checked) {
                        addToBiosphereOrganize('Cell Organelle Level');
                    } else {
                        removeFromBiosphereOrganize('Cell Organelle Level');
                    }
                });
                $scope.$watch('organizationalModel.molecule.checked', function () {
                    if ($scope.organizationalModel.molecule.checked) {
                        addToBiosphereOrganize('Molecule Level');
                    } else {
                        removeFromBiosphereOrganize('Molecule Level');
                    }
                });
                $scope.$watch('organizationalModel.atom.checked', function () {
                    if ($scope.organizationalModel.atom.checked) {
                        addToBiosphereOrganize('Atmom Level');
                    } else {
                        removeFromBiosphereOrganize('Atmom Level');
                    }
                });

                

                /////////////////////////////////////////////////
                ////////////////// Pedosphere ///////////////////
                /////////////////////////////////////////////////

                //Add record (checked checkbox) to pedosphere model
                var addToPedosphereZones = function (name) {
                    var addedAtmL = {
                        soilHorizon: {
                            value: name,
                            url: ''
                        }
                    };

                    var namedObj = $filter('spherePedosphereNamedLayersFilter')(
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.namedSoilLayers, name);
                    if (!namedObj)
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.namedSoilLayers.push(addedAtmL);

                    if ($scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.numericSoilLayers
                        .length ==
                        0 && $scope.settings.depthEnabled) {
                        var addedAtmN = {
                            minimumSoilDepth: $scope.pedosphereHeight.min,
                            minimumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 },
                            maximumSoilDepth: $scope.pedosphereHeight.max,
                            maximumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 }
                        };
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.numericSoilLayers
                            .push(addedAtmN);
                    } else {
                        var num = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .numericSoilLayers[0];
                        if (num) {
                            num.minimumSoilDepth = $scope.pedosphereHeight.min;
                            num.minimumSoilDepthUnit = { value: $scope.pedosphereHeight.uom, uri: '', id: 0 };
                            num.maximumSoilDepth = $scope.pedosphereHeight.max;
                            num.maximumSoilDepthUnit = { value: $scope.pedosphereHeight.uom, uri: '', id: 0 };
                        }
                    }
                };
                var addToPedosphereAcidities = function (name) {
                    var addedAtmL = {
                        soilAcidityType: {
                            value: name,
                            url: ''
                        }
                    };
                    var namedObj = $filter('spherePedosphereAciditiesFilter')(
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilAcidities, name);
                    if (!namedObj)
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilAcidities.push(addedAtmL);
                };
                var addToPedosphereMorphology = function (name) {
                    var addedAtmL = {
                        soilMorphologyType: {
                            value: name,
                            url: ''
                        }
                    };
                    //$scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilMorphologies.push(addedAtmL);
                    $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilMorphologies = [addedAtmL];
                };

                //Remove record from pedosphere model
                var removeFromPedosphereZones = function (name)
                {
                    var namedSoilLayers = $scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.namedSoilLayers;
                    
                    if (namedSoilLayers) {
                        var namedObj = $filter('spherePedosphereNamedLayersFilter')(namedSoilLayers, name);

                        var namedIndex = $scope.sphereContext.spheres[0]
                            .pedosphere.pedosphereCompartments[0].soil.namedSoilLayers.indexOf(namedObj);

                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .namedSoilLayers.splice(namedIndex, 1);

                        //$scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.
                        //    numericSoilLayers.splice(0, 1);

                    }
                    $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.
                        numericSoilLayers = [];

                };
                var removeFromPedosphereAcidities = function (name)
                {
                    var pedosphereAcidities = $scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.soilAcidities;

                    if (pedosphereAcidities) {
                        var namedObj = $filter('spherePedosphereAciditiesFilter')(pedosphereAcidities, name);

                        var namedIndex = $scope.sphereContext.spheres[0]
                            .pedosphere.pedosphereCompartments[0].soil.soilAcidities.indexOf(namedObj);

                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .soilAcidities.splice(namedIndex, 1);
                    }
                };
                var removeFromPedosphereMorphology = function () {
                    //$scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                    //    .soilMorphologies.splice(0, 1);
                    $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                        .soilMorphologies = [];
                };

                $scope.$watch('perdoZoneModel.oHorizon.checked', function () {
                    if ($scope.perdoZoneModel.oHorizon.checked) {
                        addToPedosphereZones('O Horizon');
                    } else {
                        removeFromPedosphereZones('O Horizon');
                    }
                });
                $scope.$watch('perdoZoneModel.aHorizon.checked', function () {
                    if ($scope.perdoZoneModel.aHorizon.checked) {
                        addToPedosphereZones('A Horizon');
                    } else {
                        removeFromPedosphereZones('A Horizon');
                    }
                });
                $scope.$watch('perdoZoneModel.eHorizon.checked', function () {
                    if ($scope.perdoZoneModel.eHorizon.checked) {
                        addToPedosphereZones('E Horizon');
                    } else {
                        removeFromPedosphereZones('E Horizon');
                    }
                });
                $scope.$watch('perdoZoneModel.bHorizon.checked', function () {
                    if ($scope.perdoZoneModel.bHorizon.checked) {
                        addToPedosphereZones('B Horizon');
                    } else {
                        removeFromPedosphereZones('B Horizon');
                    }
                });
                $scope.$watch('perdoZoneModel.cHorizon.checked', function () {
                    if ($scope.perdoZoneModel.cHorizon.checked) {
                        addToPedosphereZones('C Horizon');
                    } else {
                        removeFromPedosphereZones('C Horizon');
                    }
                });
                $scope.$watch('perdoZoneModel.rHorizon.checked', function () {
                    if ($scope.perdoZoneModel.rHorizon.checked) {
                        addToPedosphereZones('R Horizon');
                    } else {
                        removeFromPedosphereZones('R Horizon');
                    }
                });
                $scope.$watch('perdoZoneModel.lHorizon.checked', function () {
                    if ($scope.perdoZoneModel.lHorizon.checked) {
                        addToPedosphereZones('L Horizon');
                    } else {
                        removeFromPedosphereZones('L Horizon');
                    }
                });

                $scope.$watch('pedosphereHeight.min', function () {
                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.numericSoilLayers.length == 0 && $scope.settings.depthEnabled) {
                        foundRecord = {
                            minimumSoilDepth: $scope.pedosphereHeight.min,
                            minimumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 },
                            maximumSoilDepth: $scope.pedosphereHeight.max,
                            maximumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 }
                        };
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.numericSoilLayers.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .numericSoilLayers[0];
                        if (foundRecord) {
                            foundRecord.minimumSoilDepth = $scope.pedosphereHeight.min;
                            foundRecord.minimumSoilDepthUnit = { value: $scope.pedosphereHeight.uom, uri: '', id: 0 };
                            //$scope.settings.depthEnabled = true;
                            $scope.ReloadSliders();
                        }
                    }
                });
                $scope.$watch('pedosphereHeight.max', function () {
                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.numericSoilLayers.length == 0 && $scope.settings.depthEnabled) {
                        foundRecord = {
                            minimumSoilDepth: $scope.pedosphereHeight.min,
                            minimumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 },
                            maximumSoilDepth: $scope.pedosphereHeight.max,
                            maximumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 }
                        };
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.numericSoilLayers.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .numericSoilLayers[0];
                        if (foundRecord) {
                            foundRecord.maximumSoilDepth = $scope.pedosphereHeight.max;
                            foundRecord.maximumSoilDepthUnit = { value: $scope.pedosphereHeight.uom, uri: '', id: 0 };
                        }
                    }
                });
                $scope.$watch('pedosphereHeight.uom', function () {
                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.numericSoilLayers.length == 0 && $scope.settings.depthEnabled) {
                        foundRecord = {
                            minimumSoilDepth: $scope.pedosphereHeight.min,
                            minimumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 },
                            maximumSoilDepth: $scope.pedosphereHeight.max,
                            maximumSoilDepthUnit: { value: $scope.pedosphereHeight.uom, uri: '', id: 0 }
                        };
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.numericSoilLayers.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .numericSoilLayers[0];
                        if (foundRecord) {
                            foundRecord.minimumSoilDepthUnit = { value: $scope.pedosphereHeight.uom, uri: '', id: 0 };
                            foundRecord.maximumSoilDepthUnit = { value: $scope.pedosphereHeight.uom, uri: '', id: 0 };
                        }
                    }

                });

                $scope.$watch('soilQualityModel.acidic.checked', function () {
                    if ($scope.soilQualityModel.acidic.checked) {
                        addToPedosphereAcidities('Acidic');
                    } else {
                        removeFromPedosphereAcidities('Acidic');
                    }
                });
                $scope.$watch('soilQualityModel.neutral.checked', function () {
                    if ($scope.soilQualityModel.neutral.checked) {
                        addToPedosphereAcidities('Neutral');
                    } else {
                        removeFromPedosphereAcidities('Neutral');
                    }
                });
                $scope.$watch('soilQualityModel.alkaline.checked', function () {
                    if ($scope.soilQualityModel.alkaline.checked) {
                        addToPedosphereAcidities('Alkaline');
                    } else {
                        removeFromPedosphereAcidities('Alkaline');
                    }
                });

                $scope.$watch('soilMorphology.selected', function () {
                    removeFromPedosphereMorphology();
                    addToPedosphereMorphology($scope.soilMorphology.selected);
                });

                $scope.$watch('sandySlider.value', function () {
                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.soilTextures.length == 0 && $scope.settings.soilTextureEnabled) {
                        foundRecord = {
                            sandPercent: $scope.sandySlider.value,
                            siltPercent: $scope.siltySlider.value,
                            loamPercent: $scope.loamySlider.value
                        };
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilTextures.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .soilTextures[0];
                        if (foundRecord) {
                            foundRecord.sandPercent = $scope.sandySlider.value;
                            //$scope.settings.soilTextureEnabled = true;
                            $scope.ReloadSliders();
                        }
                    }
                });
                $scope.$watch('siltySlider.value', function () {
                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.soilTextures.length == 0 && $scope.settings.soilTextureEnabled) {
                        foundRecord = {
                            sandPercent: $scope.sandySlider.value,
                            siltPercent: $scope.siltySlider.value,
                            loamPercent: $scope.loamySlider.value
                        };
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilTextures.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .soilTextures[0];
                        if (foundRecord) {
                            foundRecord.siltPercent = $scope.siltySlider.value;
                        }
                    }
                });
                $scope.$watch('loamySlider.value', function () {
                    var foundRecord = {};
                    if ($scope.sphereContext.spheres[0]
                        .pedosphere.pedosphereCompartments[0].soil.soilTextures.length == 0 && $scope.settings.soilTextureEnabled) {
                        foundRecord = {
                            sandPercent: $scope.sandySlider.value,
                            siltPercent: $scope.siltySlider.value,
                            loamPercent: $scope.loamySlider.value
                        };
                        $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil.soilTextures.push(foundRecord);
                    } else {
                        foundRecord = $scope.sphereContext.spheres[0].pedosphere.pedosphereCompartments[0].soil
                            .soilTextures[0];
                        if (foundRecord) {
                            foundRecord.loamPercent = $scope.loamySlider.value;
                        }
                    }
                });


                /////////////////////////////////////////////////
                ////////////////// Hydrosphere //////////////////
                /////////////////////////////////////////////////

                var addToHydroRiverLong = function (name) {
                    var addedAtmL = {
                        longitudinalRiverZone: {
                            value: name,
                            url: ''
                        },
                        verticalRiverZone: {
                            value: '',
                            url: ''
                        }
                    };
                    var found =
                        $filter('sphereUniversalHydrosphereFilter')($scope.sphereContext.spheres[0].hydrosphere
                            .hydrosphereCompartments[0].river.namedRiverZones,
                            { property: 'longitudinalRiverZone', value: name });
                    if (found) {
                        found.longitudinalRiverZone.value = name;
                    } else {
                        $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones.push(addedAtmL);
                    }
                };
                var removeFromHydroRiverLong = function (name) {
                    var namedObj = $filter('sphereUniversalHydrosphereFilter')
                        ($scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones, { property: 'longitudinalRiverZone', value: name });

                    var namedIndex = $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones.splice(namedIndex, 1);
                };

                $scope.$watch('hydrosphereModel.river.crenon.checked', function () {
                    if ($scope.hydrosphereModel.river.crenon.checked) {
                        addToHydroRiverLong('Crenon');
                    } else {
                        removeFromHydroRiverLong('Crenon');
                    }
                });
                $scope.$watch('hydrosphereModel.river.rhitron.checked', function () {
                    if ($scope.hydrosphereModel.river.rhitron.checked) {
                        addToHydroRiverLong('Rhitron');
                    } else {
                        removeFromHydroRiverLong('Rhitron');
                    }
                });
                $scope.$watch('hydrosphereModel.river.potamon.checked', function () {
                    if ($scope.hydrosphereModel.river.potamon.checked) {
                        addToHydroRiverLong('Potamon');
                    } else {
                        removeFromHydroRiverLong('Potamon');
                    }
                });

                var addToHydroRiverVert = function (name) {
                    var addedAtmL = {
                        longitudinalRiverZone: {
                            value: '',
                            url: ''
                        },
                        verticalRiverZone: {
                            value: name,
                            url: ''
                        }
                    };
                    var found =
                        $filter('sphereUniversalHydrosphereFilter')($scope.sphereContext.spheres[0].hydrosphere
                            .hydrosphereCompartments[0].river.namedRiverZones,
                            { property: 'verticalRiverZone', value: name });
                    if (found) {
                        found.verticalRiverZone.value = name;
                    } else {
                        $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones.push(addedAtmL);
                    }
                };
                var removeFromHydroRiverVert = function (name) {
                    var namedObj = $filter('sphereUniversalHydrosphereFilter')
                        ($scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones, { property: 'verticalRiverZone', value: name });

                    var namedIndex = $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].river.namedRiverZones.splice(namedIndex, 1);
                };

                $scope.$watch('hydrosphereModel.river.floodplain.checked', function () {
                    if ($scope.hydrosphereModel.river.floodplain.checked) {
                        addToHydroRiverVert('Floodplain');
                    } else {
                        removeFromHydroRiverVert('Floodplain');
                    }
                });
                $scope.$watch('hydrosphereModel.river.riverBank.checked', function () {
                    if ($scope.hydrosphereModel.river.riverBank.checked) {
                        addToHydroRiverVert('River Bank');
                    } else {
                        removeFromHydroRiverVert('River Bank');
                    }
                });
                $scope.$watch('hydrosphereModel.river.riverWater.checked', function () {
                    if ($scope.hydrosphereModel.river.riverWater.checked) {
                        addToHydroRiverVert('River Water Column');
                    } else {
                        removeFromHydroRiverVert('River Water Column');
                    }
                });
                $scope.$watch('hydrosphereModel.river.riverBed.checked', function () {
                    if ($scope.hydrosphereModel.river.riverBed.checked) {
                        addToHydroRiverVert('River Bed');
                    } else {
                        removeFromHydroRiverVert('River Bed');
                    }
                });

                var addToHydroLakeBenthic = function (name) {
                    var addedAtmL = {
                        benthicLakeZone: {
                            value: name,
                            url: ''
                        },
                        pelagicLakeZone: {
                            value: '',
                            url: ''
                        }
                    };
                    var found =
                        $filter('sphereUniversalHydrosphereFilter')($scope.sphereContext.spheres[0].hydrosphere
                            .hydrosphereCompartments[0].lake.namedLakeZones,
                            { property: 'benthicLakeZone', value: name });
                    if (found) {
                        found.benthicLakeZone.value = name;
                    } else {
                        $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones.push(addedAtmL);
                    }
                };
                var removeFromHydroLakeBenthic = function (name) {
                    var namedObj = $filter('sphereUniversalHydrosphereFilter')
                        ($scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones, { property: 'benthicLakeZone', value: name });

                    var namedIndex = $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones.splice(namedIndex, 1);
                };

                $scope.$watch('hydrosphereModel.lake.litoral.checked', function () {
                    if ($scope.hydrosphereModel.lake.litoral.checked) {
                        addToHydroLakeBenthic('Litoral');
                    } else {
                        removeFromHydroLakeBenthic('Litoral');
                    }
                });
                $scope.$watch('hydrosphereModel.lake.benthic.checked', function () {
                    if ($scope.hydrosphereModel.lake.benthic.checked) {
                        addToHydroLakeBenthic('Benthic');
                    } else {
                        removeFromHydroLakeBenthic('Benthic');
                    }
                });
                $scope.$watch('hydrosphereModel.lake.profundal.checked', function () {
                    if ($scope.hydrosphereModel.lake.profundal.checked) {
                        addToHydroLakeBenthic('Profundal');
                    } else {
                        removeFromHydroLakeBenthic('Profundal');
                    }
                });

                var addToHydroLakePelagic = function (name) {
                    var addedAtmL = {
                        benthicLakeZone: {
                            value: '',
                            url: ''
                        },
                        pelagicLakeZone: {
                            value: name,
                            url: ''
                        }
                    };
                    var found =
                        $filter('sphereUniversalHydrosphereFilter')($scope.sphereContext.spheres[0].hydrosphere
                            .hydrosphereCompartments[0].lake.namedLakeZones,
                            { property: 'pelagicLakeZone', value: name });
                    if (found) {
                        found.pelagicLakeZone.value = name;
                    } else {
                        $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones.push(addedAtmL);
                    }
                };
                var removeFromHydroLakePelagic = function (name) {
                    var namedObj = $filter('sphereUniversalHydrosphereFilter')
                        ($scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones, { property: 'pelagicLakeZone', value: name });

                    var namedIndex = $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].lake.namedLakeZones.splice(namedIndex, 1);
                };

                $scope.$watch('hydrosphereModel.lake.pelagic.checked', function () {
                    if ($scope.hydrosphereModel.lake.pelagic.checked) {
                        addToHydroLakePelagic('Pelagic');
                    } else {
                        removeFromHydroLakePelagic('Pelagic');
                    }
                });
                $scope.$watch('hydrosphereModel.lake.empilimnion.checked', function () {
                    if ($scope.hydrosphereModel.lake.empilimnion.checked) {
                        addToHydroLakePelagic('Empilimnion');
                    } else {
                        removeFromHydroLakePelagic('Empilimnion');
                    }
                });
                $scope.$watch('hydrosphereModel.lake.metalimnion.checked', function () {
                    if ($scope.hydrosphereModel.lake.metalimnion.checked) {
                        addToHydroLakePelagic('Metalimnion');
                    } else {
                        removeFromHydroLakePelagic('Metalimnion');
                    }
                });
                $scope.$watch('hydrosphereModel.lake.hypolimnion.checked', function () {
                    if ($scope.hydrosphereModel.lake.hypolimnion.checked) {
                        addToHydroLakePelagic('Hypolimnion');
                    } else {
                        removeFromHydroLakePelagic('Hypolimnion');
                    }
                });

                var addToHydroSeaBenthic = function (name) {
                    var addedAtmL = {
                        benthicSeaZone: {
                            value: name,
                            url: ''
                        },
                        pelagicSeaZone: {
                            value: '',
                            url: ''
                        }
                    };
                    var found =
                        $filter('sphereUniversalHydrosphereFilter')($scope.sphereContext.spheres[0].hydrosphere
                            .hydrosphereCompartments[0].sea.namedSeaZones,
                            { property: 'benthicSeaZone', value: name });
                    if (found) {
                        found.benthicSeaZone.value = name;
                    } else {
                        $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones.push(addedAtmL);
                    }
                };
                var removeFromHydroSeaBenthic = function (name) {
                    var namedObj = $filter('sphereUniversalHydrosphereFilter')
                        ($scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones, { property: 'benthicSeaZone', value: name });

                    var namedIndex = $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones.splice(namedIndex, 1);
                };

                $scope.$watch('hydrosphereModel.sea.benthic.checked', function () {
                    if ($scope.hydrosphereModel.sea.benthic.checked) {
                        addToHydroSeaBenthic('Benthic');
                    } else {
                        removeFromHydroSeaBenthic('Benthic');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.litoral.checked', function () {
                    if ($scope.hydrosphereModel.sea.litoral.checked) {
                        addToHydroSeaBenthic('Litoral');
                    } else {
                        removeFromHydroSeaBenthic('Litoral');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.neritic.checked', function () {
                    if ($scope.hydrosphereModel.sea.neritic.checked) {
                        addToHydroSeaBenthic('Neritic');
                    } else {
                        removeFromHydroSeaBenthic('Neritic');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.bathyal.checked', function () {
                    if ($scope.hydrosphereModel.sea.bathyal.checked) {
                        addToHydroSeaBenthic('Bathyal');
                    } else {
                        removeFromHydroSeaBenthic('Bathyal');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.abyssal.checked', function () {
                    if ($scope.hydrosphereModel.sea.abyssal.checked) {
                        addToHydroSeaBenthic('Abyssal');
                    } else {
                        removeFromHydroSeaBenthic('Abyssal');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.hadal.checked', function () {
                    if ($scope.hydrosphereModel.sea.hadal.checked) {
                        addToHydroSeaBenthic('Hadal');
                    } else {
                        removeFromHydroSeaBenthic('Hadal');
                    }
                });

                var addToHydroSeaPelagic = function (name) {
                    var addedAtmL = {
                        benthicSeaZone: {
                            value: '',
                            url: ''
                        },
                        pelagicSeaZone: {
                            value: name,
                            url: ''
                        }
                    };
                    var found =
                        $filter('sphereUniversalHydrosphereFilter')($scope.sphereContext.spheres[0].hydrosphere
                            .hydrosphereCompartments[0].sea.namedSeaZones,
                            { property: 'pelagicSeaZone', value: name });
                    if (found) {
                        found.pelagicSeaZone.value = name;
                    } else {
                        $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones.push(addedAtmL);
                    }
                };
                var removeFromHydroSeaPelagic = function (name) {
                    var namedObj = $filter('sphereUniversalHydrosphereFilter')
                        ($scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones, { property: 'pelagicSeaZone', value: name });

                    var namedIndex = $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones.indexOf(namedObj);

                    $scope.sphereContext.spheres[0].hydrosphere.hydrosphereCompartments[0].sea.namedSeaZones.splice(namedIndex, 1);
                };

                $scope.$watch('hydrosphereModel.sea.pelagic.checked', function () {
                    if ($scope.hydrosphereModel.sea.pelagic.checked) {
                        addToHydroSeaPelagic('Pelagic');
                    } else {
                        removeFromHydroSeaPelagic('PelagicPelagic');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.epipelagic.checked', function () {
                    if ($scope.hydrosphereModel.sea.epipelagic.checked) {
                        addToHydroSeaPelagic('Epipelagic');
                    } else {
                        removeFromHydroSeaPelagic('Epipelagic');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.mesopelagic.checked', function () {
                    if ($scope.hydrosphereModel.sea.mesopelagic.checked) {
                        addToHydroSeaPelagic('Mesopilagic');
                    } else {
                        removeFromHydroSeaPelagic('Mesopilagic');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.bathypelagic.checked', function () {
                    if ($scope.hydrosphereModel.sea.bathypelagic.checked) {
                        addToHydroSeaPelagic('Bathypelagic');
                    } else {
                        removeFromHydroSeaPelagic('Bathypelagic');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.abyssopelagic.checked', function () {
                    if ($scope.hydrosphereModel.sea.abyssopelagic.checked) {
                        addToHydroSeaPelagic('Abyssopelagic');
                    } else {
                        removeFromHydroSeaPelagic('Abyssopelagic');
                    }
                });
                $scope.$watch('hydrosphereModel.sea.hadopelagic.checked', function () {
                    if ($scope.hydrosphereModel.sea.hadopelagic.checked) {
                        addToHydroSeaPelagic('Hadopelagic');
                    } else {
                        removeFromHydroSeaPelagic('Hadopelagic');
                    }
                });


                fillExistData();
                refreshSlider();
            }]);
})();

