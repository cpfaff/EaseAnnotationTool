
using System;
using System.Collections.Generic;

namespace CAFE.Core.Plugins
{
    /// <summary>
    /// Contract for plugins provider implementation
    /// Managing plugins
    /// </summary>
    public interface IPluginsProvider
    {
        /// <summary>
        /// Returns of collection loaded plugins with external sources of vocabulary values
        /// </summary>
        ICollection<IExternalSourcePlugin> Sources { get; }

        /// <summary>
        /// Returns collection of plugins that for type T
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <returns>Collection of plugins</returns>
        IEnumerable<IExternalSourcePlugin> GetPluginsFor<T>() where T : struct, ICloneable;

        /// <summary>
        /// Returns collection of plugins that for type
        /// </summary>
        /// <param name="type">Type of vocabulary</param>
        /// <returns>Collection of plugins</returns>
        IEnumerable<IExternalSourcePlugin> GetPluginsFor(Type type);
    }
}
