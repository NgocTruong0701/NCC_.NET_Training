using System.Threading.Tasks;
using ASP.NET_Boilerplate.Configuration.Dto;

namespace ASP.NET_Boilerplate.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
