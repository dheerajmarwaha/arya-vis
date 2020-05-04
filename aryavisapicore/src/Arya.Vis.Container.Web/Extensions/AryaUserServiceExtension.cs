using Arya.Vis.Container.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Extensions
{
    public static class AryaUserServiceExtension
    {
        public static IApplicationBuilder LoadCurrentUser(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UserServiceMiddleware>();
        }
    }
}
