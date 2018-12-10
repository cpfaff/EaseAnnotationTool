
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

            c.CreateMap<DbSchemaItemDescription, SchemaItemDescription>();

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

            c.CreateMap<AnnotationItem, DbAnnotationItem>(MemberList.Destination);
            c.CreateMap<DbAnnotationItem, AnnotationItem>();
            c.CreateMap<AnnotationObject, DbAnnotationObject>(MemberList.Destination);
            c.CreateMap<DbAnnotationObject, AnnotationObject>();
            c.CreateMap<Context, DbAnnotationContext>(MemberList.Destination);
            c.CreateMap<DbAnnotationContext, Context>();
            c.CreateMap<TimeContext, DbTimeContext>(MemberList.Destination);
            c.CreateMap<DbTimeContext, TimeContext>();
            c.CreateMap<TimeRanges, DbTimeRanges>(MemberList.Destination);
            c.CreateMap<DbTimeRanges, TimeRanges>();
            c.CreateMap<TimeRange, DbTimeRange>(MemberList.Destination);
            c.CreateMap<DbTimeRange, TimeRange>();
            c.CreateMap<RangeStart, DbRangeStart>(MemberList.Destination);
            c.CreateMap<DbRangeStart, RangeStart>();
            c.CreateMap<DateAndTime, DbDateAndTime>(MemberList.Destination);
            c.CreateMap<DbDateAndTime, DateAndTime>();
            c.CreateMap<Timezone, DbTimezone>(MemberList.Destination);
            c.CreateMap<DbTimezone, Timezone>();
            c.CreateMap<RangeEnd, DbRangeEnd>(MemberList.Destination);
            c.CreateMap<DbRangeEnd, RangeEnd>();
            c.CreateMap<DateAndTime, DbDateAndTime>(MemberList.Destination);
            c.CreateMap<DbDateAndTime, DateAndTime>();
            c.CreateMap<Timezone, DbTimezone>(MemberList.Destination);
            c.CreateMap<DbTimezone, Timezone>();
            c.CreateMap<TimePeriods, DbTimePeriods>(MemberList.Destination);
            c.CreateMap<DbTimePeriods, TimePeriods>();
            c.CreateMap<GeologicalTimePeriod, DbGeologicalTimePeriod>(MemberList.Destination);
            c.CreateMap<DbGeologicalTimePeriod, GeologicalTimePeriod>();
            c.CreateMap<GeologicalEon, DbGeologicalEon>(MemberList.Destination);
            c.CreateMap<DbGeologicalEon, GeologicalEon>();
            c.CreateMap<GeologicalEra, DbGeologicalEra>(MemberList.Destination);
            c.CreateMap<DbGeologicalEra, GeologicalEra>();
            c.CreateMap<GeologicalPeriod, DbGeologicalPeriod>(MemberList.Destination);
            c.CreateMap<DbGeologicalPeriod, GeologicalPeriod>();
            c.CreateMap<GeologicalEpoch, DbGeologicalEpoch>(MemberList.Destination);
            c.CreateMap<DbGeologicalEpoch, GeologicalEpoch>();
            c.CreateMap<GeologicalAge, DbGeologicalAge>(MemberList.Destination);
            c.CreateMap<DbGeologicalAge, GeologicalAge>();
            c.CreateMap<TemporalResolution, DbTemporalResolution>(MemberList.Destination);
            c.CreateMap<DbTemporalResolution, TemporalResolution>();
            c.CreateMap<TemporalResolution, DbTemporalResolution>(MemberList.Destination);
            c.CreateMap<DbTemporalResolution, TemporalResolution>();
            c.CreateMap<TemporalExtentType, DbTemporalExtentType>(MemberList.Destination);
            c.CreateMap<DbTemporalExtentType, TemporalExtentType>();
            c.CreateMap<TemporalResolutionType, DbTemporalResolutionType>(MemberList.Destination);
            c.CreateMap<DbTemporalResolutionType, TemporalResolutionType>();
            c.CreateMap<SpaceContext, DbSpaceContext>(MemberList.Destination);
            c.CreateMap<DbSpaceContext, SpaceContext>();
            c.CreateMap<BoundingBox, DbBoundingBox>(MemberList.Destination);
            c.CreateMap<DbBoundingBox, BoundingBox>();
            c.CreateMap<BoundingBox, DbBoundingBox>(MemberList.Destination);
            c.CreateMap<DbBoundingBox, BoundingBox>();
            c.CreateMap<Elevation, DbElevation>(MemberList.Destination);
            c.CreateMap<DbElevation, Elevation>();
            c.CreateMap<Elevation, DbElevation>(MemberList.Destination);
            c.CreateMap<DbElevation, Elevation>();
            c.CreateMap<ElevationUnit, DbElevationUnit>(MemberList.Destination);
            c.CreateMap<DbElevationUnit, ElevationUnit>();
            c.CreateMap<ElevationUnit, DbElevationUnit>(MemberList.Destination);
            c.CreateMap<DbElevationUnit, ElevationUnit>();
            c.CreateMap<ElevationDatum, DbElevationDatum>(MemberList.Destination);
            c.CreateMap<DbElevationDatum, ElevationDatum>();
            c.CreateMap<Coordinate, DbCoordinate>(MemberList.Destination);
            c.CreateMap<DbCoordinate, Coordinate>();
            c.CreateMap<UtmCoordinate, DbUtmCoordinate>(MemberList.Destination);
            c.CreateMap<DbUtmCoordinate, UtmCoordinate>();
            c.CreateMap<UtmCoordinateHemisphere, DbUtmCoordinateHemisphere>(MemberList.Destination);
            c.CreateMap<DbUtmCoordinateHemisphere, UtmCoordinateHemisphere>();
            c.CreateMap<UtmCoordinateUnit, DbUtmCoordinateUnit>(MemberList.Destination);
            c.CreateMap<DbUtmCoordinateUnit, UtmCoordinateUnit>();
            c.CreateMap<UtmCoordinateUnit, DbUtmCoordinateUnit>(MemberList.Destination);
            c.CreateMap<DbUtmCoordinateUnit, UtmCoordinateUnit>();
            c.CreateMap<UtmCoordinateGeodeticDatum, DbUtmCoordinateGeodeticDatum>(MemberList.Destination);
            c.CreateMap<DbUtmCoordinateGeodeticDatum, UtmCoordinateGeodeticDatum>();
            c.CreateMap<Location, DbLocation>(MemberList.Destination);
            c.CreateMap<DbLocation, Location>();
            c.CreateMap<LocationType, DbLocationType>(MemberList.Destination);
            c.CreateMap<DbLocationType, LocationType>();
            c.CreateMap<CountryName, DbCountryName>(MemberList.Destination);
            c.CreateMap<DbCountryName, CountryName>();
            c.CreateMap<ContinentName, DbContinentName>(MemberList.Destination);
            c.CreateMap<DbContinentName, ContinentName>();
            c.CreateMap<SpatialResolution, DbSpatialResolution>(MemberList.Destination);
            c.CreateMap<DbSpatialResolution, SpatialResolution>();
            c.CreateMap<SpatialResolution, DbSpatialResolution>(MemberList.Destination);
            c.CreateMap<DbSpatialResolution, SpatialResolution>();
            c.CreateMap<SpatialResolutionType, DbSpatialResolutionType>(MemberList.Destination);
            c.CreateMap<DbSpatialResolutionType, SpatialResolutionType>();
            c.CreateMap<SpatialResolutionType, DbSpatialResolutionType>(MemberList.Destination);
            c.CreateMap<DbSpatialResolutionType, SpatialResolutionType>();
            c.CreateMap<SphereContext, DbSphereContext>(MemberList.Destination);
            c.CreateMap<DbSphereContext, SphereContext>();
            c.CreateMap<Sphere, DbSphere>(MemberList.Destination);
            c.CreateMap<DbSphere, Sphere>();
            c.CreateMap<Atmosphere, DbAtmosphere>(MemberList.Destination);
            c.CreateMap<DbAtmosphere, Atmosphere>();
            c.CreateMap<NamedAtmosphereLayer, DbNamedAtmosphereLayer>(MemberList.Destination);
            c.CreateMap<DbNamedAtmosphereLayer, NamedAtmosphereLayer>();
            c.CreateMap<AtmosphereLayerName, DbAtmosphereLayerName>(MemberList.Destination);
            c.CreateMap<DbAtmosphereLayerName, AtmosphereLayerName>();
            c.CreateMap<NumericAtmosphereLayer, DbNumericAtmosphereLayer>(MemberList.Destination);
            c.CreateMap<DbNumericAtmosphereLayer, NumericAtmosphereLayer>();
            c.CreateMap<AtmosphereHeightUnit, DbAtmosphereHeightUnit>(MemberList.Destination);
            c.CreateMap<DbAtmosphereHeightUnit, AtmosphereHeightUnit>();
            c.CreateMap<AtmosphereHeightUnit, DbAtmosphereHeightUnit>(MemberList.Destination);
            c.CreateMap<DbAtmosphereHeightUnit, AtmosphereHeightUnit>();
            c.CreateMap<Hydrosphere, DbHydrosphere>(MemberList.Destination);
            c.CreateMap<DbHydrosphere, Hydrosphere>();
            c.CreateMap<HydrosphereCompartment, DbHydrosphereCompartment>();
            c.CreateMap<DbHydrosphereCompartment, HydrosphereCompartment>();
            c.CreateMap<River, DbRiver>();
            c.CreateMap<DbRiver, River>();
            c.CreateMap<NamedRiverZone, DbNamedRiverZone>(MemberList.Destination);
            c.CreateMap<DbNamedRiverZone, NamedRiverZone>();
            c.CreateMap<LongitudinalRiverZone, DbLongitudinalRiverZone>(MemberList.Destination);
            c.CreateMap<DbLongitudinalRiverZone, LongitudinalRiverZone>();
            c.CreateMap<VerticalRiverZone, DbVerticalRiverZone>(MemberList.Destination);
            c.CreateMap<DbVerticalRiverZone, VerticalRiverZone>();
            c.CreateMap<Lake, DbLake>(MemberList.Destination);
            c.CreateMap<DbLake, Lake>();
            c.CreateMap<NamedLakeZone, DbNamedLakeZone>(MemberList.Destination);
            c.CreateMap<DbNamedLakeZone, NamedLakeZone>();
            c.CreateMap<BenthicLakeZone, DbBenthicLakeZone>(MemberList.Destination);
            c.CreateMap<DbBenthicLakeZone, BenthicLakeZone>();
            c.CreateMap<PelagicLakeZone, DbPelagicLakeZone>(MemberList.Destination);
            c.CreateMap<DbPelagicLakeZone, PelagicLakeZone>();
            c.CreateMap<Sea, DbSea>(MemberList.Destination);
            c.CreateMap<DbSea, Sea>();
            c.CreateMap<NamedSeaZone, DbNamedSeaZone>(MemberList.Destination);
            c.CreateMap<DbNamedSeaZone, NamedSeaZone>();
            c.CreateMap<BenthicSeaZone, DbBenthicSeaZone>(MemberList.Destination);
            c.CreateMap<DbBenthicSeaZone, BenthicSeaZone>();
            c.CreateMap<PelagicSeaZone, DbPelagicSeaZone>(MemberList.Destination);
            c.CreateMap<DbPelagicSeaZone, PelagicSeaZone>();
            c.CreateMap<Pedosphere, DbPedosphere>(MemberList.Destination);
            c.CreateMap<DbPedosphere, Pedosphere>();
            c.CreateMap<PedosphereCompartment, DbPedosphereCompartment>(MemberList.Destination);
            c.CreateMap<DbPedosphereCompartment, PedosphereCompartment>();
            c.CreateMap<Soil, DbSoil>(MemberList.Destination);
            c.CreateMap<DbSoil, Soil>();
            c.CreateMap<NamedSoilLayer, DbNamedSoilLayer>(MemberList.Destination);
            c.CreateMap<DbNamedSoilLayer, NamedSoilLayer>();
            c.CreateMap<SoilHorizon, DbSoilHorizon>(MemberList.Destination);
            c.CreateMap<DbSoilHorizon, SoilHorizon>();
            c.CreateMap<NumericSoilLayer, DbNumericSoilLayer>(MemberList.Destination);
            c.CreateMap<DbNumericSoilLayer, NumericSoilLayer>();
            c.CreateMap<SoilDepthUnit, DbSoilDepthUnit>(MemberList.Destination);
            c.CreateMap<DbSoilDepthUnit, SoilDepthUnit>();
            c.CreateMap<SoilDepthUnit, DbSoilDepthUnit>(MemberList.Destination);
            c.CreateMap<DbSoilDepthUnit, SoilDepthUnit>();
            c.CreateMap<SoilTexture, DbSoilTexture>(MemberList.Destination);
            c.CreateMap<DbSoilTexture, SoilTexture>();
            c.CreateMap<SoilMorphology, DbSoilMorphology>(MemberList.Destination);
            c.CreateMap<DbSoilMorphology, SoilMorphology>();
            c.CreateMap<SoilMorphologyType, DbSoilMorphologyType>(MemberList.Destination);
            c.CreateMap<DbSoilMorphologyType, SoilMorphologyType>();
            c.CreateMap<SoilAcidity, DbSoilAcidity>(MemberList.Destination);
            c.CreateMap<DbSoilAcidity, SoilAcidity>();
            c.CreateMap<SoilAcidityType, DbSoilAcidityType>(MemberList.Destination);
            c.CreateMap<DbSoilAcidityType, SoilAcidityType>();
            c.CreateMap<Ecosphere, DbEcosphere>(MemberList.Destination);
            c.CreateMap<DbEcosphere, Ecosphere>();
            c.CreateMap<NamedEcosphereLayer, DbNamedEcosphereLayer>(MemberList.Destination);
            c.CreateMap<DbNamedEcosphereLayer, NamedEcosphereLayer>();
            c.CreateMap<EcosphereLayerName, DbEcosphereLayerName>(MemberList.Destination);
            c.CreateMap<DbEcosphereLayerName, EcosphereLayerName>();
            c.CreateMap<NumericEcosphereLayer, DbNumericEcosphereLayer>(MemberList.Destination);
            c.CreateMap<DbNumericEcosphereLayer, NumericEcosphereLayer>();
            c.CreateMap<VegetationHeightUnit, DbVegetationHeightUnit>(MemberList.Destination);
            c.CreateMap<DbVegetationHeightUnit, VegetationHeightUnit>();
            c.CreateMap<VegetationHeightUnit, DbVegetationHeightUnit>(MemberList.Destination);
            c.CreateMap<DbVegetationHeightUnit, VegetationHeightUnit>();
            c.CreateMap<OrganizationalHierarchy, DbOrganizationalHierarchy>(MemberList.Destination);
            c.CreateMap<DbOrganizationalHierarchy, OrganizationalHierarchy>();
            c.CreateMap<OrganizationHierarchyName, DbOrganizationHierarchyName>(MemberList.Destination);
            c.CreateMap<DbOrganizationHierarchyName, OrganizationHierarchyName>();
            c.CreateMap<BiomeContext, DbBiomeContext>(MemberList.Destination);
            c.CreateMap<DbBiomeContext, BiomeContext>();
            c.CreateMap<ZonoBiome, DbZonoBiome>(MemberList.Destination);
            c.CreateMap<DbZonoBiome, ZonoBiome>();
            c.CreateMap<BiomeType, DbBiomeType>(MemberList.Destination);
            c.CreateMap<DbBiomeType, BiomeType>();
            c.CreateMap<BiomeLatitudinalZone, DbBiomeLatitudinalZone>(MemberList.Destination);
            c.CreateMap<DbBiomeLatitudinalZone, BiomeLatitudinalZone>();
            c.CreateMap<BiomeHumidityType, DbBiomeHumidityType>(MemberList.Destination);
            c.CreateMap<DbBiomeHumidityType, BiomeHumidityType>();
            c.CreateMap<BiomeContinentalityType, DbBiomeContinentalityType>(MemberList.Destination);
            c.CreateMap<DbBiomeContinentalityType, BiomeContinentalityType>();
            c.CreateMap<BiomeHemisphere, DbBiomeHemisphere>(MemberList.Destination);
            c.CreateMap<DbBiomeHemisphere, BiomeHemisphere>();
            c.CreateMap<OroBiome, DbOroBiome>(MemberList.Destination);
            c.CreateMap<DbOroBiome, OroBiome>();
            c.CreateMap<OroBiomeType, DbOroBiomeType>(MemberList.Destination);
            c.CreateMap<DbOroBiomeType, OroBiomeType>();
            c.CreateMap<PedoBiome, DbPedoBiome>(MemberList.Destination);
            c.CreateMap<DbPedoBiome, PedoBiome>();
            c.CreateMap<PedoBiomeType, DbPedoBiomeType>(MemberList.Destination);
            c.CreateMap<DbPedoBiomeType, PedoBiomeType>();
            c.CreateMap<Physiognomy, DbPhysiognomy>(MemberList.Destination);
            c.CreateMap<DbPhysiognomy, Physiognomy>();
            c.CreateMap<PhysiognomyType, DbPhysiognomyType>(MemberList.Destination);
            c.CreateMap<DbPhysiognomyType, PhysiognomyType>();
            c.CreateMap<TerrestrialPhysiognomy, DbTerrestrialPhysiognomy>(MemberList.Destination);
            c.CreateMap<DbTerrestrialPhysiognomy, TerrestrialPhysiognomy>();
            c.CreateMap<TerrestrialPhysiognomyType, DbTerrestrialPhysiognomyType>(MemberList.Destination);
            c.CreateMap<DbTerrestrialPhysiognomyType, TerrestrialPhysiognomyType>();
            c.CreateMap<SemiAquaticPhysiognomy, DbSemiAquaticPhysiognomy>(MemberList.Destination);
            c.CreateMap<DbSemiAquaticPhysiognomy, SemiAquaticPhysiognomy>();
            c.CreateMap<SemiAquaticPhysiognomyType, DbSemiAquaticPhysiognomyType>(MemberList.Destination);
            c.CreateMap<DbSemiAquaticPhysiognomyType, SemiAquaticPhysiognomyType>();
            c.CreateMap<AquaticPhysiognomy, DbAquaticPhysiognomy>(MemberList.Destination);
            c.CreateMap<DbAquaticPhysiognomy, AquaticPhysiognomy>();
            c.CreateMap<PlantCharacterizedAquaticPhysiognomyType, DbPlantCharacterizedAquaticPhysiognomyType>(MemberList.Destination);
            c.CreateMap<DbPlantCharacterizedAquaticPhysiognomyType, PlantCharacterizedAquaticPhysiognomyType>();
            c.CreateMap<HabitatCharacterizedAquaticPhysiognomy, DbHabitatCharacterizedAquaticPhysiognomy>(MemberList.Destination);
            c.CreateMap<DbHabitatCharacterizedAquaticPhysiognomy, HabitatCharacterizedAquaticPhysiognomy>();
            c.CreateMap<LandUse, DbLandUse>(MemberList.Destination);
            c.CreateMap<DbLandUse, LandUse>();
            c.CreateMap<LandUseType, DbLandUseType>(MemberList.Destination);
            c.CreateMap<DbLandUseType, LandUseType>();
            c.CreateMap<LandUseForm, DbLandUseForm>(MemberList.Destination);
            c.CreateMap<DbLandUseForm, LandUseForm>();
            c.CreateMap<OrganismContext, DbOrganismContext>(MemberList.Destination);
            c.CreateMap<DbOrganismContext, OrganismContext>();
            c.CreateMap<Organism, DbOrganism>(MemberList.Destination);
            c.CreateMap<DbOrganism, Organism>();
            c.CreateMap<Taxonomy, DbTaxonomy>(MemberList.Destination);
            c.CreateMap<DbTaxonomy, Taxonomy>();
            c.CreateMap<Domain, DbDomain>(MemberList.Destination);
            c.CreateMap<DbDomain, Domain>();
            c.CreateMap<Kingdom, DbKingdom>(MemberList.Destination);
            c.CreateMap<DbKingdom, Kingdom>();
            c.CreateMap<Division, DbDivision>(MemberList.Destination);
            c.CreateMap<DbDivision, Division>();
            c.CreateMap<ProcessContext, DbProcessContext>(MemberList.Destination);
            c.CreateMap<DbProcessContext, ProcessContext>();
            c.CreateMap<Process, DbProcess>(MemberList.Destination);
            c.CreateMap<DbProcess, Process>();
            c.CreateMap<ProcessName, DbProcessName>(MemberList.Destination);
            c.CreateMap<DbProcessName, ProcessName>();
            c.CreateMap<ProcessSubject, DbProcessSubject>(MemberList.Destination);
            c.CreateMap<DbProcessSubject, ProcessSubject>();
            //c.CreateMap<ProcessType, DbProcessType>();
            //c.CreateMap<DbProcessType, ProcessType>();
            c.CreateMap<Interaction, DbInteraction>(MemberList.Destination);
            c.CreateMap<DbInteraction, Interaction>();
            c.CreateMap<InteractionPartner, DbInteractionPartner>(MemberList.Destination);
            c.CreateMap<DbInteractionPartner, InteractionPartner>();
            c.CreateMap<InteractionDirection, DbInteractionDirection>(MemberList.Destination);
            c.CreateMap<DbInteractionDirection, InteractionDirection>();
            c.CreateMap<InteractionQuality, DbInteractionQuality>(MemberList.Destination);
            c.CreateMap<DbInteractionQuality, InteractionQuality>();
            c.CreateMap<InteractionPartner, DbInteractionPartner>(MemberList.Destination);
            c.CreateMap<DbInteractionPartner, InteractionPartner>();
            c.CreateMap<ChemicalContext, DbChemicalContext>(MemberList.Destination);
            c.CreateMap<DbChemicalContext, ChemicalContext>();
            c.CreateMap<Element, DbElement>(MemberList.Destination);
            c.CreateMap<DbElement, Element>();
            c.CreateMap<ElementName, DbElementName>(MemberList.Destination);
            c.CreateMap<DbElementName, ElementName>();
            c.CreateMap<Isotope, DbIsotope>(MemberList.Destination);
            c.CreateMap<DbIsotope, Isotope>();
            c.CreateMap<IsotopeName, DbIsotopeName>(MemberList.Destination);
            c.CreateMap<DbIsotopeName, IsotopeName>();
            c.CreateMap<Compound, DbCompound>(MemberList.Destination);
            c.CreateMap<DbCompound, Compound>();
            c.CreateMap<CompoundClass, DbCompoundClass>(MemberList.Destination);
            c.CreateMap<DbCompoundClass, CompoundClass>();
            c.CreateMap<CompoundType, DbCompoundType>(MemberList.Destination);
            c.CreateMap<DbCompoundType, CompoundType>();
            c.CreateMap<Function, DbFunction>(MemberList.Destination);
            c.CreateMap<DbFunction, Function>();
            c.CreateMap<ChemicalFunctionType, DbChemicalFunctionType>(MemberList.Destination);
            c.CreateMap<DbChemicalFunctionType, ChemicalFunctionType>();
            c.CreateMap<MethodContext, DbMethodContext>(MemberList.Destination);
            c.CreateMap<DbMethodContext, MethodContext>();
            c.CreateMap<Approach, DbApproach>(MemberList.Destination);
            c.CreateMap<DbApproach, Approach>();
            c.CreateMap<ApproachType, DbApproachType>(MemberList.Destination);
            c.CreateMap<DbApproachType, ApproachType>();
            c.CreateMap<ApproachLocalization, DbApproachLocalization>(MemberList.Destination);
            c.CreateMap<DbApproachLocalization, ApproachLocalization>();
            c.CreateMap<Factor, DbFactor>(MemberList.Destination);
            c.CreateMap<DbFactor, Factor>();
            c.CreateMap<FactorName, DbFactorName>(MemberList.Destination);
            c.CreateMap<DbFactorName, FactorName>();
            c.CreateMap<FactorType, DbFactorType>(MemberList.Destination);
            c.CreateMap<DbFactorType, FactorType>();
            c.CreateMap<DataFormat, DbDataFormat>(MemberList.Destination);
            c.CreateMap<DbDataFormat, DataFormat>();
            //c.CreateMap<DataFormatType, DbDataFormatType>();
            //c.CreateMap<DbDataFormatType, DataFormatType>();
            //c.CreateMap<DataSource, DbDataSource>();
            //c.CreateMap<DbDataSource, DataSource>();
            //c.CreateMap<DataSourceType, DbDataSourceType>();
            //c.CreateMap<DbDataSourceType, DataSourceType>();
            c.CreateMap<References, DbReferences>(MemberList.Destination);
            c.CreateMap<DbReferences, References>();
            c.CreateMap<Hoster, DbHoster>(MemberList.Destination);
            c.CreateMap<DbHoster, Hoster>();
            c.CreateMap<HosterName, DbHosterName>(MemberList.Destination);
            c.CreateMap<DbHosterName, HosterName>();
            c.CreateMap<Person, DbPerson>(MemberList.Destination);
            c.CreateMap<DbPerson, Person>();
            c.CreateMap<Position, DbPosition>(MemberList.Destination);
            c.CreateMap<DbPosition, Position>();
            c.CreateMap<Description, DbDescription>(MemberList.Destination);
            c.CreateMap<DbDescription, Description>();
            c.CreateMap<Resource, DbResource>(MemberList.Destination);
            c.CreateMap<DbResource, Resource>();
            c.CreateMap<OnlineResource, DbOnlineResource>(MemberList.Destination);
            c.CreateMap<DbOnlineResource, OnlineResource>();
            c.CreateMap<EmbeddedResource, DbEmbeddedResource>(MemberList.Destination);
            c.CreateMap<DbEmbeddedResource, EmbeddedResource>();
            c.CreateMap<Base64Resource, DbBase64Resource>(MemberList.Destination);
            c.CreateMap<DbBase64Resource, Base64Resource>();

            c.CreateMap<DbClass, Class>();
            c.CreateMap<Class, DbClass>(MemberList.Destination);
            c.CreateMap<DbOrder, Order>();
            c.CreateMap<Order, DbOrder>(MemberList.Destination);
            c.CreateMap<DbFamily, Family>();
            c.CreateMap<Family, DbFamily>(MemberList.Destination);
            c.CreateMap<DbGenus, Genus>();
            c.CreateMap<Genus, DbGenus>(MemberList.Destination);
            //c.CreateMap<CAFE.Core.Integration.Uri, DbUri>();
            //c.CreateMap<DbUri, CAFE.Core.Integration.Uri>();
        }
    }
}
