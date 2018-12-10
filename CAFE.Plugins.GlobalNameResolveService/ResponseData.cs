
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CAFE.Plugins.GlobalNameResolveService
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ResponseData
    {
        /// <summary>
        /// Resolver request id. Your request is stored temporarily in the database and is assigned an id.
        /// </summary>
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Using the url you can access your results for 7 days.
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        ///// <summary>
        ///// A list of data source ids you used for name resolution. If no data sources were given the list is empty.
        ///// </summary>
        //[JsonProperty(PropertyName = "data_sources")]
        //public int[] DataSources { get; set; }

        /// <summary>
        /// Appears if 'with_context' parameter is set to true.
        /// </summary>
        [JsonProperty(PropertyName = "context")]
        public List<ContextData> Context { get; set; } = new List<ContextData>();

        /// <summary>
        /// A container for the resolution data.
        /// </summary>
        [JsonProperty(PropertyName = "data")]
        public List<DataItem> Data { get; set; } = new List<DataItem>();

        /// <summary>
        /// The final status of the request -- 'success' or 'failure'
        /// </summary>
        [JsonProperty(PropertyName = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Message associated with the status
        /// </summary>
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "parameters")]
        public Parameters Pars { get; set; }

        [JsonObject(MemberSerialization.OptIn)]
        public class ContextData
        {
            /// <summary>
            /// The id of a data source used to create the context.
            /// </summary>
            [JsonProperty(PropertyName = "context_data_source_id")]
            public string ContextDataSourceId { get; set; }

            /// <summary>
            /// A lowest taxonomic level in the data source that contains 90% or more of all names found. 
            /// If there are too few names to determine, this element remains empty.
            /// </summary>
            [JsonProperty(PropertyName = "context_clade")]
            public string ContextClade { get; set; }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class DataItem
        {
            /// <summary>
            /// The name string in the query.
            /// </summary>
            [JsonProperty(PropertyName = "supplied_name_string")]
            public string SuppliedNameString { get; set; }

            /// <summary>
            /// True if name was found by exact match, or by matching the name's canonical form 
            /// (without authors etc). False otherwise.
            /// </summary>
            [JsonProperty(PropertyName = "is_known_name")]
            public bool IsKnownName { get; set; }

            /// <summary>
            /// The id of the name string in the query (if provided).
            /// </summary>
            [JsonProperty(PropertyName = "supplied_id")]
            public string SuppliedId { get; set; }

            /// <summary>
            /// A container for displaying results for a particular name string.
            /// </summary>
            [JsonProperty(PropertyName = "results")]
            public List<DataItemResult> Results { get; set; } = new List<DataItemResult>();

            [JsonProperty(PropertyName = "data_sources_number")]
            public int DataSourcesNumber { get; set; }

            [JsonProperty(PropertyName = "in_curated_sources")]
            public bool InCuratedSources { get; set; }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class DataItemResult
        {
            /// <summary>
            /// The id of the data source where a name was found.
            /// </summary>
            [JsonProperty(PropertyName = "data_source_id")]
            public int DataSourceId { get; set; }

            /// <summary>
            /// Data source user friendly name
            /// </summary>
            [JsonProperty(PropertyName = "data_source_title")]
            public string DataSourceTitle { get; set; }

            /// <summary>
            /// An identifier for the found name string used in Global Names.
            /// </summary>
            [JsonProperty(PropertyName = "gni_uuid")]
            public string GniUuid { get; set; }

            /// <summary>
            /// The name string found in this data source.
            /// </summary>
            [JsonProperty(PropertyName = "name_string")]
            public string NameString { get; set; }

            /// <summary>
            /// A "canonical" version of the name generated by the Global Names parser
            /// </summary>
            [JsonProperty(PropertyName = "canonical_form")]
            public string CanonicalForm { get; set; }

            /// <summary>
            /// Tree path to the root if a name string was found within a data source classification.
            /// </summary>
            [JsonProperty(PropertyName = "classification_path")]
            public string ClassificationPath { get; set; }

            /// <summary>
            /// 
            /// </summary>
            [JsonProperty(PropertyName = "classification_path_ranks")]
            public string ClassificationPathRanks { get; set; }

            /// <summary>
            /// Same tree path using taxon_ids (see below)
            /// </summary>
            [JsonProperty(PropertyName = "classification_path_ids")]
            public string ClassificationPathIds { get; set; }

            /// <summary>
            /// An identifier supplied in the source Darwin Core Archive for the name string record
            /// </summary>
            [JsonProperty(PropertyName = "taxon_id")]
            public string TaxonId { get; set; }

            [JsonProperty(PropertyName = "edit_distance")]
            public int EditDistance { get; set; }

            [JsonProperty(PropertyName = "imported_at")]
            public string ImportedAt { get; set; }

            //[JsonProperty(PropertyName = "vernaculars")]
            //public List<string> Vernaculars { get; set; } = new List<string>();

            /// <summary>
            /// Explains how resolver found the name. If the resolver cannot find names corresponding to the entire 
            /// queried name string, it sequentially removes terminal portions of the name string until a match is found.
            ///     1 - Exact match
            ///     2 - Exact match by canonical form of a name
            ///     3 - Fuzzy match by canonical form
            ///     4 - Partial exact match by species part of canonical form
            ///     5 - Partial fuzzy match by species part of canonical form
            ///     6 - Exact match by genus part of a canonical form
            /// </summary>
            [JsonProperty(PropertyName = "match_type")]
            public int MatchType { get; set; }

            [JsonProperty(PropertyName = "match_value")]
            public string MatchValue { get; set; }

            /// <summary>
            /// Displays points used to calculate the score delimited by '|' -- "Match points|Author match points|Context points". 
            /// Negative points decrease the final result.
            /// </summary>
            [JsonProperty(PropertyName = "prescore")]
            public string Prescore { get; set; }

            /// <summary>
            /// A confidence score calculated for the match. 0.5 means an uncertain result that will require investigation. 
            /// Results higher than 0.9 correspond to 'good' matches. Results between 0.5 and 0.9 should be taken with caution. 
            /// Results less than 0.5 are likely poor matches. The scoring is described in more details on the About page
            /// </summary>
            [JsonProperty(PropertyName = "score")]
            public float Score { get; set; }
        }

        [JsonObject(MemberSerialization.OptIn)]
        public class Parameters
        {
            [JsonProperty(PropertyName = "with_context")]
            public bool WithContext { get; set; }

            [JsonProperty(PropertyName = "header_only")]
            public bool HeaderOnly { get; set; }

            [JsonProperty(PropertyName = "with_canonical_ranks")]
            public bool WithCanonicalRanks { get; set; }

            [JsonProperty(PropertyName = "with_vernaculars")]
            public bool WithVernaculars { get; set; }

            [JsonProperty(PropertyName = "best_match_only")]
            public bool BestMatchOnly { get; set; }

            [JsonProperty(PropertyName = "data_sources")]
            public int[] DataSources { get; set; }

            [JsonProperty(PropertyName = "preferred_data_sources")]
            public List<string> PreferredDataSources { get; set; } = new List<string>();

            [JsonProperty(PropertyName = "resolve_once")]
            public bool ResolveOnce { get; set; }
        }
    }

}
