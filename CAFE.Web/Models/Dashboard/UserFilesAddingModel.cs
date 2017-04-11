using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Models.Dashboard
{
    public class UserFilesAddingModel
    {
        public enum FileAccessMode
        {
            Private,
            Explicit,
            Internal,
            Public
        }
        public List<UsersAndGroupsSearchResultsModel> UsersAndGroups { get; set; }

        public FileAccessMode AccessMode { get; set; }

        public string Description { get; set; }

       public System.Net.Http.Formatting.FormDataCollection Files { get; set; }
    }
}