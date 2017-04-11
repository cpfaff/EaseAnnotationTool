
using System;
using System.Data.Entity;
using System.Linq;
using AutoMapper;
using CAFE.Core;
using CAFE.Core.Integration;
using CAFE.Core.Messaging;
using CAFE.Core.Resources;
using CAFE.Core.Searching;
using CAFE.Core.Security;
using CAFE.DAL.Models;
using CAFE.DAL.Models.Resources;

namespace CAFE.Services.Mapping
{
    public class MapperConfig
    {
        public static void Init(IMapperConfigurationExpression c)
        {
            c.CreateMap<string, Guid>().ConvertUsing(s =>
            {
                var guid = Guid.Empty;
                Guid.TryParse(s, out guid);
                return guid;
            });
            c.CreateMap<Guid, string>().ConvertUsing(g => g.ToString());

            c.CreateMap<AnnotationItemAccessibleGroups, DbAnnotationItemAccessibleGroups>(MemberList.Destination).MaxDepth(2).PreserveReferences();
            c.CreateMap<DbAnnotationItemAccessibleGroups, AnnotationItemAccessibleGroups>(MemberList.Source).MaxDepth(2).PreserveReferences();

            c.CreateMap<AnnotationItemAccessibleUsers, DbAnnotationItemAccessibleUsers>(MemberList.Destination).MaxDepth(2).PreserveReferences();
            c.CreateMap<DbAnnotationItemAccessibleUsers, AnnotationItemAccessibleUsers>(MemberList.Source).MaxDepth(2).PreserveReferences();

            c.CreateMap<User, DbUser>(MemberList.Destination).MaxDepth(2).PreserveReferences();
            c.CreateMap<DbUser, User>(MemberList.Source).MaxDepth(2).PreserveReferences()
                .ForMember(m => m.Role, opt => opt.MapFrom(s => s.Roles.FirstOrDefault(g => !g.IsGroup.Value).Name ?? ""))
                .ForMember(m => m.Groups, opts => opts.MapFrom(s => s.Roles))
                .ForMember(m => m.OwnedFiles, opts => opts.MapFrom(s=>s.OwnedFiles))
                .ForMember(m => m.AccessibleFiles, opts => opts.MapFrom(s => s.AccessibleFiles))
                /*.ForMember(m => m.AccessibleAnnotationItems, opts => opts.MapFrom(s => s.AccessibleAnnotationItems))*/;

            //.ForMember(m => m.OwnedFiles, opts => opts.Ignore()).
            //.ForMember(m => m.AccessibleFiles, opts => opts.Ignore());


            c.CreateMap<Group, DbRole>(MemberList.Destination).MaxDepth(2).PreserveReferences();
            c.CreateMap<DbRole, Group>(MemberList.Source).MaxDepth(2).PreserveReferences()
                .ForMember(m => m.Users, opts => opts.Ignore())
                .ForMember(m => m.AccessibleAnnotationItems, opts => opts.Ignore()
                );

            c.CreateMap<UserFile, DbUserFile>(MemberList.Destination).MaxDepth(2).PreserveReferences();
            c.CreateMap<DbUserFile, UserFile>(MemberList.Source).MaxDepth(2).PreserveReferences()
                .ForMember(m => m.AcceptedGroups, opts => opts.MapFrom(s => s.AcceptedGroups))
                .ForMember(m => m.AcceptedUsers, opts => opts.MapFrom(s => s.AcceptedUsers))
                .ForMember(m => m.OwnerId, opts => opts.MapFrom(f => f.Owner.Id))
                .ForMember(m => m.OwnerName, opts => opts.MapFrom(f => f.Owner.Name));

            c.CreateMap<string, FilterType>().ConvertUsing(s =>
            {
                FilterType en;
                Enum.TryParse(s, true, out en);
                return en;
            });
            c.CreateMap<FilterType, string>().ConvertUsing(g => g.ToString());
            c.CreateMap<string, SearchResultItemType>().ConvertUsing(s =>
            {
                SearchResultItemType en;
                Enum.TryParse(s, true, out en);
                return en;
            });
            c.CreateMap<SearchResultItemType, string>().ConvertUsing(g => g.ToString());

            c.CreateMap<AccessibleResourceKind, DbAccessibleResourceKind>().ConvertUsing(g =>
            {
                if(g == AccessibleResourceKind.File) return DbAccessibleResourceKind.File;
                return DbAccessibleResourceKind.AnnotationItem;
            });
            c.CreateMap<DbAccessibleResourceKind, AccessibleResourceKind>().ConvertUsing(g =>
            {
                if (g == DbAccessibleResourceKind.File) return AccessibleResourceKind.File;
                return AccessibleResourceKind.AnnotationItem;
            });

            c.CreateMap<DbAccessRequest, AccessRequest>()
                .ForMember(m => m.RequestedResources, opts => opts.Ignore()).MaxDepth(2).PreserveReferences();
            c.CreateMap<AccessRequest, DbAccessRequest>()
                .ForMember(m => m.RequestedResources, opts => opts.Ignore()).MaxDepth(2).PreserveReferences();

            c.CreateMap<DbAccessibleResource, AccessibleResource>();

            c.CreateMap<DbVocabularyValue, VocabularyValue>();
            c.CreateMap<VocabularyValue, DbVocabularyValue>();
            c.CreateMap<DbVocabularyUserValue, VocabularyUserValue>();
            c.CreateMap<VocabularyUserValue, DbVocabularyUserValue>();

            c.CreateMap<VocabularyUserValue, VocabularyValue>(MemberList.Destination);

            c.CreateMap<DbMessage, Message>();
            c.CreateMap<DbConversation, Conversation>().MaxDepth(2).PreserveReferences();

            c.CreateMap<DbVocabularyUserValue, VocabularyUserValue>().MaxDepth(2).PreserveReferences().
                ForMember(m => m.Owner, opts => opts.MapFrom(s => s.User.Name + " " + s.User.Surname + " (" + s.User.UserName + ")"));

            c.CreateMap<DbSearchFilterCachedItem, SearchRequestFilterItem>();
            c.CreateMap<SearchRequestFilterItem, DbSearchFilterCachedItem>();

            c.CreateMap<AnnotationItem, DbAnnotationItem>();
            c.CreateMap<DbAnnotationItem, AnnotationItem>();
            c.CreateMap<AnnotationObject, DbAnnotationObject>();
            c.CreateMap<DbAnnotationObject, AnnotationObject>();
            c.CreateMap<Context, DbAnnotationContext>();
            c.CreateMap<DbAnnotationContext, Context>();
            c.CreateMap<TimeContext, DbTimeContext>();
            c.CreateMap<DbTimeContext, TimeContext>();
            c.CreateMap<TimeRanges, DbTimeRanges>();
            c.CreateMap<DbTimeRanges, TimeRanges>();
            c.CreateMap<TimeRange, DbTimeRange>();
            c.CreateMap<DbTimeRange, TimeRange>();
            c.CreateMap<RangeStart, DbRangeStart>();
            c.CreateMap<DbRangeStart, RangeStart>();
            c.CreateMap<DateAndTime, DbDateAndTime>();
            c.CreateMap<DbDateAndTime, DateAndTime>();
            c.CreateMap<Timezone, DbTimezone>();
            c.CreateMap<DbTimezone, Timezone>();
            c.CreateMap<RangeEnd, DbRangeEnd>();
            c.CreateMap<DbRangeEnd, RangeEnd>();
            c.CreateMap<DateAndTime, DbDateAndTime>();
            c.CreateMap<DbDateAndTime, DateAndTime>();
            c.CreateMap<Timezone, DbTimezone>();
            c.CreateMap<DbTimezone, Timezone>();
            c.CreateMap<TimePeriods, DbTimePeriods>();
            c.CreateMap<DbTimePeriods, TimePeriods>();
            c.CreateMap<GeologicalTimePeriod, DbGeologicalTimePeriod>();
            c.CreateMap<DbGeologicalTimePeriod, GeologicalTimePeriod>();
            c.CreateMap<GeologicalEon, DbGeologicalEon>();
            c.CreateMap<DbGeologicalEon, GeologicalEon>();
            c.CreateMap<GeologicalEra, DbGeologicalEra>();
            c.CreateMap<DbGeologicalEra, GeologicalEra>();
            c.CreateMap<GeologicalPeriod, DbGeologicalPeriod>();
            c.CreateMap<DbGeologicalPeriod, GeologicalPeriod>();
            c.CreateMap<GeologicalEpoch, DbGeologicalEpoch>();
            c.CreateMap<DbGeologicalEpoch, GeologicalEpoch>();
            c.CreateMap<GeologicalAge, DbGeologicalAge>();
            c.CreateMap<DbGeologicalAge, GeologicalAge>();
            c.CreateMap<TemporalResolution, DbTemporalResolution>();
            c.CreateMap<DbTemporalResolution, TemporalResolution>();
            c.CreateMap<TemporalResolution, DbTemporalResolution>();
            c.CreateMap<DbTemporalResolution, TemporalResolution>();
            c.CreateMap<TemporalExtentType, DbTemporalExtentType>();
            c.CreateMap<DbTemporalExtentType, TemporalExtentType>();
            c.CreateMap<TemporalResolutionType, DbTemporalResolutionType>();
            c.CreateMap<DbTemporalResolutionType, TemporalResolutionType>();
            c.CreateMap<SpaceContext, DbSpaceContext>();
            c.CreateMap<DbSpaceContext, SpaceContext>();
            c.CreateMap<BoundingBox, DbBoundingBox>();
            c.CreateMap<DbBoundingBox, BoundingBox>();
            c.CreateMap<BoundingBox, DbBoundingBox>();
            c.CreateMap<DbBoundingBox, BoundingBox>();
            c.CreateMap<Elevation, DbElevation>();
            c.CreateMap<DbElevation, Elevation>();
            c.CreateMap<Elevation, DbElevation>();
            c.CreateMap<DbElevation, Elevation>();
            c.CreateMap<ElevationUnit, DbElevationUnit>();
            c.CreateMap<DbElevationUnit, ElevationUnit>();
            c.CreateMap<ElevationUnit, DbElevationUnit>();
            c.CreateMap<DbElevationUnit, ElevationUnit>();
            c.CreateMap<ElevationDatum, DbElevationDatum>();
            c.CreateMap<DbElevationDatum, ElevationDatum>();
            c.CreateMap<Coordinate, DbCoordinate>();
            c.CreateMap<DbCoordinate, Coordinate>();
            c.CreateMap<UtmCoordinate, DbUtmCoordinate>();
            c.CreateMap<DbUtmCoordinate, UtmCoordinate>();
            c.CreateMap<UtmCoordinateHemisphere, DbUtmCoordinateHemisphere>();
            c.CreateMap<DbUtmCoordinateHemisphere, UtmCoordinateHemisphere>();
            c.CreateMap<UtmCoordinateUnit, DbUtmCoordinateUnit>();
            c.CreateMap<DbUtmCoordinateUnit, UtmCoordinateUnit>();
            c.CreateMap<UtmCoordinateUnit, DbUtmCoordinateUnit>();
            c.CreateMap<DbUtmCoordinateUnit, UtmCoordinateUnit>();
            c.CreateMap<UtmCoordinateGeodeticDatum, DbUtmCoordinateGeodeticDatum>();
            c.CreateMap<DbUtmCoordinateGeodeticDatum, UtmCoordinateGeodeticDatum>();
            c.CreateMap<Location, DbLocation>();
            c.CreateMap<DbLocation, Location>();
            c.CreateMap<LocationType, DbLocationType>();
            c.CreateMap<DbLocationType, LocationType>();
            c.CreateMap<CountryName, DbCountryName>();
            c.CreateMap<DbCountryName, CountryName>();
            c.CreateMap<ContinentName, DbContinentName>();
            c.CreateMap<DbContinentName, ContinentName>();
            c.CreateMap<SpatialResolution, DbSpatialResolution>();
            c.CreateMap<DbSpatialResolution, SpatialResolution>();
            c.CreateMap<SpatialResolution, DbSpatialResolution>();
            c.CreateMap<DbSpatialResolution, SpatialResolution>();
            c.CreateMap<SpatialResolutionType, DbSpatialResolutionType>();
            c.CreateMap<DbSpatialResolutionType, SpatialResolutionType>();
            c.CreateMap<SpatialResolutionType, DbSpatialResolutionType>();
            c.CreateMap<DbSpatialResolutionType, SpatialResolutionType>();
            c.CreateMap<SphereContext, DbSphereContext>();
            c.CreateMap<DbSphereContext, SphereContext>();
            c.CreateMap<Sphere, DbSphere>();
            c.CreateMap<DbSphere, Sphere>();
            c.CreateMap<Atmosphere, DbAtmosphere>();
            c.CreateMap<DbAtmosphere, Atmosphere>();
            c.CreateMap<NamedAtmosphereLayer, DbNamedAtmosphereLayer>();
            c.CreateMap<DbNamedAtmosphereLayer, NamedAtmosphereLayer>();
            c.CreateMap<AtmosphereLayerName, DbAtmosphereLayerName>();
            c.CreateMap<DbAtmosphereLayerName, AtmosphereLayerName>();
            c.CreateMap<NumericAtmosphereLayer, DbNumericAtmosphereLayer>();
            c.CreateMap<DbNumericAtmosphereLayer, NumericAtmosphereLayer>();
            c.CreateMap<AtmosphereHeightUnit, DbAtmosphereHeightUnit>();
            c.CreateMap<DbAtmosphereHeightUnit, AtmosphereHeightUnit>();
            c.CreateMap<AtmosphereHeightUnit, DbAtmosphereHeightUnit>();
            c.CreateMap<DbAtmosphereHeightUnit, AtmosphereHeightUnit>();
            c.CreateMap<Hydrosphere, DbHydrosphere>();
            c.CreateMap<DbHydrosphere, Hydrosphere>();
            c.CreateMap<HydrosphereCompartment, DbHydrosphereCompartment>();
            c.CreateMap<DbHydrosphereCompartment, HydrosphereCompartment>();
            c.CreateMap<River, DbRiver>();
            c.CreateMap<DbRiver, River>();
            c.CreateMap<NamedRiverZone, DbNamedRiverZone>();
            c.CreateMap<DbNamedRiverZone, NamedRiverZone>();
            c.CreateMap<LongitudinalRiverZone, DbLongitudinalRiverZone>();
            c.CreateMap<DbLongitudinalRiverZone, LongitudinalRiverZone>();
            c.CreateMap<VerticalRiverZone, DbVerticalRiverZone>();
            c.CreateMap<DbVerticalRiverZone, VerticalRiverZone>();
            c.CreateMap<Lake, DbLake>();
            c.CreateMap<DbLake, Lake>();
            c.CreateMap<NamedLakeZone, DbNamedLakeZone>();
            c.CreateMap<DbNamedLakeZone, NamedLakeZone>();
            c.CreateMap<BenthicLakeZone, DbBenthicLakeZone>();
            c.CreateMap<DbBenthicLakeZone, BenthicLakeZone>();
            c.CreateMap<PelagicLakeZone, DbPelagicLakeZone>();
            c.CreateMap<DbPelagicLakeZone, PelagicLakeZone>();
            c.CreateMap<Sea, DbSea>();
            c.CreateMap<DbSea, Sea>();
            c.CreateMap<NamedSeaZone, DbNamedSeaZone>();
            c.CreateMap<DbNamedSeaZone, NamedSeaZone>();
            c.CreateMap<BenthicSeaZone, DbBenthicSeaZone>();
            c.CreateMap<DbBenthicSeaZone, BenthicSeaZone>();
            c.CreateMap<PelagicSeaZone, DbPelagicSeaZone>();
            c.CreateMap<DbPelagicSeaZone, PelagicSeaZone>();
            c.CreateMap<Pedosphere, DbPedosphere>();
            c.CreateMap<DbPedosphere, Pedosphere>();
            c.CreateMap<PedosphereCompartment, DbPedosphereCompartment>();
            c.CreateMap<DbPedosphereCompartment, PedosphereCompartment>();
            c.CreateMap<Soil, DbSoil>();
            c.CreateMap<DbSoil, Soil>();
            c.CreateMap<NamedSoilLayer, DbNamedSoilLayer>();
            c.CreateMap<DbNamedSoilLayer, NamedSoilLayer>();
            c.CreateMap<SoilHorizon, DbSoilHorizon>();
            c.CreateMap<DbSoilHorizon, SoilHorizon>();
            c.CreateMap<NumericSoilLayer, DbNumericSoilLayer>();
            c.CreateMap<DbNumericSoilLayer, NumericSoilLayer>();
            c.CreateMap<SoilDepthUnit, DbSoilDepthUnit>();
            c.CreateMap<DbSoilDepthUnit, SoilDepthUnit>();
            c.CreateMap<SoilDepthUnit, DbSoilDepthUnit>();
            c.CreateMap<DbSoilDepthUnit, SoilDepthUnit>();
            c.CreateMap<SoilTexture, DbSoilTexture>();
            c.CreateMap<DbSoilTexture, SoilTexture>();
            c.CreateMap<SoilMorphology, DbSoilMorphology>();
            c.CreateMap<DbSoilMorphology, SoilMorphology>();
            c.CreateMap<SoilMorphologyType, DbSoilMorphologyType>();
            c.CreateMap<DbSoilMorphologyType, SoilMorphologyType>();
            c.CreateMap<SoilAcidity, DbSoilAcidity>();
            c.CreateMap<DbSoilAcidity, SoilAcidity>();
            c.CreateMap<SoilAcidityType, DbSoilAcidityType>();
            c.CreateMap<DbSoilAcidityType, SoilAcidityType>();
            c.CreateMap<Ecosphere, DbEcosphere>();
            c.CreateMap<DbEcosphere, Ecosphere>();
            c.CreateMap<NamedEcosphereLayer, DbNamedEcosphereLayer>();
            c.CreateMap<DbNamedEcosphereLayer, NamedEcosphereLayer>();
            c.CreateMap<EcosphereLayerName, DbEcosphereLayerName>();
            c.CreateMap<DbEcosphereLayerName, EcosphereLayerName>();
            c.CreateMap<NumericEcosphereLayer, DbNumericEcosphereLayer>();
            c.CreateMap<DbNumericEcosphereLayer, NumericEcosphereLayer>();
            c.CreateMap<VegetationHeightUnit, DbVegetationHeightUnit>();
            c.CreateMap<DbVegetationHeightUnit, VegetationHeightUnit>();
            c.CreateMap<VegetationHeightUnit, DbVegetationHeightUnit>();
            c.CreateMap<DbVegetationHeightUnit, VegetationHeightUnit>();
            c.CreateMap<OrganizationalHierarchy, DbOrganizationalHierarchy>();
            c.CreateMap<DbOrganizationalHierarchy, OrganizationalHierarchy>();
            c.CreateMap<OrganizationHierarchyName, DbOrganizationHierarchyName>();
            c.CreateMap<DbOrganizationHierarchyName, OrganizationHierarchyName>();
            c.CreateMap<BiomeContext, DbBiomeContext>();
            c.CreateMap<DbBiomeContext, BiomeContext>();
            c.CreateMap<ZonoBiome, DbZonoBiome>();
            c.CreateMap<DbZonoBiome, ZonoBiome>();
            c.CreateMap<BiomeType, DbBiomeType>();
            c.CreateMap<DbBiomeType, BiomeType>();
            c.CreateMap<BiomeZone, DbBiomeZone>();
            c.CreateMap<DbBiomeZone, BiomeZone>();
            c.CreateMap<BiomeHumidityType, DbBiomeHumidityType>();
            c.CreateMap<DbBiomeHumidityType, BiomeHumidityType>();
            c.CreateMap<BiomeContinentalityType, DbBiomeContinentalityType>();
            c.CreateMap<DbBiomeContinentalityType, BiomeContinentalityType>();
            c.CreateMap<BiomeHemisphere, DbBiomeHemisphere>();
            c.CreateMap<DbBiomeHemisphere, BiomeHemisphere>();
            c.CreateMap<OroBiome, DbOroBiome>();
            c.CreateMap<DbOroBiome, OroBiome>();
            c.CreateMap<OroBiomeType, DbOroBiomeType>();
            c.CreateMap<DbOroBiomeType, OroBiomeType>();
            c.CreateMap<PedoBiome, DbPedoBiome>();
            c.CreateMap<DbPedoBiome, PedoBiome>();
            c.CreateMap<PedoBiomeType, DbPedoBiomeType>();
            c.CreateMap<DbPedoBiomeType, PedoBiomeType>();
            c.CreateMap<Physiognomy, DbPhysiognomy>();
            c.CreateMap<DbPhysiognomy, Physiognomy>();
            c.CreateMap<PhysiognomyType, DbPhysiognomyType>();
            c.CreateMap<DbPhysiognomyType, PhysiognomyType>();
            c.CreateMap<TerrestrialPhysiognomy, DbTerrestrialPhysiognomy>();
            c.CreateMap<DbTerrestrialPhysiognomy, TerrestrialPhysiognomy>();
            c.CreateMap<TerrestrialPhysiognomyType, DbTerrestrialPhysiognomyType>();
            c.CreateMap<DbTerrestrialPhysiognomyType, TerrestrialPhysiognomyType>();
            c.CreateMap<SemiAquaticPhysiognomy, DbSemiAquaticPhysiognomy>();
            c.CreateMap<DbSemiAquaticPhysiognomy, SemiAquaticPhysiognomy>();
            c.CreateMap<SemiAquaticPhysiognomyType, DbSemiAquaticPhysiognomyType>();
            c.CreateMap<DbSemiAquaticPhysiognomyType, SemiAquaticPhysiognomyType>();
            c.CreateMap<AquaticPhysiognomy, DbAquaticPhysiognomy>();
            c.CreateMap<DbAquaticPhysiognomy, AquaticPhysiognomy>();
            c.CreateMap<PlantCharacterizedAquaticPhysiognomyType, DbPlantCharacterizedAquaticPhysiognomyType>();
            c.CreateMap<DbPlantCharacterizedAquaticPhysiognomyType, PlantCharacterizedAquaticPhysiognomyType>();
            c.CreateMap<HabitatCharacterizedAquaticPhysiognomy, DbHabitatCharacterizedAquaticPhysiognomy>();
            c.CreateMap<DbHabitatCharacterizedAquaticPhysiognomy, HabitatCharacterizedAquaticPhysiognomy>();
            c.CreateMap<LandUse, DbLandUse>();
            c.CreateMap<DbLandUse, LandUse>();
            c.CreateMap<LandUseType, DbLandUseType>();
            c.CreateMap<DbLandUseType, LandUseType>();
            c.CreateMap<LandUseForm, DbLandUseForm>();
            c.CreateMap<DbLandUseForm, LandUseForm>();
            c.CreateMap<OrganismContext, DbOrganismContext>();
            c.CreateMap<DbOrganismContext, OrganismContext>();
            c.CreateMap<Organism, DbOrganism>();
            c.CreateMap<DbOrganism, Organism>();
            c.CreateMap<Taxonomy, DbTaxonomy>();
            c.CreateMap<DbTaxonomy, Taxonomy>();
            c.CreateMap<Domain, DbDomain>();
            c.CreateMap<DbDomain, Domain>();
            c.CreateMap<Kingdom, DbKingdom>();
            c.CreateMap<DbKingdom, Kingdom>();
            c.CreateMap<Division, DbDivision>();
            c.CreateMap<DbDivision, Division>();
            c.CreateMap<OrganismName, DbOrganismName>();
            c.CreateMap<DbOrganismName, OrganismName>();
            c.CreateMap<BacterialName, DbBacterialName>();
            c.CreateMap<DbBacterialName, BacterialName>();
            c.CreateMap<FungalName, DbFungalName>();
            c.CreateMap<DbFungalName, FungalName>();
            c.CreateMap<BotanicalName, DbBotanicalName>();
            c.CreateMap<DbBotanicalName, BotanicalName>();
            c.CreateMap<HybridFlag, DbHybridFlag>();
            c.CreateMap<DbHybridFlag, HybridFlag>();
            c.CreateMap<ZoologicalName, DbZoologicalName>();
            c.CreateMap<DbZoologicalName, ZoologicalName>();
            c.CreateMap<ViralName, DbViralName>();
            c.CreateMap<DbViralName, ViralName>();
            c.CreateMap<ProcessContext, DbProcessContext>();
            c.CreateMap<DbProcessContext, ProcessContext>();
            c.CreateMap<Process, DbProcess>();
            c.CreateMap<DbProcess, Process>();
            c.CreateMap<ProcessName, DbProcessName>();
            c.CreateMap<DbProcessName, ProcessName>();
            c.CreateMap<ProcessSubject, DbProcessSubject>();
            c.CreateMap<DbProcessSubject, ProcessSubject>();
            //c.CreateMap<ProcessType, DbProcessType>();
            //c.CreateMap<DbProcessType, ProcessType>();
            c.CreateMap<Interaction, DbInteraction>();
            c.CreateMap<DbInteraction, Interaction>();
            c.CreateMap<InteractionPartner, DbInteractionPartner>();
            c.CreateMap<DbInteractionPartner, InteractionPartner>();
            c.CreateMap<InteractionDirection, DbInteractionDirection>();
            c.CreateMap<DbInteractionDirection, InteractionDirection>();
            c.CreateMap<InteractionQuality, DbInteractionQuality>();
            c.CreateMap<DbInteractionQuality, InteractionQuality>();
            c.CreateMap<InteractionPartner, DbInteractionPartner>();
            c.CreateMap<DbInteractionPartner, InteractionPartner>();
            c.CreateMap<ChemicalContext, DbChemicalContext>();
            c.CreateMap<DbChemicalContext, ChemicalContext>();
            c.CreateMap<Element, DbElement>();
            c.CreateMap<DbElement, Element>();
            c.CreateMap<ElementName, DbElementName>();
            c.CreateMap<DbElementName, ElementName>();
            c.CreateMap<Isotope, DbIsotope>();
            c.CreateMap<DbIsotope, Isotope>();
            c.CreateMap<IsotopeName, DbIsotopeName>();
            c.CreateMap<DbIsotopeName, IsotopeName>();
            c.CreateMap<Compound, DbCompound>();
            c.CreateMap<DbCompound, Compound>();
            c.CreateMap<CompoundClass, DbCompoundClass>();
            c.CreateMap<DbCompoundClass, CompoundClass>();
            c.CreateMap<CompoundType, DbCompoundType>();
            c.CreateMap<DbCompoundType, CompoundType>();
            c.CreateMap<Function, DbFunction>();
            c.CreateMap<DbFunction, Function>();
            c.CreateMap<ChemicalFunctionType, DbChemicalFunctionType>();
            c.CreateMap<DbChemicalFunctionType, ChemicalFunctionType>();
            c.CreateMap<MethodContext, DbMethodContext>();
            c.CreateMap<DbMethodContext, MethodContext>();
            c.CreateMap<Approach, DbApproach>();
            c.CreateMap<DbApproach, Approach>();
            c.CreateMap<ApproachType, DbApproachType>();
            c.CreateMap<DbApproachType, ApproachType>();
            c.CreateMap<ApproachLocalization, DbApproachLocalization>();
            c.CreateMap<DbApproachLocalization, ApproachLocalization>();
            c.CreateMap<Factor, DbFactor>();
            c.CreateMap<DbFactor, Factor>();
            c.CreateMap<FactorName, DbFactorName>();
            c.CreateMap<DbFactorName, FactorName>();
            c.CreateMap<FactorType, DbFactorType>();
            c.CreateMap<DbFactorType, FactorType>();
            c.CreateMap<DataFormat, DbDataFormat>();
            c.CreateMap<DbDataFormat, DataFormat>();
            //c.CreateMap<DataFormatType, DbDataFormatType>();
            //c.CreateMap<DbDataFormatType, DataFormatType>();
            //c.CreateMap<DataSource, DbDataSource>();
            //c.CreateMap<DbDataSource, DataSource>();
            //c.CreateMap<DataSourceType, DbDataSourceType>();
            //c.CreateMap<DbDataSourceType, DataSourceType>();
            c.CreateMap<References, DbReferences>();
            c.CreateMap<DbReferences, References>();
            c.CreateMap<Hoster, DbHoster>();
            c.CreateMap<DbHoster, Hoster>();
            c.CreateMap<HosterName, DbHosterName>();
            c.CreateMap<DbHosterName, HosterName>();
            c.CreateMap<Person, DbPerson>();
            c.CreateMap<DbPerson, Person>();
            c.CreateMap<Position, DbPosition>();
            c.CreateMap<DbPosition, Position>();
            c.CreateMap<Description, DbDescription>();
            c.CreateMap<DbDescription, Description>();
            c.CreateMap<Resource, DbResource>();
            c.CreateMap<DbResource, Resource>();
            c.CreateMap<OnlineResource, DbOnlineResource>();
            c.CreateMap<DbOnlineResource, OnlineResource>();
            c.CreateMap<EmbeddedResource, DbEmbeddedResource>();
            c.CreateMap<DbEmbeddedResource, EmbeddedResource>();
            c.CreateMap<Base64Resource, DbBase64Resource>();
            c.CreateMap<DbBase64Resource, Base64Resource>();

            c.CreateMap<DbClass, Class>();
            c.CreateMap<Class, DbClass>();
            c.CreateMap<DbOrder, Order>();
            c.CreateMap<Order, DbOrder>();
            c.CreateMap<DbFamily, Family>();
            c.CreateMap<Family, DbFamily>();

            //c.CreateMap<CAFE.Core.Integration.Uri, DbUri>();
            //c.CreateMap<DbUri, CAFE.Core.Integration.Uri>();
        }
    }
}
