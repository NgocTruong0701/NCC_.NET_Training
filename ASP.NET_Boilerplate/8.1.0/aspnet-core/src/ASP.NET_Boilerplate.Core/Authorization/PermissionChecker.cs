using Abp.Authorization;
using ASP.NET_Boilerplate.Authorization.Roles;
using ASP.NET_Boilerplate.Authorization.Users;

namespace ASP.NET_Boilerplate.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
