using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Middleware
{
    public class RequestTimeLoggerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestTimeLoggerMiddleware> _logger;

        public RequestTimeLoggerMiddleware(RequestDelegate next, ILogger<RequestTimeLoggerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var requestStartTime = DateTime.UtcNow;
            await _next(context);
            var requestEndTime = DateTime.UtcNow;

            var totalRequestTimeSpan = requestEndTime - requestStartTime;
            var totalRequestTimeInMilliseconds = totalRequestTimeSpan.TotalMilliseconds;
            
            _logger.LogInformation("{@LogEvent}, {@TotalRequestTime}", "REQUEST_TIME", totalRequestTimeInMilliseconds);
        }
    }
}
