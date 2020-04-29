using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Security
{
    public static class AuthConstants
    {
        public const string AuthSchemes = ApiKeyAuthConstants.Scheme + "," + JwtBearerDefaults.AuthenticationScheme;
    }
}
