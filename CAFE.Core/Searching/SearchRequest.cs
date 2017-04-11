﻿
using System.Collections.Generic;

namespace CAFE.Core.Searching
{
    public class SearchRequest
    {
        public SearchResultItemType ItemType { get; set; }
        public string OrderBy { get; set; } = "";
        public string SearchText { get; set; } = "";
        public IEnumerable<SearchRequestFilterItem> Filters { get; set; } = new List<SearchRequestFilterItem>();

    }
}
