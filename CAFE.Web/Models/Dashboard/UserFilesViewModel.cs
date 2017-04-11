using CAFE.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CAFE.Web.Models.Dashboard
{
    public class UserFilesViewModel
    {
        public UserFilesViewModel()
        {
            SelectedUsersAndGroups = new List<UsersAndGroupsSearchResultsModel>();
        }
        /// <summary>
        /// Files unique identifier
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Files name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Files creation date
        /// </summary>
        public string CreationDate { get; set; }

        /// <summary>
        /// Files discriminator
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Files owner
        /// </summary>
        public User Owner { get; set; }

        /// <summary>
        /// File's type
        /// </summary>
        public UserFile.FileType Type { get; set; }

        /// <summary>
        /// Files owner
        /// </summary>
        public Core.Resources.AccessModes AccessMode { get; set; }

        /// <summary>
        /// File dowload link
        /// </summary>
        public string DownloadURL { get; set; }

        /// <summary>
        /// File permission
        /// </summary>
        public string Permission { get; set; }

        /// <summary>
        /// Users, which have access to file
        /// </summary>
        public List<UsersAndGroupsSearchResultsModel> SelectedUsersAndGroups { get; set; }

    }
}