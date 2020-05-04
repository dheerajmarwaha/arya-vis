using Arya.Vis.Container.Web.Middleware;
using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Extensions
{
    public static class RequestTimeLoggerExtension
    {
        public static IApplicationBuilder UseRequestTimeLogger(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RequestTimeLoggerMiddleware>();
        }
    }
}
