
using System.Collections.Generic;

namespace CAFE.Core.Plugins
{
    /// <summary>
    /// Contract for pluggable components that returns Vocabulary values from external online sources
    /// </summary>
    public interface IVocabularyExternalSourcePlugin<out T> : IExternalSourcePlugin
    {

        /// <summary>
        /// Values that returns from external online source
        /// </summary>
        /// <returns>Collection of vocabulary values</returns>
        IEnumerable<T> GetValues();

        /// <summary>
        /// Values that returns from external online source
        /// </summary>
        /// <returns>Collection of vocabulary values</returns>
        IEnumerable<T> GetValuesExtended(string search, string elementName);
    }
}
