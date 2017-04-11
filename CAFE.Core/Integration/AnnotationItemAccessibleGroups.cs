using CAFE.Core.Integration;
using CAFE.Core.Security;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.Core.Integration
{
    public class AnnotationItemAccessibleGroups
    {
        /// <summary>
        /// AnnotationItem model
        /// </summary>
        public string Id { get; set; }

        /// <summary>
		/// AnnotationItem's model
		/// </summary>

        public AnnotationItem AnnotationItem { get; set; }

        /// <summary>
        /// User's model
        /// </summary>
        public Group Group { get; set; }
    }
}
