using AutoMapper;
using System;
using System.Configuration;
using System.Web.Configuration;

namespace CAFE.Plugins.GlobalNameResolveService
{
    public static class ConfigurationResolver
    {
        public static GlobalNameServiceConfiguration GetConfig()
        {
            Configuration config = null;
            object section = null;

            section = WebConfigurationManager.GetSection("GlobalNameServiceConfigurationSection");

            if (section == null)
            {
                section = Activator.CreateInstance(typeof(GlobalNameServiceConfigurationSection));
                if (config == null)
                    config = WebConfigurationManager.OpenWebConfiguration("~");

                config.Sections.Add("GlobalNameServiceConfigurationSection", (ConfigurationSection)section);
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("GlobalNameServiceConfigurationSection");
            }

            return Mapper.Map<GlobalNameServiceConfiguration>(section);
        }

        public static void RegisterMapping(IMapperConfigurationExpression c)
        {
            c.CreateMap<GlobalNameServiceConfigurationSection, GlobalNameServiceConfiguration>();
            c.CreateMap<GlobalNameServiceConfiguration, GlobalNameServiceConfigurationSection>();
            c.CreateMap<GlobalNameServiceConfiguration, RequestData>();
        }
    }
}
