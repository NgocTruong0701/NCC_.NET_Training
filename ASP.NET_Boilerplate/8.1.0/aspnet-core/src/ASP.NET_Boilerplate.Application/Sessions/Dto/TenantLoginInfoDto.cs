using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ASP.NET_Boilerplate.MultiTenancy;

namespace ASP.NET_Boilerplate.Sessions.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class TenantLoginInfoDto : EntityDto
    {
        public string TenancyName { get; set; }

        public string Name { get; set; }
    }
}
