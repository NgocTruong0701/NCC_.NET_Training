using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ASP.NET_Boilerplate.EntityFrameworkCore;
using ASP.NET_Boilerplate.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace ASP.NET_Boilerplate.Web.Tests
{
    [DependsOn(
        typeof(NET_BoilerplateWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class NET_BoilerplateWebTestModule : AbpModule
    {
        public NET_BoilerplateWebTestModule(NET_BoilerplateEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NET_BoilerplateWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(NET_BoilerplateWebMvcModule).Assembly);
        }
    }
}