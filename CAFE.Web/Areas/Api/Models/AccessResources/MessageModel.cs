
using System;

namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    /// <summary>
    /// Model of access request conversation message
    /// </summary>
    public class MessageModel
    {
        /// <summary>
        /// Date and time when message was created
        /// </summary>
        public string CreationDate { get; set; }
        /// <summary>
        /// Message text
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// User that sended this message
        /// </summary>
        public string Sender { get; set; }
        /// <summary>
        /// User that received this message
        /// </summary>
        public string Receiver { get; set; }
    }
}