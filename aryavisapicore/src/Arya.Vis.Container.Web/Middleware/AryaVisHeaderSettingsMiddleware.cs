using Arya.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Middleware
{
    public class AryaVisHeaderSettingsMiddleware
    {
        private const string CacheControl = "Cache-Control";
        private readonly RequestDelegate _next;
        private readonly ILogger<AryaVisHeaderSettingsMiddleware> _logger;

        public AryaVisHeaderSettingsMiddleware(RequestDelegate next, ILogger<AryaVisHeaderSettingsMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            SetHeaderValue(context, CacheControl, "no-cache");
            await _next(context);
        }

        private void SetHeaderValue(HttpContext context, string headerKey, string headerValue)
        {
            if (string.IsNullOrWhiteSpace(headerKey))
            {
                throw new InvalidArgumentException(nameof(headerKey), headerKey, "non empty header key");
            }
            if (context == null)
            {
                throw new InvalidArgumentException(nameof(context), null, "non null HttpContext");
            }
            if (headerValue == null)
            {
                throw new InvalidArgumentException(nameof(headerValue), null, "non empty header value");
            }

            context.Response.Headers[CacheControl] = headerValue;
        }
    }
}
