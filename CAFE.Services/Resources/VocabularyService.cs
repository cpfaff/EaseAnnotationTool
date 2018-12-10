using CAFE.Core.Integration;
using CAFE.Core.Resources;
using CAFE.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CAFE.Core.Plugins;
using Type = System.Type;
using CAFE.DAL.Interfaces;
using CAFE.DAL.Models.Resources;

namespace CAFE.Services.Resources
{
    public class VocabularyService : IVocabularyService
    {
        private readonly IExtensibleVocabularyService _extensibleVocabularyService;
        private readonly IPluginsProvider _pluginsProvider;
        private readonly IRepository<DbVocabularyValue> _vocabularyValuesRepository;
        private readonly IRepository<DbVocabularyUserValue> _vocabularyUserValuesRepository;

        public VocabularyService(IExtensibleVocabularyService extensibleVocabularyService,
            IPluginsProvider pluginsProvider, IRepository<DbVocabularyValue> vocabularyValuesRepository,
            IRepository<DbVocabularyUserValue> vocabularyUserValuesRepository)
        {
            _extensibleVocabularyService = extensibleVocabularyService;
            _pluginsProvider = pluginsProvider;
            _vocabularyValuesRepository = vocabularyValuesRepository;
            _vocabularyUserValuesRepository = vocabularyUserValuesRepository;
        }

        public string[] Import<T>(User importer, IExternalVocabularySource source) where T : struct, IConvertible
        {
            return null;
        }

        /// <summary>
        /// Imports values from external source
        /// </summary>
        /// <param name="importer">User who importing values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="source">External source</param>
        public Dictionary<string, string> Import(User importer, System.Type vocabularyType, IExternalVocabularySource source)
        {
            var newValuesArray = source.GetValuesFromFile();
            _extensibleVocabularyService.AddNewExtendedValuesBy(importer.Id, vocabularyType, newValuesArray);

            return newValuesArray;
        }

        /// <summary>
        /// Imports collection of values from external source
        /// </summary>
        /// <param name="importer">User who importing values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="source">External source</param>
        public List<Dictionary<string, object>> ImportCollection(System.Type AIClassType, IExternalVocabularySource source)
        {
            var newValuesArray = source.GetCollectionValuesFromFile(AIClassType);
            return newValuesArray;
        }

        /// <summary>
        /// Manualy addition value to the vocabulary
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="creator">User who adds values</param>
        /// <param name="values">List of addition values</param>
        public void ManualAdd<T>(User creator, IEnumerable<string> values) where T : struct, IConvertible
        {

        }

        /// <summary>
        /// Manualy addition value to the vocabulary
        /// </summary>
        /// <param name="creator">User who adds values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">List of addition values</param>
        public void ManualAdd(User creator, System.Type vocabularyType, Dictionary<string, string> values)
        {
            _extensibleVocabularyService.AddNewExtendedValuesBy(creator.Id, vocabularyType, values);

            return;
        }

        /// <summary>
        /// Accepts user's defined vocabulary values to global list of values
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="creator">User whose values are importing</param>
        /// <param name="values">List of acception values</param>
        public void AcceptUserMetadata<T>(IEnumerable<long> ids) where T : struct, IConvertible
        {

        }

        /// <summary>
        /// Accepts user's defined vocabulary values to global list of values
        /// </summary>
        /// <param name="creator">User whose values are importing</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">List of acception values</param>
        public void AcceptUserMetadata(System.Type vocabularyType, IEnumerable<long> ids)
        {
            _extensibleVocabularyService.MakeExtenededValuesGlobaly(vocabularyType, ids);
        }

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <returns>List of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetVocabularyValues<T>() where T : struct, IConvertible
        {
            return null;
        }

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>List of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetVocabularyValues(System.Type vocabularyType, string userId)
        {
            var resultCollection = new List<VocabularyValue>();
            resultCollection.AddRange(_extensibleVocabularyService.GetAllExtenededValues(vocabularyType, userId).ToList());

            resultCollection.AddRange(_extensibleVocabularyService.GetExtenededValuesBy(userId, vocabularyType));

            var plugins = _pluginsProvider.GetPluginsFor(vocabularyType).ToList();
            foreach (var plugin in plugins)
            {
                var vocabularyPlugin = plugin as IVocabularyExtenalSourcePlugin;
                if (vocabularyPlugin != null)
                {
                    resultCollection.AddRange(vocabularyPlugin.GetValues());
                }
            }

             // GetValuesExtended(string search, string elementName)
            return resultCollection.OrderBy(o => o.Value);

        }


        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="searchToken">Search by this value</param>
        /// <returns>List of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetVocabularyValues(Type vocabularyType, string userId, string searchToken)
        {
            var resultCollection = new List<VocabularyValue>();
            if (!string.IsNullOrEmpty(searchToken))
            {
                resultCollection.AddRange(_extensibleVocabularyService
                    .GetAllExtenededValues(vocabularyType, searchToken, userId).ToList());
                resultCollection.AddRange(
                    _extensibleVocabularyService.GetExtenededValuesBy(userId, vocabularyType, searchToken));

            }
            else
            {
                resultCollection.AddRange(_extensibleVocabularyService.GetAllExtenededValues(vocabularyType, userId)
                    .ToList());
                resultCollection.AddRange(_extensibleVocabularyService.GetExtenededValuesBy(userId, vocabularyType));
            }


            // GetValuesExtended(string search, string elementName)
            return resultCollection.OrderBy(o => o.Value);

        }

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>List of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetVocabularyValuesExtended(System.Type vocabularyType, string itemType, string search, string userId)
        {

            var plugins =_pluginsProvider.GetPluginsFor(vocabularyType).ToList();
            var stringValues = new List<VocabularyValue>();
            foreach (var plugin in plugins)
            {
                var vocabularyPlugin = plugin as IVocabularyExtenalSourcePlugin;
                if (vocabularyPlugin != null)
                {
                    stringValues.AddRange(vocabularyPlugin.GetValuesExtended(search, itemType));
                }
            }
            var pluginsCollection = stringValues.Distinct().ToList();

            var systemCollection = _extensibleVocabularyService.GetAllExtenededValues(vocabularyType, userId).Where(v => v.Value.StartsWith(search)).ToList();

            if ("Country".ToLower() == itemType.ToLower())
                foreach (var item in pluginsCollection.Select(f => f.Value).Distinct().ToList())
                    if (null == systemCollection.FirstOrDefault(c => c.Value == item))
                        systemCollection.Add(new VocabularyValue { Value = item.First().ToString().ToUpper() + item.Substring(1) });

            if (itemType.ToLower() == "class" || itemType.ToLower() == "order" || itemType.ToLower() == "family")
                systemCollection.AddRange(pluginsCollection.Select(f => f.Value).Distinct().Select(p => new VocabularyValue { Value = p }));

            if("LocationName".ToLower() == itemType.ToLower())
                systemCollection.AddRange(pluginsCollection.Distinct());
            
            return systemCollection.OrderBy(o => o.Value);
        }

        public Dictionary<string, string> GetSimpleTypesDescriptions()
        {
            return _extensibleVocabularyService.GetAllSimpleTypesVocabularies();
        }

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <returns>List of vocabulary values</returns>
        public IEnumerable<VocabularyUserValue> GetAllExtenededUsersValues()
        {
            return _extensibleVocabularyService.GetAllExtenededUsersValues().OrderBy(o => o.Value);
        }

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>List of vocabulary values</returns>

        public IEnumerable<VocabularyValue> GetVocabularyValues_(Type vocabularyType, string userId)
        {
            var resultCollection = new List<VocabularyValue>();
            resultCollection.AddRange(_extensibleVocabularyService.GetAllExtenededValues(vocabularyType, userId).ToList());

            resultCollection.AddRange(Mapper.Map<IEnumerable<VocabularyValue>>(_extensibleVocabularyService.GetExtenededValuesBy_(userId.ToString(), vocabularyType)));

            var plugins = _pluginsProvider.GetPluginsFor(vocabularyType).ToList();
            foreach (var plugin in plugins)
            {
                var vocabularyPlugin = plugin as IVocabularyExtenalSourcePlugin;
                if (vocabularyPlugin != null)
                {
                    resultCollection.AddRange(vocabularyPlugin.GetValues());
                }
            }
            return resultCollection.OrderBy(o => o.Value);
        }

        public IEnumerable<VocabularyValue> GetAllGlobalValues()
        {
            return _extensibleVocabularyService.GetAllExtenededValues().OrderBy(o => o.Value);
        }

        public VocabularyValue Save(VocabularyValue value)
        {
            VocabularyValue result;

            if (value.Id == 0)
            {
                result = _extensibleVocabularyService.AddGlobalValue(value);
            }
            else
            {
                _extensibleVocabularyService.UpdateGlobalValue(value);
                result = value;
            }

            return result;
        }

        public void Remove(long id)
        {
            _extensibleVocabularyService.DeleteGlobalValue(id);
        }

        public IEnumerable<string> GetVocabularyTypes()
        {
            List<string> types = new List<string>();

            types.AddRange(_vocabularyValuesRepository.FindCollection(s => s.Value != "-1").Select(s => s.Type).Distinct());
            types.AddRange(_vocabularyUserValuesRepository.Select(s => s.Type).Distinct());

            return types.Distinct().OrderBy(o => o);
        }
    }
}
