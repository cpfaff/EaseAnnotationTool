using System.Configuration;

namespace CAFE.Plugins.GlobalNameResolveService
{
    /// <summary>
    /// Web/App.config section for store global and common parameters configuration
    /// </summary>
    public class GlobalNameServiceConfigurationSection : ConfigurationSection
    {
        /// <summary>
        /// (optional)
        /// A pipe-delimited list of data sources. See a the list of data sources(http://resolver.globalnames.org/data_sources)
        /// </summary>
        [ConfigurationProperty("DataSourceIds", IsRequired = false)]
        public string DataSourceIds
        {
            get
            {
                return (string)this["DataSourceIds"];
            }
            set
            {
                this["DataSourceIds"] = value;
            }
        }


        /// <summary>
        /// (optional)
        /// Find the first available match instead of matches across all data sources 
        /// with all possible renderings of a name. When 'true', response is rapid but incomplete.
        /// </summary>
        [ConfigurationProperty("ResolveOnce", DefaultValue = false, IsRequired = false)]
        public bool ResolveOnce
        {
            get
            {
                return (bool)this["ResolveOnce"];
            }
            set
            {
                this["ResolveOnce"] = value;
            }
        }


        /// <summary>
        /// Returns just one result with the highest score.
        /// </summary>
        [ConfigurationProperty("BestMatchOnly", DefaultValue = false, IsRequired = false)]
        public bool BestMatchOnly
        {
            get
            {
                return (bool)this["BestMatchOnly"];
            }
            set
            {
                this["BestMatchOnly"] = value;
            }
        }


        /// <summary>
        /// A pipe-delimited list of data sources (see data_source_ids parameter). 
        /// Creates a new section in results -- 'preferred_results' in addtion to 'results'. 
        /// Preferred results contain only data received from requested data sources.When used togther with 'best_match_only' 
        /// returnes only one highest scored result per a preffered data source.
        /// The resolution is still performed according to 'data_source_id' parameter.
        /// </summary>
        [ConfigurationProperty("PreferredDataSources", IsRequired = false)]
        public string PreferredDataSources
        {
            get
            {
                return (string)this["PreferredDataSources"];
            }
            set
            {
                this["PreferredDataSources"] = value;
            }
        }


        /// <summary>
        /// (optional)
        /// Reduce the likelihood of matches to taxonomic homonyms. When 'true' a common taxonomic context is calculated 
        /// for all supplied names from matches in data sources that have classification tree paths. 
        /// Names out of determined context are penalized during score calculation.
        /// </summary>
        [ConfigurationProperty("WithContext", DefaultValue = true, IsRequired = false)]
        public bool WithContext
        {
            get
            {
                return (bool)this["WithContext"];
            }
            set
            {
                this["WithContext"] = value;
            }
        }


        /// <summary>
        /// (optional)
        /// Return 'vernacular' field to present common names provided by a data source for a particular match
        /// </summary>
        [ConfigurationProperty("WithVernaculars", DefaultValue = false, IsRequired = false)]
        public bool WithVernaculars
        {
            get
            {
                return (bool)this["WithVernaculars"];
            }
            set
            {
                this["WithVernaculars"] = value;
            }
        }


        /// <summary>
        /// (optional)
        /// Returns 'canonical_form' with infraspecific ranks, if they are present.
        /// </summary>
        [ConfigurationProperty("WithCanonicalRanks", DefaultValue = false, IsRequired = false)]
        public bool WithCanonicalRanks
        {
            get
            {
                return (bool)this["WithCanonicalRanks"];
            }
            set
            {
                this["WithCanonicalRanks"] = value;
            }
        }

        /// <summary>
        /// Indicate that can changes
        /// </summary>
        /// <returns></returns>
        public override bool IsReadOnly()
        {
            return false;
        }
    }

}
