
using System;
using System.Collections.Generic;
using CAFE.Core.Integration;
using CAFE.Core.Security;
using Type = System.Type;

namespace CAFE.Core.Resources
{
    /// <summary>
    /// Provide access to vocabulary values
    /// </summary>
    public interface IVocabularyService
    {
        /// <summary>
        /// Imports values from external source
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="importer">User who importing values</param>
        /// <param name="source">External source</param>
        string[] Import<T>(User importer, IExternalVocabularySource source) where T : struct, IConvertible;

        /// <summary>
        /// Imports collection of values
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="importer">User who importing values</param>
        /// <param name="source">External source</param>
        List<Dictionary<string, object>> ImportCollection(System.Type AIClassType, IExternalVocabularySource source);

        /// <summary>
        /// Imports values from external source
        /// </summary>
        /// <param name="importer">User who importing values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="source">External source</param>
        Dictionary<string, string> Import(User importer, Type vocabularyType, IExternalVocabularySource source);

        /// <summary>
        /// Manualy addition value to the vocabulary
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="creator">User who adds values</param>
        /// <param name="values">List of addition values</param>
        void ManualAdd<T>(User creator, IEnumerable<string> values) where T : struct, IConvertible;

        /// <summary>
        /// Manualy addition value to the vocabulary
        /// </summary>
        /// <param name="creator">User who adds values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">List of addition values</param>
        void ManualAdd(User creator, Type vocabularyType, Dictionary<string, string> values);

        /// <summary>
        /// Accepts user's defined vocabulary values to global list of values
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="creator">User whose values are importing</param>
        /// <param name="values">List of acception values</param>
        void AcceptUserMetadata<T>(IEnumerable<long> ids) where T : struct, IConvertible;

        /// <summary>
        /// Accepts user's defined vocabulary values to global list of values
        /// </summary>
        /// <param name="creator">User whose values are importing</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">List of acception values</param>
        void AcceptUserMetadata(Type vocabularyType, IEnumerable<long> ids);

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <returns>List of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetVocabularyValues<T>() where T : struct, IConvertible;

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>List of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetVocabularyValues(Type vocabularyType, string userId);

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="searchToken">Search by this value</param>
        /// <returns>List of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetVocabularyValues(Type vocabularyType, string searchToken, string userId);


        /// <summary>
        ///  Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>List of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetVocabularyValues_(Type vocabularyType, string userId);

        /// <summary>
        /// Returns list of vocabulary values
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>List of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetVocabularyValuesExtended(System.Type vocabularyType, string itemType, string search, string userId);

        /// <summary>
        /// Get descriptions for all simple types
        /// </summary>
        /// <returns></returns>
        Dictionary<string, string> GetSimpleTypesDescriptions();

        /// <summary>
        /// Returns list of user's values
        /// </summary>
        /// <returns>List of vocabulary values</returns>
        IEnumerable<VocabularyUserValue> GetAllExtenededUsersValues();

        /// <summary>
        /// Returns list of global values
        /// </summary>
        /// <returns>List of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetAllGlobalValues();

        VocabularyValue Save(VocabularyValue value);
        void Remove(long id);
        IEnumerable<string> GetVocabularyTypes();
    }
}
