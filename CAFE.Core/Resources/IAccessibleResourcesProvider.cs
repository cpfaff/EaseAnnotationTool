
using System;

namespace CAFE.Core.Resources
{
    /// <summary>
    /// Provide access to resource knowing only resource kind and resource unique identifier
    /// </summary>
    public interface IAccessibleResourcesProvider
    {
        /// <summary>
        /// Returns resource by resource kind and unique identifier
        /// </summary>
        /// <param name="resourceKind">Resource kind</param>
        /// <param name="resourceId">Resource unique identifier</param>
        /// <returns>AccessibleResource</returns>
        AccessibleResource GetResource(AccessibleResourceKind resourceKind, Guid resourceId);
    }
}
