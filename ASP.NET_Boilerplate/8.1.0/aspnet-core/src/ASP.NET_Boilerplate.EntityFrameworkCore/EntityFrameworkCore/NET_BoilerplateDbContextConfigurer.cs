using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace ASP.NET_Boilerplate.EntityFrameworkCore
{
    public static class NET_BoilerplateDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<NET_BoilerplateDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<NET_BoilerplateDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
