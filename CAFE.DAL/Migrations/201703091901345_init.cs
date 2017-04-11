namespace CAFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbAccessibleResources",
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
                .ForeignKey("dbo.DbAccessRequests", t => t.DbAccessRequest_Id)
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
                        Id = c.Guid(nullable: false),
                        Name = c.String(),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Description = c.String(),
                        Type = c.Int(nullable: false),
                        AccessMode = c.Int(nullable: false),
                        Owner_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
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
                "dbo.DbAccessRequests",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RequestSubject = c.String(nullable: false),
                        RequestMessage = c.String(),
                        CreationDate = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbConversations",
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
                .ForeignKey("dbo.DbUsers", t => t.Receiver_Id, cascadeDelete: false)
                .ForeignKey("dbo.DbAccessRequests", t => t.Request_Id, cascadeDelete: false)
                .ForeignKey("dbo.DbUsers", t => t.Requester_Id, cascadeDelete: false)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Request_Id)
                .Index(t => t.Requester_Id);
            
            CreateTable(
                "dbo.DbMessages",
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
                .ForeignKey("dbo.DbConversations", t => t.Conversation_Id, cascadeDelete: false)
                .ForeignKey("dbo.DbUsers", t => t.Receiver_Id, cascadeDelete: false)
                .ForeignKey("dbo.DbUsers", t => t.Sender_Id, cascadeDelete: false)
                .Index(t => t.Conversation_Id)
                .Index(t => t.Receiver_Id)
                .Index(t => t.Sender_Id);
            
            CreateTable(
                "dbo.DbAnnotationItems",
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
                .ForeignKey("dbo.DbAnnotationObjects", t => t.Object_Id)
                .Index(t => t.Object_Id);
            
            CreateTable(
                "dbo.DbAnnotationObjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        References_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbReferences", t => t.References_Id)
                .Index(t => t.References_Id);
            
            CreateTable(
                "dbo.DbAnnotationContexts",
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
                .ForeignKey("dbo.DbBiomeContexts", t => t.BiomeContext_Id)
                .ForeignKey("dbo.DbChemicalContexts", t => t.ChemicalContext_Id)
                .ForeignKey("dbo.DbMethodContexts", t => t.MethodContext_Id)
                .ForeignKey("dbo.DbOrganismContexts", t => t.OrganismContext_Id)
                .ForeignKey("dbo.DbProcessContexts", t => t.ProcessContext_Id)
                .ForeignKey("dbo.DbSpaceContexts", t => t.SpaceContext_Id)
                .ForeignKey("dbo.DbSphereContexts", t => t.SphereContext_Id)
                .ForeignKey("dbo.DbTimeContexts", t => t.TimeContext_Id)
                .ForeignKey("dbo.DbAnnotationObjects", t => t.DbAnnotationObject_Id)
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
                "dbo.DbBiomeContexts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLandUses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LandUseForm_Id = c.Long(),
                        LandUseType_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbLandUseForms", t => t.LandUseForm_Id)
                .ForeignKey("dbo.DbLandUseTypes", t => t.LandUseType_Id)
                .ForeignKey("dbo.DbBiomeContexts", t => t.DbBiomeContext_Id)
                .Index(t => t.LandUseForm_Id)
                .Index(t => t.LandUseType_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbLandUseForms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLandUseTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOroBiomes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OroBiomeType_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbOroBiomeTypes", t => t.OroBiomeType_Id)
                .ForeignKey("dbo.DbBiomeContexts", t => t.DbBiomeContext_Id)
                .Index(t => t.OroBiomeType_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbOroBiomeTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPedoBiomes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PedoBiomeType_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbPedoBiomeTypes", t => t.PedoBiomeType_Id)
                .ForeignKey("dbo.DbBiomeContexts", t => t.DbBiomeContext_Id)
                .Index(t => t.PedoBiomeType_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbPedoBiomeTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPhysiognomies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBiomeContexts", t => t.DbBiomeContext_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbPhysiognomyTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DbPhysiognomy_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbPhysiognomies", t => t.DbPhysiognomy_Id)
                .Index(t => t.DbPhysiognomy_Id);
            
            CreateTable(
                "dbo.DbAquaticPhysiognomies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HabitatCharacterizedAquaticPhysiognomy_Id = c.Long(),
                        PlantCharacterizedAquaticPhysiognomyType_Id = c.Long(),
                        DbPhysiognomyType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbHabitatCharacterizedAquaticPhysiognomies", t => t.HabitatCharacterizedAquaticPhysiognomy_Id)
                .ForeignKey("dbo.DbPlantCharacterizedAquaticPhysiognomyTypes", t => t.PlantCharacterizedAquaticPhysiognomyType_Id)
                .ForeignKey("dbo.DbPhysiognomyTypes", t => t.DbPhysiognomyType_Id)
                .Index(t => t.HabitatCharacterizedAquaticPhysiognomy_Id)
                .Index(t => t.PlantCharacterizedAquaticPhysiognomyType_Id)
                .Index(t => t.DbPhysiognomyType_Id);
            
            CreateTable(
                "dbo.DbHabitatCharacterizedAquaticPhysiognomies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPlantCharacterizedAquaticPhysiognomyTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSemiAquaticPhysiognomies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SemiAquaticPhysiognomyType_Id = c.Long(),
                        DbPhysiognomyType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSemiAquaticPhysiognomyTypes", t => t.SemiAquaticPhysiognomyType_Id)
                .ForeignKey("dbo.DbPhysiognomyTypes", t => t.DbPhysiognomyType_Id)
                .Index(t => t.SemiAquaticPhysiognomyType_Id)
                .Index(t => t.DbPhysiognomyType_Id);
            
            CreateTable(
                "dbo.DbSemiAquaticPhysiognomyTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbTerrestrialPhysiognomies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TerrestrialPhysiognomyType_Id = c.Long(),
                        DbPhysiognomyType_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTerrestrialPhysiognomyTypes", t => t.TerrestrialPhysiognomyType_Id)
                .ForeignKey("dbo.DbPhysiognomyTypes", t => t.DbPhysiognomyType_Id)
                .Index(t => t.TerrestrialPhysiognomyType_Id)
                .Index(t => t.DbPhysiognomyType_Id);
            
            CreateTable(
                "dbo.DbTerrestrialPhysiognomyTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbZonoBiomes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BiomeContinentalityType_Id = c.Long(),
                        BiomeHemisphere_Id = c.Long(),
                        BiomeHumidityType_Id = c.Long(),
                        BiomeType_Id = c.Long(),
                        BiomeZone_Id = c.Long(),
                        DbBiomeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBiomeContinentalityTypes", t => t.BiomeContinentalityType_Id)
                .ForeignKey("dbo.DbBiomeHemispheres", t => t.BiomeHemisphere_Id)
                .ForeignKey("dbo.DbBiomeHumidityTypes", t => t.BiomeHumidityType_Id)
                .ForeignKey("dbo.DbBiomeTypes", t => t.BiomeType_Id)
                .ForeignKey("dbo.DbBiomeZones", t => t.BiomeZone_Id)
                .ForeignKey("dbo.DbBiomeContexts", t => t.DbBiomeContext_Id)
                .Index(t => t.BiomeContinentalityType_Id)
                .Index(t => t.BiomeHemisphere_Id)
                .Index(t => t.BiomeHumidityType_Id)
                .Index(t => t.BiomeType_Id)
                .Index(t => t.BiomeZone_Id)
                .Index(t => t.DbBiomeContext_Id);
            
            CreateTable(
                "dbo.DbBiomeContinentalityTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeHemispheres",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeHumidityTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBiomeZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbChemicalContexts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbCompounds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        CompoundName = c.String(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbChemicalContexts", t => t.DbChemicalContext_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbCompoundClasses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbCompound_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbCompounds", t => t.DbCompound_Id)
                .Index(t => t.DbCompound_Id);
            
            CreateTable(
                "dbo.DbCompoundTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbCompound_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbCompounds", t => t.DbCompound_Id)
                .Index(t => t.DbCompound_Id);
            
            CreateTable(
                "dbo.DbElements",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ElementName_Id = c.Long(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbElementNames", t => t.ElementName_Id)
                .ForeignKey("dbo.DbChemicalContexts", t => t.DbChemicalContext_Id)
                .Index(t => t.ElementName_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbElementNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFunctions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ChemicalFunctionType_Id = c.Long(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbChemicalFunctionTypes", t => t.ChemicalFunctionType_Id)
                .ForeignKey("dbo.DbChemicalContexts", t => t.DbChemicalContext_Id)
                .Index(t => t.ChemicalFunctionType_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbChemicalFunctionTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbIsotopes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        IsotopeName_Id = c.Long(),
                        DbChemicalContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbIsotopeNames", t => t.IsotopeName_Id)
                .ForeignKey("dbo.DbChemicalContexts", t => t.DbChemicalContext_Id)
                .Index(t => t.IsotopeName_Id)
                .Index(t => t.DbChemicalContext_Id);
            
            CreateTable(
                "dbo.DbIsotopeNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbMethodContexts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbApproaches",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ApproachLocalization_Id = c.Long(),
                        ApproachType_Id = c.Long(),
                        DbMethodContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbApproachLocalizations", t => t.ApproachLocalization_Id)
                .ForeignKey("dbo.DbApproachTypes", t => t.ApproachType_Id)
                .ForeignKey("dbo.DbMethodContexts", t => t.DbMethodContext_Id)
                .Index(t => t.ApproachLocalization_Id)
                .Index(t => t.ApproachType_Id)
                .Index(t => t.DbMethodContext_Id);
            
            CreateTable(
                "dbo.DbApproachLocalizations",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbApproachTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFactors",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FactorName_Id = c.Long(),
                        FactorType_Id = c.Long(),
                        DbMethodContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbFactorNames", t => t.FactorName_Id)
                .ForeignKey("dbo.DbFactorTypes", t => t.FactorType_Id)
                .ForeignKey("dbo.DbMethodContexts", t => t.DbMethodContext_Id)
                .Index(t => t.FactorName_Id)
                .Index(t => t.FactorType_Id)
                .Index(t => t.DbMethodContext_Id);
            
            CreateTable(
                "dbo.DbFactorNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        Was = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFactorTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrganismContexts",
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
                .ForeignKey("dbo.DbTaxonomies", t => t.Taxonomy_Id)
                .ForeignKey("dbo.DbOrganismContexts", t => t.DbOrganismContext_Id)
                .Index(t => t.Taxonomy_Id)
                .Index(t => t.DbOrganismContext_Id);
            
            CreateTable(
                "dbo.DbTaxonomies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Species = c.String(),
                        Class_Id = c.Long(),
                        Division_Id = c.Long(),
                        Domain_Id = c.Long(),
                        Family_Id = c.Long(),
                        Kingdom_Id = c.Long(),
                        Order_Id = c.Long(),
                        OrganismName_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbClasses", t => t.Class_Id)
                .ForeignKey("dbo.DbDivisions", t => t.Division_Id)
                .ForeignKey("dbo.DbDomains", t => t.Domain_Id)
                .ForeignKey("dbo.DbFamilies", t => t.Family_Id)
                .ForeignKey("dbo.DbKingdoms", t => t.Kingdom_Id)
                .ForeignKey("dbo.DbOrders", t => t.Order_Id)
                .ForeignKey("dbo.DbOrganismNames", t => t.OrganismName_Id)
                .Index(t => t.Class_Id)
                .Index(t => t.Division_Id)
                .Index(t => t.Domain_Id)
                .Index(t => t.Family_Id)
                .Index(t => t.Kingdom_Id)
                .Index(t => t.Order_Id)
                .Index(t => t.OrganismName_Id);
            
            CreateTable(
                "dbo.DbClasses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbDivisions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbDomains",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFamilies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbKingdoms",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrders",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrganismNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BacterialName_Id = c.Long(),
                        BotanicalName_Id = c.Long(),
                        FungalName_Id = c.Long(),
                        ViralName_Id = c.Long(),
                        ZoologicalName_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBacterialNames", t => t.BacterialName_Id)
                .ForeignKey("dbo.DbBotanicalNames", t => t.BotanicalName_Id)
                .ForeignKey("dbo.DbFungalNames", t => t.FungalName_Id)
                .ForeignKey("dbo.DbViralNames", t => t.ViralName_Id)
                .ForeignKey("dbo.DbZoologicalNames", t => t.ZoologicalName_Id)
                .Index(t => t.BacterialName_Id)
                .Index(t => t.BotanicalName_Id)
                .Index(t => t.FungalName_Id)
                .Index(t => t.ViralName_Id)
                .Index(t => t.ZoologicalName_Id);
            
            CreateTable(
                "dbo.DbBacterialNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FullBacterialName = c.String(),
                        GenusOrMonomial = c.String(),
                        Subgenus = c.String(),
                        SubgenusAuthorAndYear = c.String(),
                        SpeciesEpithet = c.String(),
                        SubspeciesEpithet = c.String(),
                        ParentheticalAuthorTeamAndYear = c.String(),
                        AuthorTeamAndYear = c.String(),
                        NameApprobation = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBotanicalNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FullBotanicalName = c.String(),
                        GenusOrMonomial = c.String(),
                        FirstEpithet = c.String(),
                        InfraspecificEpithet = c.String(),
                        Rank = c.String(),
                        AuthorTeamParenthesis = c.String(),
                        AuthorTeam = c.String(),
                        CultivarGroupName = c.String(),
                        CultivarName = c.String(),
                        TradeDesignationNames_SerializedValue = c.String(),
                        HybridFlag_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbHybridFlags", t => t.HybridFlag_Id)
                .Index(t => t.HybridFlag_Id);
            
            CreateTable(
                "dbo.DbHybridFlags",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Insertionpoint = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbFungalNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FullFungalName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbViralNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FullViralName = c.String(),
                        GenusOrMonomial = c.String(),
                        ViralSpeciesDesignation = c.String(),
                        Acronym = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbZoologicalNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FullZoologicalName = c.String(),
                        GenusOrMonomial = c.String(),
                        Subgenus = c.String(),
                        SpeciesEpithet = c.String(),
                        SubspeciesEpithet = c.String(),
                        AuthorTeamOriginalAndYear = c.String(),
                        AuthorTeamParenthesisAndYear = c.String(),
                        CombinationAuthorTeamAndYear = c.String(),
                        Breed = c.String(),
                        NamedIndividual = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProcessContexts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbInteractions",
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
                .ForeignKey("dbo.DbInteractionDirections", t => t.InteractionDirection_Id)
                .ForeignKey("dbo.DbInteractionPartners", t => t.InteractionPartnerOne_Id)
                .ForeignKey("dbo.DbInteractionPartners", t => t.InteractionPartnerTwo_Id)
                .ForeignKey("dbo.DbInteractionQualities", t => t.InteractionQuality_Id)
                .ForeignKey("dbo.DbProcessContexts", t => t.DbProcessContext_Id)
                .Index(t => t.InteractionDirection_Id)
                .Index(t => t.InteractionPartnerOne_Id)
                .Index(t => t.InteractionPartnerTwo_Id)
                .Index(t => t.InteractionQuality_Id)
                .Index(t => t.DbProcessContext_Id);
            
            CreateTable(
                "dbo.DbInteractionDirections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbInteractionPartners",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbInteractionQualities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProcesses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        ProcessName_Id = c.Long(),
                        DbProcessContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbProcessNames", t => t.ProcessName_Id)
                .ForeignKey("dbo.DbProcessContexts", t => t.DbProcessContext_Id)
                .Index(t => t.ProcessName_Id)
                .Index(t => t.DbProcessContext_Id);
            
            CreateTable(
                "dbo.DbProcessNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbProcessSubjects",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbProcess_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbProcesses", t => t.DbProcess_Id)
                .Index(t => t.DbProcess_Id);
            
            CreateTable(
                "dbo.DbSpaceContexts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbBoundingBoxes",
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
                .ForeignKey("dbo.DbSpaceContexts", t => t.DbSpaceContext_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbCoordinates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UtmCoordinate_Id = c.Long(),
                        DbSpaceContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUtmCoordinates", t => t.UtmCoordinate_Id)
                .ForeignKey("dbo.DbSpaceContexts", t => t.DbSpaceContext_Id)
                .Index(t => t.UtmCoordinate_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbUtmCoordinates",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        UtmCoordinateZone = c.String(),
                        UtmCoordinateSubZone = c.String(),
                        UtmCoordinateEasting = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UtmCoordinateNorthing = c.Decimal(nullable: false, precision: 18, scale: 2),
                        UtmCoordinateEastingUnit_Id = c.Long(),
                        UtmCoordinateGeodeticDatum_Id = c.Long(),
                        UtmCoordinateHemisphere_Id = c.Long(),
                        UtmCoordinateNorthingUnit_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbUtmCoordinateUnits", t => t.UtmCoordinateEastingUnit_Id)
                .ForeignKey("dbo.DbUtmCoordinateGeodeticDatums", t => t.UtmCoordinateGeodeticDatum_Id)
                .ForeignKey("dbo.DbUtmCoordinateHemispheres", t => t.UtmCoordinateHemisphere_Id)
                .ForeignKey("dbo.DbUtmCoordinateUnits", t => t.UtmCoordinateNorthingUnit_Id)
                .Index(t => t.UtmCoordinateEastingUnit_Id)
                .Index(t => t.UtmCoordinateGeodeticDatum_Id)
                .Index(t => t.UtmCoordinateHemisphere_Id)
                .Index(t => t.UtmCoordinateNorthingUnit_Id);
            
            CreateTable(
                "dbo.DbUtmCoordinateUnits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbUtmCoordinateGeodeticDatums",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbUtmCoordinateHemispheres",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbElevations",
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
                .ForeignKey("dbo.DbElevationDatums", t => t.ElevationDatum_Id)
                .ForeignKey("dbo.DbElevationUnits", t => t.MaximumElevationUnit_Id)
                .ForeignKey("dbo.DbElevationUnits", t => t.MinimumElevationUnit_Id)
                .ForeignKey("dbo.DbSpaceContexts", t => t.DbSpaceContext_Id)
                .Index(t => t.ElevationDatum_Id)
                .Index(t => t.MaximumElevationUnit_Id)
                .Index(t => t.MinimumElevationUnit_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbElevationDatums",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbElevationUnits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLocations",
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
                .ForeignKey("dbo.DbContinentNames", t => t.ContinentName_Id)
                .ForeignKey("dbo.DbCountryNames", t => t.CountryName_Id)
                .ForeignKey("dbo.DbLocationTypes", t => t.LocationType_Id)
                .ForeignKey("dbo.DbSpaceContexts", t => t.DbSpaceContext_Id)
                .Index(t => t.ContinentName_Id)
                .Index(t => t.CountryName_Id)
                .Index(t => t.LocationType_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbContinentNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbCountryNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbLocationTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSpatialResolutions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SpatialExtentType_Id = c.Long(),
                        SpatialResolutionType_Id = c.Long(),
                        DbSpaceContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSpatialResolutionTypes", t => t.SpatialExtentType_Id)
                .ForeignKey("dbo.DbSpatialResolutionTypes", t => t.SpatialResolutionType_Id)
                .ForeignKey("dbo.DbSpaceContexts", t => t.DbSpaceContext_Id)
                .Index(t => t.SpatialExtentType_Id)
                .Index(t => t.SpatialResolutionType_Id)
                .Index(t => t.DbSpaceContext_Id);
            
            CreateTable(
                "dbo.DbSpatialResolutionTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSphereContexts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSpheres",
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
                .ForeignKey("dbo.DbAtmospheres", t => t.Atmosphere_Id)
                .ForeignKey("dbo.DbEcospheres", t => t.Ecosphere_Id)
                .ForeignKey("dbo.DbHydrospheres", t => t.Hydrosphere_Id)
                .ForeignKey("dbo.DbPedospheres", t => t.Pedosphere_Id)
                .ForeignKey("dbo.DbSphereContexts", t => t.DbSphereContext_Id)
                .Index(t => t.Atmosphere_Id)
                .Index(t => t.Ecosphere_Id)
                .Index(t => t.Hydrosphere_Id)
                .Index(t => t.Pedosphere_Id)
                .Index(t => t.DbSphereContext_Id);
            
            CreateTable(
                "dbo.DbAtmospheres",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedAtmosphereLayers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AtmosphereLayerName_Id = c.Long(),
                        DbAtmosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAtmosphereLayerNames", t => t.AtmosphereLayerName_Id)
                .ForeignKey("dbo.DbAtmospheres", t => t.DbAtmosphere_Id)
                .Index(t => t.AtmosphereLayerName_Id)
                .Index(t => t.DbAtmosphere_Id);
            
            CreateTable(
                "dbo.DbAtmosphereLayerNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNumericAtmosphereLayers",
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
                .ForeignKey("dbo.DbAtmosphereHeightUnits", t => t.MaximumAtmosphereHeightUnit_Id)
                .ForeignKey("dbo.DbAtmosphereHeightUnits", t => t.MinimumAtmosphereHeightUnit_Id)
                .ForeignKey("dbo.DbAtmospheres", t => t.DbAtmosphere_Id)
                .Index(t => t.MaximumAtmosphereHeightUnit_Id)
                .Index(t => t.MinimumAtmosphereHeightUnit_Id)
                .Index(t => t.DbAtmosphere_Id);
            
            CreateTable(
                "dbo.DbAtmosphereHeightUnits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbEcospheres",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedEcosphereLayers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        EcosphereLayerName_Id = c.Long(),
                        DbEcosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbEcosphereLayerNames", t => t.EcosphereLayerName_Id)
                .ForeignKey("dbo.DbEcospheres", t => t.DbEcosphere_Id)
                .Index(t => t.EcosphereLayerName_Id)
                .Index(t => t.DbEcosphere_Id);
            
            CreateTable(
                "dbo.DbEcosphereLayerNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNumericEcosphereLayers",
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
                .ForeignKey("dbo.DbVegetationHeightUnits", t => t.MaximumVegetationHeightUnit_Id)
                .ForeignKey("dbo.DbVegetationHeightUnits", t => t.MinimumVegetationHeightUnit_Id)
                .ForeignKey("dbo.DbEcospheres", t => t.DbEcosphere_Id)
                .Index(t => t.MaximumVegetationHeightUnit_Id)
                .Index(t => t.MinimumVegetationHeightUnit_Id)
                .Index(t => t.DbEcosphere_Id);
            
            CreateTable(
                "dbo.DbVegetationHeightUnits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOrganizationalHierarchies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        OrganizationHierarchyName_Id = c.Long(),
                        DbEcosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbOrganizationHierarchyNames", t => t.OrganizationHierarchyName_Id)
                .ForeignKey("dbo.DbEcospheres", t => t.DbEcosphere_Id)
                .Index(t => t.OrganizationHierarchyName_Id)
                .Index(t => t.DbEcosphere_Id);
            
            CreateTable(
                "dbo.DbOrganizationHierarchyNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbHydrospheres",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbHydrosphereCompartments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Lake_Id = c.Long(),
                        River_Id = c.Long(),
                        Sea_Id = c.Long(),
                        DbHydrosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbLakes", t => t.Lake_Id)
                .ForeignKey("dbo.DbRivers", t => t.River_Id)
                .ForeignKey("dbo.DbSeas", t => t.Sea_Id)
                .ForeignKey("dbo.DbHydrospheres", t => t.DbHydrosphere_Id)
                .Index(t => t.Lake_Id)
                .Index(t => t.River_Id)
                .Index(t => t.Sea_Id)
                .Index(t => t.DbHydrosphere_Id);
            
            CreateTable(
                "dbo.DbLakes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedLakeZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BenthicLakeZone_Id = c.Long(),
                        PelagicLakeZone_Id = c.Long(),
                        DbLake_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBenthicLakeZones", t => t.BenthicLakeZone_Id)
                .ForeignKey("dbo.DbPelagicLakeZones", t => t.PelagicLakeZone_Id)
                .ForeignKey("dbo.DbLakes", t => t.DbLake_Id)
                .Index(t => t.BenthicLakeZone_Id)
                .Index(t => t.PelagicLakeZone_Id)
                .Index(t => t.DbLake_Id);
            
            CreateTable(
                "dbo.DbBenthicLakeZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPelagicLakeZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbRivers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedRiverZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        LongitudinalRiverZone_Id = c.Long(),
                        VerticalRiverZone_Id = c.Long(),
                        DbRiver_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbLongitudinalRiverZones", t => t.LongitudinalRiverZone_Id)
                .ForeignKey("dbo.DbVerticalRiverZones", t => t.VerticalRiverZone_Id)
                .ForeignKey("dbo.DbRivers", t => t.DbRiver_Id)
                .Index(t => t.LongitudinalRiverZone_Id)
                .Index(t => t.VerticalRiverZone_Id)
                .Index(t => t.DbRiver_Id);
            
            CreateTable(
                "dbo.DbLongitudinalRiverZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbVerticalRiverZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSeas",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedSeaZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        BenthicSeaZone_Id = c.Long(),
                        PelagicSeaZone_Id = c.Long(),
                        DbSea_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBenthicSeaZones", t => t.BenthicSeaZone_Id)
                .ForeignKey("dbo.DbPelagicSeaZones", t => t.PelagicSeaZone_Id)
                .ForeignKey("dbo.DbSeas", t => t.DbSea_Id)
                .Index(t => t.BenthicSeaZone_Id)
                .Index(t => t.PelagicSeaZone_Id)
                .Index(t => t.DbSea_Id);
            
            CreateTable(
                "dbo.DbBenthicSeaZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPelagicSeaZones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPedospheres",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPedosphereCompartments",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Soil_Id = c.Long(),
                        DbPedosphere_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoils", t => t.Soil_Id)
                .ForeignKey("dbo.DbPedospheres", t => t.DbPedosphere_Id)
                .Index(t => t.Soil_Id)
                .Index(t => t.DbPedosphere_Id);
            
            CreateTable(
                "dbo.DbSoils",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNamedSoilLayers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SoilHorizon_Id = c.Long(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoilHorizons", t => t.SoilHorizon_Id)
                .ForeignKey("dbo.DbSoils", t => t.DbSoil_Id)
                .Index(t => t.SoilHorizon_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilHorizons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbNumericSoilLayers",
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
                .ForeignKey("dbo.DbSoilDepthUnits", t => t.MaximumSoilDepthUnit_Id)
                .ForeignKey("dbo.DbSoilDepthUnits", t => t.MinimumSoilDepthUnit_Id)
                .ForeignKey("dbo.DbSoils", t => t.DbSoil_Id)
                .Index(t => t.MaximumSoilDepthUnit_Id)
                .Index(t => t.MinimumSoilDepthUnit_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilDepthUnits",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSoilAcidities",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SoilAcidityType_Id = c.Long(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoilAcidityTypes", t => t.SoilAcidityType_Id)
                .ForeignKey("dbo.DbSoils", t => t.DbSoil_Id)
                .Index(t => t.SoilAcidityType_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilAcidityTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSoilMorphologies",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SoilMorphologyType_Id = c.Long(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoilMorphologyTypes", t => t.SoilMorphologyType_Id)
                .ForeignKey("dbo.DbSoils", t => t.DbSoil_Id)
                .Index(t => t.SoilMorphologyType_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbSoilMorphologyTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbSoilTextures",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SandPercent = c.String(),
                        SiltPercent = c.String(),
                        LoamPercent = c.String(),
                        DbSoil_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbSoils", t => t.DbSoil_Id)
                .Index(t => t.DbSoil_Id);
            
            CreateTable(
                "dbo.DbTimeContexts",
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
                "dbo.DbTemporalResolutions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TemporalExtentType_Id = c.Long(),
                        TemporalResolutionType_Id = c.Long(),
                        DbTimeContext_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTemporalExtentTypes", t => t.TemporalExtentType_Id)
                .ForeignKey("dbo.DbTemporalResolutionTypes", t => t.TemporalResolutionType_Id)
                .ForeignKey("dbo.DbTimeContexts", t => t.DbTimeContext_Id)
                .Index(t => t.TemporalExtentType_Id)
                .Index(t => t.TemporalResolutionType_Id)
                .Index(t => t.DbTimeContext_Id);
            
            CreateTable(
                "dbo.DbTemporalExtentTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        Modifyer = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbTemporalResolutionTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        Modifyer = c.String(),
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
                "dbo.DbGeologicalTimePeriods",
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
                .ForeignKey("dbo.DbGeologicalAges", t => t.GeologicalAge_Id)
                .ForeignKey("dbo.DbGeologicalEons", t => t.GeologicalEon_Id)
                .ForeignKey("dbo.DbGeologicalEpoches", t => t.GeologicalEpoch_Id)
                .ForeignKey("dbo.DbGeologicalEras", t => t.GeologicalEra_Id)
                .ForeignKey("dbo.DbGeologicalPeriods", t => t.GeologicalPeriod_Id)
                .ForeignKey("dbo.DbTimePeriods", t => t.DbTimePeriods_Id)
                .Index(t => t.GeologicalAge_Id)
                .Index(t => t.GeologicalEon_Id)
                .Index(t => t.GeologicalEpoch_Id)
                .Index(t => t.GeologicalEra_Id)
                .Index(t => t.GeologicalPeriod_Id)
                .Index(t => t.DbTimePeriods_Id);
            
            CreateTable(
                "dbo.DbGeologicalAges",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalEons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalEpoches",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalEras",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbGeologicalPeriods",
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
                .ForeignKey("dbo.DbTimeRanges1", t => t.TimeRange_Id)
                .Index(t => t.TimeRange_Id);
            
            CreateTable(
                "dbo.DbTimeRanges1",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RangeEnd_Id = c.Long(),
                        RangeStart_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbRangeEnds", t => t.RangeEnd_Id)
                .ForeignKey("dbo.DbRangeStarts", t => t.RangeStart_Id)
                .Index(t => t.RangeEnd_Id)
                .Index(t => t.RangeStart_Id);
            
            CreateTable(
                "dbo.DbRangeEnds",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateTime_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDateAndTimes", t => t.DateTime_Id)
                .Index(t => t.DateTime_Id);
            
            CreateTable(
                "dbo.DbDateAndTimes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        Time = c.String(),
                        Timezone_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbTimezones", t => t.Timezone_Id)
                .Index(t => t.Timezone_Id);
            
            CreateTable(
                "dbo.DbTimezones",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbRangeStarts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DateTime_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDateAndTimes", t => t.DateTime_Id)
                .Index(t => t.DateTime_Id);
            
            CreateTable(
                "dbo.DbReferences",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbDescriptions",
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
                "dbo.DbHosters",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        HosterName_Id = c.Long(),
                        DbReferences_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbHosterNames", t => t.HosterName_Id)
                .ForeignKey("dbo.DbReferences", t => t.DbReferences_Id)
                .Index(t => t.HosterName_Id)
                .Index(t => t.DbReferences_Id);
            
            CreateTable(
                "dbo.DbHosterNames",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbPersons",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Salutation_SerializedValue = c.String(),
                        GivenName_SerializedValue = c.String(),
                        SurName_SerializedValue = c.String(),
                        EmailAddress_SerializedValue = c.String(),
                        PhoneNumber_SerializedValue = c.String(),
                        DbReferences_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbReferences", t => t.DbReferences_Id)
                .Index(t => t.DbReferences_Id);
            
            CreateTable(
                "dbo.DbPositions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                        DbPerson_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbPersons", t => t.DbPerson_Id)
                .Index(t => t.DbPerson_Id);
            
            CreateTable(
                "dbo.DbResources",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DbAnnotationObject_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAnnotationObjects", t => t.DbAnnotationObject_Id)
                .Index(t => t.DbAnnotationObject_Id);
            
            CreateTable(
                "dbo.DbEmbeddedResources",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Base64Resource_Id = c.Long(),
                        DbResource_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbBase64Resource", t => t.Base64Resource_Id)
                .ForeignKey("dbo.DbResources", t => t.DbResource_Id)
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
                        DataFormat_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDataFormats", t => t.DataFormat_Id)
                .Index(t => t.DataFormat_Id);
            
            CreateTable(
                "dbo.DbDataFormats",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Value = c.String(),
                        Uri = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbOfflineResources",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FileID = c.String(),
                        FileName = c.String(),
                        FilePath = c.String(),
                        MimeType = c.String(),
                        DataFormat_Id = c.Long(),
                        DbResource_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDataFormats", t => t.DataFormat_Id)
                .ForeignKey("dbo.DbResources", t => t.DbResource_Id)
                .Index(t => t.DataFormat_Id)
                .Index(t => t.DbResource_Id);
            
            CreateTable(
                "dbo.DbOnlineResources",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DownloadUrl = c.String(),
                        ReferenceUrl = c.String(),
                        DataFormat_Id = c.Long(),
                        DbResource_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbDataFormats", t => t.DataFormat_Id)
                .ForeignKey("dbo.DbResources", t => t.DbResource_Id)
                .Index(t => t.DataFormat_Id)
                .Index(t => t.DbResource_Id);
            
            CreateTable(
                "dbo.DbAnnotationItemAccessibleGroups",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        AnnotationItem_Id = c.Guid(),
                        Group_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DbAnnotationItems", t => t.AnnotationItem_Id)
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
                .ForeignKey("dbo.DbAnnotationItems", t => t.AnnotationItem_Id)
                .ForeignKey("dbo.DbUsers", t => t.User_Id)
                .Index(t => t.AnnotationItem_Id)
                .Index(t => t.User_Id);
            
            CreateTable(
                "dbo.DbRemoteVersions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Version = c.String(),
                        IsNew = c.Boolean(nullable: false),
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
                "dbo.DbVocabularyUserValues",
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
                "dbo.DbVocabularyValues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Type = c.String(nullable: false),
                        Value = c.String(nullable: false),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.DbRoleDbUserFiles",
                c => new
                    {
                        DbRole_Id = c.Guid(nullable: false),
                        DbUserFile_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbRole_Id, t.DbUserFile_Id })
                .ForeignKey("dbo.DbRoles", t => t.DbRole_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbUserFiles", t => t.DbUserFile_Id, cascadeDelete: true)
                .Index(t => t.DbRole_Id)
                .Index(t => t.DbUserFile_Id);
            
            CreateTable(
                "dbo.DbRoleDbUsers",
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
                "dbo.DbUserDbUserFiles",
                c => new
                    {
                        DbUser_Id = c.Guid(nullable: false),
                        DbUserFile_Id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => new { t.DbUser_Id, t.DbUserFile_Id })
                .ForeignKey("dbo.DbUsers", t => t.DbUser_Id, cascadeDelete: true)
                .ForeignKey("dbo.DbUserFiles", t => t.DbUserFile_Id, cascadeDelete: true)
                .Index(t => t.DbUser_Id)
                .Index(t => t.DbUserFile_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.DbVocabularyUserValues", "User_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbAnnotationItemAccessibleUsers", "User_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbAnnotationItemAccessibleUsers", "AnnotationItem_Id", "dbo.DbAnnotationItems");
            DropForeignKey("dbo.DbAnnotationItemAccessibleGroups", "Group_Id", "dbo.DbRoles");
            DropForeignKey("dbo.DbAnnotationItemAccessibleGroups", "AnnotationItem_Id", "dbo.DbAnnotationItems");
            DropForeignKey("dbo.DbAnnotationItems", "Object_Id", "dbo.DbAnnotationObjects");
            DropForeignKey("dbo.DbResources", "DbAnnotationObject_Id", "dbo.DbAnnotationObjects");
            DropForeignKey("dbo.DbOnlineResources", "DbResource_Id", "dbo.DbResources");
            DropForeignKey("dbo.DbOnlineResources", "DataFormat_Id", "dbo.DbDataFormats");
            DropForeignKey("dbo.DbOfflineResources", "DbResource_Id", "dbo.DbResources");
            DropForeignKey("dbo.DbOfflineResources", "DataFormat_Id", "dbo.DbDataFormats");
            DropForeignKey("dbo.DbEmbeddedResources", "DbResource_Id", "dbo.DbResources");
            DropForeignKey("dbo.DbEmbeddedResources", "Base64Resource_Id", "dbo.DbBase64Resource");
            DropForeignKey("dbo.DbBase64Resource", "DataFormat_Id", "dbo.DbDataFormats");
            DropForeignKey("dbo.DbAnnotationObjects", "References_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbPersons", "DbReferences_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbPositions", "DbPerson_Id", "dbo.DbPersons");
            DropForeignKey("dbo.DbHosters", "DbReferences_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbHosters", "HosterName_Id", "dbo.DbHosterNames");
            DropForeignKey("dbo.DbDescriptions", "DbReferences_Id", "dbo.DbReferences");
            DropForeignKey("dbo.DbAnnotationContexts", "DbAnnotationObject_Id", "dbo.DbAnnotationObjects");
            DropForeignKey("dbo.DbAnnotationContexts", "TimeContext_Id", "dbo.DbTimeContexts");
            DropForeignKey("dbo.DbTimeContexts", "TimeRanges_Id", "dbo.DbTimeRanges");
            DropForeignKey("dbo.DbTimeRanges", "TimeRange_Id", "dbo.DbTimeRanges1");
            DropForeignKey("dbo.DbTimeRanges1", "RangeStart_Id", "dbo.DbRangeStarts");
            DropForeignKey("dbo.DbRangeStarts", "DateTime_Id", "dbo.DbDateAndTimes");
            DropForeignKey("dbo.DbTimeRanges1", "RangeEnd_Id", "dbo.DbRangeEnds");
            DropForeignKey("dbo.DbRangeEnds", "DateTime_Id", "dbo.DbDateAndTimes");
            DropForeignKey("dbo.DbDateAndTimes", "Timezone_Id", "dbo.DbTimezones");
            DropForeignKey("dbo.DbTimeContexts", "TimePeriods_Id", "dbo.DbTimePeriods");
            DropForeignKey("dbo.DbGeologicalTimePeriods", "DbTimePeriods_Id", "dbo.DbTimePeriods");
            DropForeignKey("dbo.DbGeologicalTimePeriods", "GeologicalPeriod_Id", "dbo.DbGeologicalPeriods");
            DropForeignKey("dbo.DbGeologicalTimePeriods", "GeologicalEra_Id", "dbo.DbGeologicalEras");
            DropForeignKey("dbo.DbGeologicalTimePeriods", "GeologicalEpoch_Id", "dbo.DbGeologicalEpoches");
            DropForeignKey("dbo.DbGeologicalTimePeriods", "GeologicalEon_Id", "dbo.DbGeologicalEons");
            DropForeignKey("dbo.DbGeologicalTimePeriods", "GeologicalAge_Id", "dbo.DbGeologicalAges");
            DropForeignKey("dbo.DbTemporalResolutions", "DbTimeContext_Id", "dbo.DbTimeContexts");
            DropForeignKey("dbo.DbTemporalResolutions", "TemporalResolutionType_Id", "dbo.DbTemporalResolutionTypes");
            DropForeignKey("dbo.DbTemporalResolutions", "TemporalExtentType_Id", "dbo.DbTemporalExtentTypes");
            DropForeignKey("dbo.DbAnnotationContexts", "SphereContext_Id", "dbo.DbSphereContexts");
            DropForeignKey("dbo.DbSpheres", "DbSphereContext_Id", "dbo.DbSphereContexts");
            DropForeignKey("dbo.DbSpheres", "Pedosphere_Id", "dbo.DbPedospheres");
            DropForeignKey("dbo.DbPedosphereCompartments", "DbPedosphere_Id", "dbo.DbPedospheres");
            DropForeignKey("dbo.DbPedosphereCompartments", "Soil_Id", "dbo.DbSoils");
            DropForeignKey("dbo.DbSoilTextures", "DbSoil_Id", "dbo.DbSoils");
            DropForeignKey("dbo.DbSoilMorphologies", "DbSoil_Id", "dbo.DbSoils");
            DropForeignKey("dbo.DbSoilMorphologies", "SoilMorphologyType_Id", "dbo.DbSoilMorphologyTypes");
            DropForeignKey("dbo.DbSoilAcidities", "DbSoil_Id", "dbo.DbSoils");
            DropForeignKey("dbo.DbSoilAcidities", "SoilAcidityType_Id", "dbo.DbSoilAcidityTypes");
            DropForeignKey("dbo.DbNumericSoilLayers", "DbSoil_Id", "dbo.DbSoils");
            DropForeignKey("dbo.DbNumericSoilLayers", "MinimumSoilDepthUnit_Id", "dbo.DbSoilDepthUnits");
            DropForeignKey("dbo.DbNumericSoilLayers", "MaximumSoilDepthUnit_Id", "dbo.DbSoilDepthUnits");
            DropForeignKey("dbo.DbNamedSoilLayers", "DbSoil_Id", "dbo.DbSoils");
            DropForeignKey("dbo.DbNamedSoilLayers", "SoilHorizon_Id", "dbo.DbSoilHorizons");
            DropForeignKey("dbo.DbSpheres", "Hydrosphere_Id", "dbo.DbHydrospheres");
            DropForeignKey("dbo.DbHydrosphereCompartments", "DbHydrosphere_Id", "dbo.DbHydrospheres");
            DropForeignKey("dbo.DbHydrosphereCompartments", "Sea_Id", "dbo.DbSeas");
            DropForeignKey("dbo.DbNamedSeaZones", "DbSea_Id", "dbo.DbSeas");
            DropForeignKey("dbo.DbNamedSeaZones", "PelagicSeaZone_Id", "dbo.DbPelagicSeaZones");
            DropForeignKey("dbo.DbNamedSeaZones", "BenthicSeaZone_Id", "dbo.DbBenthicSeaZones");
            DropForeignKey("dbo.DbHydrosphereCompartments", "River_Id", "dbo.DbRivers");
            DropForeignKey("dbo.DbNamedRiverZones", "DbRiver_Id", "dbo.DbRivers");
            DropForeignKey("dbo.DbNamedRiverZones", "VerticalRiverZone_Id", "dbo.DbVerticalRiverZones");
            DropForeignKey("dbo.DbNamedRiverZones", "LongitudinalRiverZone_Id", "dbo.DbLongitudinalRiverZones");
            DropForeignKey("dbo.DbHydrosphereCompartments", "Lake_Id", "dbo.DbLakes");
            DropForeignKey("dbo.DbNamedLakeZones", "DbLake_Id", "dbo.DbLakes");
            DropForeignKey("dbo.DbNamedLakeZones", "PelagicLakeZone_Id", "dbo.DbPelagicLakeZones");
            DropForeignKey("dbo.DbNamedLakeZones", "BenthicLakeZone_Id", "dbo.DbBenthicLakeZones");
            DropForeignKey("dbo.DbSpheres", "Ecosphere_Id", "dbo.DbEcospheres");
            DropForeignKey("dbo.DbOrganizationalHierarchies", "DbEcosphere_Id", "dbo.DbEcospheres");
            DropForeignKey("dbo.DbOrganizationalHierarchies", "OrganizationHierarchyName_Id", "dbo.DbOrganizationHierarchyNames");
            DropForeignKey("dbo.DbNumericEcosphereLayers", "DbEcosphere_Id", "dbo.DbEcospheres");
            DropForeignKey("dbo.DbNumericEcosphereLayers", "MinimumVegetationHeightUnit_Id", "dbo.DbVegetationHeightUnits");
            DropForeignKey("dbo.DbNumericEcosphereLayers", "MaximumVegetationHeightUnit_Id", "dbo.DbVegetationHeightUnits");
            DropForeignKey("dbo.DbNamedEcosphereLayers", "DbEcosphere_Id", "dbo.DbEcospheres");
            DropForeignKey("dbo.DbNamedEcosphereLayers", "EcosphereLayerName_Id", "dbo.DbEcosphereLayerNames");
            DropForeignKey("dbo.DbSpheres", "Atmosphere_Id", "dbo.DbAtmospheres");
            DropForeignKey("dbo.DbNumericAtmosphereLayers", "DbAtmosphere_Id", "dbo.DbAtmospheres");
            DropForeignKey("dbo.DbNumericAtmosphereLayers", "MinimumAtmosphereHeightUnit_Id", "dbo.DbAtmosphereHeightUnits");
            DropForeignKey("dbo.DbNumericAtmosphereLayers", "MaximumAtmosphereHeightUnit_Id", "dbo.DbAtmosphereHeightUnits");
            DropForeignKey("dbo.DbNamedAtmosphereLayers", "DbAtmosphere_Id", "dbo.DbAtmospheres");
            DropForeignKey("dbo.DbNamedAtmosphereLayers", "AtmosphereLayerName_Id", "dbo.DbAtmosphereLayerNames");
            DropForeignKey("dbo.DbAnnotationContexts", "SpaceContext_Id", "dbo.DbSpaceContexts");
            DropForeignKey("dbo.DbSpatialResolutions", "DbSpaceContext_Id", "dbo.DbSpaceContexts");
            DropForeignKey("dbo.DbSpatialResolutions", "SpatialResolutionType_Id", "dbo.DbSpatialResolutionTypes");
            DropForeignKey("dbo.DbSpatialResolutions", "SpatialExtentType_Id", "dbo.DbSpatialResolutionTypes");
            DropForeignKey("dbo.DbLocations", "DbSpaceContext_Id", "dbo.DbSpaceContexts");
            DropForeignKey("dbo.DbLocations", "LocationType_Id", "dbo.DbLocationTypes");
            DropForeignKey("dbo.DbLocations", "CountryName_Id", "dbo.DbCountryNames");
            DropForeignKey("dbo.DbLocations", "ContinentName_Id", "dbo.DbContinentNames");
            DropForeignKey("dbo.DbElevations", "DbSpaceContext_Id", "dbo.DbSpaceContexts");
            DropForeignKey("dbo.DbElevations", "MinimumElevationUnit_Id", "dbo.DbElevationUnits");
            DropForeignKey("dbo.DbElevations", "MaximumElevationUnit_Id", "dbo.DbElevationUnits");
            DropForeignKey("dbo.DbElevations", "ElevationDatum_Id", "dbo.DbElevationDatums");
            DropForeignKey("dbo.DbCoordinates", "DbSpaceContext_Id", "dbo.DbSpaceContexts");
            DropForeignKey("dbo.DbCoordinates", "UtmCoordinate_Id", "dbo.DbUtmCoordinates");
            DropForeignKey("dbo.DbUtmCoordinates", "UtmCoordinateNorthingUnit_Id", "dbo.DbUtmCoordinateUnits");
            DropForeignKey("dbo.DbUtmCoordinates", "UtmCoordinateHemisphere_Id", "dbo.DbUtmCoordinateHemispheres");
            DropForeignKey("dbo.DbUtmCoordinates", "UtmCoordinateGeodeticDatum_Id", "dbo.DbUtmCoordinateGeodeticDatums");
            DropForeignKey("dbo.DbUtmCoordinates", "UtmCoordinateEastingUnit_Id", "dbo.DbUtmCoordinateUnits");
            DropForeignKey("dbo.DbBoundingBoxes", "DbSpaceContext_Id", "dbo.DbSpaceContexts");
            DropForeignKey("dbo.DbAnnotationContexts", "ProcessContext_Id", "dbo.DbProcessContexts");
            DropForeignKey("dbo.DbProcesses", "DbProcessContext_Id", "dbo.DbProcessContexts");
            DropForeignKey("dbo.DbProcessSubjects", "DbProcess_Id", "dbo.DbProcesses");
            DropForeignKey("dbo.DbProcesses", "ProcessName_Id", "dbo.DbProcessNames");
            DropForeignKey("dbo.DbInteractions", "DbProcessContext_Id", "dbo.DbProcessContexts");
            DropForeignKey("dbo.DbInteractions", "InteractionQuality_Id", "dbo.DbInteractionQualities");
            DropForeignKey("dbo.DbInteractions", "InteractionPartnerTwo_Id", "dbo.DbInteractionPartners");
            DropForeignKey("dbo.DbInteractions", "InteractionPartnerOne_Id", "dbo.DbInteractionPartners");
            DropForeignKey("dbo.DbInteractions", "InteractionDirection_Id", "dbo.DbInteractionDirections");
            DropForeignKey("dbo.DbAnnotationContexts", "OrganismContext_Id", "dbo.DbOrganismContexts");
            DropForeignKey("dbo.DbOrganism", "DbOrganismContext_Id", "dbo.DbOrganismContexts");
            DropForeignKey("dbo.DbOrganism", "Taxonomy_Id", "dbo.DbTaxonomies");
            DropForeignKey("dbo.DbTaxonomies", "OrganismName_Id", "dbo.DbOrganismNames");
            DropForeignKey("dbo.DbOrganismNames", "ZoologicalName_Id", "dbo.DbZoologicalNames");
            DropForeignKey("dbo.DbOrganismNames", "ViralName_Id", "dbo.DbViralNames");
            DropForeignKey("dbo.DbOrganismNames", "FungalName_Id", "dbo.DbFungalNames");
            DropForeignKey("dbo.DbOrganismNames", "BotanicalName_Id", "dbo.DbBotanicalNames");
            DropForeignKey("dbo.DbBotanicalNames", "HybridFlag_Id", "dbo.DbHybridFlags");
            DropForeignKey("dbo.DbOrganismNames", "BacterialName_Id", "dbo.DbBacterialNames");
            DropForeignKey("dbo.DbTaxonomies", "Order_Id", "dbo.DbOrders");
            DropForeignKey("dbo.DbTaxonomies", "Kingdom_Id", "dbo.DbKingdoms");
            DropForeignKey("dbo.DbTaxonomies", "Family_Id", "dbo.DbFamilies");
            DropForeignKey("dbo.DbTaxonomies", "Domain_Id", "dbo.DbDomains");
            DropForeignKey("dbo.DbTaxonomies", "Division_Id", "dbo.DbDivisions");
            DropForeignKey("dbo.DbTaxonomies", "Class_Id", "dbo.DbClasses");
            DropForeignKey("dbo.DbAnnotationContexts", "MethodContext_Id", "dbo.DbMethodContexts");
            DropForeignKey("dbo.DbFactors", "DbMethodContext_Id", "dbo.DbMethodContexts");
            DropForeignKey("dbo.DbFactors", "FactorType_Id", "dbo.DbFactorTypes");
            DropForeignKey("dbo.DbFactors", "FactorName_Id", "dbo.DbFactorNames");
            DropForeignKey("dbo.DbApproaches", "DbMethodContext_Id", "dbo.DbMethodContexts");
            DropForeignKey("dbo.DbApproaches", "ApproachType_Id", "dbo.DbApproachTypes");
            DropForeignKey("dbo.DbApproaches", "ApproachLocalization_Id", "dbo.DbApproachLocalizations");
            DropForeignKey("dbo.DbAnnotationContexts", "ChemicalContext_Id", "dbo.DbChemicalContexts");
            DropForeignKey("dbo.DbIsotopes", "DbChemicalContext_Id", "dbo.DbChemicalContexts");
            DropForeignKey("dbo.DbIsotopes", "IsotopeName_Id", "dbo.DbIsotopeNames");
            DropForeignKey("dbo.DbFunctions", "DbChemicalContext_Id", "dbo.DbChemicalContexts");
            DropForeignKey("dbo.DbFunctions", "ChemicalFunctionType_Id", "dbo.DbChemicalFunctionTypes");
            DropForeignKey("dbo.DbElements", "DbChemicalContext_Id", "dbo.DbChemicalContexts");
            DropForeignKey("dbo.DbElements", "ElementName_Id", "dbo.DbElementNames");
            DropForeignKey("dbo.DbCompounds", "DbChemicalContext_Id", "dbo.DbChemicalContexts");
            DropForeignKey("dbo.DbCompoundTypes", "DbCompound_Id", "dbo.DbCompounds");
            DropForeignKey("dbo.DbCompoundClasses", "DbCompound_Id", "dbo.DbCompounds");
            DropForeignKey("dbo.DbAnnotationContexts", "BiomeContext_Id", "dbo.DbBiomeContexts");
            DropForeignKey("dbo.DbZonoBiomes", "DbBiomeContext_Id", "dbo.DbBiomeContexts");
            DropForeignKey("dbo.DbZonoBiomes", "BiomeZone_Id", "dbo.DbBiomeZones");
            DropForeignKey("dbo.DbZonoBiomes", "BiomeType_Id", "dbo.DbBiomeTypes");
            DropForeignKey("dbo.DbZonoBiomes", "BiomeHumidityType_Id", "dbo.DbBiomeHumidityTypes");
            DropForeignKey("dbo.DbZonoBiomes", "BiomeHemisphere_Id", "dbo.DbBiomeHemispheres");
            DropForeignKey("dbo.DbZonoBiomes", "BiomeContinentalityType_Id", "dbo.DbBiomeContinentalityTypes");
            DropForeignKey("dbo.DbPhysiognomies", "DbBiomeContext_Id", "dbo.DbBiomeContexts");
            DropForeignKey("dbo.DbPhysiognomyTypes", "DbPhysiognomy_Id", "dbo.DbPhysiognomies");
            DropForeignKey("dbo.DbTerrestrialPhysiognomies", "DbPhysiognomyType_Id", "dbo.DbPhysiognomyTypes");
            DropForeignKey("dbo.DbTerrestrialPhysiognomies", "TerrestrialPhysiognomyType_Id", "dbo.DbTerrestrialPhysiognomyTypes");
            DropForeignKey("dbo.DbSemiAquaticPhysiognomies", "DbPhysiognomyType_Id", "dbo.DbPhysiognomyTypes");
            DropForeignKey("dbo.DbSemiAquaticPhysiognomies", "SemiAquaticPhysiognomyType_Id", "dbo.DbSemiAquaticPhysiognomyTypes");
            DropForeignKey("dbo.DbAquaticPhysiognomies", "DbPhysiognomyType_Id", "dbo.DbPhysiognomyTypes");
            DropForeignKey("dbo.DbAquaticPhysiognomies", "PlantCharacterizedAquaticPhysiognomyType_Id", "dbo.DbPlantCharacterizedAquaticPhysiognomyTypes");
            DropForeignKey("dbo.DbAquaticPhysiognomies", "HabitatCharacterizedAquaticPhysiognomy_Id", "dbo.DbHabitatCharacterizedAquaticPhysiognomies");
            DropForeignKey("dbo.DbPedoBiomes", "DbBiomeContext_Id", "dbo.DbBiomeContexts");
            DropForeignKey("dbo.DbPedoBiomes", "PedoBiomeType_Id", "dbo.DbPedoBiomeTypes");
            DropForeignKey("dbo.DbOroBiomes", "DbBiomeContext_Id", "dbo.DbBiomeContexts");
            DropForeignKey("dbo.DbOroBiomes", "OroBiomeType_Id", "dbo.DbOroBiomeTypes");
            DropForeignKey("dbo.DbLandUses", "DbBiomeContext_Id", "dbo.DbBiomeContexts");
            DropForeignKey("dbo.DbLandUses", "LandUseType_Id", "dbo.DbLandUseTypes");
            DropForeignKey("dbo.DbLandUses", "LandUseForm_Id", "dbo.DbLandUseForms");
            DropForeignKey("dbo.DbAccessibleResources", "DbAccessRequest_Id", "dbo.DbAccessRequests");
            DropForeignKey("dbo.DbConversations", "Requester_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbConversations", "Request_Id", "dbo.DbAccessRequests");
            DropForeignKey("dbo.DbConversations", "Receiver_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbMessages", "Sender_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbMessages", "Receiver_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbMessages", "Conversation_Id", "dbo.DbConversations");
            DropForeignKey("dbo.DbAccessibleResources", "Owner_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbUserFiles", "Owner_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbUserDbUserFiles", "DbUserFile_Id", "dbo.DbUserFiles");
            DropForeignKey("dbo.DbUserDbUserFiles", "DbUser_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbRoleDbUsers", "DbUser_Id", "dbo.DbUsers");
            DropForeignKey("dbo.DbRoleDbUsers", "DbRole_Id", "dbo.DbRoles");
            DropForeignKey("dbo.DbRoleDbUserFiles", "DbUserFile_Id", "dbo.DbUserFiles");
            DropForeignKey("dbo.DbRoleDbUserFiles", "DbRole_Id", "dbo.DbRoles");
            DropIndex("dbo.DbUserDbUserFiles", new[] { "DbUserFile_Id" });
            DropIndex("dbo.DbUserDbUserFiles", new[] { "DbUser_Id" });
            DropIndex("dbo.DbRoleDbUsers", new[] { "DbUser_Id" });
            DropIndex("dbo.DbRoleDbUsers", new[] { "DbRole_Id" });
            DropIndex("dbo.DbRoleDbUserFiles", new[] { "DbUserFile_Id" });
            DropIndex("dbo.DbRoleDbUserFiles", new[] { "DbRole_Id" });
            DropIndex("dbo.DbVocabularyUserValues", new[] { "User_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleUsers", new[] { "User_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleUsers", new[] { "AnnotationItem_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleGroups", new[] { "Group_Id" });
            DropIndex("dbo.DbAnnotationItemAccessibleGroups", new[] { "AnnotationItem_Id" });
            DropIndex("dbo.DbOnlineResources", new[] { "DbResource_Id" });
            DropIndex("dbo.DbOnlineResources", new[] { "DataFormat_Id" });
            DropIndex("dbo.DbOfflineResources", new[] { "DbResource_Id" });
            DropIndex("dbo.DbOfflineResources", new[] { "DataFormat_Id" });
            DropIndex("dbo.DbBase64Resource", new[] { "DataFormat_Id" });
            DropIndex("dbo.DbEmbeddedResources", new[] { "DbResource_Id" });
            DropIndex("dbo.DbEmbeddedResources", new[] { "Base64Resource_Id" });
            DropIndex("dbo.DbResources", new[] { "DbAnnotationObject_Id" });
            DropIndex("dbo.DbPositions", new[] { "DbPerson_Id" });
            DropIndex("dbo.DbPersons", new[] { "DbReferences_Id" });
            DropIndex("dbo.DbHosters", new[] { "DbReferences_Id" });
            DropIndex("dbo.DbHosters", new[] { "HosterName_Id" });
            DropIndex("dbo.DbDescriptions", new[] { "DbReferences_Id" });
            DropIndex("dbo.DbRangeStarts", new[] { "DateTime_Id" });
            DropIndex("dbo.DbDateAndTimes", new[] { "Timezone_Id" });
            DropIndex("dbo.DbRangeEnds", new[] { "DateTime_Id" });
            DropIndex("dbo.DbTimeRanges1", new[] { "RangeStart_Id" });
            DropIndex("dbo.DbTimeRanges1", new[] { "RangeEnd_Id" });
            DropIndex("dbo.DbTimeRanges", new[] { "TimeRange_Id" });
            DropIndex("dbo.DbGeologicalTimePeriods", new[] { "DbTimePeriods_Id" });
            DropIndex("dbo.DbGeologicalTimePeriods", new[] { "GeologicalPeriod_Id" });
            DropIndex("dbo.DbGeologicalTimePeriods", new[] { "GeologicalEra_Id" });
            DropIndex("dbo.DbGeologicalTimePeriods", new[] { "GeologicalEpoch_Id" });
            DropIndex("dbo.DbGeologicalTimePeriods", new[] { "GeologicalEon_Id" });
            DropIndex("dbo.DbGeologicalTimePeriods", new[] { "GeologicalAge_Id" });
            DropIndex("dbo.DbTemporalResolutions", new[] { "DbTimeContext_Id" });
            DropIndex("dbo.DbTemporalResolutions", new[] { "TemporalResolutionType_Id" });
            DropIndex("dbo.DbTemporalResolutions", new[] { "TemporalExtentType_Id" });
            DropIndex("dbo.DbTimeContexts", new[] { "TimeRanges_Id" });
            DropIndex("dbo.DbTimeContexts", new[] { "TimePeriods_Id" });
            DropIndex("dbo.DbSoilTextures", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbSoilMorphologies", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbSoilMorphologies", new[] { "SoilMorphologyType_Id" });
            DropIndex("dbo.DbSoilAcidities", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbSoilAcidities", new[] { "SoilAcidityType_Id" });
            DropIndex("dbo.DbNumericSoilLayers", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbNumericSoilLayers", new[] { "MinimumSoilDepthUnit_Id" });
            DropIndex("dbo.DbNumericSoilLayers", new[] { "MaximumSoilDepthUnit_Id" });
            DropIndex("dbo.DbNamedSoilLayers", new[] { "DbSoil_Id" });
            DropIndex("dbo.DbNamedSoilLayers", new[] { "SoilHorizon_Id" });
            DropIndex("dbo.DbPedosphereCompartments", new[] { "DbPedosphere_Id" });
            DropIndex("dbo.DbPedosphereCompartments", new[] { "Soil_Id" });
            DropIndex("dbo.DbNamedSeaZones", new[] { "DbSea_Id" });
            DropIndex("dbo.DbNamedSeaZones", new[] { "PelagicSeaZone_Id" });
            DropIndex("dbo.DbNamedSeaZones", new[] { "BenthicSeaZone_Id" });
            DropIndex("dbo.DbNamedRiverZones", new[] { "DbRiver_Id" });
            DropIndex("dbo.DbNamedRiverZones", new[] { "VerticalRiverZone_Id" });
            DropIndex("dbo.DbNamedRiverZones", new[] { "LongitudinalRiverZone_Id" });
            DropIndex("dbo.DbNamedLakeZones", new[] { "DbLake_Id" });
            DropIndex("dbo.DbNamedLakeZones", new[] { "PelagicLakeZone_Id" });
            DropIndex("dbo.DbNamedLakeZones", new[] { "BenthicLakeZone_Id" });
            DropIndex("dbo.DbHydrosphereCompartments", new[] { "DbHydrosphere_Id" });
            DropIndex("dbo.DbHydrosphereCompartments", new[] { "Sea_Id" });
            DropIndex("dbo.DbHydrosphereCompartments", new[] { "River_Id" });
            DropIndex("dbo.DbHydrosphereCompartments", new[] { "Lake_Id" });
            DropIndex("dbo.DbOrganizationalHierarchies", new[] { "DbEcosphere_Id" });
            DropIndex("dbo.DbOrganizationalHierarchies", new[] { "OrganizationHierarchyName_Id" });
            DropIndex("dbo.DbNumericEcosphereLayers", new[] { "DbEcosphere_Id" });
            DropIndex("dbo.DbNumericEcosphereLayers", new[] { "MinimumVegetationHeightUnit_Id" });
            DropIndex("dbo.DbNumericEcosphereLayers", new[] { "MaximumVegetationHeightUnit_Id" });
            DropIndex("dbo.DbNamedEcosphereLayers", new[] { "DbEcosphere_Id" });
            DropIndex("dbo.DbNamedEcosphereLayers", new[] { "EcosphereLayerName_Id" });
            DropIndex("dbo.DbNumericAtmosphereLayers", new[] { "DbAtmosphere_Id" });
            DropIndex("dbo.DbNumericAtmosphereLayers", new[] { "MinimumAtmosphereHeightUnit_Id" });
            DropIndex("dbo.DbNumericAtmosphereLayers", new[] { "MaximumAtmosphereHeightUnit_Id" });
            DropIndex("dbo.DbNamedAtmosphereLayers", new[] { "DbAtmosphere_Id" });
            DropIndex("dbo.DbNamedAtmosphereLayers", new[] { "AtmosphereLayerName_Id" });
            DropIndex("dbo.DbSpheres", new[] { "DbSphereContext_Id" });
            DropIndex("dbo.DbSpheres", new[] { "Pedosphere_Id" });
            DropIndex("dbo.DbSpheres", new[] { "Hydrosphere_Id" });
            DropIndex("dbo.DbSpheres", new[] { "Ecosphere_Id" });
            DropIndex("dbo.DbSpheres", new[] { "Atmosphere_Id" });
            DropIndex("dbo.DbSpatialResolutions", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbSpatialResolutions", new[] { "SpatialResolutionType_Id" });
            DropIndex("dbo.DbSpatialResolutions", new[] { "SpatialExtentType_Id" });
            DropIndex("dbo.DbLocations", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbLocations", new[] { "LocationType_Id" });
            DropIndex("dbo.DbLocations", new[] { "CountryName_Id" });
            DropIndex("dbo.DbLocations", new[] { "ContinentName_Id" });
            DropIndex("dbo.DbElevations", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbElevations", new[] { "MinimumElevationUnit_Id" });
            DropIndex("dbo.DbElevations", new[] { "MaximumElevationUnit_Id" });
            DropIndex("dbo.DbElevations", new[] { "ElevationDatum_Id" });
            DropIndex("dbo.DbUtmCoordinates", new[] { "UtmCoordinateNorthingUnit_Id" });
            DropIndex("dbo.DbUtmCoordinates", new[] { "UtmCoordinateHemisphere_Id" });
            DropIndex("dbo.DbUtmCoordinates", new[] { "UtmCoordinateGeodeticDatum_Id" });
            DropIndex("dbo.DbUtmCoordinates", new[] { "UtmCoordinateEastingUnit_Id" });
            DropIndex("dbo.DbCoordinates", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbCoordinates", new[] { "UtmCoordinate_Id" });
            DropIndex("dbo.DbBoundingBoxes", new[] { "DbSpaceContext_Id" });
            DropIndex("dbo.DbProcessSubjects", new[] { "DbProcess_Id" });
            DropIndex("dbo.DbProcesses", new[] { "DbProcessContext_Id" });
            DropIndex("dbo.DbProcesses", new[] { "ProcessName_Id" });
            DropIndex("dbo.DbInteractions", new[] { "DbProcessContext_Id" });
            DropIndex("dbo.DbInteractions", new[] { "InteractionQuality_Id" });
            DropIndex("dbo.DbInteractions", new[] { "InteractionPartnerTwo_Id" });
            DropIndex("dbo.DbInteractions", new[] { "InteractionPartnerOne_Id" });
            DropIndex("dbo.DbInteractions", new[] { "InteractionDirection_Id" });
            DropIndex("dbo.DbBotanicalNames", new[] { "HybridFlag_Id" });
            DropIndex("dbo.DbOrganismNames", new[] { "ZoologicalName_Id" });
            DropIndex("dbo.DbOrganismNames", new[] { "ViralName_Id" });
            DropIndex("dbo.DbOrganismNames", new[] { "FungalName_Id" });
            DropIndex("dbo.DbOrganismNames", new[] { "BotanicalName_Id" });
            DropIndex("dbo.DbOrganismNames", new[] { "BacterialName_Id" });
            DropIndex("dbo.DbTaxonomies", new[] { "OrganismName_Id" });
            DropIndex("dbo.DbTaxonomies", new[] { "Order_Id" });
            DropIndex("dbo.DbTaxonomies", new[] { "Kingdom_Id" });
            DropIndex("dbo.DbTaxonomies", new[] { "Family_Id" });
            DropIndex("dbo.DbTaxonomies", new[] { "Domain_Id" });
            DropIndex("dbo.DbTaxonomies", new[] { "Division_Id" });
            DropIndex("dbo.DbTaxonomies", new[] { "Class_Id" });
            DropIndex("dbo.DbOrganism", new[] { "DbOrganismContext_Id" });
            DropIndex("dbo.DbOrganism", new[] { "Taxonomy_Id" });
            DropIndex("dbo.DbFactors", new[] { "DbMethodContext_Id" });
            DropIndex("dbo.DbFactors", new[] { "FactorType_Id" });
            DropIndex("dbo.DbFactors", new[] { "FactorName_Id" });
            DropIndex("dbo.DbApproaches", new[] { "DbMethodContext_Id" });
            DropIndex("dbo.DbApproaches", new[] { "ApproachType_Id" });
            DropIndex("dbo.DbApproaches", new[] { "ApproachLocalization_Id" });
            DropIndex("dbo.DbIsotopes", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbIsotopes", new[] { "IsotopeName_Id" });
            DropIndex("dbo.DbFunctions", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbFunctions", new[] { "ChemicalFunctionType_Id" });
            DropIndex("dbo.DbElements", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbElements", new[] { "ElementName_Id" });
            DropIndex("dbo.DbCompoundTypes", new[] { "DbCompound_Id" });
            DropIndex("dbo.DbCompoundClasses", new[] { "DbCompound_Id" });
            DropIndex("dbo.DbCompounds", new[] { "DbChemicalContext_Id" });
            DropIndex("dbo.DbZonoBiomes", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbZonoBiomes", new[] { "BiomeZone_Id" });
            DropIndex("dbo.DbZonoBiomes", new[] { "BiomeType_Id" });
            DropIndex("dbo.DbZonoBiomes", new[] { "BiomeHumidityType_Id" });
            DropIndex("dbo.DbZonoBiomes", new[] { "BiomeHemisphere_Id" });
            DropIndex("dbo.DbZonoBiomes", new[] { "BiomeContinentalityType_Id" });
            DropIndex("dbo.DbTerrestrialPhysiognomies", new[] { "DbPhysiognomyType_Id" });
            DropIndex("dbo.DbTerrestrialPhysiognomies", new[] { "TerrestrialPhysiognomyType_Id" });
            DropIndex("dbo.DbSemiAquaticPhysiognomies", new[] { "DbPhysiognomyType_Id" });
            DropIndex("dbo.DbSemiAquaticPhysiognomies", new[] { "SemiAquaticPhysiognomyType_Id" });
            DropIndex("dbo.DbAquaticPhysiognomies", new[] { "DbPhysiognomyType_Id" });
            DropIndex("dbo.DbAquaticPhysiognomies", new[] { "PlantCharacterizedAquaticPhysiognomyType_Id" });
            DropIndex("dbo.DbAquaticPhysiognomies", new[] { "HabitatCharacterizedAquaticPhysiognomy_Id" });
            DropIndex("dbo.DbPhysiognomyTypes", new[] { "DbPhysiognomy_Id" });
            DropIndex("dbo.DbPhysiognomies", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbPedoBiomes", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbPedoBiomes", new[] { "PedoBiomeType_Id" });
            DropIndex("dbo.DbOroBiomes", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbOroBiomes", new[] { "OroBiomeType_Id" });
            DropIndex("dbo.DbLandUses", new[] { "DbBiomeContext_Id" });
            DropIndex("dbo.DbLandUses", new[] { "LandUseType_Id" });
            DropIndex("dbo.DbLandUses", new[] { "LandUseForm_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "DbAnnotationObject_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "TimeContext_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "SphereContext_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "SpaceContext_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "ProcessContext_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "OrganismContext_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "MethodContext_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "ChemicalContext_Id" });
            DropIndex("dbo.DbAnnotationContexts", new[] { "BiomeContext_Id" });
            DropIndex("dbo.DbAnnotationObjects", new[] { "References_Id" });
            DropIndex("dbo.DbAnnotationItems", new[] { "Object_Id" });
            DropIndex("dbo.DbMessages", new[] { "Sender_Id" });
            DropIndex("dbo.DbMessages", new[] { "Receiver_Id" });
            DropIndex("dbo.DbMessages", new[] { "Conversation_Id" });
            DropIndex("dbo.DbConversations", new[] { "Requester_Id" });
            DropIndex("dbo.DbConversations", new[] { "Request_Id" });
            DropIndex("dbo.DbConversations", new[] { "Receiver_Id" });
            DropIndex("dbo.DbUserFiles", new[] { "Owner_Id" });
            DropIndex("dbo.DbAccessibleResources", new[] { "DbAccessRequest_Id" });
            DropIndex("dbo.DbAccessibleResources", new[] { "Owner_Id" });
            DropTable("dbo.DbUserDbUserFiles");
            DropTable("dbo.DbRoleDbUsers");
            DropTable("dbo.DbRoleDbUserFiles");
            DropTable("dbo.DbVocabularyValues");
            DropTable("dbo.DbVocabularyUserValues");
            DropTable("dbo.DbUserLogins");
            DropTable("dbo.DbSearchFilterCachedItems");
            DropTable("dbo.DbRemoteVersions");
            DropTable("dbo.DbAnnotationItemAccessibleUsers");
            DropTable("dbo.DbAnnotationItemAccessibleGroups");
            DropTable("dbo.DbOnlineResources");
            DropTable("dbo.DbOfflineResources");
            DropTable("dbo.DbDataFormats");
            DropTable("dbo.DbBase64Resource");
            DropTable("dbo.DbEmbeddedResources");
            DropTable("dbo.DbResources");
            DropTable("dbo.DbPositions");
            DropTable("dbo.DbPersons");
            DropTable("dbo.DbHosterNames");
            DropTable("dbo.DbHosters");
            DropTable("dbo.DbDescriptions");
            DropTable("dbo.DbReferences");
            DropTable("dbo.DbRangeStarts");
            DropTable("dbo.DbTimezones");
            DropTable("dbo.DbDateAndTimes");
            DropTable("dbo.DbRangeEnds");
            DropTable("dbo.DbTimeRanges1");
            DropTable("dbo.DbTimeRanges");
            DropTable("dbo.DbGeologicalPeriods");
            DropTable("dbo.DbGeologicalEras");
            DropTable("dbo.DbGeologicalEpoches");
            DropTable("dbo.DbGeologicalEons");
            DropTable("dbo.DbGeologicalAges");
            DropTable("dbo.DbGeologicalTimePeriods");
            DropTable("dbo.DbTimePeriods");
            DropTable("dbo.DbTemporalResolutionTypes");
            DropTable("dbo.DbTemporalExtentTypes");
            DropTable("dbo.DbTemporalResolutions");
            DropTable("dbo.DbTimeContexts");
            DropTable("dbo.DbSoilTextures");
            DropTable("dbo.DbSoilMorphologyTypes");
            DropTable("dbo.DbSoilMorphologies");
            DropTable("dbo.DbSoilAcidityTypes");
            DropTable("dbo.DbSoilAcidities");
            DropTable("dbo.DbSoilDepthUnits");
            DropTable("dbo.DbNumericSoilLayers");
            DropTable("dbo.DbSoilHorizons");
            DropTable("dbo.DbNamedSoilLayers");
            DropTable("dbo.DbSoils");
            DropTable("dbo.DbPedosphereCompartments");
            DropTable("dbo.DbPedospheres");
            DropTable("dbo.DbPelagicSeaZones");
            DropTable("dbo.DbBenthicSeaZones");
            DropTable("dbo.DbNamedSeaZones");
            DropTable("dbo.DbSeas");
            DropTable("dbo.DbVerticalRiverZones");
            DropTable("dbo.DbLongitudinalRiverZones");
            DropTable("dbo.DbNamedRiverZones");
            DropTable("dbo.DbRivers");
            DropTable("dbo.DbPelagicLakeZones");
            DropTable("dbo.DbBenthicLakeZones");
            DropTable("dbo.DbNamedLakeZones");
            DropTable("dbo.DbLakes");
            DropTable("dbo.DbHydrosphereCompartments");
            DropTable("dbo.DbHydrospheres");
            DropTable("dbo.DbOrganizationHierarchyNames");
            DropTable("dbo.DbOrganizationalHierarchies");
            DropTable("dbo.DbVegetationHeightUnits");
            DropTable("dbo.DbNumericEcosphereLayers");
            DropTable("dbo.DbEcosphereLayerNames");
            DropTable("dbo.DbNamedEcosphereLayers");
            DropTable("dbo.DbEcospheres");
            DropTable("dbo.DbAtmosphereHeightUnits");
            DropTable("dbo.DbNumericAtmosphereLayers");
            DropTable("dbo.DbAtmosphereLayerNames");
            DropTable("dbo.DbNamedAtmosphereLayers");
            DropTable("dbo.DbAtmospheres");
            DropTable("dbo.DbSpheres");
            DropTable("dbo.DbSphereContexts");
            DropTable("dbo.DbSpatialResolutionTypes");
            DropTable("dbo.DbSpatialResolutions");
            DropTable("dbo.DbLocationTypes");
            DropTable("dbo.DbCountryNames");
            DropTable("dbo.DbContinentNames");
            DropTable("dbo.DbLocations");
            DropTable("dbo.DbElevationUnits");
            DropTable("dbo.DbElevationDatums");
            DropTable("dbo.DbElevations");
            DropTable("dbo.DbUtmCoordinateHemispheres");
            DropTable("dbo.DbUtmCoordinateGeodeticDatums");
            DropTable("dbo.DbUtmCoordinateUnits");
            DropTable("dbo.DbUtmCoordinates");
            DropTable("dbo.DbCoordinates");
            DropTable("dbo.DbBoundingBoxes");
            DropTable("dbo.DbSpaceContexts");
            DropTable("dbo.DbProcessSubjects");
            DropTable("dbo.DbProcessNames");
            DropTable("dbo.DbProcesses");
            DropTable("dbo.DbInteractionQualities");
            DropTable("dbo.DbInteractionPartners");
            DropTable("dbo.DbInteractionDirections");
            DropTable("dbo.DbInteractions");
            DropTable("dbo.DbProcessContexts");
            DropTable("dbo.DbZoologicalNames");
            DropTable("dbo.DbViralNames");
            DropTable("dbo.DbFungalNames");
            DropTable("dbo.DbHybridFlags");
            DropTable("dbo.DbBotanicalNames");
            DropTable("dbo.DbBacterialNames");
            DropTable("dbo.DbOrganismNames");
            DropTable("dbo.DbOrders");
            DropTable("dbo.DbKingdoms");
            DropTable("dbo.DbFamilies");
            DropTable("dbo.DbDomains");
            DropTable("dbo.DbDivisions");
            DropTable("dbo.DbClasses");
            DropTable("dbo.DbTaxonomies");
            DropTable("dbo.DbOrganism");
            DropTable("dbo.DbOrganismContexts");
            DropTable("dbo.DbFactorTypes");
            DropTable("dbo.DbFactorNames");
            DropTable("dbo.DbFactors");
            DropTable("dbo.DbApproachTypes");
            DropTable("dbo.DbApproachLocalizations");
            DropTable("dbo.DbApproaches");
            DropTable("dbo.DbMethodContexts");
            DropTable("dbo.DbIsotopeNames");
            DropTable("dbo.DbIsotopes");
            DropTable("dbo.DbChemicalFunctionTypes");
            DropTable("dbo.DbFunctions");
            DropTable("dbo.DbElementNames");
            DropTable("dbo.DbElements");
            DropTable("dbo.DbCompoundTypes");
            DropTable("dbo.DbCompoundClasses");
            DropTable("dbo.DbCompounds");
            DropTable("dbo.DbChemicalContexts");
            DropTable("dbo.DbBiomeZones");
            DropTable("dbo.DbBiomeTypes");
            DropTable("dbo.DbBiomeHumidityTypes");
            DropTable("dbo.DbBiomeHemispheres");
            DropTable("dbo.DbBiomeContinentalityTypes");
            DropTable("dbo.DbZonoBiomes");
            DropTable("dbo.DbTerrestrialPhysiognomyTypes");
            DropTable("dbo.DbTerrestrialPhysiognomies");
            DropTable("dbo.DbSemiAquaticPhysiognomyTypes");
            DropTable("dbo.DbSemiAquaticPhysiognomies");
            DropTable("dbo.DbPlantCharacterizedAquaticPhysiognomyTypes");
            DropTable("dbo.DbHabitatCharacterizedAquaticPhysiognomies");
            DropTable("dbo.DbAquaticPhysiognomies");
            DropTable("dbo.DbPhysiognomyTypes");
            DropTable("dbo.DbPhysiognomies");
            DropTable("dbo.DbPedoBiomeTypes");
            DropTable("dbo.DbPedoBiomes");
            DropTable("dbo.DbOroBiomeTypes");
            DropTable("dbo.DbOroBiomes");
            DropTable("dbo.DbLandUseTypes");
            DropTable("dbo.DbLandUseForms");
            DropTable("dbo.DbLandUses");
            DropTable("dbo.DbBiomeContexts");
            DropTable("dbo.DbAnnotationContexts");
            DropTable("dbo.DbAnnotationObjects");
            DropTable("dbo.DbAnnotationItems");
            DropTable("dbo.DbMessages");
            DropTable("dbo.DbConversations");
            DropTable("dbo.DbAccessRequests");
            DropTable("dbo.DbRoles");
            DropTable("dbo.DbUserFiles");
            DropTable("dbo.DbUsers");
            DropTable("dbo.DbAccessibleResources");
        }
    }
}
