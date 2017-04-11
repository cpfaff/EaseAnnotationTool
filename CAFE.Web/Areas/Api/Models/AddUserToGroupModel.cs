using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class AddUserToGroupModel
    {
        public List<string> userAddedIds { get; set; }
        public List<string> userDeletedIds { get; set; }
        public string groupId { get; set; }
        public string groupName { get; set; }
    }
}