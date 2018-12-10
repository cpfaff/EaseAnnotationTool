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

        public static int[] MapFromString(string compressedArray)
        {
            if(string.IsNullOrEmpty(compressedArray)) return new int[0];

            var parsed = compressedArray.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
            int[] resulr = new int[parsed.Length];
            for (int i = 0; i < parsed.Length; i++)
            {
                resulr[i] = int.Parse(parsed[i]);
            }

            return resulr;
        }

        public static void RegisterMapping(IMapperConfigurationExpression c)
        {
            c.CreateMap<GlobalNameServiceConfigurationSection, GlobalNameServiceConfiguration>();
            c.CreateMap<GlobalNameServiceConfiguration, GlobalNameServiceConfigurationSection>();
            c.CreateMap<GlobalNameServiceConfigurationSection.UIElement, GlobalNameServiceConfiguration.UIElement>();
            c.CreateMap<GlobalNameServiceConfiguration.UIElement, GlobalNameServiceConfigurationSection.UIElement>();
            c.CreateMap<GlobalNameServiceConfiguration, RequestData>()
                //.ForMember(m => m.DataSourceIds, expression => expression.MapFrom(m => MapFromString(m.DataSourceIds)))
                ;
        }
    }
}
