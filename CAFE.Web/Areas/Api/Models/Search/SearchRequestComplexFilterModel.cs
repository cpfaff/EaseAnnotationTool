
using System.Collections.Generic;

namespace CAFE.Web.Areas.Api.Models.Search
{
    public class SearchRequestComplexFilterModel
    {
        public string Name { get; set; }
        public List<SearchRequestFilterModel> Items { get; set; } = new List<SearchRequestFilterModel>();

    }
}