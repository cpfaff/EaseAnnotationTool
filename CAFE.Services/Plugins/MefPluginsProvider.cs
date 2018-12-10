
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using CAFE.Core.Plugins;

namespace CAFE.Services.Plugins
{
    /// <summary>
    /// Plugins provider that managed by MEF composite container
    /// Managing plugins
    /// </summary>
    public class MefPluginsProvider : IPluginsProvider
    {
        private readonly CompositionContainer _compositionContainer;

        public MefPluginsProvider()
        {
            //Thats need for MEF. Collection must be initialized or available for write (have setter)
            Sources = new List<IExternalSourcePlugin>();

            //Prepare MEF containet
            var pth = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory);
            var fullPath = Path.Combine(pth.FullName, "App_Code\\Plugins");
            if (!Directory.Exists(fullPath))
                throw new Exception("Path " + fullPath + " doesn't exists.");

            var requiredPlugins = new List<string> {
                "CAFE.Plugins.GeoNamesPlugin",
                "CAFE.Plugins.GlobalNameResolveService",
                "CAFE.Plugins.OrganismSpecifiesPlugin"
            };

            requiredPlugins.ForEach(p =>
            {
                var fileFullPath = fullPath +"\\"+ p + ".dll";

                if (!File.Exists(fileFullPath))
                    throw new Exception($"Plugin '{p}' doesn't exists. Expected location: " + fileFullPath);
            });

            _compositionContainer = new CompositionContainer(new DirectoryCatalog(fullPath));

            try
            {
                _compositionContainer.ComposeParts(this);
            }
            catch(Exception exception)
            {
                throw exception;
            }
        }


        /// <summary>
        /// Returns of collection loaded plugins with external sources of vocabulary values
        /// </summary>
        [ImportMany(typeof(IExternalSourcePlugin))]
        public ICollection<IExternalSourcePlugin> Sources { get; }


        /// <summary>
        /// Returns collection of plugins that for type T
        /// </summary>
        /// <typeparam name="T">Type of vocabulary</typeparam>
        /// <returns>Collection of plugins</returns>
        public IEnumerable<IExternalSourcePlugin> GetPluginsFor<T>() where T : struct, ICloneable
        {
            return GetPluginsFor(typeof (T));
        }

        /// <summary>
        /// Returns collection of plugins that for type
        /// </summary>
        /// <param name="type">Type of vocabulary</param>
        /// <returns>Collection of plugins</returns>
        public IEnumerable<IExternalSourcePlugin> GetPluginsFor(Type type)
        {
            return Sources.Where(s => s.ForType.FullName == type.FullName);
        }
    }
}
