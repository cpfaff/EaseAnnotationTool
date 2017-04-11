
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    /// <summary>
    /// Access request conversation message
    /// </summary>
    public class DbMessage : DbBase
    {
        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// Date and time when message was created
        /// </summary>
        [Required]
        public System.DateTime CreationDate { get; set; }
        /// <summary>
        /// Message text
        /// </summary>
        [Required]
        public string Text { get; set; }
        /// <summary>
        /// User that sended this message
        /// </summary>
        [Required]
        public virtual DbUser Sender { get; set; }

        /// <summary>
        /// User that received this message
        /// </summary>
        [Required]
        public virtual DbUser Receiver { get; set; }

        /// <summary>
        /// Associated conversation
        /// </summary>
        [Required]
        public virtual DbConversation Conversation { get; set; }
    }
}
