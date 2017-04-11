
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CAFE.DAL.Models
{
    /// <summary>
    /// Access request data model
    /// </summary>
    public class DbAccessRequest : DbBase
    {
        public DbAccessRequest()
        {
            RequestedResources = new List<DbAccessibleResource>();
            Conversations = new List<DbConversation>();
        }

        /// <summary>
        /// Unique identifier
        /// </summary>
        [Key, Required, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        /// <summary>
        /// Subject that user provider for receiver
        /// </summary>
        [Required]
        public string RequestSubject { get; set; }
        /// <summary>
        /// Short message or description that user provider for receiver
        /// </summary>
        //[Required]
        public string RequestMessage { get; set; }

        /// <summary>
        /// Date and time when request was created
        /// </summary>
        [Required]
        public System.DateTime CreationDate { get; set; }
        /// <summary>
        /// Resources that user requested
        /// </summary>
        public virtual ICollection<DbAccessibleResource> RequestedResources { get; set; }
        /// <summary>
        /// Conversations associated with this request
        /// </summary>
        public virtual ICollection<DbConversation> Conversations { get; set; }

    }
}
