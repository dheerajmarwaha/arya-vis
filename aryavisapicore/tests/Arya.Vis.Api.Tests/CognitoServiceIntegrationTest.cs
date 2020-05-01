using Amazon.CognitoIdentityProvider.Model;
using Arya.Exceptions;
using Arya.Vis.Api.Config;
using Arya.Vis.Api.IdentityProviders;
using Arya.Vis.Api.ServiceImpl;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Config;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Services;
using Arya.Vis.Core.ServicesImpl;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Arya.Vis.Api.Tests
{
    public class CognitoServiceIntegrationTest
    {
        private readonly TestServer testServer;
        private readonly IServiceProvider provider;

        public CognitoServiceIntegrationTest()
        {
            testServer = new TestServer(new WebHostBuilder()
                   .UseStartup<Startup>());

            var client = testServer.Host.Services.CreateScope();
            provider = client.ServiceProvider;
        }

        [Theory]
        [InlineData("My Organization")]
        public async Task CreateOrganizationAsync_ShouldPass(string orgName)
        {
            var logger = provider.GetRequiredService<ILogger<CognitoService>>();
            //var client = provider.GetRequiredService<IAryaIdentityProviderClient>();
            var client = provider.GetRequiredService<AryaCognitoIdentityProviderClient>();
            var userService = provider.GetRequiredService<UserService>();
            var orgService = provider.GetRequiredService<OrganizationService>();
            var cognitoIAMCredentials = provider.GetRequiredService<IOptions<CognitoIAMCredentials>>();
            var cognitoClientConfig = provider.GetRequiredService<IOptions<CognitoClientConfiguration>>();



            var createGroupRequest = new CreateGroupRequest
            {
                UserPoolId = It.IsAny<string>(),
                GroupName = orgName
            };
            var trimmedOrgName = orgName.Replace(" ", "");
            var command = new OrganizationCreateCommand
            {
                OrganizationName = orgName
            };


            var _cognitoService = new CognitoService(logger,
                client,
                userService,
                orgService,
                cognitoIAMCredentials,
                cognitoClientConfig);

            var result = await _cognitoService.CreateOrganizationAsync(command);

            Assert.IsAssignableFrom<Organization>(result);
        }



       //[Fact]
        //public async Task CreateOrganizationAsync_NullInput_ShouldThrowInvalidArgumentException()
        //{
        //    var mocklogger = new Mock<ILogger<CognitoService>>();
        //    var mockClient = new Mock<IAryaIdentityProviderClient>();
        //    var mockUserService = new Mock<IUserService>();
        //    var mockOrgService = new Mock<IOrganizationService>();


        //    var _cognitoService = new CognitoService(mocklogger.Object,
        //        mockClient.Object,
        //        mockUserService.Object,
        //        mockOrgService.Object,
        //         _cognitoIAMCredentials,
        //        _cognitoClientConfig);

        //    await Assert.ThrowsAsync<InvalidArgumentException>(async () => await _cognitoService.CreateOrganizationAsync(null));
        //}
    }
}
