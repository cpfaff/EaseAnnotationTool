
using System;

namespace CAFE.Core.Resources
{
    /// <summary>
    /// Access wrapper for requested resource
    /// </summary>
    public class AccessibleResource
    {
        /// <summary>
        /// Accessible resource unique identifier
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Unique identifier
        /// </summary>
        public Guid ResourceId { get; set; }
        /// <summary>
        /// Accessible resource content (or simple "Resource")
        /// </summary>
        public IAccessibleResourceDescriptor Content { get; set; }
        /// <summary>
        /// Kind of resource content (or simple "Resource")
        /// </summary>
        public AccessibleResourceKind Kind { get; set; }
    }
}
