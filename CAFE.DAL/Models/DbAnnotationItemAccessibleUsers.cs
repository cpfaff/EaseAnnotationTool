using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    [Table("DbAnnotationItemAccessibleUsers")]
    public class DbAnnotationItemAccessibleUsers: DbBase
    {
        /// <summary>
        /// AnnotationItem model
        /// </summary>
        [Key, Required]
        public Guid Id { get; set; }

        /// <summary>
		/// AnnotationItem's model
		/// </summary>

        public virtual DbAnnotationItem AnnotationItem { get; set; }

        /// <summary>
        /// User's model
        /// </summary>
        public virtual DbUser User { get; set; }
    }
}
