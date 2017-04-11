
using System.Collections.Generic;

namespace CAFE.Web.Areas.Api.Models.Search
{
    public class SearchRequestModel
    {
        public string SearchItemsType { get; set; } = "";
        public string OrderBy { get; set; } = "";
        public string SearchText { get; set; } = "";
        public IEnumerable<SearchRequestFilterModel> Filters { get; set; } = new List<SearchRequestFilterModel>();

    }
}