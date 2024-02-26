using Abp.MultiTenancy;
using ASP.NET_Boilerplate.Authorization.Users;

namespace ASP.NET_Boilerplate.MultiTenancy
{
    public class Tenant : AbpTenant<User>
    {
        public Tenant()
        {            
        }

        public Tenant(string tenancyName, string name)
            : base(tenancyName, name)
        {
        }
    }
}
