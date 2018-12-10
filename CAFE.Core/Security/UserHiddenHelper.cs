using System;
using System.Collections.Generic;
using CAFE.Core.Resources;

namespace CAFE.Core.Security
{
    public class UserHiddenHelper
    {
        /// <summary>
        /// Helper's unique identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Helper's name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// User's ID
        /// </summary>
        public Guid UserId { get; set; }
    }
}
