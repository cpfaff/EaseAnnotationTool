
using CAFE.Core.Configuration;

namespace CAFE.Plugins.GlobalNameResolveService
{
    public class GlobalNameServiceConfiguration : IConfiguration
    {
        /// <summary>
        /// (optional)
        /// A pipe-delimited list of data sources. See a the list of data sources(http://resolver.globalnames.org/data_sources)
        /// </summary>
        public string DataSourceIds { get; set; }


        /// <summary>
        /// (optional)
        /// Find the first available match instead of matches across all data sources 
        /// with all possible renderings of a name. When 'true', response is rapid but incomplete.
        /// </summary>
        public bool ResolveOnce { get; set; }


        /// <summary>
        /// Returns just one result with the highest score.
        /// </summary>
        public bool BestMatchOnly { get; set; }


        /// <summary>
        /// A pipe-delimited list of data sources (see data_source_ids parameter). 
        /// Creates a new section in results -- 'preferred_results' in addtion to 'results'. 
        /// Preferred results contain only data received from requested data sources.When used togther with 'best_match_only' 
        /// returnes only one highest scored result per a preffered data source.
        /// The resolution is still performed according to 'data_source_id' parameter.
        /// </summary>
        public string PreferredDataSources { get; set; }


        /// <summary>
        /// (optional)
        /// Reduce the likelihood of matches to taxonomic homonyms. When 'true' a common taxonomic context is calculated 
        /// for all supplied names from matches in data sources that have classification tree paths. 
        /// Names out of determined context are penalized during score calculation.
        /// </summary>
        public bool WithContext { get; set; }


        /// <summary>
        /// (optional)
        /// Return 'vernacular' field to present common names provided by a data source for a particular match
        /// </summary>
        public bool WithVernaculars { get; set; }


        /// <summary>
        /// (optional)
        /// Returns 'canonical_form' with infraspecific ranks, if they are present.
        /// </summary>
        public bool WithCanonicalRanks { get; set; }

    }
}
