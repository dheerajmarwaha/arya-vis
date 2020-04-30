using Amazon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Config
{
    public class CognitoIAMCredentials
    {
        public string AccessKeyId { get; set; }
        public string SecretAccessKey { get; set; }
        public string UserPoolId { get; set; }

        public string RegionEndpointSystemName { get; set; }

        public RegionEndpoint RegionEndpoint
        {
            get { return RegionEndpoint.GetBySystemName(RegionEndpointSystemName); }
        }
    }
}
