using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    [Table("DbUserFiles")]
    public class DbUserFile : DbBase
    {
        public DbUserFile()
        {
            AcceptedUsers = new List<DbUser>();
            AcceptedGroups = new List<DbRole>();
        }
        public enum DbFileAccessMode
        {
            Private,
            Explicit,
            Internal,
            Public
        }

        public enum DbFileType
        {
            Audio,
            Image,
            Tabular,
            Video,
            Other
        }

        /// <summary>
        /// File's unique identifier
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// DB Unique Identity number
        /// </summary>
        [Key, Required]
        public int FileId { get; set; }

        /// <summary>
        /// File's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// File's creation date
        /// </summary>
        public System.DateTime CreationDate { get; set; }

        /// <summary>
        /// File's discriminator
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// File's owner
        /// </summary>
        public virtual DbUser Owner { get; set; }

        /// <summary>
        /// File's type
        /// </summary>
        public DbFileType Type { get; set; }

        /// <summary>
        /// User's, which have access to file
        /// </summary>
        public DbFileAccessMode AccessMode { get; set; }

        public virtual ICollection<DbUser> AcceptedUsers { get; set; }

        public virtual ICollection<DbRole> AcceptedGroups { get; set; }
    }
}
