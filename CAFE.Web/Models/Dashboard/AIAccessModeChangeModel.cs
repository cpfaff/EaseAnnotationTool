using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Models.Dashboard
{
    public class AIAccessModeChangeModel
    {
        public string[] Ids { get; set; }

        public List<UsersAndGroupsSearchResultsModel> UsersAndGroups { get; set; }

        public Core.Resources.AccessModes AccessMode { get; set; }
    }
}