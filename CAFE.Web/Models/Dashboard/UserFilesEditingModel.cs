using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Models.Dashboard
{
    public class UserFilesEditingModel
    {
        public string Id { get; set; }
        public List<UsersAndGroupsSearchResultsModel> UsersAndGroups { get; set; }

        public Core.Resources.AccessModes AccessMode { get; set; }

        public string Description { get; set; }
    }
}