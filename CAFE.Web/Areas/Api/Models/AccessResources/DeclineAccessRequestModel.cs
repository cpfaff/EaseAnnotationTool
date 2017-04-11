
namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    /// <summary>
    /// Model used when user declines access request
    /// </summary>
    public class DeclineAccessRequestModel
    {
        /// <summary>
        /// Access request identifier
        /// </summary>
        public long ConversationId { get; set; }

        /// <summary>
        /// Decline decision reason
        /// </summary>
        public string Reason { get; set; }
    }
}