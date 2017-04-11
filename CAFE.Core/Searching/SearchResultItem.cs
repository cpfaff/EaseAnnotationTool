
using System;
using CAFE.Core.Resources;
using CAFE.Core.Security;

namespace CAFE.Core.Searching
{
    /// <summary>
    /// Item of search result. Contains common information of found data
    /// </summary>
    public class SearchResultItem
    {
        /// <summary>
        /// Found item type
        /// </summary>
        public SearchResultItemType ItemType { get; set; }

        /// <summary>
        /// Name of found item
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Link to show found item
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Description of found item
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Indecate that item is accessible for requester
        /// </summary>
        public bool IsAccessible { get; set; }

        public AccessModes AccessMode { get; set; }

        /// <summary>
        /// Date and time when resource was created
        /// </summary>
        public DateTime? CreationDate { get; set; }

        /// <summary>
        /// Unique identifier of found data
        /// </summary>
        public Guid ItemId { get; set; }

        /// <summary>
        /// User that owns of item
        /// </summary>
        public User Owner { get; set; }
    }
}
