
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    /// <summary>
    /// Conversation that opens when one user request resource of another user
    /// </summary>
    public class DbConversation : DbBase
    {
        public DbConversation()
        {
            Messages = new List<DbMessage>();
        }

        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        /// <summary>
        /// Is unread messages in the conversation
        /// </summary>
        public bool HasRecieverUnreadMessages { get; set; }

        /// <summary>
        /// Access request to which that conversation
        /// </summary>
        [Required]
        public virtual DbAccessRequest Request { get; set; }

        /// <summary>
        /// Messages that was created within this conversation
        /// </summary>
        public virtual ICollection<DbMessage> Messages { get; set; }

        /// <summary>
        /// Initiator. User that make a request
        /// </summary>
        [Required]
        public virtual DbUser Requester { get; set; }
        
        /// <summary>
        /// Receiver. User that receive a request
        /// </summary>
        [Required]
        public virtual DbUser Receiver { get; set; }

        /// <summary>
        /// Status of request
        /// </summary>
        [Required]
        public DbAccessRequestStatus Status { get; set; }
        /// <summary>
        /// Access request status reason
        /// </summary>
        public string StatusReason { get; set; }
    }
}
