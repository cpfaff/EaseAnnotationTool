namespace CAFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UIElementsadded : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DbUIElement",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ElementIdOnUI = c.String(),
                        UrlForGetData = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.DbUIElement");
        }
    }
}
