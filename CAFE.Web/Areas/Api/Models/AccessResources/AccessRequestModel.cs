using System.Collections.Generic;

namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    /// <summary>
    /// Model of request that user manage
    /// </summary>
    public class AccessRequestModel
    {
        public AccessRequestModel()
        {
            RequestedResources = new List<AccessibleResourceModel>();
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
        public System.DateTime CreationDate { get; set; }
        /// <summary>
        /// Resources that user requested
        /// </summary>
        public IEnumerable<AccessibleResourceModel> RequestedResources { get; set; }
        /// <summary>
        /// Conversations associated with this request
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Access request status reason
        /// </summary>
        public string StatusReason { get; set; }
    }
}