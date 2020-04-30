using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arya.Vis.Api.Security
{
    public class ApiKeyAuthenticationOptions : AuthenticationSchemeOptions
    {
        public ApiKeyAuthenticationOptions() { }

        public new ApiKeyAuthenticationEvents Events
        {
            get { return (ApiKeyAuthenticationEvents)base.Events; }

            set { base.Events = value; }
        }
    }
}
