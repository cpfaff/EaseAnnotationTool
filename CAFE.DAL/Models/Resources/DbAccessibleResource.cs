
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    /// <summary>
    /// Resource which user accessed
    /// </summary>
    public class DbAccessibleResource : DbBase
    {
        /// <summary>
        /// Accessible resource unique identifier
        /// </summary>
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// Kind of accessible resource
        /// </summary>
        [Required]
        public DbAccessibleResourceKind Kind { get; set; }
        /// <summary>
        /// Specific resource kind unique identifier
        /// </summary>
        [Required]
        public Guid ResourceId { get; set; }
        /// <summary>
        /// Resource owner
        /// </summary>
        [Required]
        public virtual DbUser Owner { get; set; }
    }
}
