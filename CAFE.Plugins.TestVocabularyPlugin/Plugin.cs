using System.Collections.Generic;
using System.ComponentModel.Composition;
using CAFE.Core.Integration;
using CAFE.Core.Plugins;
using CAFE.Core.Resources;
using Type = System.Type;
using AutoMapper;

namespace CAFE.Plugins.TestVocabularyPlugin
{

    /// <summary>
    /// Test Vocabulary source plugin implementation
    /// </summary>
    [Export(typeof(IExternalSourcePlugin))]
    public class Plugin : IVocabularyExtenalSourcePlugin
    {
        /// <summary>
        /// Returns type that plugin for
        /// </summary>
        public Type ForType => typeof (ApproachLocalizationVocabulary);

        /// <summary>
        /// Return external vocabulary values
        /// </summary>
        /// <returns>Collection of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetValues()
        {
            return new[]
            {
                new VocabularyValue {Value = "Value 1", Uri = "http://blablabla/1"},
                new VocabularyValue {Value = "Value 2", Uri = "http://blablabla/2"}
            };
        }
        public IEnumerable<VocabularyValue> GetValuesExtended(string search, string elementName)
        {
            return new List<VocabularyValue>();
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
