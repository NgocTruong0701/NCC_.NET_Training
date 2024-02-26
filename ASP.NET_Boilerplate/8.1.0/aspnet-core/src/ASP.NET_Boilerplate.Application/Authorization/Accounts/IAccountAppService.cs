using System.Threading.Tasks;
using Abp.Application.Services;
using ASP.NET_Boilerplate.Authorization.Accounts.Dto;

namespace ASP.NET_Boilerplate.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
