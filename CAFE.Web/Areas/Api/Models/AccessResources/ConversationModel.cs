
using System.Collections.Generic;

namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    /// <summary>
    /// Model of conversation that opens when one user request resource of another user
    /// </summary>
    public class ConversationModel
    {
        public ConversationModel()
        {
            Messages = new List<MessageModel>();
            Resources = new Dictionary<string, string>();
        }

        /// <summary>
        /// Unique identifier
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Is unread messages in the conversation
        /// </summary>
        public bool HasRecieverUnreadMessages { get; set; }
        /// <summary>
        /// Initiator. User that make a request
        /// </summary> 
        public string Requester { get; set; }
        /// <summary>
        /// Initiator's identifier
        /// </summary>
        public string RequesterId { get; set; }
        /// <summary>
        /// Receiver. User that receive a request
        /// </summary>
        public string Receiver { get; set; }
        /// <summary>
        /// Receiver's identifier
        /// </summary>
        public string ReceiverId { get; set; }
        /// <summary>
        /// Access request unique identifier to which that conversation
        /// </summary>
        public long RequestId { get; set; }
        /// <summary>
        /// Messages that was created within this conversation
        /// </summary>
        public IEnumerable<MessageModel> Messages { get; set; }
        /// <summary>
        /// Status of request
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Access request subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Access request creation date
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// Access request status reason
        /// </summary>
        public string StatusReason { get; set; }
        /// <summary>
        /// Name of resources ralated with this conversation
        /// </summary>
        public Dictionary<string, string> Resources { get; set; } 
    }
}