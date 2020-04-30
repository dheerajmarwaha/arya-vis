using Amazon;
using Arya.Vis.Api.Config;
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
        public  void RegionEndpoint_ValidInput_ShouldPass()
        {
            CognitoIAMCredentials cognitoIAMCredentials = new CognitoIAMCredentials { 
                RegionEndpointSystemName = "us-east-1"
            };

            var regionEndPoint = cognitoIAMCredentials.RegionEndpoint;

             Assert.Equal(RegionEndpoint.USEast1, regionEndPoint);
        }
    }
}
