
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using CAFE.Core.Searching;
using CAFE.Core.Security;
using CAFE.Web.Areas.Api.Models.Search;
using Microsoft.AspNet.Identity;

namespace CAFE.Web.Areas.Api.Controllers
{
    //[Authorize]
    [AllowAnonymous]
    public class SearchController : ApiController
    {
        private readonly ISearchService _searchService;
        private readonly ISecurityServiceAsync _securityService;

        public SearchController(ISearchService searchService, ISecurityServiceAsync securityService)
        {
            _searchService = searchService;
            _securityService = securityService;
        }

        [HttpPost]
        public async Task<IEnumerable<SearchResultItemViewModel>> SearchItems([FromBody]SearchRequestModel model)
        {
            var mappedRequest = Mapper.Map(model, new SearchRequest());
            var searchResult =
                await
                    _searchService.SearchWithFiltersAsync(
                        await _securityService.GetUserByIdAsync(User.Identity.GetUserId()), mappedRequest);

            var mappedResult =
                Mapper.Map<IEnumerable<SearchResultItem>, IEnumerable<SearchResultItemViewModel>>(searchResult);
            return mappedResult;
        }

        [HttpGet]
        public async Task<IEnumerable<SearchResultItemViewModel>> SearchItems(string itemsType = "All", string orderBy = "-CreationDate", string searchText = "",
            SearchRequestFilterUrlParameters filters = null)
        {
            SearchRequestModel searchRequestModel;
            try
            {
                searchRequestModel = await ConvertUrlParametersToSearchModelAsync(itemsType, searchText, orderBy, filters);
            }
            catch (Exception e)
            {
                throw;
            }
            var mappedRequest = Mapper.Map(searchRequestModel, new SearchRequest());
            IEnumerable<SearchResultItem> searchResult;
            try
            {
                searchResult =
                    await
                        _searchService.SearchWithFiltersAsync(
                            await _securityService.GetUserByIdAsync(User.Identity.GetUserId()), mappedRequest);
            }
            catch (Exception e)
            {
                throw;
            }

            var mappedResult =
                Mapper.Map<IEnumerable<SearchResultItem>, IEnumerable<SearchResultItemViewModel>>(searchResult);
            return mappedResult;
        }

        [HttpGet]
        public async Task<IEnumerable<SearchRequestFilterModel>> GetFilters(string itemsType = "All")
        {
            var serchResultItemType = (SearchResultItemType) Enum.Parse(typeof (SearchResultItemType), itemsType);
            var parameters = await _searchService.GetFilterParametersAsync(serchResultItemType, true);

            var mappedParameters =
                Mapper.Map<IEnumerable<SearchRequestFilterItem>, IEnumerable<SearchRequestFilterModel>>(parameters);

            //Fill additional related filter models
            foreach(var filter in parameters)
            {
                if(filter.RelatedFilters.Count > 0)
                {
                    var matchedMappedFilter = mappedParameters.FirstOrDefault(f => f.Name == filter.Name);
                    foreach(var relatedFilter in filter.RelatedFilters)
                    {
                        matchedMappedFilter.RelatedFilterModels.Add(Mapper.Map<SearchRequestFilterModel>(relatedFilter));
                    }
                }
            }
            return mappedParameters;
        }

        [HttpPost]
        public async Task<IEnumerable<SearchFilterSelectionNamedModel>> GetSelectValuesForFilter(SearchRequestFilterModel filter)
        {
            var searchRequestFilter = Mapper.Map(filter, new SearchRequestFilterItem());

            var result = await _searchService.GetSelectValuesAsync(searchRequestFilter, System.Web.HttpContext.Current.User.Identity.GetUserId());

            return Mapper.Map<IEnumerable<string>, IEnumerable<SearchFilterSelectionNamedModel>>(result);
        }

        [HttpGet]
        public async Task<IEnumerable<QuickSearchResultItemModel>> SearchForAutocomplete(string q)
        {
            var result = await _searchService.SearchForAutocompleteAsync(q, System.Web.HttpContext.Current.User.Identity.GetUserId());

            return Mapper.Map<IEnumerable<QuickSearchResultItemModel>>(result);
        }


        private async Task<SearchRequestModel> ConvertUrlParametersToSearchModelAsync(string itemsType, string searchText, string orderBy, SearchRequestFilterUrlParameters filters)
        {
            var searchRequestModel = new SearchRequestModel();
            searchRequestModel.SearchItemsType = itemsType;
            searchRequestModel.SearchText = searchText;
            searchRequestModel.OrderBy = orderBy;

            var serchResultItemType = (SearchResultItemType)Enum.Parse(typeof(SearchResultItemType), itemsType);
            var parameters =  (await _searchService.GetFilterParametersAsync(serchResultItemType, false)).ToList();

            if (filters != null)
            {
                foreach (var filter in filters)
                {
                    var foundFilter = parameters.Where(f => f.Name == filter.Key).FirstOrDefault();

                    ((List<SearchRequestFilterModel>)searchRequestModel.Filters).Add(new SearchRequestFilterModel()
                    {
                        FilterType = foundFilter.FilterType.ToString(),
                        Name = filter.Key,
                        Value = filter.Value
                    });
                }
            }

            return searchRequestModel;
        }
    }
}