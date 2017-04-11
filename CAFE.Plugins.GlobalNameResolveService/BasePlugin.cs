
using AutoMapper;
using CAFE.Core.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CAFE.Plugins.GlobalNameResolveService
{
    public abstract class BasePlugin
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

        protected IEnumerable<VocabularyValue> MakeRequest(string search)
        {
            List<VocabularyValue> values = new List<VocabularyValue>();

            try
            {
                var request = Mapper.Map<RequestData>(Configuration);
                request.Names = search;
                var response = GlobalNameRequester.GetNames(request);

                values.AddRange(response.Data.SelectMany(sm => sm.Results.Select(s => new VocabularyValue() { Uri = "http://resolver.globalnames.org/name_resolvers.json", Value = s.NameString })));
            }
            catch (Exception ex)
            {
                //TODO: log here
            }

            return values;
        }
    }
}
