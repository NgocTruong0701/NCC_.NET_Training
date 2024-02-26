using System.Threading.Tasks;
using ASP.NET_Boilerplate.Models.TokenAuth;
using ASP.NET_Boilerplate.Web.Controllers;
using Shouldly;
using Xunit;

namespace ASP.NET_Boilerplate.Web.Tests.Controllers
{
    public class HomeController_Tests: NET_BoilerplateWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}