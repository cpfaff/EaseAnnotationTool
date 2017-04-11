
using System.Collections.Generic;

namespace CAFE.Web.Areas.Api.Models.Search
{
    public class SearchRequestFilterModel
    {
        public SearchRequestFilterModel()
        {
            RelatedFilterModels = new List<SearchRequestFilterModel>();
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public string FilterType { get; set; }
        public string Value { get; set; }

        public List<SearchRequestFilterModel> RelatedFilterModels { get; set; }

    }
}