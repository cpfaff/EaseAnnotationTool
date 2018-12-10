using AutoMapper;
using CAFE.Core.Integration;
using CAFE.Core.Plugins;
using CAFE.Core.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace CAFE.Plugins.GeoNamesPlugin
{
    public static class Extension
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source,Func<TSource, TKey> keySelector)
        {
            return
                source
                    ?.GroupBy(keySelector)
                    .Select(grp => grp.First());
        }
    }
    /// <summary>
    /// Test Vocabulary source plugin implementation
    /// </summary>
    [Export(typeof(IExternalSourcePlugin))]
    public class Plugin : IVocabularyExtenalSourcePlugin
    {
        /// <summary>
        /// Returns type that plugin for
        /// </summary>
        public System.Type ForType => typeof(CountryNameVocabulary);
        /// <summary>
        /// Return external vocabulary values
        /// </summary>
        /// <returns>Collection of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetValues()
        {
            return new List<VocabularyValue>();
        }

        public Dictionary<LocationTypeVocabulary, List<string>> _locationTypeCodes = new Dictionary<LocationTypeVocabulary, List<string>>
        {
            { LocationTypeVocabulary.Administrative_Division, new List<string> { "ADM2", "ADM2H", "ADM3", "ADM3H", "ADM4", "ADM4H", "ADM5", "ADMD", "ADMDH"} },
            { LocationTypeVocabulary.Country, new List<string> { "PCLI" } },
            { LocationTypeVocabulary.Forest, new List<string> {  "FRST", "FRSTF" } },
            { LocationTypeVocabulary.Human_Settlement, new List<string> { "PPL", "PPLA", "PPLA2", "PPLA3", "PPLA4", "PPLC", "PPLCH", "PPLF", "PPLG", "PPLH", "PPLL", "PPLQ", "PPLR", "PPLS", "PPLW", "PPLX", "STLMT" } },
            { LocationTypeVocabulary.Lake, new List<string> { "LBED", "LK", "LKC", "LKI", "LKN", "LKNI", "LKO", "LKOI", "LKS", "LKSB", "LKSC", "LKSI", "LKSNI", "LKX" } },
            { LocationTypeVocabulary.Mountain, new List<string> { "MT", "MTS" } },
            { LocationTypeVocabulary.Nature_Reserve, new List<string> { "RESN" } },
            { LocationTypeVocabulary.Park, new List<string> { "PRK" } },
            { LocationTypeVocabulary.Region, new List<string> { "RGN", "RGNE", "RGNH", "RGNL" } },
            { LocationTypeVocabulary.Sea, new List<string> { "SEA" } },
            { LocationTypeVocabulary.State, new List<string> { "ADM1", "ADM1H" } },
            { LocationTypeVocabulary.Stream, new List<string> { "STM", "STMB", "STMC", "STMD", "STMH", "STMI", "STMIX", "STMM", "STMQ", "STMS", "STMX" } }
        };

        public Dictionary<ContinentNameVocabulary, string> _continentCodes = new Dictionary<ContinentNameVocabulary, string>
        {
            { ContinentNameVocabulary.Africa, "AF"},
            { ContinentNameVocabulary.Antarctica, "AN" },
            { ContinentNameVocabulary.Asia, "AS" },
            { ContinentNameVocabulary.Europe, "EU" },
            { ContinentNameVocabulary.North_America, "NA" },
            { ContinentNameVocabulary.Oceania, "OC" },
            { ContinentNameVocabulary.South_America , "SA" }
        };

        /// <summary>
        /// Return external vocabulary values
        /// </summary>
        /// <returns>Collection of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetValuesExtended(string search, string elementName)
        {
            try
            {
                var userName = "cafeprojecttester";

                var request = elementName == "Country" ?
                    String.Format("http://api.geonames.org/search?username={0}&name_startsWith={1}&featureCode=PCLI", userName, search) :
                    String.Format("http://api.geonames.org/search?username={0}&name_startsWith={1}&style=FULL", userName, search);

                var xml = XElement.Load(request);
                var results = xml.Descendants("geoname");

                if (elementName != "Country")
                {
                    var allowedLocationTypes = _locationTypeCodes.SelectMany(l => l.Value).ToList();

                    return
                        results.
                        Where(g => allowedLocationTypes.Contains(g.Element("fcode").Value)).
                        Select(g => 
                        {
                            var continent = 
                                _continentCodes.Any(l => !string.IsNullOrEmpty(g.Element("continentCode")?.Value) && l.Value == g.Element("continentCode").Value) ? _continentCodes.Single(l => l.Value == g.Element("continentCode").Value).Key.ToString().Replace("_", " ") : string.Empty;
                            var locationType = 
                                _locationTypeCodes.Any(l => !string.IsNullOrEmpty(g.Element("fcode")?.Value) && l.Value.Contains(g.Element("fcode")?.Value)) ? _locationTypeCodes.Single(l => l.Value.Contains(g.Element("fcode").Value)).Key.ToString().Replace("_", " ") : string.Empty;
                            var country = g.Element("countryName").Value;
                            var name = g.Element("name").Value;
                            var item = new VocabularyValue
                            {
                                Value = name,
                                FullValue = $"{name} ({locationType}, {country}, {continent})",
                                Uri = string.Empty,
                                Country = country,
                                ContinentName = continent,
                                LocationType = locationType
                            };

                            return item;
                        }).
                        DistinctBy(g => new { g.Value, g.ContinentName, g.Country, g.LocationType }).
                        ToList();
                }
                else
                    return results.Select(g => new VocabularyValue { Value = g.Element("name").Value, Uri = string.Empty }).ToList();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void RegisterMapConfigs(IMapperConfigurationExpression c)
        {

        }

        /// <summary>
        /// Init plugin
        /// </summary>
        /// <returns>Indicator that says plugin init successfull or not</returns>
        public bool Init()
        {
            return true;
        }
    }
}
