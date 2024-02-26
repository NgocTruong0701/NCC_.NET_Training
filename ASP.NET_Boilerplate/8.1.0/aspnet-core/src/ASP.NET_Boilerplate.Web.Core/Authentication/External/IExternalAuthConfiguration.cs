using System.Collections.Generic;

namespace ASP.NET_Boilerplate.Authentication.External
{
    public interface IExternalAuthConfiguration
    {
        List<ExternalLoginProviderInfo> Providers { get; }
    }
}
