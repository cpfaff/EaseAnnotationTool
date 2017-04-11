
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace CAFE.Web.Areas.Api.Models.Search
{
    [ModelBinder(typeof(SearchFilerUrlParametersModelBinder))]
    public class SearchRequestFilterUrlParameters : List<KeyValuePair<string,string>>
    {

    }
}