
using System.Collections.Generic;

namespace CAFE.Core.Integration
{
    /// <summary>
    /// Provide external values for vocabulary
    /// </summary>
    public interface IExternalVocabularySource
    {
        /// <summary>
        /// Returns list of values from Excel or CSV file
        /// </summary>
        /// <returns>List of values</returns>
        Dictionary<string, string> GetValuesFromFile();

        /// <summary>
        /// Returns list of values from Excel or CSV file
        /// </summary>
        /// <returns>List of values</returns>
        List<Dictionary<string, object>> GetCollectionValuesFromFile(System.Type AIClassType);

        /// <summary>
        /// Return wxcel or CSV file with headers
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        byte[] CreateHeadersFile(System.Type type);
    }
}
