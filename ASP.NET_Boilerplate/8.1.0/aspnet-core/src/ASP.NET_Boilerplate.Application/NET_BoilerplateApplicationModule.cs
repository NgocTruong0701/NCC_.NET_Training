using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ASP.NET_Boilerplate.Authorization;

namespace ASP.NET_Boilerplate
{
    [DependsOn(
        typeof(NET_BoilerplateCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class NET_BoilerplateApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<NET_BoilerplateAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(NET_BoilerplateApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
