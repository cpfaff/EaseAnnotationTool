
using System;
using System.Collections.Generic;
using CAFE.Core.Configuration;

namespace CAFE.Core.Searching
{
    /// <summary>
    /// Represents predefined complex filter scope(group) composed from search filters
    /// </summary>
    public class SearchRequestComplexFilter
    {
        /// <summary>
        /// Name of filter group
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Search filters that compose this complex group
        /// </summary>
        public List<SearchRequestFilterItem> Items { get; set; } = new List<SearchRequestFilterItem>();


        /// <summary>
        /// Convert ComplexFilterScope to SearchRequestComplexFilter
        /// </summary>
        /// <param name="filterScope">ComplexFilterScope</param>
        /// <returns>SearchRequestComplexFilter instance</returns>
        public static SearchRequestComplexFilter FromConfig(ComplexFilterScope filterScope)
        {
            var complexFilter = new SearchRequestComplexFilter();
            complexFilter.Name = filterScope.Type;
            complexFilter.Items = new List<SearchRequestFilterItem>();

            
            //enumerate compound filter kes
            foreach (var filterElement in filterScope.ComplexFiltersCollection)
            {
                var filter = new SearchRequestFilterItem();
                filter.Name = string.Concat(filterScope.BasePath, ".", filterElement.Property.ToString());
                filter.Description = filterElement.Description.ToString();
                filter.FilterType = (FilterType)Enum.Parse(typeof(FilterType), filterElement.Type.ToString());

                complexFilter.Items.Add(filter);
            }

            return complexFilter;
        }
    }

    public static class ComplexFiltersExtensions
    {

        /// <summary>
        /// Convert ComplexFilterScope collection to SearchRequestComplexFilter collection
        /// </summary>
        /// <param name="source">ComplexFilterScope collection</param>
        /// <returns>SearchRequestComplexFilter collection</returns>
        public static IEnumerable<SearchRequestComplexFilter> ToDomainList(this IEnumerable<ComplexFilterScope> source)
        {
            if(source == null) throw new ArgumentException("ConfigurationScope collection is null");

            var result = new List<SearchRequestComplexFilter>();
            foreach (var filterScope in source)
            {
                result.Add(SearchRequestComplexFilter.FromConfig(filterScope));
            }

            return result;
        }
    }
}

