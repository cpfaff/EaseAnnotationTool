namespace CAFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbAccessibleResource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Kind = c.Int(nullable: false),
                        ResourceId = c.Guid(nullable: false),
                        Owner_Id = c.Guid(nullable: false),
                        DbAccessRequest_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUsers", t => t.Owner_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbAccessRequest", t => t.DbAccessRequest_Id)
                .Index(t => t.Owner_Id)
                .Index(t => t.DbAccessRequest_Id);
            
            CreateTable(
                "dbo.DbUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Email = c.String(nullable: false),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(nullable: false),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(precision: 7, storeType: "datetime2"),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false),
                        Name = c.String(),
                        Surname = c.String(),
                        PostalAddress = c.String(),
                        Salutation = c.String(),
                        IsActive = c.Boolean(nullable: false),
                        IsAccepted = c.Boolean(nullable: false),
                        AcceptanceDate = c.DateTime(precision: 7, storeType: "datetime2"),
                        PhotoUrl = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbUserFiles",
                c => new
                    {
                        FileId = c.Int(nullable: false, identity: true),
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        AccessMode = c.Int(nullable: false),
                        Owner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.FileId)
                .ForeignKey("dbo.DbUsers", t => t.Owner_Id)
                .Index(t => t.Owner_Id);
            
            CreateTable(
                "dbo.DbRoles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        IsGroup = c.Boolean(),
                        Discriminator = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbUserHiddenHelpers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        UserId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.DbAccessRequest",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestSubject = c.String(nullable: false),
                        RequestMessage = c.String(),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbConversation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HasRecieverUnreadMessages = c.Boolean(nullable: false),
                        Status = c.Int(nullable: false),
                        StatusReason = c.String(),
                        Receiver_Id = c.Guid(nullable: false),
                        Request_Id = c.Long(nullable: false),
                        Requester_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUsers", t => t.Receiver_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbAccessRequest", t => t.Request_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.Requester_Id, cascadeDelete: false)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Request_Id)
                .Index(t => t.Requester_Id);
            
            CreateTable(
                "dbo.DbMessage",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Text = c.String(nullable: false),
                        Conversation_Id = c.Long(nullable: false),
                        Receiver_Id = c.Guid(nullable: false),
                        Sender_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbConversation", t => t.Conversation_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.Receiver_Id, cascadeDelete: false)
                .ForeignKey("dbo.DbUsers", t => t.Sender_Id, cascadeDelete: false)
                .Index(t => t.Conversation_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.DbAnnotationItem",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AccessMode = c.Int(nullable: false),
                        OwnerId = c.String(),
                        OwnerName = c.String(),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Object_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAnnotationObject", t => t.Object_Id)
                .Index(t => t.Object_Id);
            
            CreateTable(
                "dbo.DbAnnotationObject",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        References_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbReferences", t => t.References_Id)
                .Index(t => t.References_Id);
            
            CreateTable(
                "dbo.DbAnnotationContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiomeContext_Id = c.Long(),
                        ChemicalContext_Id = c.Long(),
                        MethodContext_Id = c.Long(),
                        OrganismContext_Id = c.Long(),
                        ProcessContext_Id = c.Long(),
                        SpaceContext_Id = c.Long(),
                        SphereContext_Id = c.Long(),
                        TimeContext_Id = c.Long(),
                        DbAnnotationObject_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBiomeContext", t => t.BiomeContext_Id)
                .ForeignKey("dbo.DbChemicalContext", t => t.ChemicalContext_Id)
                .ForeignKey("dbo.DbMethodContext", t => t.MethodContext_Id)
                .ForeignKey("dbo.DbOrganismContext", t => t.OrganismContext_Id)
                .ForeignKey("dbo.DbProcessContext", t => t.ProcessContext_Id)
                .ForeignKey("dbo.DbSpaceContext", t => t.SpaceContext_Id)
                .ForeignKey("dbo.DbSphereContext", t => t.SphereContext_Id)
                .ForeignKey("dbo.DbTimeContext", t => t.TimeContext_Id)
                .ForeignKey("dbo.DbAnnotationObject", t => t.DbAnnotationObject_Id)
                .Index(t => t.BiomeContext_Id)
                .Index(t => t.ChemicalContext_Id)
                .Index(t => t.MethodContext_Id)
                .Index(t => t.OrganismContext_Id)
                .Index(t => t.ProcessContext_Id)
                .Index(t => t.SpaceContext_Id)
                .Index(t => t.SphereContext_Id)
                .Index(t => t.TimeContext_Id)
                .Index(t => t.DbAnnotationObject_Id);
            
            CreateTable(
                "dbo.DbBiomeContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLandUse",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LandUseForm_Id = c.Long(),
                        LandUseType_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbLandUseForm", t => t.LandUseForm_Id)
                .ForeignKey("dbo.DbLandUseType", t => t.LandUseType_Id)
                .ForeignKey("dbo.DbBiomeContext", t => t.DbBiomeContext_Id)
                .Index(t => t.LandUseForm_Id)
                .Index(t => t.LandUseType_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbLandUseForm",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLandUseType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOroBiome",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OroBiomeType_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbOroBiomeType", t => t.OroBiomeType_Id)
                .ForeignKey("dbo.DbBiomeContext", t => t.DbBiomeContext_Id)
                .Index(t => t.OroBiomeType_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbOroBiomeType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPedoBiome",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PedoBiomeType_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbPedoBiomeType", t => t.PedoBiomeType_Id)
                .ForeignKey("dbo.DbBiomeContext", t => t.DbBiomeContext_Id)
                .Index(t => t.PedoBiomeType_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbPedoBiomeType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPhysiognomy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBiomeContext", t => t.DbBiomeContext_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbPhysiognomyType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DbPhysiognomy_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbPhysiognomy", t => t.DbPhysiognomy_Id)
                .Index(t => t.DbPhysiognomy_Id);
            
            CreateTable(
                "dbo.DbAquaticPhysiognomy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HabitatCharacterizedAquaticPhysiognomy_Id = c.Long(),
                        PlantCharacterizedAquaticPhysiognomyType_Id = c.Long(),
                        DbPhysiognomyType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbHabitatCharacterizedAquaticPhysiognomy", t => t.HabitatCharacterizedAquaticPhysiognomy_Id)
                .ForeignKey("dbo.DbPlantCharacterizedAquaticPhysiognomyType", t => t.PlantCharacterizedAquaticPhysiognomyType_Id)
                .ForeignKey("dbo.DbPhysiognomyType", t => t.DbPhysiognomyType_Id)
                .Index(t => t.HabitatCharacterizedAquaticPhysiognomy_Id)
                .Index(t => t.PlantCharacterizedAquaticPhysiognomyType_Id)
                .Index(t => t.DbPhysiognomyType_Id);
            
            CreateTable(
                "dbo.DbHabitatCharacterizedAquaticPhysiognomy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPlantCharacterizedAquaticPhysiognomyType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSemiAquaticPhysiognomy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SemiAquaticPhysiognomyType_Id = c.Long(),
                        DbPhysiognomyType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSemiAquaticPhysiognomyType", t => t.SemiAquaticPhysiognomyType_Id)
                .ForeignKey("dbo.DbPhysiognomyType", t => t.DbPhysiognomyType_Id)
                .Index(t => t.SemiAquaticPhysiognomyType_Id)
                .Index(t => t.DbPhysiognomyType_Id);
            
            CreateTable(
                "dbo.DbSemiAquaticPhysiognomyType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbTerrestrialPhysiognomy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TerrestrialPhysiognomyType_Id = c.Long(),
                        DbPhysiognomyType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTerrestrialPhysiognomyType", t => t.TerrestrialPhysiognomyType_Id)
                .ForeignKey("dbo.DbPhysiognomyType", t => t.DbPhysiognomyType_Id)
                .Index(t => t.TerrestrialPhysiognomyType_Id)
                .Index(t => t.DbPhysiognomyType_Id);
            
            CreateTable(
                "dbo.DbTerrestrialPhysiognomyType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbZonoBiome",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiomeContinentalityType_Id = c.Long(),
                        BiomeHemisphere_Id = c.Long(),
                        BiomeHumidityType_Id = c.Long(),
                        BiomeLatitudinalZone_Id = c.Long(),
                        BiomeType_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBiomeContinentalityType", t => t.BiomeContinentalityType_Id)
                .ForeignKey("dbo.DbBiomeHemisphere", t => t.BiomeHemisphere_Id)
                .ForeignKey("dbo.DbBiomeHumidityType", t => t.BiomeHumidityType_Id)
                .ForeignKey("dbo.DbBiomeLatitudinalZone", t => t.BiomeLatitudinalZone_Id)
                .ForeignKey("dbo.DbBiomeType", t => t.BiomeType_Id)
                .ForeignKey("dbo.DbBiomeContext", t => t.DbBiomeContext_Id)
                .Index(t => t.BiomeContinentalityType_Id)
                .Index(t => t.BiomeHemisphere_Id)
                .Index(t => t.BiomeHumidityType_Id)
                .Index(t => t.BiomeLatitudinalZone_Id)
                .Index(t => t.BiomeType_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbBiomeContinentalityType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeHemisphere",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeHumidityType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeLatitudinalZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbChemicalContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbCompound",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompoundName = c.String(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbChemicalContext", t => t.DbChemicalContext_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbCompoundClass",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbCompound_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbCompound", t => t.DbCompound_Id)
                .Index(t => t.DbCompound_Id);
            
            CreateTable(
                "dbo.DbCompoundType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbCompound_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbCompound", t => t.DbCompound_Id)
                .Index(t => t.DbCompound_Id);
            
            CreateTable(
                "dbo.DbElement",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ElementName_Id = c.Long(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbElementName", t => t.ElementName_Id)
                .ForeignKey("dbo.DbChemicalContext", t => t.DbChemicalContext_Id)
                .Index(t => t.ElementName_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbElementName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFunction",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChemicalFunctionType_Id = c.Long(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbChemicalFunctionType", t => t.ChemicalFunctionType_Id)
                .ForeignKey("dbo.DbChemicalContext", t => t.DbChemicalContext_Id)
                .Index(t => t.ChemicalFunctionType_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbChemicalFunctionType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbIsotope",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsotopeName_Id = c.Long(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbIsotopeName", t => t.IsotopeName_Id)
                .ForeignKey("dbo.DbChemicalContext", t => t.DbChemicalContext_Id)
                .Index(t => t.IsotopeName_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbIsotopeName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbMethodContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbApproach",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ApproachLocalization_Id = c.Long(),
                        ApproachType_Id = c.Long(),
                        DbMethodContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbApproachLocalization", t => t.ApproachLocalization_Id)
                .ForeignKey("dbo.DbApproachType", t => t.ApproachType_Id)
                .ForeignKey("dbo.DbMethodContext", t => t.DbMethodContext_Id)
                .Index(t => t.ApproachLocalization_Id)
                .Index(t => t.ApproachType_Id)
                .Index(t => t.DbMethodContext_Id);
            
            CreateTable(
                "dbo.DbApproachLocalization",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbApproachType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFactor",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Was = c.String(),
                        FactorName_Id = c.Long(),
                        FactorType_Id = c.Long(),
                        DbMethodContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbFactorName", t => t.FactorName_Id)
                .ForeignKey("dbo.DbFactorType", t => t.FactorType_Id)
                .ForeignKey("dbo.DbMethodContext", t => t.DbMethodContext_Id)
                .Index(t => t.FactorName_Id)
                .Index(t => t.FactorType_Id)
                .Index(t => t.DbMethodContext_Id);
            
            CreateTable(
                "dbo.DbFactorName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFactorType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrganizationalHierarchy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrganizationHierarchyName_Id = c.Long(),
                        DbMethodContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbOrganizationHierarchyName", t => t.OrganizationHierarchyName_Id)
                .ForeignKey("dbo.DbMethodContext", t => t.DbMethodContext_Id)
                .Index(t => t.OrganizationHierarchyName_Id)
                .Index(t => t.DbMethodContext_Id);
            
            CreateTable(
                "dbo.DbOrganizationHierarchyName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrganismContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrganism",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Taxonomy_Id = c.Long(),
                        DbOrganismContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTaxonomy", t => t.Taxonomy_Id)
                .ForeignKey("dbo.DbOrganismContext", t => t.DbOrganismContext_Id)
                .Index(t => t.Taxonomy_Id)
                .Index(t => t.DbOrganismContext_Id);
            
            CreateTable(
                "dbo.DbTaxonomy",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Species = c.String(),
                        Class_Id = c.Long(),
                        Division_Id = c.Long(),
                        Domain_Id = c.Long(),
                        Family_Id = c.Long(),
                        Genus_Id = c.Long(),
                        Kingdom_Id = c.Long(),
                        Order_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbClass", t => t.Class_Id)
                .ForeignKey("dbo.DbDivision", t => t.Division_Id)
                .ForeignKey("dbo.DbDomain", t => t.Domain_Id)
                .ForeignKey("dbo.DbFamily", t => t.Family_Id)
                .ForeignKey("dbo.DbGenus", t => t.Genus_Id)
                .ForeignKey("dbo.DbKingdom", t => t.Kingdom_Id)
                .ForeignKey("dbo.DbOrder", t => t.Order_Id)
                .Index(t => t.Class_Id)
                .Index(t => t.Division_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.Family_Id)
                .Index(t => t.Genus_Id)
                .Index(t => t.Kingdom_Id)
                .Index(t => t.Order_Id);
            
            CreateTable(
                "dbo.DbClass",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbDivision",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbDomain",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFamily",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGenus",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbKingdom",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrder",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProcessContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbInteraction",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        InteractionName = c.String(),
                        InteractionDirection_Id = c.Long(),
                        InteractionPartnerOne_Id = c.Long(),
                        InteractionPartnerTwo_Id = c.Long(),
                        InteractionQuality_Id = c.Long(),
                        DbProcessContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbInteractionDirection", t => t.InteractionDirection_Id)
                .ForeignKey("dbo.DbInteractionPartner", t => t.InteractionPartnerOne_Id)
                .ForeignKey("dbo.DbInteractionPartner", t => t.InteractionPartnerTwo_Id)
                .ForeignKey("dbo.DbInteractionQuality", t => t.InteractionQuality_Id)
                .ForeignKey("dbo.DbProcessContext", t => t.DbProcessContext_Id)
                .Index(t => t.InteractionDirection_Id)
                .Index(t => t.InteractionPartnerOne_Id)
                .Index(t => t.InteractionPartnerTwo_Id)
                .Index(t => t.InteractionQuality_Id)
                .Index(t => t.DbProcessContext_Id);
            
            CreateTable(
                "dbo.DbInteractionDirection",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbInteractionPartner",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbInteractionQuality",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProcess",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProcessName_Id = c.Long(),
                        DbProcessContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbProcessName", t => t.ProcessName_Id)
                .ForeignKey("dbo.DbProcessContext", t => t.DbProcessContext_Id)
                .Index(t => t.ProcessName_Id)
                .Index(t => t.DbProcessContext_Id);
            
            CreateTable(
                "dbo.DbProcessName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProcessSubject",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbProcess_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbProcess", t => t.DbProcess_Id)
                .Index(t => t.DbProcess_Id);
            
            CreateTable(
                "dbo.DbSpaceContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBoundingBox",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        NorthBoundingCoordinate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SouthBoundingCoordinate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        EastBoundingCoordinate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        WestBoundingCoordinate = c.Decimal(nullable: false, precision: 18, scale: 2),
                        DbSpaceContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSpaceContext", t => t.DbSpaceContext_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbCoordinate",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UtmCoordinate_Id = c.Long(),
                        DbSpaceContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUtmCoordinate", t => t.UtmCoordinate_Id)
                .ForeignKey("dbo.DbSpaceContext", t => t.DbSpaceContext_Id)
                .Index(t => t.UtmCoordinate_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbUtmCoordinate",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UtmCoordinateZone = c.Int(nullable: false),
                        UtmCoordinateSubZone = c.String(),
                        UtmCoordinateEasting = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UtmCoordinateNorthing = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UtmCoordinateEastingUnit_Id = c.Long(),
                        UtmCoordinateGeodeticDatum_Id = c.Long(),
                        UtmCoordinateHemisphere_Id = c.Long(),
                        UtmCoordinateNorthingUnit_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUtmCoordinateUnit", t => t.UtmCoordinateEastingUnit_Id)
                .ForeignKey("dbo.DbUtmCoordinateGeodeticDatum", t => t.UtmCoordinateGeodeticDatum_Id)
                .ForeignKey("dbo.DbUtmCoordinateHemisphere", t => t.UtmCoordinateHemisphere_Id)
                .ForeignKey("dbo.DbUtmCoordinateUnit", t => t.UtmCoordinateNorthingUnit_Id)
                .Index(t => t.UtmCoordinateEastingUnit_Id)
                .Index(t => t.UtmCoordinateGeodeticDatum_Id)
                .Index(t => t.UtmCoordinateHemisphere_Id)
                .Index(t => t.UtmCoordinateNorthingUnit_Id);
            
            CreateTable(
                "dbo.DbUtmCoordinateUnit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbUtmCoordinateGeodeticDatum",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbUtmCoordinateHemisphere",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbElevation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MaximumElevation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MinimumElevation = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ElevationDatum_Id = c.Long(),
                        MaximumElevationUnit_Id = c.Long(),
                        MinimumElevationUnit_Id = c.Long(),
                        DbSpaceContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbElevationDatum", t => t.ElevationDatum_Id)
                .ForeignKey("dbo.DbElevationUnit", t => t.MaximumElevationUnit_Id)
                .ForeignKey("dbo.DbElevationUnit", t => t.MinimumElevationUnit_Id)
                .ForeignKey("dbo.DbSpaceContext", t => t.DbSpaceContext_Id)
                .Index(t => t.ElevationDatum_Id)
                .Index(t => t.MaximumElevationUnit_Id)
                .Index(t => t.MinimumElevationUnit_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbElevationDatum",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbElevationUnit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLocation",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LocationName = c.String(),
                        ContinentName_Id = c.Long(),
                        CountryName_Id = c.Long(),
                        LocationType_Id = c.Long(),
                        DbSpaceContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbContinentName", t => t.ContinentName_Id)
                .ForeignKey("dbo.DbCountryName", t => t.CountryName_Id)
                .ForeignKey("dbo.DbLocationType", t => t.LocationType_Id)
                .ForeignKey("dbo.DbSpaceContext", t => t.DbSpaceContext_Id)
                .Index(t => t.ContinentName_Id)
                .Index(t => t.CountryName_Id)
                .Index(t => t.LocationType_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbContinentName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbCountryName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLocationType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSpatialResolution",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SpatialExtentType_Id = c.Long(),
                        SpatialResolutionType_Id = c.Long(),
                        DbSpaceContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSpatialResolutionType", t => t.SpatialExtentType_Id)
                .ForeignKey("dbo.DbSpatialResolutionType", t => t.SpatialResolutionType_Id)
                .ForeignKey("dbo.DbSpaceContext", t => t.DbSpaceContext_Id)
                .Index(t => t.SpatialExtentType_Id)
                .Index(t => t.SpatialResolutionType_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbSpatialResolutionType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSphereContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSphere",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Atmosphere_Id = c.Long(),
                        Ecosphere_Id = c.Long(),
                        Hydrosphere_Id = c.Long(),
                        Pedosphere_Id = c.Long(),
                        DbSphereContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAtmosphere", t => t.Atmosphere_Id)
                .ForeignKey("dbo.DbEcosphere", t => t.Ecosphere_Id)
                .ForeignKey("dbo.DbHydrosphere", t => t.Hydrosphere_Id)
                .ForeignKey("dbo.DbPedosphere", t => t.Pedosphere_Id)
                .ForeignKey("dbo.DbSphereContext", t => t.DbSphereContext_Id)
                .Index(t => t.Atmosphere_Id)
                .Index(t => t.Ecosphere_Id)
                .Index(t => t.Hydrosphere_Id)
                .Index(t => t.Pedosphere_Id)
                .Index(t => t.DbSphereContext_Id);
            
            CreateTable(
                "dbo.DbAtmosphere",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedAtmosphereLayer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AtmosphereLayerName_Id = c.Long(),
                        DbAtmosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAtmosphereLayerName", t => t.AtmosphereLayerName_Id)
                .ForeignKey("dbo.DbAtmosphere", t => t.DbAtmosphere_Id)
                .Index(t => t.AtmosphereLayerName_Id)
                .Index(t => t.DbAtmosphere_Id);
            
            CreateTable(
                "dbo.DbAtmosphereLayerName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNumericAtmosphereLayer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MinimumAtmosphereHeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumAtmosphereHeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumAtmosphereHeightUnit_Id = c.Long(),
                        MinimumAtmosphereHeightUnit_Id = c.Long(),
                        DbAtmosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAtmosphereHeightUnit", t => t.MaximumAtmosphereHeightUnit_Id)
                .ForeignKey("dbo.DbAtmosphereHeightUnit", t => t.MinimumAtmosphereHeightUnit_Id)
                .ForeignKey("dbo.DbAtmosphere", t => t.DbAtmosphere_Id)
                .Index(t => t.MaximumAtmosphereHeightUnit_Id)
                .Index(t => t.MinimumAtmosphereHeightUnit_Id)
                .Index(t => t.DbAtmosphere_Id);
            
            CreateTable(
                "dbo.DbAtmosphereHeightUnit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbEcosphere",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedEcosphereLayer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EcosphereLayerName_Id = c.Long(),
                        DbEcosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbEcosphereLayerName", t => t.EcosphereLayerName_Id)
                .ForeignKey("dbo.DbEcosphere", t => t.DbEcosphere_Id)
                .Index(t => t.EcosphereLayerName_Id)
                .Index(t => t.DbEcosphere_Id);
            
            CreateTable(
                "dbo.DbEcosphereLayerName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNumericEcosphereLayer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MinimumVegetationHeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumVegetationHeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumVegetationHeightUnit_Id = c.Long(),
                        MinimumVegetationHeightUnit_Id = c.Long(),
                        DbEcosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbVegetationHeightUnit", t => t.MaximumVegetationHeightUnit_Id)
                .ForeignKey("dbo.DbVegetationHeightUnit", t => t.MinimumVegetationHeightUnit_Id)
                .ForeignKey("dbo.DbEcosphere", t => t.DbEcosphere_Id)
                .Index(t => t.MaximumVegetationHeightUnit_Id)
                .Index(t => t.MinimumVegetationHeightUnit_Id)
                .Index(t => t.DbEcosphere_Id);
            
            CreateTable(
                "dbo.DbVegetationHeightUnit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbHydrosphere",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbHydrosphereCompartment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Lake_Id = c.Long(),
                        River_Id = c.Long(),
                        Sea_Id = c.Long(),
                        DbHydrosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbLake", t => t.Lake_Id)
                .ForeignKey("dbo.DbRiver", t => t.River_Id)
                .ForeignKey("dbo.DbSea", t => t.Sea_Id)
                .ForeignKey("dbo.DbHydrosphere", t => t.DbHydrosphere_Id)
                .Index(t => t.Lake_Id)
                .Index(t => t.River_Id)
                .Index(t => t.Sea_Id)
                .Index(t => t.DbHydrosphere_Id);
            
            CreateTable(
                "dbo.DbLake",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedLakeZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BenthicLakeZone_Id = c.Long(),
                        PelagicLakeZone_Id = c.Long(),
                        DbLake_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBenthicLakeZone", t => t.BenthicLakeZone_Id)
                .ForeignKey("dbo.DbPelagicLakeZone", t => t.PelagicLakeZone_Id)
                .ForeignKey("dbo.DbLake", t => t.DbLake_Id)
                .Index(t => t.BenthicLakeZone_Id)
                .Index(t => t.PelagicLakeZone_Id)
                .Index(t => t.DbLake_Id);
            
            CreateTable(
                "dbo.DbBenthicLakeZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPelagicLakeZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbRiver",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedRiverZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LongitudinalRiverZone_Id = c.Long(),
                        VerticalRiverZone_Id = c.Long(),
                        DbRiver_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbLongitudinalRiverZone", t => t.LongitudinalRiverZone_Id)
                .ForeignKey("dbo.DbVerticalRiverZone", t => t.VerticalRiverZone_Id)
                .ForeignKey("dbo.DbRiver", t => t.DbRiver_Id)
                .Index(t => t.LongitudinalRiverZone_Id)
                .Index(t => t.VerticalRiverZone_Id)
                .Index(t => t.DbRiver_Id);
            
            CreateTable(
                "dbo.DbLongitudinalRiverZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbVerticalRiverZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSea",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedSeaZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BenthicSeaZone_Id = c.Long(),
                        PelagicSeaZone_Id = c.Long(),
                        DbSea_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBenthicSeaZone", t => t.BenthicSeaZone_Id)
                .ForeignKey("dbo.DbPelagicSeaZone", t => t.PelagicSeaZone_Id)
                .ForeignKey("dbo.DbSea", t => t.DbSea_Id)
                .Index(t => t.BenthicSeaZone_Id)
                .Index(t => t.PelagicSeaZone_Id)
                .Index(t => t.DbSea_Id);
            
            CreateTable(
                "dbo.DbBenthicSeaZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPelagicSeaZone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPedosphere",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPedosphereCompartment",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Soil_Id = c.Long(),
                        DbPedosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoil", t => t.Soil_Id)
                .ForeignKey("dbo.DbPedosphere", t => t.DbPedosphere_Id)
                .Index(t => t.Soil_Id)
                .Index(t => t.DbPedosphere_Id);
            
            CreateTable(
                "dbo.DbSoil",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedSoilLayer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SoilHorizon_Id = c.Long(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoilHorizon", t => t.SoilHorizon_Id)
                .ForeignKey("dbo.DbSoil", t => t.DbSoil_Id)
                .Index(t => t.SoilHorizon_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilHorizon",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNumericSoilLayer",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        MinimumSoilDepth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumSoilDepth = c.Decimal(nullable: false, precision: 18, scale: 2),
                        MaximumSoilDepthUnit_Id = c.Long(),
                        MinimumSoilDepthUnit_Id = c.Long(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoilDepthUnit", t => t.MaximumSoilDepthUnit_Id)
                .ForeignKey("dbo.DbSoilDepthUnit", t => t.MinimumSoilDepthUnit_Id)
                .ForeignKey("dbo.DbSoil", t => t.DbSoil_Id)
                .Index(t => t.MaximumSoilDepthUnit_Id)
                .Index(t => t.MinimumSoilDepthUnit_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilDepthUnit",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSoilAcidity",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SoilAcidityType_Id = c.Long(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoilAcidityType", t => t.SoilAcidityType_Id)
                .ForeignKey("dbo.DbSoil", t => t.DbSoil_Id)
                .Index(t => t.SoilAcidityType_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilAcidityType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSoilMorphology",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SoilMorphologyType_Id = c.Long(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoilMorphologyType", t => t.SoilMorphologyType_Id)
                .ForeignKey("dbo.DbSoil", t => t.DbSoil_Id)
                .Index(t => t.SoilMorphologyType_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilMorphologyType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSoilTexture",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SandPercent = c.Int(nullable: false),
                        SiltPercent = c.Int(nullable: false),
                        LoamPercent = c.Int(nullable: false),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoil", t => t.DbSoil_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbTimeContext",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TimePeriods_Id = c.Long(),
                        TimeRanges_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTimePeriods", t => t.TimePeriods_Id)
                .ForeignKey("dbo.DbTimeRanges", t => t.TimeRanges_Id)
                .Index(t => t.TimePeriods_Id)
                .Index(t => t.TimeRanges_Id);
            
            CreateTable(
                "dbo.DbTemporalResolution",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FactorPeriodId = c.Int(nullable: false),
                        TemporalExtentType_Id = c.Long(),
                        TemporalResolutionType_Id = c.Long(),
                        DbTimeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTemporalExtentType", t => t.TemporalExtentType_Id)
                .ForeignKey("dbo.DbTemporalResolutionType", t => t.TemporalResolutionType_Id)
                .ForeignKey("dbo.DbTimeContext", t => t.DbTimeContext_Id)
                .Index(t => t.TemporalExtentType_Id)
                .Index(t => t.TemporalResolutionType_Id)
                .Index(t => t.DbTimeContext_Id);
            
            CreateTable(
                "dbo.DbTemporalExtentType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        Modifyer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbTemporalResolutionType",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        Modifyer = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbTimePeriods",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalTimePeriod",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GeologicalAge_Id = c.Long(),
                        GeologicalEon_Id = c.Long(),
                        GeologicalEpoch_Id = c.Long(),
                        GeologicalEra_Id = c.Long(),
                        GeologicalPeriod_Id = c.Long(),
                        DbTimePeriods_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbGeologicalAge", t => t.GeologicalAge_Id)
                .ForeignKey("dbo.DbGeologicalEon", t => t.GeologicalEon_Id)
                .ForeignKey("dbo.DbGeologicalEpoch", t => t.GeologicalEpoch_Id)
                .ForeignKey("dbo.DbGeologicalEra", t => t.GeologicalEra_Id)
                .ForeignKey("dbo.DbGeologicalPeriod", t => t.GeologicalPeriod_Id)
                .ForeignKey("dbo.DbTimePeriods", t => t.DbTimePeriods_Id)
                .Index(t => t.GeologicalAge_Id)
                .Index(t => t.GeologicalEon_Id)
                .Index(t => t.GeologicalEpoch_Id)
                .Index(t => t.GeologicalEra_Id)
                .Index(t => t.GeologicalPeriod_Id)
                .Index(t => t.DbTimePeriods_Id);
            
            CreateTable(
                "dbo.DbGeologicalAge",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalEon",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalEpoch",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalEra",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalPeriod",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbTimeRanges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TimeRange_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTimeRange", t => t.TimeRange_Id)
                .Index(t => t.TimeRange_Id);
            
            CreateTable(
                "dbo.DbTimeRange",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RangeEnd_Id = c.Long(),
                        RangeStart_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbRangeEnd", t => t.RangeEnd_Id)
                .ForeignKey("dbo.DbRangeStart", t => t.RangeStart_Id)
                .Index(t => t.RangeEnd_Id)
                .Index(t => t.RangeStart_Id);
            
            CreateTable(
                "dbo.DbRangeEnd",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateTime_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDateAndTime", t => t.DateTime_Id)
                .Index(t => t.DateTime_Id);
            
            CreateTable(
                "dbo.DbDateAndTime",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Time = c.String(),
                        Timezone_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTimezone", t => t.Timezone_Id)
                .Index(t => t.Timezone_Id);
            
            CreateTable(
                "dbo.DbTimezone",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbRangeStart",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateTime_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDateAndTime", t => t.DateTime_Id)
                .Index(t => t.DateTime_Id);
            
            CreateTable(
                "dbo.DbReferences",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbDescription",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Abstract = c.String(),
                        DbReferences_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbReferences", t => t.DbReferences_Id)
                .Index(t => t.DbReferences_Id);
            
            CreateTable(
                "dbo.DbHoster",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HosterName_Id = c.Long(),
                        DbReferences_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbHosterName", t => t.HosterName_Id)
                .ForeignKey("dbo.DbReferences", t => t.DbReferences_Id)
                .Index(t => t.HosterName_Id)
                .Index(t => t.DbReferences_Id);
            
            CreateTable(
                "dbo.DbHosterName",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPerson",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DbReferences_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbReferences", t => t.DbReferences_Id)
                .Index(t => t.DbReferences_Id);
            
            CreateTable(
                "dbo.DbPosition",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbPerson_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbPerson", t => t.DbPerson_Id)
                .Index(t => t.DbPerson_Id);
            
            CreateTable(
                "dbo.DbResource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DbAnnotationObject_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAnnotationObject", t => t.DbAnnotationObject_Id)
                .Index(t => t.DbAnnotationObject_Id);
            
            CreateTable(
                "dbo.DbEmbeddedResource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Base64Resource_Id = c.Long(),
                        DbResource_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBase64Resource", t => t.Base64Resource_Id)
                .ForeignKey("dbo.DbResource", t => t.DbResource_Id)
                .Index(t => t.Base64Resource_Id)
                .Index(t => t.DbResource_Id);
            
            CreateTable(
                "dbo.DbBase64Resource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileName = c.String(),
                        MimeType = c.String(),
                        Data = c.Binary(),
                        EncodingDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        EncodingTime = c.String(),
                        Sha2Hash = c.String(),
                        DataFormat_Value = c.String(),
                        DataFormat_Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOfflineResource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileID = c.Int(nullable: false),
                        FileName = c.String(),
                        FilePath = c.String(),
                        MimeType = c.String(),
                        DataFormat_Value = c.String(),
                        DataFormat_Uri = c.String(),
                        DbResource_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbResource", t => t.DbResource_Id)
                .Index(t => t.DbResource_Id);
            
            CreateTable(
                "dbo.DbOnlineResource",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DownloadUrl = c.String(),
                        ReferenceUrl = c.String(),
                        DataFormat_Id = c.Long(),
                        DbResource_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDataFormat", t => t.DataFormat_Id)
                .ForeignKey("dbo.DbResource", t => t.DbResource_Id)
                .Index(t => t.DataFormat_Id)
                .Index(t => t.DbResource_Id);
            
            CreateTable(
                "dbo.DbDataFormat",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbAnnotationItemAccessibleGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AnnotationItem_Id = c.Guid(),
                        Group_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAnnotationItem", t => t.AnnotationItem_Id)
                .ForeignKey("dbo.DbRoles", t => t.Group_Id)
                .Index(t => t.AnnotationItem_Id)
                .Index(t => t.Group_Id);
            
            CreateTable(
                "dbo.DbAnnotationItemAccessibleUsers",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AnnotationItem_Id = c.Guid(),
                        User_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAnnotationItem", t => t.AnnotationItem_Id)
                .ForeignKey("dbo.DbUsers", t => t.User_Id)
                .Index(t => t.AnnotationItem_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.DbRemoteVersion",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.String(),
                        IsNew = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSchemaItemDescription",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Path = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSearchFilterCachedItems",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 500),
                        Description = c.String(),
                        FilterType = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.DbUserLogins",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        UserId = c.Guid(nullable: false),
                        Time = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbVocabularyUserValue",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Value = c.String(nullable: false),
                        Description = c.String(),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        User_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUsers", t => t.User_Id, cascadeDelete: true)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.DbVocabularyValue",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Value = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbRoleDbUserFile",
                c => new
                    {
                        DbRole_Id = c.Guid(nullable: false),
                        DbUserFile_FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbRole_Id, t.DbUserFile_FileId })
                .ForeignKey("dbo.DbRoles", t => t.DbRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbUserFiles", t => t.DbUserFile_FileId, cascadeDelete: true)
                .Index(t => t.DbRole_Id)
                .Index(t => t.DbUserFile_FileId);
            
            CreateTable(
                "dbo.DbRoleDbUser",
                c => new
                    {
                        DbRole_Id = c.Guid(nullable: false),
                        DbUser_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbRole_Id, t.DbUser_Id })
                .ForeignKey("dbo.DbRoles", t => t.DbRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbUsers", t => t.DbUser_Id, cascadeDelete: true)
                .Index(t => t.DbRole_Id)
                .Index(t => t.DbUser_Id);
            
            CreateTable(
                "dbo.DbUserDbUserFile",
                c => new
                    {
                        DbUser_Id = c.Guid(nullable: false),
                        DbUserFile_FileId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbUser_Id, t.DbUserFile_FileId })
                .ForeignKey("dbo.DbUsers", t => t.DbUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbUserFiles", t => t.DbUserFile_FileId, cascadeDelete: true)
                .Index(t => t.DbUser_Id)
                .Index(t => t.DbUserFile_FileId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DbVocabularyUserValue", "User_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbAnnotationItemAccessibleUsers", "User_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbAnnotationItemAccessibleUsers", "AnnotationItem_Id", "dbo.DbAnnotationItem");
            DropForeignKey("dbo.DbAnnotationItemAccessibleGroups", "Group_Id", "dbo.DbRoles");
            DropForeignKey("dbo.DbAnnotationItemAccessibleGroups", "AnnotationItem_Id", "dbo.DbAnnotationItem");
            DropForeignKey("dbo.DbAnnotationItem", "Object_Id", "dbo.DbAnnotationObject");
            DropForeignKey("dbo.DbResource", "DbAnnotationObject_Id", "dbo.DbAnnotationObject");
            DropForeignKey("dbo.DbOnlineResource", "DbResource_Id", "dbo.DbResource");
            DropForeignKey("dbo.DbOnlineResource", "DataFormat_Id", "dbo.DbDataFormat");
            DropForeignKey("dbo.DbOfflineResource", "DbResource_Id", "dbo.DbResource");
            DropForeignKey("dbo.DbEmbeddedResource", "DbResource_Id", "dbo.DbResource");
            DropForeignKey("dbo.DbEmbeddedResource", "Base64Resource_Id", "dbo.DbBase64Resource");
            DropForeignKey("dbo.DbAnnotationObject", "References_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbPerson", "DbReferences_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbPosition", "DbPerson_Id", "dbo.DbPerson");
            DropForeignKey("dbo.DbHoster", "DbReferences_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbHoster", "HosterName_Id", "dbo.DbHosterName");
            DropForeignKey("dbo.DbDescription", "DbReferences_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbAnnotationContext", "DbAnnotationObject_Id", "dbo.DbAnnotationObject");
            DropForeignKey("dbo.DbAnnotationContext", "TimeContext_Id", "dbo.DbTimeContext");
            DropForeignKey("dbo.DbTimeContext", "TimeRanges_Id", "dbo.DbTimeRanges");
            DropForeignKey("dbo.DbTimeRanges", "TimeRange_Id", "dbo.DbTimeRange");
            DropForeignKey("dbo.DbTimeRange", "RangeStart_Id", "dbo.DbRangeStart");
            DropForeignKey("dbo.DbRangeStart", "DateTime_Id", "dbo.DbDateAndTime");
            DropForeignKey("dbo.DbTimeRange", "RangeEnd_Id", "dbo.DbRangeEnd");
            DropForeignKey("dbo.DbRangeEnd", "DateTime_Id", "dbo.DbDateAndTime");
            DropForeignKey("dbo.DbDateAndTime", "Timezone_Id", "dbo.DbTimezone");
            DropForeignKey("dbo.DbTimeContext", "TimePeriods_Id", "dbo.DbTimePeriods");
            DropForeignKey("dbo.DbGeologicalTimePeriod", "DbTimePeriods_Id", "dbo.DbTimePeriods");
            DropForeignKey("dbo.DbGeologicalTimePeriod", "GeologicalPeriod_Id", "dbo.DbGeologicalPeriod");
            DropForeignKey("dbo.DbGeologicalTimePeriod", "GeologicalEra_Id", "dbo.DbGeologicalEra");
            DropForeignKey("dbo.DbGeologicalTimePeriod", "GeologicalEpoch_Id", "dbo.DbGeologicalEpoch");
            DropForeignKey("dbo.DbGeologicalTimePeriod", "GeologicalEon_Id", "dbo.DbGeologicalEon");
            DropForeignKey("dbo.DbGeologicalTimePeriod", "GeologicalAge_Id", "dbo.DbGeologicalAge");
            DropForeignKey("dbo.DbTemporalResolution", "DbTimeContext_Id", "dbo.DbTimeContext");
            DropForeignKey("dbo.DbTemporalResolution", "TemporalResolutionType_Id", "dbo.DbTemporalResolutionType");
            DropForeignKey("dbo.DbTemporalResolution", "TemporalExtentType_Id", "dbo.DbTemporalExtentType");
            DropForeignKey("dbo.DbAnnotationContext", "SphereContext_Id", "dbo.DbSphereContext");
            DropForeignKey("dbo.DbSphere", "DbSphereContext_Id", "dbo.DbSphereContext");
            DropForeignKey("dbo.DbSphere", "Pedosphere_Id", "dbo.DbPedosphere");
            DropForeignKey("dbo.DbPedosphereCompartment", "DbPedosphere_Id", "dbo.DbPedosphere");
            DropForeignKey("dbo.DbPedosphereCompartment", "Soil_Id", "dbo.DbSoil");
            DropForeignKey("dbo.DbSoilTexture", "DbSoil_Id", "dbo.DbSoil");
            DropForeignKey("dbo.DbSoilMorphology", "DbSoil_Id", "dbo.DbSoil");
            DropForeignKey("dbo.DbSoilMorphology", "SoilMorphologyType_Id", "dbo.DbSoilMorphologyType");
            DropForeignKey("dbo.DbSoilAcidity", "DbSoil_Id", "dbo.DbSoil");
            DropForeignKey("dbo.DbSoilAcidity", "SoilAcidityType_Id", "dbo.DbSoilAcidityType");
            DropForeignKey("dbo.DbNumericSoilLayer", "DbSoil_Id", "dbo.DbSoil");
            DropForeignKey("dbo.DbNumericSoilLayer", "MinimumSoilDepthUnit_Id", "dbo.DbSoilDepthUnit");
            DropForeignKey("dbo.DbNumericSoilLayer", "MaximumSoilDepthUnit_Id", "dbo.DbSoilDepthUnit");
            DropForeignKey("dbo.DbNamedSoilLayer", "DbSoil_Id", "dbo.DbSoil");
            DropForeignKey("dbo.DbNamedSoilLayer", "SoilHorizon_Id", "dbo.DbSoilHorizon");
            DropForeignKey("dbo.DbSphere", "Hydrosphere_Id", "dbo.DbHydrosphere");
            DropForeignKey("dbo.DbHydrosphereCompartment", "DbHydrosphere_Id", "dbo.DbHydrosphere");
            DropForeignKey("dbo.DbHydrosphereCompartment", "Sea_Id", "dbo.DbSea");
            DropForeignKey("dbo.DbNamedSeaZone", "DbSea_Id", "dbo.DbSea");
            DropForeignKey("dbo.DbNamedSeaZone", "PelagicSeaZone_Id", "dbo.DbPelagicSeaZone");
            DropForeignKey("dbo.DbNamedSeaZone", "BenthicSeaZone_Id", "dbo.DbBenthicSeaZone");
            DropForeignKey("dbo.DbHydrosphereCompartment", "River_Id", "dbo.DbRiver");
            DropForeignKey("dbo.DbNamedRiverZone", "DbRiver_Id", "dbo.DbRiver");
            DropForeignKey("dbo.DbNamedRiverZone", "VerticalRiverZone_Id", "dbo.DbVerticalRiverZone");
            DropForeignKey("dbo.DbNamedRiverZone", "LongitudinalRiverZone_Id", "dbo.DbLongitudinalRiverZone");
            DropForeignKey("dbo.DbHydrosphereCompartment", "Lake_Id", "dbo.DbLake");
            DropForeignKey("dbo.DbNamedLakeZone", "DbLake_Id", "dbo.DbLake");
            DropForeignKey("dbo.DbNamedLakeZone", "PelagicLakeZone_Id", "dbo.DbPelagicLakeZone");
            DropForeignKey("dbo.DbNamedLakeZone", "BenthicLakeZone_Id", "dbo.DbBenthicLakeZone");
            DropForeignKey("dbo.DbSphere", "Ecosphere_Id", "dbo.DbEcosphere");
            DropForeignKey("dbo.DbNumericEcosphereLayer", "DbEcosphere_Id", "dbo.DbEcosphere");
            DropForeignKey("dbo.DbNumericEcosphereLayer", "MinimumVegetationHeightUnit_Id", "dbo.DbVegetationHeightUnit");
            DropForeignKey("dbo.DbNumericEcosphereLayer", "MaximumVegetationHeightUnit_Id", "dbo.DbVegetationHeightUnit");
            DropForeignKey("dbo.DbNamedEcosphereLayer", "DbEcosphere_Id", "dbo.DbEcosphere");
            DropForeignKey("dbo.DbNamedEcosphereLayer", "EcosphereLayerName_Id", "dbo.DbEcosphereLayerName");
            DropForeignKey("dbo.DbSphere", "Atmosphere_Id", "dbo.DbAtmosphere");
            DropForeignKey("dbo.DbNumericAtmosphereLayer", "DbAtmosphere_Id", "dbo.DbAtmosphere");
            DropForeignKey("dbo.DbNumericAtmosphereLayer", "MinimumAtmosphereHeightUnit_Id", "dbo.DbAtmosphereHeightUnit");
            DropForeignKey("dbo.DbNumericAtmosphereLayer", "MaximumAtmosphereHeightUnit_Id", "dbo.DbAtmosphereHeightUnit");
            DropForeignKey("dbo.DbNamedAtmosphereLayer", "DbAtmosphere_Id", "dbo.DbAtmosphere");
            DropForeignKey("dbo.DbNamedAtmosphereLayer", "AtmosphereLayerName_Id", "dbo.DbAtmosphereLayerName");
            DropForeignKey("dbo.DbAnnotationContext", "SpaceContext_Id", "dbo.DbSpaceContext");
            DropForeignKey("dbo.DbSpatialResolution", "DbSpaceContext_Id", "dbo.DbSpaceContext");
            DropForeignKey("dbo.DbSpatialResolution", "SpatialResolutionType_Id", "dbo.DbSpatialResolutionType");
            DropForeignKey("dbo.DbSpatialResolution", "SpatialExtentType_Id", "dbo.DbSpatialResolutionType");
            DropForeignKey("dbo.DbLocation", "DbSpaceContext_Id", "dbo.DbSpaceContext");
            DropForeignKey("dbo.DbLocation", "LocationType_Id", "dbo.DbLocationType");
            DropForeignKey("dbo.DbLocation", "CountryName_Id", "dbo.DbCountryName");
            DropForeignKey("dbo.DbLocation", "ContinentName_Id", "dbo.DbContinentName");
            DropForeignKey("dbo.DbElevation", "DbSpaceContext_Id", "dbo.DbSpaceContext");
            DropForeignKey("dbo.DbElevation", "MinimumElevationUnit_Id", "dbo.DbElevationUnit");
            DropForeignKey("dbo.DbElevation", "MaximumElevationUnit_Id", "dbo.DbElevationUnit");
            DropForeignKey("dbo.DbElevation", "ElevationDatum_Id", "dbo.DbElevationDatum");
            DropForeignKey("dbo.DbCoordinate", "DbSpaceContext_Id", "dbo.DbSpaceContext");
            DropForeignKey("dbo.DbCoordinate", "UtmCoordinate_Id", "dbo.DbUtmCoordinate");
            DropForeignKey("dbo.DbUtmCoordinate", "UtmCoordinateNorthingUnit_Id", "dbo.DbUtmCoordinateUnit");
            DropForeignKey("dbo.DbUtmCoordinate", "UtmCoordinateHemisphere_Id", "dbo.DbUtmCoordinateHemisphere");
            DropForeignKey("dbo.DbUtmCoordinate", "UtmCoordinateGeodeticDatum_Id", "dbo.DbUtmCoordinateGeodeticDatum");
            DropForeignKey("dbo.DbUtmCoordinate", "UtmCoordinateEastingUnit_Id", "dbo.DbUtmCoordinateUnit");
            DropForeignKey("dbo.DbBoundingBox", "DbSpaceContext_Id", "dbo.DbSpaceContext");
            DropForeignKey("dbo.DbAnnotationContext", "ProcessContext_Id", "dbo.DbProcessContext");
            DropForeignKey("dbo.DbProcess", "DbProcessContext_Id", "dbo.DbProcessContext");
            DropForeignKey("dbo.DbProcessSubject", "DbProcess_Id", "dbo.DbProcess");
            DropForeignKey("dbo.DbProcess", "ProcessName_Id", "dbo.DbProcessName");
            DropForeignKey("dbo.DbInteraction", "DbProcessContext_Id", "dbo.DbProcessContext");
            DropForeignKey("dbo.DbInteraction", "InteractionQuality_Id", "dbo.DbInteractionQuality");
            DropForeignKey("dbo.DbInteraction", "InteractionPartnerTwo_Id", "dbo.DbInteractionPartner");
            DropForeignKey("dbo.DbInteraction", "InteractionPartnerOne_Id", "dbo.DbInteractionPartner");
            DropForeignKey("dbo.DbInteraction", "InteractionDirection_Id", "dbo.DbInteractionDirection");
            DropForeignKey("dbo.DbAnnotationContext", "OrganismContext_Id", "dbo.DbOrganismContext");
            DropForeignKey("dbo.DbOrganism", "DbOrganismContext_Id", "dbo.DbOrganismContext");
            DropForeignKey("dbo.DbOrganism", "Taxonomy_Id", "dbo.DbTaxonomy");
            DropForeignKey("dbo.DbTaxonomy", "Order_Id", "dbo.DbOrder");
            DropForeignKey("dbo.DbTaxonomy", "Kingdom_Id", "dbo.DbKingdom");
            DropForeignKey("dbo.DbTaxonomy", "Genus_Id", "dbo.DbGenus");
            DropForeignKey("dbo.DbTaxonomy", "Family_Id", "dbo.DbFamily");
            DropForeignKey("dbo.DbTaxonomy", "Domain_Id", "dbo.DbDomain");
            DropForeignKey("dbo.DbTaxonomy", "Division_Id", "dbo.DbDivision");
            DropForeignKey("dbo.DbTaxonomy", "Class_Id", "dbo.DbClass");
            DropForeignKey("dbo.DbAnnotationContext", "MethodContext_Id", "dbo.DbMethodContext");
            DropForeignKey("dbo.DbOrganizationalHierarchy", "DbMethodContext_Id", "dbo.DbMethodContext");
            DropForeignKey("dbo.DbOrganizationalHierarchy", "OrganizationHierarchyName_Id", "dbo.DbOrganizationHierarchyName");
            DropForeignKey("dbo.DbFactor", "DbMethodContext_Id", "dbo.DbMethodContext");
            DropForeignKey("dbo.DbFactor", "FactorType_Id", "dbo.DbFactorType");
            DropForeignKey("dbo.DbFactor", "FactorName_Id", "dbo.DbFactorName");
            DropForeignKey("dbo.DbApproach", "DbMethodContext_Id", "dbo.DbMethodContext");
            DropForeignKey("dbo.DbApproach", "ApproachType_Id", "dbo.DbApproachType");
            DropForeignKey("dbo.DbApproach", "ApproachLocalization_Id", "dbo.DbApproachLocalization");
            DropForeignKey("dbo.DbAnnotationContext", "ChemicalContext_Id", "dbo.DbChemicalContext");
            DropForeignKey("dbo.DbIsotope", "DbChemicalContext_Id", "dbo.DbChemicalContext");
            DropForeignKey("dbo.DbIsotope", "IsotopeName_Id", "dbo.DbIsotopeName");
            DropForeignKey("dbo.DbFunction", "DbChemicalContext_Id", "dbo.DbChemicalContext");
            DropForeignKey("dbo.DbFunction", "ChemicalFunctionType_Id", "dbo.DbChemicalFunctionType");
            DropForeignKey("dbo.DbElement", "DbChemicalContext_Id", "dbo.DbChemicalContext");
            DropForeignKey("dbo.DbElement", "ElementName_Id", "dbo.DbElementName");
            DropForeignKey("dbo.DbCompound", "DbChemicalContext_Id", "dbo.DbChemicalContext");
            DropForeignKey("dbo.DbCompoundType", "DbCompound_Id", "dbo.DbCompound");
            DropForeignKey("dbo.DbCompoundClass", "DbCompound_Id", "dbo.DbCompound");
            DropForeignKey("dbo.DbAnnotationContext", "BiomeContext_Id", "dbo.DbBiomeContext");
            DropForeignKey("dbo.DbZonoBiome", "DbBiomeContext_Id", "dbo.DbBiomeContext");
            DropForeignKey("dbo.DbZonoBiome", "BiomeType_Id", "dbo.DbBiomeType");
            DropForeignKey("dbo.DbZonoBiome", "BiomeLatitudinalZone_Id", "dbo.DbBiomeLatitudinalZone");
            DropForeignKey("dbo.DbZonoBiome", "BiomeHumidityType_Id", "dbo.DbBiomeHumidityType");
            DropForeignKey("dbo.DbZonoBiome", "BiomeHemisphere_Id", "dbo.DbBiomeHemisphere");
            DropForeignKey("dbo.DbZonoBiome", "BiomeContinentalityType_Id", "dbo.DbBiomeContinentalityType");
            DropForeignKey("dbo.DbPhysiognomy", "DbBiomeContext_Id", "dbo.DbBiomeContext");
            DropForeignKey("dbo.DbPhysiognomyType", "DbPhysiognomy_Id", "dbo.DbPhysiognomy");
            DropForeignKey("dbo.DbTerrestrialPhysiognomy", "DbPhysiognomyType_Id", "dbo.DbPhysiognomyType");
            DropForeignKey("dbo.DbTerrestrialPhysiognomy", "TerrestrialPhysiognomyType_Id", "dbo.DbTerrestrialPhysiognomyType");
            DropForeignKey("dbo.DbSemiAquaticPhysiognomy", "DbPhysiognomyType_Id", "dbo.DbPhysiognomyType");
            DropForeignKey("dbo.DbSemiAquaticPhysiognomy", "SemiAquaticPhysiognomyType_Id", "dbo.DbSemiAquaticPhysiognomyType");
            DropForeignKey("dbo.DbAquaticPhysiognomy", "DbPhysiognomyType_Id", "dbo.DbPhysiognomyType");
            DropForeignKey("dbo.DbAquaticPhysiognomy", "PlantCharacterizedAquaticPhysiognomyType_Id", "dbo.DbPlantCharacterizedAquaticPhysiognomyType");
            DropForeignKey("dbo.DbAquaticPhysiognomy", "HabitatCharacterizedAquaticPhysiognomy_Id", "dbo.DbHabitatCharacterizedAquaticPhysiognomy");
            DropForeignKey("dbo.DbPedoBiome", "DbBiomeContext_Id", "dbo.DbBiomeContext");
            DropForeignKey("dbo.DbPedoBiome", "PedoBiomeType_Id", "dbo.DbPedoBiomeType");
            DropForeignKey("dbo.DbOroBiome", "DbBiomeContext_Id", "dbo.DbBiomeContext");
            DropForeignKey("dbo.DbOroBiome", "OroBiomeType_Id", "dbo.DbOroBiomeType");
            DropForeignKey("dbo.DbLandUse", "DbBiomeContext_Id", "dbo.DbBiomeContext");
            DropForeignKey("dbo.DbLandUse", "LandUseType_Id", "dbo.DbLandUseType");
            DropForeignKey("dbo.DbLandUse", "LandUseForm_Id", "dbo.DbLandUseForm");
            DropForeignKey("dbo.DbAccessibleResource", "DbAccessRequest_Id", "dbo.DbAccessRequest");
            DropForeignKey("dbo.DbConversation", "Requester_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbConversation", "Request_Id", "dbo.DbAccessRequest");
            DropForeignKey("dbo.DbConversation", "Receiver_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbMessage", "Sender_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbMessage", "Receiver_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbMessage", "Conversation_Id", "dbo.DbConversation");
            DropForeignKey("dbo.DbAccessibleResource", "Owner_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbUserFiles", "Owner_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbUserHiddenHelpers", "UserId", "dbo.DbUsers");
            DropForeignKey("dbo.DbUserDbUserFile", "DbUserFile_FileId", "dbo.DbUserFiles");
            DropForeignKey("dbo.DbUserDbUserFile", "DbUser_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbRoleDbUser", "DbUser_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbRoleDbUser", "DbRole_Id", "dbo.DbRoles");
            DropForeignKey("dbo.DbRoleDbUserFile", "DbUserFile_FileId", "dbo.DbUserFiles");
            DropForeignKey("dbo.DbRoleDbUserFile", "DbRole_Id", "dbo.DbRoles");
            DropIndex("dbo.DbUserDbUserFile", new[] { "DbUserFile_FileId" });
            DropIndex("dbo.DbUserDbUserFile", new[] { "DbUser_Id" });
            DropIndex("dbo.DbRoleDbUser", new[] { "DbUser_Id" });
            DropIndex("dbo.DbRoleDbUser", new[] { "DbRole_Id" });
            DropIndex("dbo.DbRoleDbUserFile", new[] { "DbUserFile_FileId" });
            DropIndex("dbo.DbRoleDbUserFile", new[] { "DbRole_Id" });
            DropIndex("dbo.DbVocabularyUserValue", new[] { "User_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleUsers", new[] { "User_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleUsers", new[] { "AnnotationItem_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleGroups", new[] { "Group_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleGroups", new[] { "AnnotationItem_Id" });
            DropIndex("dbo.DbOnlineResource", new[] { "DbResource_Id" });
            DropIndex("dbo.DbOnlineResource", new[] { "DataFormat_Id" });
            DropIndex("dbo.DbOfflineResource", new[] { "DbResource_Id" });
            DropIndex("dbo.DbEmbeddedResource", new[] { "DbResource_Id" });
            DropIndex("dbo.DbEmbeddedResource", new[] { "Base64Resource_Id" });
            DropIndex("dbo.DbResource", new[] { "DbAnnotationObject_Id" });
            DropIndex("dbo.DbPosition", new[] { "DbPerson_Id" });
            DropIndex("dbo.DbPerson", new[] { "DbReferences_Id" });
            DropIndex("dbo.DbHoster", new[] { "DbReferences_Id" });
            DropIndex("dbo.DbHoster", new[] { "HosterName_Id" });
            DropIndex("dbo.DbDescription", new[] { "DbReferences_Id" });
            DropIndex("dbo.DbRangeStart", new[] { "DateTime_Id" });
            DropIndex("dbo.DbDateAndTime", new[] { "Timezone_Id" });
            DropIndex("dbo.DbRangeEnd", new[] { "DateTime_Id" });
            DropIndex("dbo.DbTimeRange", new[] { "RangeStart_Id" });
            DropIndex("dbo.DbTimeRange", new[] { "RangeEnd_Id" });
            DropIndex("dbo.DbTimeRanges", new[] { "TimeRange_Id" });
            DropIndex("dbo.DbGeologicalTimePeriod", new[] { "DbTimePeriods_Id" });
            DropIndex("dbo.DbGeologicalTimePeriod", new[] { "GeologicalPeriod_Id" });
            DropIndex("dbo.DbGeologicalTimePeriod", new[] { "GeologicalEra_Id" });
            DropIndex("dbo.DbGeologicalTimePeriod", new[] { "GeologicalEpoch_Id" });
            DropIndex("dbo.DbGeologicalTimePeriod", new[] { "GeologicalEon_Id" });
            DropIndex("dbo.DbGeologicalTimePeriod", new[] { "GeologicalAge_Id" });
            DropIndex("dbo.DbTemporalResolution", new[] { "DbTimeContext_Id" });
            DropIndex("dbo.DbTemporalResolution", new[] { "TemporalResolutionType_Id" });
            DropIndex("dbo.DbTemporalResolution", new[] { "TemporalExtentType_Id" });
            DropIndex("dbo.DbTimeContext", new[] { "TimeRanges_Id" });
            DropIndex("dbo.DbTimeContext", new[] { "TimePeriods_Id" });
            DropIndex("dbo.DbSoilTexture", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbSoilMorphology", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbSoilMorphology", new[] { "SoilMorphologyType_Id" });
            DropIndex("dbo.DbSoilAcidity", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbSoilAcidity", new[] { "SoilAcidityType_Id" });
            DropIndex("dbo.DbNumericSoilLayer", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbNumericSoilLayer", new[] { "MinimumSoilDepthUnit_Id" });
            DropIndex("dbo.DbNumericSoilLayer", new[] { "MaximumSoilDepthUnit_Id" });
            DropIndex("dbo.DbNamedSoilLayer", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbNamedSoilLayer", new[] { "SoilHorizon_Id" });
            DropIndex("dbo.DbPedosphereCompartment", new[] { "DbPedosphere_Id" });
            DropIndex("dbo.DbPedosphereCompartment", new[] { "Soil_Id" });
            DropIndex("dbo.DbNamedSeaZone", new[] { "DbSea_Id" });
            DropIndex("dbo.DbNamedSeaZone", new[] { "PelagicSeaZone_Id" });
            DropIndex("dbo.DbNamedSeaZone", new[] { "BenthicSeaZone_Id" });
            DropIndex("dbo.DbNamedRiverZone", new[] { "DbRiver_Id" });
            DropIndex("dbo.DbNamedRiverZone", new[] { "VerticalRiverZone_Id" });
            DropIndex("dbo.DbNamedRiverZone", new[] { "LongitudinalRiverZone_Id" });
            DropIndex("dbo.DbNamedLakeZone", new[] { "DbLake_Id" });
            DropIndex("dbo.DbNamedLakeZone", new[] { "PelagicLakeZone_Id" });
            DropIndex("dbo.DbNamedLakeZone", new[] { "BenthicLakeZone_Id" });
            DropIndex("dbo.DbHydrosphereCompartment", new[] { "DbHydrosphere_Id" });
            DropIndex("dbo.DbHydrosphereCompartment", new[] { "Sea_Id" });
            DropIndex("dbo.DbHydrosphereCompartment", new[] { "River_Id" });
            DropIndex("dbo.DbHydrosphereCompartment", new[] { "Lake_Id" });
            DropIndex("dbo.DbNumericEcosphereLayer", new[] { "DbEcosphere_Id" });
            DropIndex("dbo.DbNumericEcosphereLayer", new[] { "MinimumVegetationHeightUnit_Id" });
            DropIndex("dbo.DbNumericEcosphereLayer", new[] { "MaximumVegetationHeightUnit_Id" });
            DropIndex("dbo.DbNamedEcosphereLayer", new[] { "DbEcosphere_Id" });
            DropIndex("dbo.DbNamedEcosphereLayer", new[] { "EcosphereLayerName_Id" });
            DropIndex("dbo.DbNumericAtmosphereLayer", new[] { "DbAtmosphere_Id" });
            DropIndex("dbo.DbNumericAtmosphereLayer", new[] { "MinimumAtmosphereHeightUnit_Id" });
            DropIndex("dbo.DbNumericAtmosphereLayer", new[] { "MaximumAtmosphereHeightUnit_Id" });
            DropIndex("dbo.DbNamedAtmosphereLayer", new[] { "DbAtmosphere_Id" });
            DropIndex("dbo.DbNamedAtmosphereLayer", new[] { "AtmosphereLayerName_Id" });
            DropIndex("dbo.DbSphere", new[] { "DbSphereContext_Id" });
            DropIndex("dbo.DbSphere", new[] { "Pedosphere_Id" });
            DropIndex("dbo.DbSphere", new[] { "Hydrosphere_Id" });
            DropIndex("dbo.DbSphere", new[] { "Ecosphere_Id" });
            DropIndex("dbo.DbSphere", new[] { "Atmosphere_Id" });
            DropIndex("dbo.DbSpatialResolution", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbSpatialResolution", new[] { "SpatialResolutionType_Id" });
            DropIndex("dbo.DbSpatialResolution", new[] { "SpatialExtentType_Id" });
            DropIndex("dbo.DbLocation", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbLocation", new[] { "LocationType_Id" });
            DropIndex("dbo.DbLocation", new[] { "CountryName_Id" });
            DropIndex("dbo.DbLocation", new[] { "ContinentName_Id" });
            DropIndex("dbo.DbElevation", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbElevation", new[] { "MinimumElevationUnit_Id" });
            DropIndex("dbo.DbElevation", new[] { "MaximumElevationUnit_Id" });
            DropIndex("dbo.DbElevation", new[] { "ElevationDatum_Id" });
            DropIndex("dbo.DbUtmCoordinate", new[] { "UtmCoordinateNorthingUnit_Id" });
            DropIndex("dbo.DbUtmCoordinate", new[] { "UtmCoordinateHemisphere_Id" });
            DropIndex("dbo.DbUtmCoordinate", new[] { "UtmCoordinateGeodeticDatum_Id" });
            DropIndex("dbo.DbUtmCoordinate", new[] { "UtmCoordinateEastingUnit_Id" });
            DropIndex("dbo.DbCoordinate", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbCoordinate", new[] { "UtmCoordinate_Id" });
            DropIndex("dbo.DbBoundingBox", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbProcessSubject", new[] { "DbProcess_Id" });
            DropIndex("dbo.DbProcess", new[] { "DbProcessContext_Id" });
            DropIndex("dbo.DbProcess", new[] { "ProcessName_Id" });
            DropIndex("dbo.DbInteraction", new[] { "DbProcessContext_Id" });
            DropIndex("dbo.DbInteraction", new[] { "InteractionQuality_Id" });
            DropIndex("dbo.DbInteraction", new[] { "InteractionPartnerTwo_Id" });
            DropIndex("dbo.DbInteraction", new[] { "InteractionPartnerOne_Id" });
            DropIndex("dbo.DbInteraction", new[] { "InteractionDirection_Id" });
            DropIndex("dbo.DbTaxonomy", new[] { "Order_Id" });
            DropIndex("dbo.DbTaxonomy", new[] { "Kingdom_Id" });
            DropIndex("dbo.DbTaxonomy", new[] { "Genus_Id" });
            DropIndex("dbo.DbTaxonomy", new[] { "Family_Id" });
            DropIndex("dbo.DbTaxonomy", new[] { "Domain_Id" });
            DropIndex("dbo.DbTaxonomy", new[] { "Division_Id" });
            DropIndex("dbo.DbTaxonomy", new[] { "Class_Id" });
            DropIndex("dbo.DbOrganism", new[] { "DbOrganismContext_Id" });
            DropIndex("dbo.DbOrganism", new[] { "Taxonomy_Id" });
            DropIndex("dbo.DbOrganizationalHierarchy", new[] { "DbMethodContext_Id" });
            DropIndex("dbo.DbOrganizationalHierarchy", new[] { "OrganizationHierarchyName_Id" });
            DropIndex("dbo.DbFactor", new[] { "DbMethodContext_Id" });
            DropIndex("dbo.DbFactor", new[] { "FactorType_Id" });
            DropIndex("dbo.DbFactor", new[] { "FactorName_Id" });
            DropIndex("dbo.DbApproach", new[] { "DbMethodContext_Id" });
            DropIndex("dbo.DbApproach", new[] { "ApproachType_Id" });
            DropIndex("dbo.DbApproach", new[] { "ApproachLocalization_Id" });
            DropIndex("dbo.DbIsotope", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbIsotope", new[] { "IsotopeName_Id" });
            DropIndex("dbo.DbFunction", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbFunction", new[] { "ChemicalFunctionType_Id" });
            DropIndex("dbo.DbElement", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbElement", new[] { "ElementName_Id" });
            DropIndex("dbo.DbCompoundType", new[] { "DbCompound_Id" });
            DropIndex("dbo.DbCompoundClass", new[] { "DbCompound_Id" });
            DropIndex("dbo.DbCompound", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbZonoBiome", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbZonoBiome", new[] { "BiomeType_Id" });
            DropIndex("dbo.DbZonoBiome", new[] { "BiomeLatitudinalZone_Id" });
            DropIndex("dbo.DbZonoBiome", new[] { "BiomeHumidityType_Id" });
            DropIndex("dbo.DbZonoBiome", new[] { "BiomeHemisphere_Id" });
            DropIndex("dbo.DbZonoBiome", new[] { "BiomeContinentalityType_Id" });
            DropIndex("dbo.DbTerrestrialPhysiognomy", new[] { "DbPhysiognomyType_Id" });
            DropIndex("dbo.DbTerrestrialPhysiognomy", new[] { "TerrestrialPhysiognomyType_Id" });
            DropIndex("dbo.DbSemiAquaticPhysiognomy", new[] { "DbPhysiognomyType_Id" });
            DropIndex("dbo.DbSemiAquaticPhysiognomy", new[] { "SemiAquaticPhysiognomyType_Id" });
            DropIndex("dbo.DbAquaticPhysiognomy", new[] { "DbPhysiognomyType_Id" });
            DropIndex("dbo.DbAquaticPhysiognomy", new[] { "PlantCharacterizedAquaticPhysiognomyType_Id" });
            DropIndex("dbo.DbAquaticPhysiognomy", new[] { "HabitatCharacterizedAquaticPhysiognomy_Id" });
            DropIndex("dbo.DbPhysiognomyType", new[] { "DbPhysiognomy_Id" });
            DropIndex("dbo.DbPhysiognomy", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbPedoBiome", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbPedoBiome", new[] { "PedoBiomeType_Id" });
            DropIndex("dbo.DbOroBiome", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbOroBiome", new[] { "OroBiomeType_Id" });
            DropIndex("dbo.DbLandUse", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbLandUse", new[] { "LandUseType_Id" });
            DropIndex("dbo.DbLandUse", new[] { "LandUseForm_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "DbAnnotationObject_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "TimeContext_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "SphereContext_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "SpaceContext_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "ProcessContext_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "OrganismContext_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "MethodContext_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "ChemicalContext_Id" });
            DropIndex("dbo.DbAnnotationContext", new[] { "BiomeContext_Id" });
            DropIndex("dbo.DbAnnotationObject", new[] { "References_Id" });
            DropIndex("dbo.DbAnnotationItem", new[] { "Object_Id" });
            DropIndex("dbo.DbMessage", new[] { "Sender_Id" });
            DropIndex("dbo.DbMessage", new[] { "Receiver_Id" });
            DropIndex("dbo.DbMessage", new[] { "Conversation_Id" });
            DropIndex("dbo.DbConversation", new[] { "Requester_Id" });
            DropIndex("dbo.DbConversation", new[] { "Request_Id" });
            DropIndex("dbo.DbConversation", new[] { "Receiver_Id" });
            DropIndex("dbo.DbUserHiddenHelpers", new[] { "UserId" });
            DropIndex("dbo.DbUserFiles", new[] { "Owner_Id" });
            DropIndex("dbo.DbAccessibleResource", new[] { "DbAccessRequest_Id" });
            DropIndex("dbo.DbAccessibleResource", new[] { "Owner_Id" });
            DropTable("dbo.DbUserDbUserFile");
            DropTable("dbo.DbRoleDbUser");
            DropTable("dbo.DbRoleDbUserFile");
            DropTable("dbo.DbVocabularyValue");
            DropTable("dbo.DbVocabularyUserValue");
            DropTable("dbo.DbUserLogins");
            DropTable("dbo.DbSearchFilterCachedItems");
            DropTable("dbo.DbSchemaItemDescription");
            DropTable("dbo.DbRemoteVersion");
            DropTable("dbo.DbAnnotationItemAccessibleUsers");
            DropTable("dbo.DbAnnotationItemAccessibleGroups");
            DropTable("dbo.DbDataFormat");
            DropTable("dbo.DbOnlineResource");
            DropTable("dbo.DbOfflineResource");
            DropTable("dbo.DbBase64Resource");
            DropTable("dbo.DbEmbeddedResource");
            DropTable("dbo.DbResource");
            DropTable("dbo.DbPosition");
            DropTable("dbo.DbPerson");
            DropTable("dbo.DbHosterName");
            DropTable("dbo.DbHoster");
            DropTable("dbo.DbDescription");
            DropTable("dbo.DbReferences");
            DropTable("dbo.DbRangeStart");
            DropTable("dbo.DbTimezone");
            DropTable("dbo.DbDateAndTime");
            DropTable("dbo.DbRangeEnd");
            DropTable("dbo.DbTimeRange");
            DropTable("dbo.DbTimeRanges");
            DropTable("dbo.DbGeologicalPeriod");
            DropTable("dbo.DbGeologicalEra");
            DropTable("dbo.DbGeologicalEpoch");
            DropTable("dbo.DbGeologicalEon");
            DropTable("dbo.DbGeologicalAge");
            DropTable("dbo.DbGeologicalTimePeriod");
            DropTable("dbo.DbTimePeriods");
            DropTable("dbo.DbTemporalResolutionType");
            DropTable("dbo.DbTemporalExtentType");
            DropTable("dbo.DbTemporalResolution");
            DropTable("dbo.DbTimeContext");
            DropTable("dbo.DbSoilTexture");
            DropTable("dbo.DbSoilMorphologyType");
            DropTable("dbo.DbSoilMorphology");
            DropTable("dbo.DbSoilAcidityType");
            DropTable("dbo.DbSoilAcidity");
            DropTable("dbo.DbSoilDepthUnit");
            DropTable("dbo.DbNumericSoilLayer");
            DropTable("dbo.DbSoilHorizon");
            DropTable("dbo.DbNamedSoilLayer");
            DropTable("dbo.DbSoil");
            DropTable("dbo.DbPedosphereCompartment");
            DropTable("dbo.DbPedosphere");
            DropTable("dbo.DbPelagicSeaZone");
            DropTable("dbo.DbBenthicSeaZone");
            DropTable("dbo.DbNamedSeaZone");
            DropTable("dbo.DbSea");
            DropTable("dbo.DbVerticalRiverZone");
            DropTable("dbo.DbLongitudinalRiverZone");
            DropTable("dbo.DbNamedRiverZone");
            DropTable("dbo.DbRiver");
            DropTable("dbo.DbPelagicLakeZone");
            DropTable("dbo.DbBenthicLakeZone");
            DropTable("dbo.DbNamedLakeZone");
            DropTable("dbo.DbLake");
            DropTable("dbo.DbHydrosphereCompartment");
            DropTable("dbo.DbHydrosphere");
            DropTable("dbo.DbVegetationHeightUnit");
            DropTable("dbo.DbNumericEcosphereLayer");
            DropTable("dbo.DbEcosphereLayerName");
            DropTable("dbo.DbNamedEcosphereLayer");
            DropTable("dbo.DbEcosphere");
            DropTable("dbo.DbAtmosphereHeightUnit");
            DropTable("dbo.DbNumericAtmosphereLayer");
            DropTable("dbo.DbAtmosphereLayerName");
            DropTable("dbo.DbNamedAtmosphereLayer");
            DropTable("dbo.DbAtmosphere");
            DropTable("dbo.DbSphere");
            DropTable("dbo.DbSphereContext");
            DropTable("dbo.DbSpatialResolutionType");
            DropTable("dbo.DbSpatialResolution");
            DropTable("dbo.DbLocationType");
            DropTable("dbo.DbCountryName");
            DropTable("dbo.DbContinentName");
            DropTable("dbo.DbLocation");
            DropTable("dbo.DbElevationUnit");
            DropTable("dbo.DbElevationDatum");
            DropTable("dbo.DbElevation");
            DropTable("dbo.DbUtmCoordinateHemisphere");
            DropTable("dbo.DbUtmCoordinateGeodeticDatum");
            DropTable("dbo.DbUtmCoordinateUnit");
            DropTable("dbo.DbUtmCoordinate");
            DropTable("dbo.DbCoordinate");
            DropTable("dbo.DbBoundingBox");
            DropTable("dbo.DbSpaceContext");
            DropTable("dbo.DbProcessSubject");
            DropTable("dbo.DbProcessName");
            DropTable("dbo.DbProcess");
            DropTable("dbo.DbInteractionQuality");
            DropTable("dbo.DbInteractionPartner");
            DropTable("dbo.DbInteractionDirection");
            DropTable("dbo.DbInteraction");
            DropTable("dbo.DbProcessContext");
            DropTable("dbo.DbOrder");
            DropTable("dbo.DbKingdom");
            DropTable("dbo.DbGenus");
            DropTable("dbo.DbFamily");
            DropTable("dbo.DbDomain");
            DropTable("dbo.DbDivision");
            DropTable("dbo.DbClass");
            DropTable("dbo.DbTaxonomy");
            DropTable("dbo.DbOrganism");
            DropTable("dbo.DbOrganismContext");
            DropTable("dbo.DbOrganizationHierarchyName");
            DropTable("dbo.DbOrganizationalHierarchy");
            DropTable("dbo.DbFactorType");
            DropTable("dbo.DbFactorName");
            DropTable("dbo.DbFactor");
            DropTable("dbo.DbApproachType");
            DropTable("dbo.DbApproachLocalization");
            DropTable("dbo.DbApproach");
            DropTable("dbo.DbMethodContext");
            DropTable("dbo.DbIsotopeName");
            DropTable("dbo.DbIsotope");
            DropTable("dbo.DbChemicalFunctionType");
            DropTable("dbo.DbFunction");
            DropTable("dbo.DbElementName");
            DropTable("dbo.DbElement");
            DropTable("dbo.DbCompoundType");
            DropTable("dbo.DbCompoundClass");
            DropTable("dbo.DbCompound");
            DropTable("dbo.DbChemicalContext");
            DropTable("dbo.DbBiomeType");
            DropTable("dbo.DbBiomeLatitudinalZone");
            DropTable("dbo.DbBiomeHumidityType");
            DropTable("dbo.DbBiomeHemisphere");
            DropTable("dbo.DbBiomeContinentalityType");
            DropTable("dbo.DbZonoBiome");
            DropTable("dbo.DbTerrestrialPhysiognomyType");
            DropTable("dbo.DbTerrestrialPhysiognomy");
            DropTable("dbo.DbSemiAquaticPhysiognomyType");
            DropTable("dbo.DbSemiAquaticPhysiognomy");
            DropTable("dbo.DbPlantCharacterizedAquaticPhysiognomyType");
            DropTable("dbo.DbHabitatCharacterizedAquaticPhysiognomy");
            DropTable("dbo.DbAquaticPhysiognomy");
            DropTable("dbo.DbPhysiognomyType");
            DropTable("dbo.DbPhysiognomy");
            DropTable("dbo.DbPedoBiomeType");
            DropTable("dbo.DbPedoBiome");
            DropTable("dbo.DbOroBiomeType");
            DropTable("dbo.DbOroBiome");
            DropTable("dbo.DbLandUseType");
            DropTable("dbo.DbLandUseForm");
            DropTable("dbo.DbLandUse");
            DropTable("dbo.DbBiomeContext");
            DropTable("dbo.DbAnnotationContext");
            DropTable("dbo.DbAnnotationObject");
            DropTable("dbo.DbAnnotationItem");
            DropTable("dbo.DbMessage");
            DropTable("dbo.DbConversation");
            DropTable("dbo.DbAccessRequest");
            DropTable("dbo.DbUserHiddenHelpers");
            DropTable("dbo.DbRoles");
            DropTable("dbo.DbUserFiles");
            DropTable("dbo.DbUsers");
            DropTable("dbo.DbAccessibleResource");
        }
    }
}
