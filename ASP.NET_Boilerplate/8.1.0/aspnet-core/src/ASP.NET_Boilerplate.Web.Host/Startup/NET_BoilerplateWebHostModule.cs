using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ASP.NET_Boilerplate.Configuration;

namespace ASP.NET_Boilerplate.Web.Host.Startup
{
    [DependsOn(
       typeof(NET_BoilerplateWebCoreModule))]
    public class NET_BoilerplateWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public NET_BoilerplateWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NET_BoilerplateWebHostModule).GetAssembly());
        }
    }
}
