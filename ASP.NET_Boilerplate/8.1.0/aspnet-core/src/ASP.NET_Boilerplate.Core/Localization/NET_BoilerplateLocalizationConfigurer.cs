using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace ASP.NET_Boilerplate.Localization
{
    public static class NET_BoilerplateLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(NET_BoilerplateConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(NET_BoilerplateLocalizationConfigurer).GetAssembly(),
                        "ASP.NET_Boilerplate.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
