using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    public class DbRole : DbBase
    {
        public DbRole()
        {
            Users = new List<DbUser>();
            AccessibleFiles = new List<DbUserFile>();
        }


        /// <summary>
        /// Role unique identifier
        /// </summary>
        [Key, Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Role name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Is entity Group
        /// </summary>
        public bool? IsGroup { get; set; }

        /// <summary>
        /// Role's discriminator
        /// </summary>
        public string Discriminator { get; set; }

        public virtual ICollection<DbUser> Users  { get; set; }

        public virtual ICollection<DbUserFile> AccessibleFiles { get; set; }
       // public virtual ICollection<DbAnnotationItemAccessibleGroups> AccessibleAnnotationItems { get; set; }
    }
}
