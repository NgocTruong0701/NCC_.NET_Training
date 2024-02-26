using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using ASP.NET_Boilerplate.Configuration.Dto;

namespace ASP.NET_Boilerplate.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : NET_BoilerplateAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
