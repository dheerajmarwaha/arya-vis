using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Security
{
    public class ApiKeyAuthenticationFailedContext : ResultContext<ApiKeyAuthenticationOptions>
    {
        public ApiKeyAuthenticationFailedContext(
            HttpContext context,
            AuthenticationScheme scheme,
            ApiKeyAuthenticationOptions options
        ) : base(context, scheme, options) { }

        public Exception Exception { get; set; }
    }
}
