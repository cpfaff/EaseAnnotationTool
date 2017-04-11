
using System.Collections.Generic;

namespace CAFE.Core.Searching
{
    public class SearchRequestFilterItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public FilterType FilterType { get; set; }
        public SearchRequestFilterValue Value { get; set; }

        public List<SearchRequestFilterItem> RelatedFilters { get; set; } = new List<SearchRequestFilterItem>();
    }
}
