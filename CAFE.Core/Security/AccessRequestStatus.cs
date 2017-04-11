
namespace CAFE.Core.Security
{
    /// <summary>
    /// Status of Access request
    /// </summary>
    public enum AccessRequestStatus
    {
        /// <summary>
        /// When create new AccessRequest it's status is Open
        /// </summary>
        Open,
        /// <summary>
        /// When user accepted AccessRequest
        /// </summary>
        Accepted,
        /// <summary>
        /// When user declined AccessRequest
        /// </summary>
        Declined
    }
}
