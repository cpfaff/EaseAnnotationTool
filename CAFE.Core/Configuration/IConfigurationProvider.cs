
namespace CAFE.Core.Configuration
{
    /// <summary>
    /// Contract for configuration providers
    /// Provide configuration by type parameter T
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Returns configuration
        /// </summary>
        /// <returns>Configuration instance</returns>
        T Get<T>() where T : IConfiguration, new();
        /// <summary>
        /// Sync configuration changes
        /// </summary>
        /// <param name="configuration">Configuration for sync</param>
        void Sync<T>(T configuration) where T : IConfiguration, new();
    }
}
