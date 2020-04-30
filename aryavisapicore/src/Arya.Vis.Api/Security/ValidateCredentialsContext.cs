using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Security
{
    public class ValidateCredentialsContext : ResultContext<ApiKeyAuthenticationOptions>
    {
        public ValidateCredentialsContext(
            HttpContext context,
            AuthenticationScheme scheme,
            ApiKeyAuthenticationOptions options
        ) : base(context, scheme, options) { }

        public Guid ApplicationId { get; set; }

        public string ApplicationKey { get; set; }
        public string Email { get; set; }
    }
}
