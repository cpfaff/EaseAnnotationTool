namespace CAFE.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixDbPersonDBstructure : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DbPerson", "SalutationValues", c => c.String());
            AddColumn("dbo.DbPerson", "GivenNameValues", c => c.String());
            AddColumn("dbo.DbPerson", "SurNameValues", c => c.String());
            AddColumn("dbo.DbPerson", "EmailAddressValues", c => c.String());
            AddColumn("dbo.DbPerson", "PhoneNumberValues", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.DbPerson", "PhoneNumberValues");
            DropColumn("dbo.DbPerson", "EmailAddressValues");
            DropColumn("dbo.DbPerson", "SurNameValues");
            DropColumn("dbo.DbPerson", "GivenNameValues");
            DropColumn("dbo.DbPerson", "SalutationValues");
        }
    }
}
