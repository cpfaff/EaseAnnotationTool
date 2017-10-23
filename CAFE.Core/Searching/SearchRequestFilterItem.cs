
using System.Collections.Generic;

namespace CAFE.Core.Searching
{
    public class SearchRequestFilterItem
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Tooltip { get; set; }
        public FilterType FilterType { get; set; }
        public string ValueType { get; set; }
        public SearchRequestFilterValue Value { get; set; }
        public List<SearchRequestFilterValue> Values { get; set; } = new List<SearchRequestFilterValue>();
        //public List<SearchRequestFilterItem> RelatedFilters { get; set; } = new List<SearchRequestFilterItem>();
    }

}
