using System.Collections.Generic;
using CAFE.Core.Resources;

namespace CAFE.Core.Security
{
    /// <summary>
    /// Contract for Access requests service that manage of access for resources
    /// </summary>
    public interface IAccessRequestService
    {
        /// <summary>
        /// Make new access request for collection of resources
        /// </summary>
        /// <param name="subject">Text subject for receiver</param>
        /// <param name="message">Text message for receiver</param>
        /// <param name="resources">List of asking resources</param>
        /// <param name="requester">User that sending request</param>
        /// <returns></returns>
        AccessRequest MakeRequest(string subject, string message, IEnumerable<AccessibleResource> resources,
            User requester);

        /// <summary>
        /// Returns access request by identifier
        /// </summary>
        /// <param name="accessRequestId">Access request identifier</param>
        /// <returns>Found access request</returns>
        AccessRequest GetAccessRequest(long accessRequestId);
    }
}
