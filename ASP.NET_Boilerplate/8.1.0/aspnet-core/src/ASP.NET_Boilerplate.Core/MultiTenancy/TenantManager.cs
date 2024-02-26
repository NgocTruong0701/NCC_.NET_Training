using Abp.Application.Features;
using Abp.Domain.Repositories;
using Abp.MultiTenancy;
using ASP.NET_Boilerplate.Authorization.Users;
using ASP.NET_Boilerplate.Editions;

namespace ASP.NET_Boilerplate.MultiTenancy
{
    public class TenantManager : AbpTenantManager<Tenant, User>
    {
        public TenantManager(
            IRepository<Tenant> tenantRepository, 
            IRepository<TenantFeatureSetting, long> tenantFeatureRepository, 
            EditionManager editionManager,
            IAbpZeroFeatureValueStore featureValueStore) 
            : base(
                tenantRepository, 
                tenantFeatureRepository, 
                editionManager,
                featureValueStore)
        {
        }
    }
}
