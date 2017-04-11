using Microsoft.AspNet.Identity;
using System.Collections.Generic;
using CAFE.Core.Integration;

namespace CAFE.Core.Security
{
    /// <summary>
    /// Application-scoped role
    /// </summary>
    public class Group : IRole<string>
    {
        public Group()
        {
            IsGroup = true;
            Users = new List<User>();
            AccessibleFiles = new List<UserFile>();
        }

        public string Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Is entity Group
        /// </summary>
        public bool? IsGroup { get; set; }

        /// <summary>
        /// Role's discriminator
        /// </summary>
        public string Discriminator { get; set; }

        public List<User> Users { get; set; }
        public List<UserFile> AccessibleFiles { get; set; }
        public List<AnnotationItemAccessibleGroups> AccessibleAnnotationItems { get; set; }
    }
}
