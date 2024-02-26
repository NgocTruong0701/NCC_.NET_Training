using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using ASP.NET_Boilerplate.Authorization.Users;

namespace ASP.NET_Boilerplate.Sessions.Dto
{
    [AutoMapFrom(typeof(User))]
    public class UserLoginInfoDto : EntityDto<long>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string UserName { get; set; }

        public string EmailAddress { get; set; }
    }
}
