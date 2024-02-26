using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using ASP.NET_Boilerplate.Authorization.Roles;
using ASP.NET_Boilerplate.Authorization.Users;
using ASP.NET_Boilerplate.MultiTenancy;

namespace ASP.NET_Boilerplate.EntityFrameworkCore
{
    public class NET_BoilerplateDbContext : AbpZeroDbContext<Tenant, Role, User, NET_BoilerplateDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public NET_BoilerplateDbContext(DbContextOptions<NET_BoilerplateDbContext> options)
            : base(options)
        {
        }
    }
}
