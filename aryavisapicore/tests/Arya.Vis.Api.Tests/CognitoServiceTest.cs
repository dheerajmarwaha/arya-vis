using Amazon.CognitoIdentityProvider.Model;
using Arya.Exceptions;
using Arya.Vis.Api.Config;
using Arya.Vis.Api.IdentityProviders;
using Arya.Vis.Api.ServiceImpl;
using Arya.Vis.Core.Commands;
using Arya.Vis.Core.Config;
using Arya.Vis.Core.Entities;
using Arya.Vis.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Arya.Vis.Api.Tests
{
    public class CognitoServiceTest
    {
        private IOptions<CognitoIAMCredentials> _cognitoIAMCredentials;
        private IOptions<CognitoClientConfiguration> _cognitoClientConfig;
        public CognitoServiceTest()
        {
            ConfigurationManager configurationManager = new ConfigurationManager("appsettings.test.json");
            _cognitoIAMCredentials = configurationManager.Configuration.GetSection("AWSCognito").Get<IOptions<CognitoIAMCredentials>>();
            _cognitoClientConfig = configurationManager.Configuration.GetSection("Authentication: Client").Get<IOptions<CognitoClientConfiguration>>();
        }
        private CreateGroupResponse GetNonNullCreateGroupResponse()
        {
            return new CreateGroupResponse();
        }

        private User GetNonNullUser()
        {
            return new User
            {
                UserGuid = Guid.NewGuid()
            };
        }

        [Theory]
        [InlineData("My Organization")]
        [InlineData(" Some  Trial Org    ")]
        public async Task CreateOrganizationAsync_ShouldPass(string orgName)
        {
            var mocklogger = new Mock<ILogger<CognitoService>>();
            var mockClient = new Mock<IAryaIdentityProviderClient>();
            var mockUserService = new Mock<IUserService>();
            var mockOrgService = new Mock<IOrganizationService>();
            
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

            mockClient.Setup(x => x.CreateGroupAsync(createGroupRequest))
                .ReturnsAsync(GetNonNullCreateGroupResponse());
            mockOrgService.Setup(x => x.CreateOrganizationAsync(command))
                .ReturnsAsync(GetNonNullOrganization());
            mockUserService.Setup(x => x.GetCurrentUser())
                .Returns(GetNonNullUser());

            var _cognitoService = new CognitoService(mocklogger.Object,
                mockClient.Object,
                mockUserService.Object,
                mockOrgService.Object,
                _cognitoIAMCredentials,
                _cognitoClientConfig);

            var result = await _cognitoService.CreateOrganizationAsync(command);

            Assert.IsAssignableFrom<Organization>(result);
            mockClient.Verify(x => x.CreateGroupAsync(It.Is<CreateGroupRequest>(y => String.Equals(y.GroupName, trimmedOrgName))), Times.Once);
            mockOrgService.Verify(x => x.CreateOrganizationAsync(It.IsAny<OrganizationCreateCommand>()), Times.Once);
            mockUserService.Verify(x => x.GetCurrentUser(), Times.Once);
        }

        private Organization GetNonNullOrganization()
        {
            return new Organization();
        }

        [Fact]
        public async Task CreateOrganizationAsync_NullInput_ShouldThrowInvalidArgumentException()
        {
            var mocklogger = new Mock<ILogger<CognitoService>>();
            var mockClient = new Mock<IAryaIdentityProviderClient>();
            var mockUserService = new Mock<IUserService>();
            var mockOrgService = new Mock<IOrganizationService>();
            

            var _cognitoService = new CognitoService(mocklogger.Object,
                mockClient.Object,
                mockUserService.Object,
                mockOrgService.Object,
                 _cognitoIAMCredentials,
                _cognitoClientConfig);

            await Assert.ThrowsAsync<InvalidArgumentException>(async () => await _cognitoService.CreateOrganizationAsync(null));
        }
    }
}
