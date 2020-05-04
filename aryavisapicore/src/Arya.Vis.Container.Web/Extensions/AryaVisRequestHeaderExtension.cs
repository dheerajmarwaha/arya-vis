using Arya.Vis.Container.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Extensions
{
    public static class AryaVisRequestHeaderExtension
    {
        public static IApplicationBuilder UseAryaVisHeaderSettings(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AryaVisHeaderSettingsMiddleware>();
        }
    }
}
