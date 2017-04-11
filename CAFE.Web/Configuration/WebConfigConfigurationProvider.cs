
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using AutoMapper;
using IConfiguration = CAFE.Core.Configuration.IConfiguration;
using System.ComponentModel.Composition;

namespace CAFE.Web.Configuration
{
    /// <summary>
    /// Web.config based configuration provider
    /// Provide configuration by type parameter T
    /// </summary>
    public class WebConfigConfigurationProvider : Core.Configuration.IConfigurationProvider
    {
        /// <summary>
        /// Returns configuration
        /// </summary>
        /// <returns>Configuration instance</returns>
        /// <typeparam name="T">Configuration type</typeparam>
        public T Get<T>() where T : IConfiguration, new()
        {
            try
            {
                var section = GetCorrespondingSection(typeof(T));
                var configInstance = new T();

                return Mapper.Map(section, configInstance);

            }
            catch (Exception exception)
            {
                //TODO: do something here
                throw;
            }
        }

        /// <summary>
        /// Sync configuration changes
        /// </summary>
        /// <param name="configuration">Configuration for sync</param>
        /// <typeparam name="T">Configuration type</typeparam>
        public void Sync<T>(T configuration) where T : IConfiguration, new()
        {
            try
            {
                var config = GetLocalConfiguration();
                var section = GetCorrespondingSection(typeof(T), config);

                Mapper.Map(configuration, section);

                config.Save(ConfigurationSaveMode.Modified);
            }
            catch (Exception exception)
            {
                //TODO: do something here
                throw;
            }
        }

        private Type FindSectionTypeCorrespondingWithConfiguration(Type configType)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();

            Type foundSectionType = default(Type);

            var derivedType = typeof(ConfigurationSection);
            foundSectionType = currentAssembly
                .GetTypes(
                ).FirstOrDefault(t => t != derivedType &&
                    derivedType.IsAssignableFrom(t) &&
                    t.Name.ToLower().StartsWith(configType.Name.ToLower()));

            return foundSectionType;
        }

        private ConfigurationSection GetCorrespondingSection(Type imputType, System.Configuration.Configuration config = null)
        {
            var sectionType = FindSectionTypeCorrespondingWithConfiguration(imputType);
            if (sectionType == null)
                throw new InvalidOperationException("Class with name \"" + imputType.Name +
                                                    "Section\" and inherited from ConfigurationSection not found.");

            var section = WebConfigurationManager.GetSection(sectionType.Name);
            if (section == null)
            //throw new ApplicationException("Some errors occurs when trying get configuration section with name \"" +
            //                           sectionType.Name + "\"");
            {
                section = Activator.CreateInstance(sectionType);
                if (config == null)
                    config = GetLocalConfiguration();

                config.Sections.Add(sectionType.Name, (ConfigurationSection)section);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(sectionType.Name);
            }

            return (ConfigurationSection)section;
        }

        private System.Configuration.Configuration GetLocalConfiguration()
        {
            return WebConfigurationManager.OpenWebConfiguration("~");
        }
    }
}