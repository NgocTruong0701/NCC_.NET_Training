FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["src/ASP.NET_Boilerplate.Web.Host/ASP.NET_Boilerplate.Web.Host.csproj", "src/ASP.NET_Boilerplate.Web.Host/"]
COPY ["src/ASP.NET_Boilerplate.Web.Core/ASP.NET_Boilerplate.Web.Core.csproj", "src/ASP.NET_Boilerplate.Web.Core/"]
COPY ["src/ASP.NET_Boilerplate.Application/ASP.NET_Boilerplate.Application.csproj", "src/ASP.NET_Boilerplate.Application/"]
COPY ["src/ASP.NET_Boilerplate.Core/ASP.NET_Boilerplate.Core.csproj", "src/ASP.NET_Boilerplate.Core/"]
COPY ["src/ASP.NET_Boilerplate.EntityFrameworkCore/ASP.NET_Boilerplate.EntityFrameworkCore.csproj", "src/ASP.NET_Boilerplate.EntityFrameworkCore/"]
WORKDIR "/src/src/ASP.NET_Boilerplate.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/ASP.NET_Boilerplate.Web.Host", "src/ASP.NET_Boilerplate.Web.Host"]
COPY ["src/ASP.NET_Boilerplate.Web.Core", "src/ASP.NET_Boilerplate.Web.Core"]
COPY ["src/ASP.NET_Boilerplate.Application", "src/ASP.NET_Boilerplate.Application"]
COPY ["src/ASP.NET_Boilerplate.Core", "src/ASP.NET_Boilerplate.Core"]
COPY ["src/ASP.NET_Boilerplate.EntityFrameworkCore", "src/ASP.NET_Boilerplate.EntityFrameworkCore"]
WORKDIR "/src/src/ASP.NET_Boilerplate.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "ASP.NET_Boilerplate.Web.Host.dll"]
