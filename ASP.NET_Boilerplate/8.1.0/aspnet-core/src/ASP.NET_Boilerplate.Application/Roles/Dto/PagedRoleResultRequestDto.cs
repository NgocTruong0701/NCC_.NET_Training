using Abp.Application.Services.Dto;

namespace ASP.NET_Boilerplate.Roles.Dto
{
    public class PagedRoleResultRequestDto : PagedResultRequestDto
    {
        public string Keyword { get; set; }
    }
}

