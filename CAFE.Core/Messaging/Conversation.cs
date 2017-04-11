using System.Collections.Generic;
using CAFE.Core.Security;

namespace CAFE.Core.Messaging
{
    /// <summary>
    /// Conversation that opens when one user request resource of another user
    /// </summary>
    public class Conversation
    {
        public Conversation()
        {
            Messages = new List<Message>();
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
        public User Requester { get; set; }
        /// <summary>
        /// Receiver. User that receive a request
        /// </summary>
        public User Receiver { get; set; }
        /// <summary>
        /// Access request to which that conversation
        /// </summary>
        public AccessRequest Request { get; set; }
        /// <summary>
        /// Messages that was created within this conversation
        /// </summary>
        public IEnumerable<Message> Messages { get; set; }
        /// <summary>
        /// Status of request
        /// </summary>
        public AccessRequestStatus Status { get; set; }
        /// <summary>
        /// Access request status reason
        /// </summary>
        public string StatusReason { get; set; }
    }
}
