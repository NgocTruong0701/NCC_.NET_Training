using Abp.AutoMapper;
using ASP.NET_Boilerplate.Authentication.External;

namespace ASP.NET_Boilerplate.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        public string Name { get; set; }

        public string ClientId { get; set; }
    }
}
