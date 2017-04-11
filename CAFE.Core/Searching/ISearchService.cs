using System.Collections.Generic;
using System.Threading.Tasks;
using CAFE.Core.Security;
using System;

namespace CAFE.Core.Searching
{
    public interface ISearchService
    {
        Task<IEnumerable<SearchResultItem>> SearchWithFiltersAsync(User user, SearchRequest searchRequest);

        Task<IEnumerable<SearchRequestFilterItem>> GetFilterParametersAsync(SearchResultItemType itemType, bool needToClear);

        Task<IEnumerable<string>> GetSelectValuesAsync(SearchRequestFilterItem filterItem, string userId);
        Task<IEnumerable<SearchResultItem>> SearchForAutocompleteAsync(string query, string UserId);

        bool CheckThatFileIsAccessible(Guid fileId, Guid userId);
        bool CheckThatAnnotationItemsIsAccessible(Guid annotationItemId, Guid userId);
    }
}
