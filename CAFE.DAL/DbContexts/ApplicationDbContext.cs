using System.Data.Entity;
using CAFE.DAL.Models;
using CAFE.DAL.Models.Resources;
using System;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace CAFE.DAL.DbContexts
{
	public class ApplicationDbContext : DbContext
	{
        public ApplicationDbContext()
            : base("name=DefaultConnection")
        {
            base.Configuration.AutoDetectChangesEnabled = true;
            //base.Configuration.ProxyCreationEnabled = true;
            //base.Configuration.LazyLoadingEnabled = false;
            base.Configuration.ProxyCreationEnabled = true;
            base.Configuration.LazyLoadingEnabled = true;
        }

        /// <summary>
        /// Users datatable
        /// </summary>
        public DbSet<DbUser> Users { get; set; }

        /// <summary>
		/// Roles database
		/// </summary>
		public DbSet<DbRole> Roles { get; set; }

        /// <summary>
        /// User's logins database
        /// </summary>
        public DbSet<DbUserLogin> UserLogins { get; set; }

        /// <summary>
        /// Annotation items
        /// </summary>
        public DbSet<DbAnnotationItem> AnnotationItems { get; set; }

        /// <summary>
        /// Access requests
        /// </summary>
        public DbSet<DbAccessRequest> AccessRequests { get; set; }

        /// <summary>
        /// Accessible resources by Access requests
        /// </summary>
        public DbSet<DbAccessibleResource> AccessibleResources { get; set; }

        /// <summary>
        /// Users conversations
        /// </summary>
        public DbSet<DbConversation> Conversations { get; set; }

        /// <summary>
        /// Messages in conversation
        /// </summary>
        public DbSet<DbMessage> Messages { get; set; }

        /// <summary>
        /// UserFiles in conversation
        /// </summary>
        public DbSet<DbUserFile> UserFiles { get; set; }

        /// <summary>
        /// Global vocabulary values
        /// </summary>
        public DbSet<DbVocabularyValue> VocabularyValues { get; set; }

        /// <summary>
        /// User defined vocabulary values
        /// </summary>
        public DbSet<DbVocabularyUserValue> VocabularyUserValues { get; set; }

        /// <summary>
        /// Versions of application on the remove storage
        /// </summary>
        public DbSet<DbRemoteVersion> RemoveVersions { get; set; }

        /// <summary>
        /// Cached search filter items
        /// </summary>
        public DbSet<DbSearchFilterCachedItem> SearchFilterCachedItems { get; set; }

        /// <summary>
        /// Annotation Item Accessible Groups
        /// </summary>
        public DbSet<DbAnnotationItemAccessibleGroups> DbAnnotationItemAccessibleGroups { get; set; }

        /// <summary>
        /// Annotation Item Accessible Users
        /// </summary>
        public DbSet<DbAnnotationItemAccessibleUsers> DbAnnotationItemAccessibleUsers { get; set; }

        /// <summary>
        /// Descriptions for each Annotation schema element
        /// </summary>
        public DbSet<DbSchemaItemDescription> SchemaItemDescriptions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
	    {
            modelBuilder.Properties<DateTime>().Configure(c => c.HasColumnType("datetime2"));

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            base.OnModelCreating(modelBuilder);
	    }
	}
}
