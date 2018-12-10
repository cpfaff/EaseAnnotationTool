
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using AutoMapper;
using CAFE.Core.Integration;
using CAFE.Core.Plugins;
using CAFE.Core.Resources;
using CAFE.Plugins.GlobalNameResolveService;

namespace CAFE.Plugins.OrganismSpecifiesPlugin
{
    [Export(typeof(IExternalSourcePlugin))]
    public class Plugin : IVocabularyExternalSourcePlugin<SpeciesResolveValue>
    {


        private GlobalNameServiceConfiguration _configuration;

        protected GlobalNameServiceConfiguration Configuration
        {
            get
            {
                if (_configuration == null)
                    _configuration = ConfigurationResolver.GetConfig();

                return _configuration;
            }
        }

        public Type ForType => typeof(SpeciesVocabulary);


        public IEnumerable<SpeciesResolveValue> GetValues()
        {
            return new List<SpeciesResolveValue>();
        }

        public IEnumerable<SpeciesResolveValue> GetValuesExtended(string search, string elementName)
        {
            return MakeRequest(search);
        }


        protected IEnumerable<SpeciesResolveValue> MakeRequest(string search)
        {
            List<SpeciesResolveValue> values = new List<SpeciesResolveValue>();

            try
            {
                var request = Mapper.Map<RequestData>(Configuration);
                request.Data = search;
                var response = GlobalNameRequester.GetNames(request);

                foreach (var dataItem in response.Data)
                {
                    foreach (var dataItemResult in dataItem.Results)
                    {
                        var ranks = dataItemResult.ClassificationPathRanks.Split(new[] {"|"},
                            StringSplitOptions.RemoveEmptyEntries);
                        var rankValues = dataItemResult.ClassificationPath.Split(new[] {"|"},
                            StringSplitOptions.None);

                        var nameValues = new List<GlobalNamesRankValue>();

                        for (int i = 0; i < ranks.Length; i++)
                        {
                            try
                            {
                                nameValues.Add(new GlobalNamesRankValue()
                                {
                                    Rank = ranks[i],
                                    Value = rankValues[i]
                                });
                            }
                            catch (Exception e)
                            {
                                throw;
                            }
                        }
                        var specie = new SpeciesResolveValue()
                        {
                            Species = dataItemResult.NameString,
                            Score = dataItemResult.Score,
                            Names = nameValues
                        };

                        values.Add(specie);
                    }
                }
            }
            catch (Exception ex)
            {
                //TODO: log here
                throw;
            }

            return values;
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
