using System;
using AutoMapper;
using CAFE.Core.Configuration;
using CAFE.Core.Integration;
using CAFE.Core.Messaging;
using CAFE.Core.Resources;
using CAFE.Core.Searching;
using CAFE.Core.Security;
using CAFE.Web.Areas.Api.Models;
using CAFE.Web.Areas.Api.Models.AccessResources;
using CAFE.Web.Areas.Api.Models.Search;
using CAFE.Web.Configuration;
using CAFE.Web.Models.Account;
using CAFE.Web.Models.Dashboard;
using CAFE.Web.Models;
using CAFE.DAL.Models;
using System.Collections.Generic;
using Microsoft.Practices.Unity;
using CAFE.Core.Plugins;

namespace CAFE.Web
{
    public class MapperConfig
    {//
        public static void Config(IUnityContainer container)
        {
            Mapper.Initialize(c =>
            {
                c.ShouldMapField = fieldInfo => { return (fieldInfo.FieldType.IsGenericType); };

                c.CreateMap<ApplicationConfigurationSection, ApplicationConfiguration>();
                c.CreateMap<ApplicationConfiguration, ApplicationConfigurationSection>();
                c.CreateMap<EmailConfigurationSection, EmailConfiguration>();
                c.CreateMap<EmailConfiguration, EmailConfigurationSection>();
                c.CreateMap<RelatedSearchFiltersConfigurationSection, RelatedSearchFiltersConfiguration>();
                c.CreateMap<RelatedSearchFiltersConfiguration, RelatedSearchFiltersConfigurationSection>();
                c.CreateMap<Configuration.RelatedFilterScope, Core.Configuration.RelatedFilterScope>();
                c.CreateMap<Core.Configuration.RelatedFilterScope, Configuration.RelatedFilterScope>();
                c.CreateMap<Configuration.FilterElement, Core.Configuration.FilterElement>();
                c.CreateMap<Core.Configuration.FilterElement, Configuration.FilterElement>();

                c.CreateMap<User, UserViewModel>(MemberList.Source).MaxDepth(2).PreserveReferences();
                c.CreateMap<UserViewModel, User>(MemberList.Destination).MaxDepth(2).PreserveReferences();
                c.CreateMap<User, GroupsUserViewModel>(MemberList.Source).MaxDepth(2).PreserveReferences();
                c.CreateMap<GroupsUserViewModel, User>(MemberList.Destination).MaxDepth(2).PreserveReferences();
                c.CreateMap<Group, UserGroupsViewModel>(MemberList.Source).MaxDepth(2).PreserveReferences();
                c.CreateMap<UserGroupsViewModel, Group>(MemberList.Destination).MaxDepth(2).PreserveReferences();

                c.CreateMap<Group, GroupViewModel>(MemberList.Source).MaxDepth(2).PreserveReferences();
                c.CreateMap<GroupViewModel, Group>(MemberList.Destination).MaxDepth(2).PreserveReferences();

                c.CreateMap<UserFile, UserFilesViewModel>(MemberList.Source);
                c.CreateMap<UserFilesViewModel, UserFile>(MemberList.Destination);

                c.CreateMap<DbUserFile, UserFile>(MemberList.Destination).MaxDepth(2).PreserveReferences();
                c.CreateMap<DbRole, Group>(MemberList.Destination).
                MaxDepth(2).PreserveReferences();

                c.CreateMap<DbAnnotationItem, DbAnnotationItem>();

                c.CreateMap<User, UserProfileViewModel>(MemberList.Destination);

                c.CreateMap<User, UserAcceptanceViewModel>(MemberList.Source)
                    .ForMember(m => m.Name, opt => opt.MapFrom(f => string.Concat(f.Name, " ", f.Surname)))
                    .ForMember(m => m.UserId, opt => opt.MapFrom(f => f.Id));
                
                c.CreateMap<User, Models.Manage.UserDetailsViewModel>(MemberList.Source)
                    .ForMember(m => m.Phone, opt => opt.MapFrom(f => f.PhoneNumber));
                c.CreateMap<User, Models.Manage.UserEditViewModel>(MemberList.Source)
                    .ForMember(m => m.Phone, opt => opt.MapFrom(f => f.PhoneNumber));
                c.CreateMap<Models.Manage.UserEditViewModel, User>(MemberList.Destination)
                    .ForMember(m => m.PhoneNumber, opt => opt.MapFrom(f => f.Phone));

                c.CreateMap<SearchRequestFilterModel, SearchRequestFilterItem>()
                    .ForMember(m => m.Value, opt => opt.MapFrom(f => ConverToSearchFilterValue(f)));

                c.CreateMap<SearchRequestFilterItem, SearchRequestFilterModel>()
                    .ForMember(m => m.Value, opts => opts.Ignore());
                    //.ForMember(m => m.RelatedFilters, opts => opts.Ignore());
                c.CreateMap<SearchRequestModel, SearchRequest>()
                    .ForMember(m => m.ItemType, opt => opt.MapFrom(f => f.SearchItemsType));
                c.CreateMap<SearchResultItem, SearchResultItemViewModel>(MemberList.Source)
                    .ForMember(m => m.UserId, opts => opts.MapFrom(f => f.Owner.Id))
                    .ForMember(m => m.UserName, opts => opts.MapFrom(f => f.Owner.Name + " " + f.Owner.Surname));

                c.CreateMap<Core.Integration.AnnotationItem, DbAnnotationItem>();
                c.CreateMap<DbAnnotationItem, Core.Integration.AnnotationItem>();

                c.CreateMap<Core.Integration.AnnotationItem, AnnotationItemModel>();
                c.CreateMap<AnnotationItemModel, Core.Integration.AnnotationItem>();

                c.CreateMap<DbAnnotationItem, AnnotationItemModel>();
                c.CreateMap<AnnotationItemModel, DbAnnotationItem>();

                c.CreateMap<Areas.Api.Models.AccessResources.AnnotationItemViewModel, DbAnnotationItem>();

                c.CreateMap<AccessibleResource, AccessibleResourceModel>()
                    .ForMember(m => m.OwnerId, opt => opt.MapFrom(f => f.Content.OwnerId))
                    .ForMember(m => m.OwnerName, opt => opt.MapFrom(f => f.Content.OwnerName));

                c.CreateMap<AccessibleResourceModel, AccessibleResource>()
                    .ForMember(m => m.Content, opts => opts.Ignore())
                    .AfterMap((s, d) =>
                    {
                        if (d.Kind == AccessibleResourceKind.File)
                        {
                            d.Content = new UserFile()
                            {
                                Id = Guid.Parse(s.ResourceId),
                                Name = s.Name,
                                Description = s.Description,
                                OwnerId = s.OwnerId,
                                OwnerName = s.OwnerName
                            };
                        }
                        else
                        {
                            d.Content = new DbAnnotationItem()
                            {
                                Id = Guid.Parse(s.ResourceId),
                                OwnerId = s.OwnerId,
                                OwnerName = s.OwnerName
                                //Owner = new User() { Id = s.OwnerId }
                            };
                        }
                    });

                c.CreateMap<AccessRequest, AccessRequestModel>()
                    .ForMember(m => m.RequestedResources, opts => opts.Ignore());

                c.CreateMap<Message, MessageModel>()
                    .ForMember(m => m.Sender, opts => opts.MapFrom(f => f.Sender.Name + " " + f.Sender.Surname))
                    .ForMember(m => m.Receiver, opts => opts.MapFrom(f => f.Receiver.Name + " " + f.Receiver.Surname))
                    .ForMember(m => m.CreationDate, opts => opts.MapFrom(f => String.Format("{0:d/M/yyyy HH:mm:ss}", f.CreationDate)));

                c.CreateMap<Conversation, ConversationModel>()
                    .ForMember(m => m.RequestId, opts => opts.MapFrom(f => f.Request.Id))
                    .ForMember(m => m.Requester, opts => opts.MapFrom(f => f.Requester.Name + " " + f.Requester.Surname))
                    .ForMember(m => m.Receiver, opts => opts.MapFrom(f => f.Receiver.Name + " " + f.Receiver.Surname))
                    .ForMember(m => m.RequesterId, opts => opts.MapFrom(f => f.Requester.Id))
                    .ForMember(m => m.ReceiverId, opts => opts.MapFrom(f => f.Receiver.Id))
                    .ForMember(m => m.CreationDate, opts => opts.MapFrom(f => f.Request.CreationDate))
                    .ForMember(m => m.Subject, opts => opts.MapFrom(f => f.Request.RequestSubject))
                    .ForMember(m => m.Status, opts => opts.MapFrom(f => f.Status.ToString()));

                c.CreateMap<ConversationModel, AccessRequestsViewModel>();

                c.CreateMap<SearchResultItem, QuickSearchResultItemModel>()
                    .ForMember(m => m.Id, opts => opts.MapFrom(f => f.ItemId.ToString()))
                    .ForMember(m => m.Name, opts => opts.MapFrom(f => f.Name));

                c.CreateMap<string, SearchFilterSelectionNamedModel>()
                    .ForMember(m => m.Name, opts => opts.MapFrom(f => f))
                    .ForMember(m => m.Value, opts => opts.MapFrom(f => f));

                c.CreateMap<VocabularyValueModel, VocabularyValue>();
                c.CreateMap<VocabularyValue, VocabularyValueModel>();
                c.CreateMap<VocabularyUserValueModel, VocabularyUserValue>();
                c.CreateMap<VocabularyUserValue, VocabularyUserValueModel>();


                c.CreateMap<AnnotationItemModel, DbAnnotationItem>();
                c.CreateMap<DbAnnotationItem, AnnotationItemModel>();
                c.CreateMap<DbAnnotationObject, AnnotationObjectModel>();
                c.CreateMap<AnnotationObjectModel, DbAnnotationObject>();


                c.CreateMap<ContextModel, DbAnnotationContext>();
                c.CreateMap<TimeContextModel, DbTimeContext>();
                c.CreateMap<TimeRangesModel, DbTimeRanges>();
                c.CreateMap<TimeRangeModel, DbTimeRange>();
                c.CreateMap<RangeStartModel, DbRangeStart>();
                c.CreateMap<DateAndTimeModel, DbDateAndTime>();
                c.CreateMap<TimezoneModel, DbTimezone>();
                c.CreateMap<RangeEndModel, DbRangeEnd>();
                c.CreateMap<TimePeriodsModel, DbTimePeriods>();
                c.CreateMap<GeologicalTimePeriodModel, DbGeologicalTimePeriod>();
                c.CreateMap<GeologicalEonModel, DbGeologicalEon>();
                c.CreateMap<GeologicalEraModel, DbGeologicalEra>();
                c.CreateMap<GeologicalPeriodModel, DbGeologicalPeriod>();
                c.CreateMap<GeologicalEpochModel, DbGeologicalEpoch>();
                c.CreateMap<GeologicalAgeModel, DbGeologicalAge>();
                //c.CreateMap<TemporalResolutionsModel, DbTemporalResolution>();
                c.CreateMap<TemporalResolutionModel, DbTemporalResolution>();
                c.CreateMap<TemporalExtentTypeModel, DbTemporalExtentType>();
                c.CreateMap<TemporalResolutionTypeModel, DbTemporalResolutionType>();
                c.CreateMap<SpaceContextModel, DbSpaceContext>();
                c.CreateMap<BoundingBoxModel, DbBoundingBox>();
                c.CreateMap<ElevationModel, DbElevation>();
                c.CreateMap<ElevationUnitModel, DbElevationUnit>();
                c.CreateMap<ElevationDatumModel, DbElevationDatum>();
                c.CreateMap<CoordinateModel, DbCoordinate>();
                c.CreateMap<UtmCoordinateModel, DbUtmCoordinate>();
                c.CreateMap<UtmCoordinateHemisphereModel, DbUtmCoordinateHemisphere>();
                c.CreateMap<UtmCoordinateUnitModel, DbUtmCoordinateUnit>();
                c.CreateMap<UtmCoordinateGeodeticDatumModel, DbUtmCoordinateGeodeticDatum>();
                c.CreateMap<LocationModel, DbLocation>();
                c.CreateMap<LocationTypeModel, DbLocationType>();
                c.CreateMap<CountryNameModel, DbCountryName>();
                c.CreateMap<ContinentNameModel, DbContinentName>();
                //c.CreateMap<SpatialResolutionsModel, DbSpatialResolutions>();
                c.CreateMap<SpatialResolutionModel, DbSpatialResolution>();
                c.CreateMap<SpatialResolutionTypeModel, DbSpatialResolutionType>();
                c.CreateMap<SphereContextModel, DbSphereContext>();
                c.CreateMap<SphereModel, DbSphere>();
                c.CreateMap<AtmosphereModel, DbAtmosphere>();
                c.CreateMap<NamedAtmosphereLayerModel, DbNamedAtmosphereLayer>();
                c.CreateMap<AtmosphereLayerNameModel, DbAtmosphereLayerName>();
                c.CreateMap<NumericAtmosphereLayerModel, DbNumericAtmosphereLayer>();
                c.CreateMap<AtmosphereHeightUnitModel, DbAtmosphereHeightUnit>();
                c.CreateMap<HydrosphereModel, DbHydrosphere>();
                c.CreateMap<HydrosphereCompartmentModel, DbHydrosphereCompartment>();
                c.CreateMap<RiverModel, DbRiver>();
                c.CreateMap<NamedRiverZoneModel, DbNamedRiverZone>();
                c.CreateMap<LongitudinalRiverZoneModel, DbLongitudinalRiverZone>();
                c.CreateMap<VerticalRiverZoneModel, DbVerticalRiverZone>();
                c.CreateMap<LakeModel, DbLake>();
                c.CreateMap<NamedLakeZoneModel, DbNamedLakeZone>();
                c.CreateMap<BenthicLakeZoneModel, DbBenthicLakeZone>();
                c.CreateMap<PelagicLakeZoneModel, DbPelagicLakeZone>();
                c.CreateMap<SeaModel, DbSea>();
                c.CreateMap<NamedSeaZoneModel, DbNamedSeaZone>();
                c.CreateMap<BenthicSeaZoneModel, DbBenthicSeaZone>();
                c.CreateMap<PelagicSeaZoneModel, DbPelagicSeaZone>();
                c.CreateMap<PedosphereModel, DbPedosphere>();
                c.CreateMap<PedosphereCompartmentModel, DbPedosphereCompartment>();
                c.CreateMap<SoilModel, DbSoil>();
                c.CreateMap<NamedSoilLayerModel, DbNamedSoilLayer>();
                c.CreateMap<SoilHorizonModel, DbSoilHorizon>();
                c.CreateMap<NumericSoilLayerModel, DbNumericSoilLayer>();
                c.CreateMap<SoilDepthUnitModel, DbSoilDepthUnit>();
                c.CreateMap<SoilTextureModel, DbSoilTexture>();
                c.CreateMap<SoilMorphologyModel, DbSoilMorphology>();
                c.CreateMap<SoilMorphologyTypeModel, DbSoilMorphologyType>();
                c.CreateMap<SoilAcidityModel, DbSoilAcidity>();
                c.CreateMap<SoilAcidityTypeModel, DbSoilAcidityType>();
                c.CreateMap<EcosphereModel, DbEcosphere>();
                c.CreateMap<NamedEcosphereLayerModel, DbNamedEcosphereLayer>();
                c.CreateMap<EcosphereLayerNameModel, DbEcosphereLayerName>();
                c.CreateMap<NumericEcosphereLayerModel, DbNumericEcosphereLayer>();
                c.CreateMap<VegetationHeightUnitModel, DbVegetationHeightUnit>();
                c.CreateMap<OrganizationalHierarchyModel, DbOrganizationalHierarchy>();
                c.CreateMap<OrganizationHierarchyNameModel, DbOrganizationHierarchyName>();
                c.CreateMap<BiomeContextModel, DbBiomeContext>();
                c.CreateMap<ZonoBiomeModel, DbZonoBiome>();
                c.CreateMap<BiomeTypeModel, DbBiomeType>();
                c.CreateMap<BiomeZoneModel, DbBiomeZone>();
                c.CreateMap<BiomeHumidityTypeModel, DbBiomeHumidityType>();
                c.CreateMap<BiomeContinentalityTypeModel, DbBiomeContinentalityType>();
                c.CreateMap<BiomeHemisphereModel, DbBiomeHemisphere>();
                c.CreateMap<OroBiomeModel, DbOroBiome>();
                c.CreateMap<OroBiomeTypeModel, DbOroBiomeType>();
                c.CreateMap<PedoBiomeModel, DbPedoBiome>();
                c.CreateMap<PedoBiomeTypeModel, DbPedoBiomeType>();
                c.CreateMap<PhysiognomyModel, DbPhysiognomy>();
                c.CreateMap<PhysiognomyTypeModel, DbPhysiognomyType>();
                c.CreateMap<TerrestrialPhysiognomyModel, DbTerrestrialPhysiognomy>();
                c.CreateMap<TerrestrialPhysiognomyTypeModel, DbTerrestrialPhysiognomyType>();
                c.CreateMap<SemiAquaticPhysiognomyModel, DbSemiAquaticPhysiognomy>();
                c.CreateMap<SemiAquaticPhysiognomyTypeModel, DbSemiAquaticPhysiognomyType>();
                c.CreateMap<AquaticPhysiognomyModel, DbAquaticPhysiognomy>();
                c.CreateMap<PlantCharacterizedAquaticPhysiognomyTypeModel, DbPlantCharacterizedAquaticPhysiognomyType>();
                c.CreateMap<HabitatCharacterizedAquaticPhysiognomyModel, DbHabitatCharacterizedAquaticPhysiognomy>();
                c.CreateMap<LandUseModel, DbLandUse>();
                c.CreateMap<LandUseTypeModel, DbLandUseType>();
                c.CreateMap<LandUseFormModel, DbLandUseForm>();
                c.CreateMap<OrganismContextModel, DbOrganismContext>();
                c.CreateMap<OrganismModel, DbOrganism>();
                c.CreateMap<TaxonomyModel, DbTaxonomy>();
                c.CreateMap<DomainModel, DbDomain>();
                c.CreateMap<KingdomModel, DbKingdom>();
                c.CreateMap<DivisionModel, DbDivision>();
                c.CreateMap<OrganismNameModel, DbOrganismName>();
                c.CreateMap<BacterialNameModel, DbBacterialName>();
                c.CreateMap<FungalNameModel, DbFungalName>();
                c.CreateMap<BotanicalNameModel, DbBotanicalName>();
                c.CreateMap<HybridFlagModel, DbHybridFlag>();
                c.CreateMap<ZoologicalNameModel, DbZoologicalName>();
                c.CreateMap<ViralNameModel, DbViralName>();
                c.CreateMap<ProcessContextModel, DbProcessContext>();
                c.CreateMap<ProcessModel, DbProcess>();
                c.CreateMap<ProcessNameModel, DbProcessName>();
                c.CreateMap<ProcessSubjectModel, DbProcessSubject>();
                c.CreateMap<InteractionModel, DbInteraction>();
                c.CreateMap<InteractionPartnerModel, DbInteractionPartner>();
                c.CreateMap<InteractionDirectionModel, DbInteractionDirection>();
                c.CreateMap<InteractionQualityModel, DbInteractionQuality>();
                c.CreateMap<ChemicalContextModel, DbChemicalContext>();
                c.CreateMap<ElementModel, DbElement>();
                c.CreateMap<ElementNameModel, DbElementName>();
                c.CreateMap<IsotopeModel, DbIsotope>();
                c.CreateMap<IsotopeNameModel, DbIsotopeName>();
                c.CreateMap<CompoundModel, DbCompound>();
                c.CreateMap<CompoundClassModel, DbCompoundClass>();
                c.CreateMap<CompoundTypeModel, DbCompoundType>();
                c.CreateMap<FunctionModel, DbFunction>();
                c.CreateMap<ChemicalFunctionTypeModel, DbChemicalFunctionType>();
                c.CreateMap<MethodContextModel, DbMethodContext>();
                c.CreateMap<ApproachModel, DbApproach>();
                c.CreateMap<ApproachTypeModel, DbApproachType>();
                c.CreateMap<ApproachLocalizationModel, DbApproachLocalization>();
                c.CreateMap<FactorModel, DbFactor>();
                c.CreateMap<FactorNameModel, DbFactorName>();
                c.CreateMap<FactorTypeModel, DbFactorType>();
                c.CreateMap<DataFormatModel, DbDataFormat>();
                c.CreateMap<ReferencesModel, DbReferences>();
                c.CreateMap<HosterModel, DbHoster>();
                c.CreateMap<HosterNameModel, DbHosterName>();
                c.CreateMap<PersonModel, DbPerson>();
                c.CreateMap<PositionModel, DbPosition>();
                c.CreateMap<DescriptionModel, DbDescription>();
                c.CreateMap<ResourceModel, DbResource>();
                c.CreateMap<OnlineResourceModel, DbOnlineResource>();
                c.CreateMap<OfflineResourceModel, DbOfflineResource>();
                c.CreateMap<OfflineResource, DbOfflineResource>();
                c.CreateMap<EmbeddedResourceModel, DbEmbeddedResource>();
                c.CreateMap<DbAnnotationContext, ContextModel>();
                c.CreateMap<DbTimeContext, TimeContextModel>();
                c.CreateMap<DbTimeRanges, TimeRangesModel>();
                c.CreateMap<DbTimeRange, TimeRangeModel>();
                c.CreateMap<DbRangeStart, RangeStartModel>();
                c.CreateMap<DbDateAndTime, DateAndTimeModel>();
                c.CreateMap<DbTimezone, TimezoneModel>();
                c.CreateMap<DbRangeEnd, RangeEndModel>();
                c.CreateMap<DbTimePeriods, TimePeriodsModel>();
                c.CreateMap<DbGeologicalTimePeriod, GeologicalTimePeriodModel>();
                c.CreateMap<DbGeologicalEon, GeologicalEonModel>();
                c.CreateMap<DbGeologicalEra, GeologicalEraModel>();
                c.CreateMap<DbGeologicalPeriod, GeologicalPeriodModel>();
                c.CreateMap<DbGeologicalEpoch, GeologicalEpochModel>();
                c.CreateMap<DbGeologicalAge, GeologicalAgeModel>();
                //c.CreateMap<DbTemporalResolutions, TemporalResolutionsModel>();
                c.CreateMap<DbTemporalResolution, TemporalResolutionModel>();
                c.CreateMap<DbTemporalExtentType, TemporalExtentTypeModel>();
                c.CreateMap<DbTemporalResolutionType, TemporalResolutionTypeModel>();
                c.CreateMap<DbSpaceContext, SpaceContextModel>();
                c.CreateMap<DbBoundingBox, BoundingBoxModel>();
                c.CreateMap<DbElevation, ElevationModel>();
                c.CreateMap<DbElevationUnit, ElevationUnitModel>();
                c.CreateMap<DbElevationDatum, ElevationDatumModel>();
                c.CreateMap<DbCoordinate, CoordinateModel>();
                c.CreateMap<DbUtmCoordinate, UtmCoordinateModel>();
                c.CreateMap<DbUtmCoordinateHemisphere, UtmCoordinateHemisphereModel>();
                c.CreateMap<DbUtmCoordinateUnit, UtmCoordinateUnitModel>();
                c.CreateMap<DbUtmCoordinateGeodeticDatum, UtmCoordinateGeodeticDatumModel>();
                c.CreateMap<DbLocation, LocationModel>();
                c.CreateMap<DbLocationType, LocationTypeModel>();
                c.CreateMap<DbCountryName, CountryNameModel>();
                c.CreateMap<DbContinentName, ContinentNameModel>();
                //c.CreateMap<DbSpatialResolutions, SpatialResolutionsModel>();
                c.CreateMap<DbSpatialResolution, SpatialResolutionModel>();
                c.CreateMap<DbSpatialResolutionType, SpatialResolutionTypeModel>();
                c.CreateMap<DbSphereContext, SphereContextModel>();
                c.CreateMap<DbSphere, SphereModel>();
                c.CreateMap<DbAtmosphere, AtmosphereModel>();
                c.CreateMap<DbNamedAtmosphereLayer, NamedAtmosphereLayerModel>();
                c.CreateMap<DbAtmosphereLayerName, AtmosphereLayerNameModel>();
                c.CreateMap<DbNumericAtmosphereLayer, NumericAtmosphereLayerModel>();
                c.CreateMap<DbAtmosphereHeightUnit, AtmosphereHeightUnitModel>();
                c.CreateMap<DbHydrosphere, HydrosphereModel>();
                c.CreateMap<DbHydrosphereCompartment, HydrosphereCompartmentModel>();
                c.CreateMap<DbRiver, RiverModel>();
                c.CreateMap<DbNamedRiverZone, NamedRiverZoneModel>();
                c.CreateMap<DbLongitudinalRiverZone, LongitudinalRiverZoneModel>();
                c.CreateMap<DbVerticalRiverZone, VerticalRiverZoneModel>();
                c.CreateMap<DbLake, LakeModel>();
                c.CreateMap<DbNamedLakeZone, NamedLakeZoneModel>();
                c.CreateMap<DbBenthicLakeZone, BenthicLakeZoneModel>();
                c.CreateMap<DbPelagicLakeZone, PelagicLakeZoneModel>();
                c.CreateMap<DbSea, SeaModel>();
                c.CreateMap<DbNamedSeaZone, NamedSeaZoneModel>();
                c.CreateMap<DbBenthicSeaZone, BenthicSeaZoneModel>();
                c.CreateMap<DbPelagicSeaZone, PelagicSeaZoneModel>();
                c.CreateMap<DbPedosphere, PedosphereModel>();
                c.CreateMap<DbPedosphereCompartment, PedosphereCompartmentModel>();
                c.CreateMap<DbSoil, SoilModel>();
                c.CreateMap<DbNamedSoilLayer, NamedSoilLayerModel>();
                c.CreateMap<DbSoilHorizon, SoilHorizonModel>();
                c.CreateMap<DbNumericSoilLayer, NumericSoilLayerModel>();
                c.CreateMap<DbSoilDepthUnit, SoilDepthUnitModel>();
                c.CreateMap<DbSoilTexture, SoilTextureModel>();
                c.CreateMap<DbSoilMorphology, SoilMorphologyModel>();
                c.CreateMap<DbSoilMorphologyType, SoilMorphologyTypeModel>();
                c.CreateMap<DbSoilAcidity, SoilAcidityModel>();
                c.CreateMap<DbSoilAcidityType, SoilAcidityTypeModel>();
                c.CreateMap<DbEcosphere, EcosphereModel>();
                c.CreateMap<DbNamedEcosphereLayer, NamedEcosphereLayerModel>();
                c.CreateMap<DbEcosphereLayerName, EcosphereLayerNameModel>();
                c.CreateMap<DbNumericEcosphereLayer, NumericEcosphereLayerModel>();
                c.CreateMap<DbVegetationHeightUnit, VegetationHeightUnitModel>();
                c.CreateMap<DbOrganizationalHierarchy, OrganizationalHierarchyModel>();
                c.CreateMap<DbOrganizationHierarchyName, OrganizationHierarchyNameModel>();
                c.CreateMap<DbBiomeContext, BiomeContextModel>();
                c.CreateMap<DbZonoBiome, ZonoBiomeModel>();
                c.CreateMap<DbBiomeType, BiomeTypeModel>();
                c.CreateMap<DbBiomeZone, BiomeZoneModel>();
                c.CreateMap<DbBiomeHumidityType, BiomeHumidityTypeModel>();
                c.CreateMap<DbBiomeContinentalityType, BiomeContinentalityTypeModel>();
                c.CreateMap<DbBiomeHemisphere, BiomeHemisphereModel>();
                c.CreateMap<DbOroBiome, OroBiomeModel>();
                c.CreateMap<DbOroBiomeType, OroBiomeTypeModel>();
                c.CreateMap<DbPedoBiome, PedoBiomeModel>();
                c.CreateMap<DbPedoBiomeType, PedoBiomeTypeModel>();
                c.CreateMap<DbPhysiognomy, PhysiognomyModel>();
                c.CreateMap<DbPhysiognomyType, PhysiognomyTypeModel>();
                c.CreateMap<DbTerrestrialPhysiognomy, TerrestrialPhysiognomyModel>();
                c.CreateMap<DbTerrestrialPhysiognomyType, TerrestrialPhysiognomyTypeModel>();
                c.CreateMap<DbSemiAquaticPhysiognomy, SemiAquaticPhysiognomyModel>();
                c.CreateMap<DbSemiAquaticPhysiognomyType, SemiAquaticPhysiognomyTypeModel>();
                c.CreateMap<DbAquaticPhysiognomy, AquaticPhysiognomyModel>();
                c.CreateMap<DbPlantCharacterizedAquaticPhysiognomyType, PlantCharacterizedAquaticPhysiognomyTypeModel>();
                c.CreateMap<DbHabitatCharacterizedAquaticPhysiognomy, HabitatCharacterizedAquaticPhysiognomyModel>();
                c.CreateMap<DbLandUse, LandUseModel>();
                c.CreateMap<DbLandUseType, LandUseTypeModel>();
                c.CreateMap<DbLandUseForm, LandUseFormModel>();
                c.CreateMap<DbOrganismContext, OrganismContextModel>();
                c.CreateMap<DbOrganism, OrganismModel>();
                c.CreateMap<DbTaxonomy, TaxonomyModel>();
                c.CreateMap<DbDomain, DomainModel>();
                c.CreateMap<DbKingdom, KingdomModel>();
                c.CreateMap<DbDivision, DivisionModel>();
                c.CreateMap<DbOrganismName, OrganismNameModel>();
                c.CreateMap<DbBacterialName, BacterialNameModel>();
                c.CreateMap<DbFungalName, FungalNameModel>();
                c.CreateMap<DbBotanicalName, BotanicalNameModel>();
                c.CreateMap<DbHybridFlag, HybridFlagModel>();
                c.CreateMap<DbZoologicalName, ZoologicalNameModel>();
                c.CreateMap<DbViralName, ViralNameModel>();
                c.CreateMap<DbProcessContext, ProcessContextModel>();
                c.CreateMap<DbProcess, ProcessModel>();
                c.CreateMap<DbProcessName, ProcessNameModel>();
                c.CreateMap<DbProcessSubject, ProcessSubjectModel>();
                c.CreateMap<DbInteraction, InteractionModel>();
                c.CreateMap<DbInteractionPartner, InteractionPartnerModel>();
                c.CreateMap<DbInteractionDirection, InteractionDirectionModel>();
                c.CreateMap<DbInteractionQuality, InteractionQualityModel>();
                c.CreateMap<DbChemicalContext, ChemicalContextModel>();
                c.CreateMap<DbElement, ElementModel>();
                c.CreateMap<DbElementName, ElementNameModel>();
                c.CreateMap<DbIsotope, IsotopeModel>();
                c.CreateMap<DbIsotopeName, IsotopeNameModel>();
                c.CreateMap<DbCompound, CompoundModel>();
                c.CreateMap<DbCompoundClass, CompoundClassModel>();
                c.CreateMap<DbCompoundType, CompoundTypeModel>();
                c.CreateMap<DbFunction, FunctionModel>();
                c.CreateMap<DbChemicalFunctionType, ChemicalFunctionTypeModel>();
                c.CreateMap<DbMethodContext, MethodContextModel>();
                c.CreateMap<DbApproach, ApproachModel>();
                c.CreateMap<DbApproachType, ApproachTypeModel>();
                c.CreateMap<DbApproachLocalization, ApproachLocalizationModel>();
                c.CreateMap<DbFactor, FactorModel>();
                c.CreateMap<DbFactorName, FactorNameModel>();
                c.CreateMap<DbFactorType, FactorTypeModel>();
                c.CreateMap<DbDataFormat, DataFormatModel>();
                c.CreateMap<DbReferences, ReferencesModel>();
                c.CreateMap<DbHoster, HosterModel>();
                c.CreateMap<DbHosterName, HosterNameModel>();
                c.CreateMap<DbPerson, PersonModel>();
                c.CreateMap<DbPosition, PositionModel>();
                c.CreateMap<DbDescription, DescriptionModel>();
                c.CreateMap<DbResource, ResourceModel>();
                c.CreateMap<DbOnlineResource, OnlineResourceModel>();
                c.CreateMap<DbOfflineResource, OfflineResourceModel>();
                c.CreateMap<DbOfflineResource, OfflineResource>();
                c.CreateMap<DbEmbeddedResource, EmbeddedResourceModel>();

                c.CreateMap<DbBase64Resource, Base64ResourceModel>();
                c.CreateMap<Base64ResourceModel, DbBase64Resource>();

                c.CreateMap<DbClass, ClassModel>();
                c.CreateMap<ClassModel, DbClass>();
                c.CreateMap<DbOrder, OrderModel>();
                c.CreateMap<OrderModel, DbOrder>();
                c.CreateMap<DbFamily, FamilyModel>();
                c.CreateMap<FamilyModel, DbFamily>();

                //c.CreateMap<DbUri, UriModel>();
                //c.CreateMap<UriModel, DbUri>();

                c.CreateMap<DbAnnotationItem, ShortAnnotationItemModel>(MemberList.Destination);
                Services.Mapping.MapperConfig.Init(c);

                //Configure mapping for plugins
                foreach(var source in container.Resolve<IPluginsProvider>().Sources)
                {
                    source.RegisterMapConfigs(c);
                }
            });
        }

        private static SearchRequestFilterValue ConverToSearchFilterValue(SearchRequestFilterModel model)
        {
            //var tp = (FilterType) Enum.Parse(typeof (FilterType), model.FilterType);
            //switch (tp)
            //{
            //    case FilterType.Input:
            //        break;
            //    case FilterType.Flag:
            //        break;
            //    case FilterType.Select:
            //        break;
            //    case FilterType.InList:
            //        break;
            //    case FilterType.DateRange:
            //        break;
            //    case FilterType.DigitalRange:
            //        break;
            //}
            return new SearchRequestFilterValue() { Value = model != null ? model.Value : "" };
        }
    }
}