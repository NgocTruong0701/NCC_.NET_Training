using Microsoft.Extensions.Configuration;
using Castle.MicroKernel.Registration;
using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using ASP.NET_Boilerplate.Configuration;
using ASP.NET_Boilerplate.EntityFrameworkCore;
using ASP.NET_Boilerplate.Migrator.DependencyInjection;

namespace ASP.NET_Boilerplate.Migrator
{
    [DependsOn(typeof(NET_BoilerplateEntityFrameworkModule))]
    public class NET_BoilerplateMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public NET_BoilerplateMigratorModule(NET_BoilerplateEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(NET_BoilerplateMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                NET_BoilerplateConsts.ConnectionStringName
            );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(
                typeof(IEventBus), 
                () => IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                )
            );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NET_BoilerplateMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}
