
using System;
using System.Collections.Generic;
using CAFE.Core.Messaging;
using CAFE.Core.Resources;

namespace CAFE.Core.Security
{
    /// <summary>
    /// Request that user creates for get access of interesting resource
    /// </summary>
    public class AccessRequest
    {
        public AccessRequest()
        {
            RequestedResources = new List<AccessibleResource>();
        }

        /// <summary>
        /// Unique identifier
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Subject that user provider for receiver
        /// </summary>
        public string RequestSubject { get; set; }
        /// <summary>
        /// Short message or description that user provider for receiver
        /// </summary>
        public string RequestMessage { get; set; }
        /// <summary>
        /// Date and time when request was created
        /// </summary>
        public DateTime CreationDate { get; set; }
        /// <summary>
        /// Resources that user requested
        /// </summary>
        public IEnumerable<AccessibleResource> RequestedResources { get; set; }
        /// <summary>
        /// Conversations associated with this request
        /// </summary>
        public IEnumerable<Conversation> Conversations { get; set; } 
    }
}
