using System.Threading.Tasks;
using Abp.Application.Services;
using ASP.NET_Boilerplate.Sessions.Dto;

namespace ASP.NET_Boilerplate.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
