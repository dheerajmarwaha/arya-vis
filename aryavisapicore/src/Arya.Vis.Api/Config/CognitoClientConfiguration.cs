using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Config
{
    public class CognitoClientConfiguration
    {
        public string Id { get; set; }
        public string Secret { get; set; }
        public string UserPoolId { get; set; }
    }
}
