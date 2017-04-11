using CAFE.Core.Messaging;
using CAFE.Core.Security;
using CAFE.Web.Areas.Api.Models.AccessResources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class AccessRequestsViewModel: ConversationModel
    {
        public enum AccessRequestsType
        {
            Incoming,
            Outgoing
        }
        public AccessRequestsType Type { get; set; }
    }
}