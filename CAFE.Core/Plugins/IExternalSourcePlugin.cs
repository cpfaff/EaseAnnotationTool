
using System;
using AutoMapper;

namespace CAFE.Core.Plugins
{
    /// <summary>
    /// Contract for pluggable components that returns values from external online sources
    /// </summary>
    public interface IExternalSourcePlugin
    {
        /// <summary>
        /// Returns type of vocabulary for that returns values
        /// </summary>
        Type ForType { get; }

        /// <summary>
        /// Register AutoMapper congiguratios over plugin
        /// </summary>
        void RegisterMapConfigs(IMapperConfigurationExpression c);

        /// <summary>
        /// Init plugin
        /// </summary>
        /// <returns>Indicator that says plugin init successfull or not</returns>
        bool Init();
    }
}
