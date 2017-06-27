
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    [Table("DbUserAnnotationItems")]
    /// <summary>
    /// Data model class for relate DbUser and AnnotationItem
    /// </summary>
    public class DbUserAnnotationItem : DbBase
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key, Required]
        public long Id { get; set; }

        /// <summary>
        /// User reference
        /// </summary>
        [Required]
        public virtual DbUser User { get; set; }

        /// <summary>
        /// Annotation item reference
        /// </summary>
        [Required]
        public virtual DbAnnotationItem AnnotationItem { get; set; }
    }
}
