using System;
using System.Collections.Generic;
using CAFE.Core.Resources;

namespace CAFE.Core.Security
{
    public class UserFile : IAccessibleResourceDescriptor
    {
        public UserFile()
        {
            AcceptedUsers = new List<User>();
            AcceptedGroups = new List<Group>();
        }

        public enum FilePermission
        {
            Owner,
            Read,
        }

        public enum FileType
        {
            Audio,
            Image,
            Tabular,
            Video,
            Other
        }

        /// <summary>
        /// Files unique identifier
        /// </summary>
        public Guid Id { get; set; }


        /// <summary>
        /// Identity number
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Files name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Files creation date
        /// </summary>
        public DateTime CreationDate { get; set; }

        /// <summary>
        /// Files discriminator
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Files discriminator
        /// </summary>
        public FilePermission Permission { get; set; }

        /// <summary>
        /// Files owner
        /// </summary>
        public virtual User Owner { get; set; }

        /// <summary>
        /// File's type
        /// </summary>
        public FileType Type { get; set; }

        /// <summary>
        /// Files owner
        /// </summary>
        public AccessModes AccessMode { get; set; }

        /// <summary>
        /// Users, which have access to file
        /// </summary>
        public virtual List<User> AcceptedUsers { get; set; }
        public virtual List<Group> AcceptedGroups { get; set; }

        public string OwnerId { get; set; }
        public string OwnerName { get; set; }
    }
}
