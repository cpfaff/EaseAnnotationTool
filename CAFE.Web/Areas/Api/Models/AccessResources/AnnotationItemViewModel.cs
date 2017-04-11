using CAFE.Core.Security;
using CAFE.Web.Models.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Areas.Api.Models.AccessResources
{
    public class AnnotationItemViewModel
    {
        public Web.Models.AnnotationItemModel AnnotationItem { get;set;}
        public Dictionary<string, string> FilesNames { get; set; }
        public List<UsersAndGroupsSearchResultsModel> AcceptedUsersAndGroups { get; set; }
        public bool IsAccessible { get; set; } = false;
    }
}