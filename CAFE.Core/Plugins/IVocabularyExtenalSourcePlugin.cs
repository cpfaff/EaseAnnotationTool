
using System;
using System.Collections.Generic;
using CAFE.Core.Resources;
using AutoMapper;

namespace CAFE.Core.Plugins
{
    /// <summary>
    /// Contract for pluggable components that returns Vocabulary values from external online sources
    /// </summary>
    public interface IVocabularyExtenalSourcePlugin
    {
        /// <summary>
        /// Returns type of vocabulary for that returns values
        /// </summary>
        Type ForType { get; }

        /// <summary>
        /// Values that returns from external online source
        /// </summary>
        /// <returns>Collection of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetValues();

        /// <summary>
        /// Values that returns from external online source
        /// </summary>
        /// <returns>Collection of vocabulary values</returns>
        IEnumerable<VocabularyValue> GetValuesExtended(string search, string elementName);

        /// <summary>
        /// Register AutoMapper congiguratios over plugin
        /// </summary>
        void RegisterMapConfigs(IMapperConfigurationExpression c);
    }
}
