
using System;
using CAFE.Core.Security;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CAFE.Core.Resources
{
    /// <summary>
    /// Contract for accessible resources
    /// </summary>
    /// 
    public enum AccessModes
    {
        Private,
        Explicit,
        Internal,
        Public
    }

    public interface IAccessibleResourceDescriptor
    {
        /// <summary>
        /// Unique identifier of resource
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// Resource owner
        /// </summary>

        AccessModes AccessMode { get; set; }
        string OwnerId { get; set; }
        string OwnerName { get; set; }
    }
}
