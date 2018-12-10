using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    [Table("DbUserHiddenHelpers")]
    public class DbUserHiddenHelper : DbBase
    {
        /// <summary>
        /// Helper's unique identifier
        /// </summary>
        [Key, Required]
        public Guid Id { get; set; }

        /// <summary>
        /// Helper's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User's ID
        /// </summary>
        [ForeignKey("User")]
        public Guid UserId { get; set; }

        /// <summary>
        /// User
        /// </summary>
        public virtual DbUser User { get; set; }
    }
}
