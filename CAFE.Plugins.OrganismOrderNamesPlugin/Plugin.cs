using CAFE.Core.Plugins;
using System.ComponentModel.Composition;
using CAFE.Core.Resources;
using System;
using System.Collections.Generic;
using CAFE.Core.Integration;
using CAFE.Plugins.GlobalNameResolveService;
using AutoMapper;

namespace CAFE.Plugins.OrganismOrderNamesPlugin
{
    [Export(typeof(IExternalSourcePlugin))]
    public class Plugin : BasePlugin, IVocabularyExtenalSourcePlugin
    {
        public Type ForType => typeof(OrderVocabulary);


        public IEnumerable<VocabularyValue> GetValues()
        {
            return new List<VocabularyValue>();
        }

        public IEnumerable<VocabularyValue> GetValuesExtended(string search, string elementName)
        {
            return MakeRequest(search);
        }

        public void RegisterMapConfigs(IMapperConfigurationExpression c)
        {
            ConfigurationResolver.RegisterMapping(c);
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
