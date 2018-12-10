
using Newtonsoft.Json;

namespace CAFE.Plugins.GlobalNameResolveService
{
    [JsonObject(MemberSerialization.OptIn)]
    public class RequestData
    {
        ///// <summary>
        ///// List of names delimited by either pipe "|" or tab "\t". Use a pipe for GET requests
        ///// </summary>
        //[JsonProperty(PropertyName = "names", NullValueHandling = NullValueHandling.Ignore)]
        //public string Names { get; set; }

        /// <summary>
        /// List of names delimited by new lines "\n". new lines. You may optionally supply your local id for each name as:
        ///     123|Parus major
        ///     125|Parus thruppi
        ///     126|Parus carpi
        /// Names in the response will contain your supplied ids, facilitating integration. You can also upload files using 
        /// a multipart POST request (see example below) with names and ids organized as in the example above.
        /// </summary>
        [JsonProperty(PropertyName = "data", NullValueHandling = NullValueHandling.Ignore)]
        public string Data { get; set; }

        [JsonProperty(PropertyName = "commit", NullValueHandling = NullValueHandling.Ignore)]
        public string Commit { get; set; } = "Resolve Names";

        ///// <summary>
        ///// (optional)
        ///// A pipe-delimited list of data sources. See a the list of data sources(http://resolver.globalnames.org/data_sources)
        ///// </summary>
        //[JsonProperty(PropertyName = "data_source_ids", NullValueHandling = NullValueHandling.Ignore)]
        ////public int[] DataSourceIds { get; set; }
        //public string DataSourceIds { get; set; }

        ///// <summary>
        ///// (optional)
        ///// Find the first available match instead of matches across all data sources 
        ///// with all possible renderings of a name. When 'true', response is rapid but incomplete.
        ///// </summary>
        //[JsonProperty(PropertyName = "resolve_once")]
        //public bool ResolveOnce { get; set; }


        ///// <summary>
        ///// Returns just one result with the highest score.
        ///// </summary>
        //[JsonProperty(PropertyName = "best_match_only")]
        //public bool BestMatchOnly { get; set; }


        ///// <summary>
        ///// A pipe-delimited list of data sources (see data_source_ids parameter). 
        ///// Creates a new section in results -- 'preferred_results' in addtion to 'results'. 
        ///// Preferred results contain only data received from requested data sources.When used togther with 'best_match_only' 
        ///// returnes only one highest scored result per a preffered data source.
        ///// The resolution is still performed according to 'data_source_id' parameter.
        ///// </summary>
        //[JsonProperty(PropertyName = "preferred_data_sources", NullValueHandling = NullValueHandling.Ignore)]
        //public string PreferredDataSources { get; set; }


        ///// <summary>
        ///// (optional)
        ///// Reduce the likelihood of matches to taxonomic homonyms. When 'true' a common taxonomic context is calculated 
        ///// for all supplied names from matches in data sources that have classification tree paths. 
        ///// Names out of determined context are penalized during score calculation.
        ///// </summary>
        //[JsonProperty(PropertyName = "with_context")]
        //public bool WithContext { get; set; }


        ///// <summary>
        ///// (optional)
        ///// Return 'vernacular' field to present common names provided by a data source for a particular match
        ///// </summary>
        //[JsonProperty(PropertyName = "with_vernaculars")]
        //public bool WithVernaculars { get; set; }


        ///// <summary>
        ///// (optional)
        ///// Returns 'canonical_form' with infraspecific ranks, if they are present.
        ///// </summary>
        //[JsonProperty(PropertyName = "with_canonical_ranks")]
        //public bool WithCanonicalRanks { get; set; }
    }
}
