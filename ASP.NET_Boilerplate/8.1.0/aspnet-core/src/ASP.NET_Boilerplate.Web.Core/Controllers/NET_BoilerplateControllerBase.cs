using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace ASP.NET_Boilerplate.Controllers
{
    public abstract class NET_BoilerplateControllerBase: AbpController
    {
        protected NET_BoilerplateControllerBase()
        {
            LocalizationSourceName = NET_BoilerplateConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
