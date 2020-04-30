using Amazon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Arya.Vis.Api.Tests")]
namespace Arya.Vis.Api.Config
{    
    public class CognitoIAMCredentials
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string UserPoolId { get; set; }

        public string RegionEndpointSystemName { get; set; }

        internal RegionEndpoint RegionEndpoint
        {
            get { return RegionEndpoint.GetBySystemName(RegionEndpointSystemName); }
        }
    }
}
