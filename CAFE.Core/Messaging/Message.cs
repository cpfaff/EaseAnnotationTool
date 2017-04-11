
using System;
using CAFE.Core.Security;

namespace CAFE.Core.Messaging
{
    /// <summary>
    /// Access request conversation message
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Date and time when message was created
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Message text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// User that sended this message
        /// </summary>
        public User Sender { get; set; }
        /// <summary>
        /// User that received this message
        /// </summary>
        public User Receiver { get; set; }
    }
}
