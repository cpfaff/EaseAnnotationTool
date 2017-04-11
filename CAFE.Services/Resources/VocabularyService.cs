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

            resultCollection.AddRange(_extensibleVocabularyService.GetExtenededValuesBy(userId.ToString(), vocabularyType));

            resultCollection.AddRange(_pluginsProvider.GetPluginsFor(vocabularyType).SelectMany(s => s.GetValues()));

            //_pluginsProvider.GetPluginsFor(vocabularyType).SelectMany(s => s.GetValuesExtended()).Select(f => f.Value);

            // GetValuesExtended(string search, string elementName)
            return resultCollection;
        }

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>List of vocabulary values</returns>
        public IEnumerable<VocabularyValue> GetVocabularyValuesExtended(System.Type vocabularyType, string itemType, string search, string userId)
        {
            var plugins =_pluginsProvider.GetPluginsFor(vocabularyType);

            var pluginsCollection = plugins.SelectMany(s => s.GetValuesExtended(search, itemType)).
                  Select(f => f.Value).Distinct().
                  ToList();

            var systemCollection = _extensibleVocabularyService.GetAllExtenededValues(vocabularyType, userId).Where(v => v.Value.StartsWith(search)).ToList();

            if ("Country" == itemType)
                foreach (var item in pluginsCollection)
                    if (null == systemCollection.FirstOrDefault(c => c.Value == item))
                        systemCollection.Add(new VocabularyValue { Value = item.First().ToString().ToUpper() + item.Substring(1) });

            if ("LocationName" == itemType || itemType.ToLower() == "class" || itemType.ToLower() == "order" || itemType.ToLower() == "family")
                systemCollection.AddRange(pluginsCollection.Select(p => new VocabularyValue { Value = p }));

            return systemCollection;
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
            return _extensibleVocabularyService.GetAllExtenededUsersValues();
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

            resultCollection.AddRange(_pluginsProvider.GetPluginsFor(vocabularyType).SelectMany(s => s.GetValues()));

            return resultCollection;
        }

        public IEnumerable<VocabularyValue> GetAllGlobalValues()
        {
            return _extensibleVocabularyService.GetAllExtenededValues();
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
