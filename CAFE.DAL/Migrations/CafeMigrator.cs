
using System;
using System.Data.Entity.Migrations;
using System.Linq;

namespace CAFE.DAL.Migrations
{
    public class Migrator : DbMigrator
    {
        public Migrator() : base(new Configuration())
        {
        }
    }

    public static class CafeMigrator
    {
        public static void Migrate()
        {
            try
            {
                var migrator = new Migrator();
                if (migrator.GetPendingMigrations().Any())
                    migrator.Update();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
