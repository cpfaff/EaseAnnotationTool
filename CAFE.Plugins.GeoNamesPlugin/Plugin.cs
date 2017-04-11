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
    /// <summary>
    /// Test Vocabulary source plugin implementation
    /// </summary>
    [Export(typeof(IVocabularyExtenalSourcePlugin))]
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
                    String.Format("http://api.geonames.org/search?username={0}&name_startsWith={1}", userName, search);

                var xml = XElement.Load(request);
                var results = xml.Descendants("geoname");
                
                if(elementName != "Country")
                    return results.Where(g => g.Element("fcode").Value != "PCLI").Select(g => new VocabularyValue { Value = g.Element("name").Value, Uri = string.Empty }).ToList();
                else
                    return results.Select(g => new VocabularyValue { Value = g.Element("name").Value, Uri = string.Empty }).ToList();
            }
            catch (Exception ex) { throw; }
        }

        public void RegisterMapConfigs(IMapperConfigurationExpression c)
        {

        }
    }
}
