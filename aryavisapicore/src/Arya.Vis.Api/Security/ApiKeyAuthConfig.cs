using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Security
{
    public class ApiKeyAuthConfig
    {
        public Guid ApplicationId { get; set; }
        public string ApplicationKey { get; set; }
    }
}
