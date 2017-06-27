namespace CAFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AfterPluralizingOff2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.DbUser", newName: "DbUsers");
            RenameTable(name: "dbo.DbUserFile", newName: "DbUserFiles");
            RenameTable(name: "dbo.DbRole", newName: "DbRoles");
            RenameTable(name: "dbo.DbSearchFilterCachedItem", newName: "DbSearchFilterCachedItems");
            RenameTable(name: "dbo.DbUserLogin", newName: "DbUserLogins");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.DbUserLogins", newName: "DbUserLogin");
            RenameTable(name: "dbo.DbSearchFilterCachedItems", newName: "DbSearchFilterCachedItem");
            RenameTable(name: "dbo.DbRoles", newName: "DbRole");
            RenameTable(name: "dbo.DbUserFiles", newName: "DbUserFile");
            RenameTable(name: "dbo.DbUsers", newName: "DbUser");
        }
    }
}
