using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero.EntityFrameworkCore;
using ASP.NET_Boilerplate.EntityFrameworkCore.Seed;

namespace ASP.NET_Boilerplate.EntityFrameworkCore
{
    [DependsOn(
        typeof(NET_BoilerplateCoreModule), 
        typeof(AbpZeroCoreEntityFrameworkCoreModule))]
    public class NET_BoilerplateEntityFrameworkModule : AbpModule
    {
        /* Used it tests to skip dbcontext registration, in order to use in-memory database of EF Core */
        public bool SkipDbContextRegistration { get; set; }

        public bool SkipDbSeed { get; set; }

        public override void PreInitialize()
        {
            if (!SkipDbContextRegistration)
            {
                Configuration.Modules.AbpEfCore().AddDbContext<NET_BoilerplateDbContext>(options =>
                {
                    if (options.ExistingConnection != null)
                    {
                        NET_BoilerplateDbContextConfigurer.Configure(options.DbContextOptions, options.ExistingConnection);
                    }
                    else
                    {
                        NET_BoilerplateDbContextConfigurer.Configure(options.DbContextOptions, options.ConnectionString);
                    }
                });
            }
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(NET_BoilerplateEntityFrameworkModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            if (!SkipDbSeed)
            {
                SeedHelper.SeedHostDb(IocManager);
            }
        }
    }
}
