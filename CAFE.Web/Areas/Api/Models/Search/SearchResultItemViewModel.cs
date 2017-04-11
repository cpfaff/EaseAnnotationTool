
using System;

namespace CAFE.Web.Areas.Api.Models.Search
{
    public class SearchResultItemViewModel
    {
        public string ItemType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CreationDate { get; set; }
        public Guid ItemId { get; set; }
        public bool IsAccessible { get; set; }
        public string AccessMode { get; set; }
    }
}