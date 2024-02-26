using Abp.Application.Services;
using ASP.NET_Boilerplate.MultiTenancy.Dto;

namespace ASP.NET_Boilerplate.MultiTenancy
{
    public interface ITenantAppService : IAsyncCrudAppService<TenantDto, int, PagedTenantResultRequestDto, CreateTenantDto, TenantDto>
    {
    }
}

