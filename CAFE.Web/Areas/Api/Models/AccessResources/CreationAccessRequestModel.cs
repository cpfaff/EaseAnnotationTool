
using System.Collections.Generic;

namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    /// <summary>
    /// Model of request that user creates for get access of interesting resource
    /// </summary>
    public class CreationAccessRequestModel
    {
        public CreationAccessRequestModel()
        {
            RequestedResources = new List<AccessibleResourceModel>();
        }
        /// <summary>
        /// Subject that user provider for receiver
        /// </summary>
        public string RequestSubject { get; set; }
        /// <summary>
        /// Short message or description that user provider for receiver
        /// </summary>
        public string RequestMessage { get; set; }
        /// <summary>
        /// Resources that user requested
        /// </summary>
        public IEnumerable<AccessibleResourceModel> RequestedResources { get; set; }

    }
}