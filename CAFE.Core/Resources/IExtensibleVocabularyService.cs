
using CAFE.Core.Integration;
using System;
using System.Collections.Generic;

namespace CAFE.Core.Resources
{
    /// <summary>
    /// Contract for service that manage extensions for *Vocabulary values
    /// That mean, vocabulary may be extended (add new value or remove)
    /// </summary>
    public interface IExtensibleVocabularyService
    {
        /// <summary>
        /// Returns all(globaly) extended values
        /// </summary>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyUserValue> GetAllExtenededUsersValues();
        /// <summary>
        /// Returns all(globaly) extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyValue> GetAllExtenededValues<T>(string userId) where T : struct, IConvertible;

        /// <summary>
        /// Returns all(globaly) extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyValue> GetAllExtenededValues(Type vocabularyType, string userId);

        Dictionary<string, string> GetAllSimpleTypesVocabularies();
        /// <summary>
        /// Returns all(globaly) extended values
        /// </summary>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyValue> GetAllExtenededValues();

        /// <summary>
        /// Returns all(globaly) extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyValue> GetAllExtenededValues_<T>() where T : struct, IConvertible;

        /// <summary>
        /// Returns all(globaly) extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyValue> GetAllExtenededValues_(Type vocabularyType);

        /// <summary>
        /// Returns user's defined extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="by">User's id that added extended values</param>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyValue> GetExtenededValuesBy<T>(string by) where T : struct, IConvertible;

        /// <summary>
        /// Returns user's defined extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyValue> GetExtenededValuesBy(string by, Type vocabularyType);

        /// <summary>
        /// Returns user's defined extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="by">User's id that added extended values</param>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyUserValue> GetExtenededValuesBy_<T>(string by) where T : struct, IConvertible;

        /// <summary>
        /// Returns user's defined extended values for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <returns>Collection of extended values</returns>
        IEnumerable<VocabularyUserValue> GetExtenededValuesBy_(string by, Type vocabularyType);

        /// <summary>
        /// Add new user's defined extended value for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="values">Extended values</param>
        void AddNewExtendedValuesBy<T>(string by, Dictionary<string, string> values) where T : struct, IConvertible;

        /// <summary>
        /// Add new user's defined extended value for vocabulary type T (that inherited from enum)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">Extended values</param>
        void AddNewExtendedValuesBy(string by, Type vocabularyType, Dictionary<string, string> values);

        /// <summary>
        /// Make user's defined extended values globaly (available for all)
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="values">user's defined extended values</param>
        /// <returns>Globaly available extended values</returns>
        void MakeExtenededValuesGlobaly<T>(IEnumerable<long> values) where T : struct, IConvertible;

        /// <summary>
        /// Make user's defined extended values globaly (available for all)
        /// </summary>
        /// <param name="by">User's id that added extended values</param>
        /// <param name="vocabularyType">Type of vocabulary</param>
        /// <param name="values">user's defined extended values</param>
        /// <returns>Globaly available extended values</returns>
        void MakeExtenededValuesGlobaly(Type vocabularyType, IEnumerable<long> values);

        VocabularyValue AddGlobalValue(VocabularyValue value);
        void UpdateGlobalValue(VocabularyValue value);
        void DeleteGlobalValue(long id);
    }
}
