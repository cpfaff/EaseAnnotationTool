using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models
{
    public class DeleteUsersModel
    {
        public List<string> UsersIds { get; set; }
        public bool RemoveOwnData { get; set; }
    }
}