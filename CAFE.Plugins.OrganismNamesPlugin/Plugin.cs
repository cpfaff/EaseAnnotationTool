using CAFE.Core.Plugins;
using System.ComponentModel.Composition;
using CAFE.Core.Resources;
using System;
using System.Collections.Generic;
using CAFE.Core.Integration;
using CAFE.Plugins.GlobalNameResolveService;
using AutoMapper;

namespace CAFE.Plugins.OrganismClassNamesPlugin
{
    [Export(typeof(IVocabularyExtenalSourcePlugin))]
    public class Plugin : BasePlugin, IVocabularyExtenalSourcePlugin
    {

        public Type ForType => typeof(ClassVocabulary);


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
    }
}
