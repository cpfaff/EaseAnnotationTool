using System;
using CAFE.Core.Integration;
using CAFE.Services.Integration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CAFE.Tests.UnitTests.Integration
{
    [TestClass]
    public class AnnotationItemIntegrationServiceTest
    {
        [TestMethod]
        public void ImportWithoutTransformationMethod()
        {
            //Arrange
            IAnnotationItemIntegrationService service = new XmlAnnotationItemIntegrationService();

            //Act
            var result = service.ImportWithTransform(_correctXmlContent);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void ImportWithoutTransformationWithFailMethod()
        {
            //Arrange
            IAnnotationItemIntegrationService service = new XmlAnnotationItemIntegrationService();

            //Act
            var result = service.ImportWithTransform(_incorrectXmlContent);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ImportWithTransformationMethod()
        {
            //Arrange
            IAnnotationItemIntegrationService service = new XmlAnnotationItemIntegrationService();

            //Act
            var result = service.ImportWithTransform(_correctXmlContent, _correctXmlStylesheet);

            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod, ExpectedException(typeof(InvalidOperationException))]
        public void ImportWithTransformationWithFailMethod()
        {
            //Arrange
            IAnnotationItemIntegrationService service = new XmlAnnotationItemIntegrationService();

            //Act
            var result = service.ImportWithTransform(_incorrectXmlContent, _incorrectXmlStylesheet);

            //Assert
            Assert.IsNotNull(result);
        }


        private static string _correctXmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Cafe><Object><Contexts><Context><TimeContext><TimeRanges><TimeRange><RangeStart><DateTime><date>2012-12-13</date><time>12:12:12</time><timezone>Africa/Abidjan</timezone></DateTime></RangeStart><RangeEnd><DateTime><date>2012-12-13</date><time>12:12:12</time><timezone>Africa/Abidjan</timezone></DateTime></RangeEnd></TimeRange></TimeRanges><TimePeriods><TimePeriod><GeologicalTimePeriod><geologicalEon>Phanerozoic</geologicalEon><geologicalEra>Cenozoic</geologicalEra><geologicalPeriod>Quaternary</geologicalPeriod><geologicalEpoch>Holocene</geologicalEpoch><geologicalAge>Calabrian</geologicalAge></GeologicalTimePeriod></TimePeriod></TimePeriods><TemporalResolutions><TemporalResolution><temporalExtentType>Second</temporalExtentType><temporalResolutionType>Second</temporalResolutionType></TemporalResolution></TemporalResolutions></TimeContext><SpaceContext><BoundingBoxes><BoundingBox><northBoundingCoordinate>80</northBoundingCoordinate><southBoundingCoordinate>80</southBoundingCoordinate><eastBoundingCoordinate>80</eastBoundingCoordinate><westBoundingCoordinate>80</westBoundingCoordinate></BoundingBox></BoundingBoxes><Elevations><Elevation><maximumElevation>123.45</maximumElevation><maximumElevationUnit>Centimetre</maximumElevationUnit><minimumElevation>123.45</minimumElevation><minimumElevationUnit>Centimetre</minimumElevationUnit><elevationDatum>Mean Sea Level</elevationDatum></Elevation></Elevations><Coordinates><Coordinate><UtmCoordinate><utmCoordinateZone>1234</utmCoordinateZone><utmCoordinateSubZone>1234</utmCoordinateSubZone><utmCoordinateHemisphere>N</utmCoordinateHemisphere><utmCoordinateEasting>123.45</utmCoordinateEasting><utmCoordinateEastingUnit>Metre</utmCoordinateEastingUnit><utmCoordinateNorthing>123.45</utmCoordinateNorthing><utmCoordinateNorthingUnit>Metre</utmCoordinateNorthingUnit><utmCoordinateGeodeticDatum>wgs84</utmCoordinateGeodeticDatum></UtmCoordinate></Coordinate></Coordinates><Locations><Location><locationName>str1234</locationName><locationType>City</locationType><countryName>Andorra</countryName><continentName>Africa</continentName></Location></Locations><SpatialResolutions><SpatialResolution><spatialExtentType>Point</spatialExtentType><spatialResolutionType>Point</spatialResolutionType></SpatialResolution></SpatialResolutions></SpaceContext><SphereContext><Spheres><Sphere><Atmosphere><NamedAtmosphereLayers><NamedAtmosphereLayer><atmosphereLayerName>Troposphere</atmosphereLayerName></NamedAtmosphereLayer></NamedAtmosphereLayers><NumericAtmosphereLayers><NumericAtmosphereLayer><minimumAtmosphereHeight>123.45</minimumAtmosphereHeight><minimumAtmosphereHeightUnit>Metre</minimumAtmosphereHeightUnit><maximumAtmosphereHeight>123.45</maximumAtmosphereHeight><maximumAtmosphereHeightUnit>Metre</maximumAtmosphereHeightUnit></NumericAtmosphereLayer></NumericAtmosphereLayers></Atmosphere><Hydrosphere><HydrosphereCompartments><HydrosphereCompartment><River><NamedRiverZones><NamedRiverZone><longitudinalRiverZone>Crenon</longitudinalRiverZone><verticalRiverZone>Floodplain</verticalRiverZone></NamedRiverZone></NamedRiverZones></River><Lake><NamedLakeZones><NamedLakeZone><benthicLakeZone>Litoral</benthicLakeZone><pelagicLakeZone>Epilimnion</pelagicLakeZone></NamedLakeZone></NamedLakeZones></Lake><Sea><NamedSeaZones><NamedSeaZone><benthicSeaZone>Litoral</benthicSeaZone><pelagicSeaZone>Epipelagic</pelagicSeaZone></NamedSeaZone></NamedSeaZones></Sea></HydrosphereCompartment></HydrosphereCompartments></Hydrosphere><Pedosphere><PedosphereCompartments><PedosphereCompartment><Soil><NamedSoilLayers><NamedSoilLayer><soilHorizon>O Horizon</soilHorizon></NamedSoilLayer></NamedSoilLayers><NumericSoilLayers><NumericSoilLayer><minimumSoilDepth>123.45</minimumSoilDepth><minimumSoilDepthUnit>Millimetre</minimumSoilDepthUnit><maximumSoilDepth>123.45</maximumSoilDepth><maximumSoilDepthUnit>Millimetre</maximumSoilDepthUnit></NumericSoilLayer></NumericSoilLayers><SoilTextures><SoilTexture><sandPercent>12</sandPercent><siltPercent>12</siltPercent><loamPercent>12</loamPercent></SoilTexture></SoilTextures><SoilMorphologies><SoilMorphology><soilMorphologyType>Histosols</soilMorphologyType></SoilMorphology></SoilMorphologies><SoilAcidities><SoilAcidity><soilAcidityType>Acidic</soilAcidityType></SoilAcidity></SoilAcidities></Soil></PedosphereCompartment></PedosphereCompartments></Pedosphere><Ecosphere><NamedEcosphereLayers><NamedEcosphereLayer><ecosphereLayerName>Tree Layer</ecosphereLayerName></NamedEcosphereLayer></NamedEcosphereLayers><NumericEcosphereLayers><NumericEcosphereLayer><minimumVegetationHeight>123.45</minimumVegetationHeight><minimumVegetationHeightUnit>Millimetre</minimumVegetationHeightUnit><maximumVegetationHeight>123.45</maximumVegetationHeight><maximumVegetationHeightUnit>Millimetre</maximumVegetationHeightUnit></NumericEcosphereLayer></NumericEcosphereLayers><OrganizationalHierarchies><OrganizationalHierarchy><organizationHierarchyName>Biome Level</organizationHierarchyName></OrganizationalHierarchy></OrganizationalHierarchies></Ecosphere></Sphere></Spheres></SphereContext><BiomeContext><ZonoBiomes><ZonoBiome><biomeType>Terrestrial</biomeType><biomeZone>Polar</biomeZone><biomeHumidityType>Humid</biomeHumidityType><biomeContinentalityType>Continental</biomeContinentalityType><biomeHemisphere>North</biomeHemisphere></ZonoBiome></ZonoBiomes><OroBiomes><OroBiome><oroBiomeType>Nivale</oroBiomeType></OroBiome></OroBiomes><PedoBiomes><PedoBiome><pedoBiomeType>Amphibiome</pedoBiomeType></PedoBiome></PedoBiomes><Physiognomies><Physiognomy><PhysiognomyTypes><PhysiognomyType><TerrestrialPhysiognomies><TerrestrialPhysiognomy><terrestrialPhysiognomyType>Forest</terrestrialPhysiognomyType></TerrestrialPhysiognomy></TerrestrialPhysiognomies><SemiAquaticPhysiognomies><SemiAquaticPhysiognomy><semiAquaticPhysiognomyType>Mire</semiAquaticPhysiognomyType></SemiAquaticPhysiognomy></SemiAquaticPhysiognomies><AquaticPhysiognomies><AquaticPhysiognomy><plantCharacterizedAquaticPhysiognomyType>Emergent Aquatic</plantCharacterizedAquaticPhysiognomyType><habitatCharacterizedAquaticPhysiognomy>Estuary</habitatCharacterizedAquaticPhysiognomy></AquaticPhysiognomy></AquaticPhysiognomies></PhysiognomyType></PhysiognomyTypes></Physiognomy></Physiognomies><LandUses><LandUse><landUseType>Natural</landUseType><landUseForm>Agriculture</landUseForm></LandUse></LandUses></BiomeContext><OrganismContext><Organisms><Organism><Taxonomy><domain>str1234</domain><kingdom>Plantae</kingdom><division>str1234</division><order>str1234</order><family>str1234</family><OrganismName><BacterialName><fullBacterialName>str1234</fullBacterialName><genusOrMonomial>str1234</genusOrMonomial><subgenus>str1234</subgenus><subgenusAuthorAndYear>str1234</subgenusAuthorAndYear><speciesEpithet>str1234</speciesEpithet><subspeciesEpithet>str1234</subspeciesEpithet><parentheticalAuthorTeamAndYear>str1234</parentheticalAuthorTeamAndYear><authorTeamAndYear>str1234</authorTeamAndYear><nameApprobation>str1234</nameApprobation></BacterialName><FungalName><fullFungalName>str1234</fullFungalName></FungalName><BotanicalName><fullBotanicalName>str1234</fullBotanicalName><genusOrMonomial>str1234</genusOrMonomial><firstEpithet>str1234</firstEpithet><infraspecificEpithet>str1234</infraspecificEpithet><rank>subgen.</rank><hybridFlag insertionpoint=\"1\">1</hybridFlag><authorTeamParenthesis>str1234</authorTeamParenthesis><authorTeam>str1234</authorTeam><cultivarGroupName>str1234</cultivarGroupName><cultivarName>str1234</cultivarName><tradeDesignationNames><tradeDesignationName>str1234</tradeDesignationName></tradeDesignationNames></BotanicalName><ZoologicalName><fullZoologicalName>str1234</fullZoologicalName><genusOrMonomial>str1234</genusOrMonomial><subgenus>str1234</subgenus><speciesEpithet>str1234</speciesEpithet><subspeciesEpithet>str1234</subspeciesEpithet><authorTeamOriginalAndYear>str1234</authorTeamOriginalAndYear><authorTeamParenthesisAndYear>str1234</authorTeamParenthesisAndYear><combinationAuthorTeamAndYear>str1234</combinationAuthorTeamAndYear><breed>str1234</breed><namedIndividual>str1234</namedIndividual></ZoologicalName><ViralName><fullViralName>str1234</fullViralName><genusOrMonomial>str1234</genusOrMonomial><viralSpeciesDesignation>str1234</viralSpeciesDesignation><acronym>str1234</acronym></ViralName></OrganismName></Taxonomy></Organism></Organisms></OrganismContext><ProcessContext><Processes><Process><processName>Adaptation</processName><processSubject>Organism</processSubject><processType>Motion</processType></Process></Processes><Interactions><Interaction><interactionName>str1234</interactionName><interactionPartnerOne>Plantae</interactionPartnerOne><interactionDirection>Affects</interactionDirection><interactionQuality>Neutralism</interactionQuality><interactionPartnerTwo>Plantae</interactionPartnerTwo></Interaction></Interactions></ProcessContext><ChemicalContext><Elements><Element><elementName>Hydrogen</elementName></Element></Elements><Isotopes><Isotope><isotopeName>11B</isotopeName></Isotope></Isotopes><Compounds><Compound><compoundName>str1234</compoundName><compoundClass>Biomolecule</compoundClass><compoundType>Nucleosides</compoundType></Compound></Compounds><Functions><Function><chemicalFunctionType>Repellent</chemicalFunctionType></Function></Functions></ChemicalContext><MethodContext><Approaches><Approach><ApproachType>Virtual</ApproachType><approachLocalization>Microcosm</approachLocalization></Approach></Approaches><Factors><Factor><factorName>Producer Diversity</factorName><factorType>Biotic</factorType></Factor></Factors><DataFormats><DataFormat><dataFormatType>Audio</dataFormatType></DataFormat></DataFormats><DataSources><DataSource><dataSourceType>Specimen</dataSourceType></DataSource></DataSources></MethodContext></Context></Contexts><References><Hosters><Hoster><hosterName>PANGEAE</hosterName></Hoster></Hosters><Persons><Person><position>Author</position><salutation>str1234</salutation><givenName>str1234</givenName><surName>str1234</surName><emailAddress>test@mail.de</emailAddress><phoneNumber>str1234</phoneNumber></Person></Persons><Descriptions><title>str1234</title><abstract>str1234</abstract></Descriptions></References><Resources><Resource><OnlineResources><OnlineResource><Uris><Uri><downloadUrl>http://www.xsd2xml.com</downloadUrl></Uri></Uris></OnlineResource></OnlineResources><EmbeddedResources><EmbeddedResource><Base64AudioResource><fileName>str1234</fileName><audioMimeType>mp3</audioMimeType><audioData>AAAAZg==</audioData><encodingDate>2012-12-13</encodingDate><encodingTime>12:12:12</encodingTime><sha2hash>str1234</sha2hash></Base64AudioResource></EmbeddedResource></EmbeddedResources></Resource></Resources></Object></Cafe>";
        private static string _incorrectXmlContent = "<?xml version=\"1.0\" encoding=\"utf-8\"?><Cafe></Cafe>";
        private static string _correctXmlStylesheet = "<?xml version=\"1.0\" encoding=\"utf-8\"?><xsl:stylesheet version=\"1.0\" xmlns:xsl=\"http://www.w3.org/1999/XSL/Transform\" xmlns:msxsl=\"urn:schemas-microsoft-com:xslt\" exclude-result-prefixes=\"msxsl\"><xsl:output method=\"xml\" indent=\"yes\"/><xsl:template match=\"/\"><Cafe><Object><xsl:apply-templates select=\"/Things/Thing[1]\" mode=\"first\"/></Object></Cafe></xsl:template><xsl:template match=\"Properties|Behaviors|Inheritances\"><Contexts><blablaBla></blablaBla></Contexts></xsl:template><xsl:template match=\"Resources\"><Resources><blablaBla></blablaBla></Resources></xsl:template></xsl:stylesheet>";
        private static string _incorrectXmlStylesheet = "<?xml version=\"1.0\" encoding=\"utf-8\"?><xsl:stylesheet";
    }
}
