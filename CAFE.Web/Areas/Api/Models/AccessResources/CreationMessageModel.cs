
namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    /// <summary>
    /// Model for creation Message of conversation
    /// </summary>
    public class CreationMessageModel
    {
        /// <summary>
        /// Message text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Conversation unique id within creating message
        /// </summary>
        public long ConversationId { get; set; }
    }
}