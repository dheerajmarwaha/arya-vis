using Amazon;
using Arya.Vis.Api.Config;
using Arya.Vis.Core.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace Arya.Vis.Api.Tests
{
    public class CognitoIAMCredentialsTests
    {
        [Fact]
        public void RegionEndpoint_ValidInput_ShouldPass()
        {
            CognitoIAMCredentials cognitoIAMCredentials = new CognitoIAMCredentials
            {
                RegionEndpointSystemName = "us-east-1"
            };

            var regionEndPoint = cognitoIAMCredentials.RegionEndpoint;

            Assert.Equal(RegionEndpoint.USEast1, regionEndPoint);
        }

        [Fact]
        public void CognitoIAMCredentials_CognitoIAMCredentials_ShouldPass()
        {
            ConfigurationManager configurationManager = new ConfigurationManager("appsettings.test.json");
            
            var _cognitoIAMCredentials = configurationManager.Configuration.GetSection("AWSCognito").Get<CognitoIAMCredentials>();

            Assert.NotNull(_cognitoIAMCredentials.AccessKeyId);
            Assert.NotNull(_cognitoIAMCredentials.SecretAccessKey);
            Assert.NotNull(_cognitoIAMCredentials.RegionEndpointSystemName);
        }

    }
}
