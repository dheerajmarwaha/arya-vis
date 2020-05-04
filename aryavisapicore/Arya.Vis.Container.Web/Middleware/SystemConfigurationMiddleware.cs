using Arya.Vis.Core.Services;
using Arya.Vis.Repository.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Web.Middleware
{
    public class SystemConfigurationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<SystemConfigurationMiddleware> _logger;
        private readonly ISystemConfigurationCache _configurationCache;

        public SystemConfigurationMiddleware(RequestDelegate next, ILogger<SystemConfigurationMiddleware> logger, ISystemConfigurationCache configurationCache)
        {
            _next = next;
            _logger = logger;
            _configurationCache = configurationCache;
        }
        public async Task InvokeAsync(
            HttpContext context,
            ISystemConfigurationService configurationService
        )
        {
            // check if system configuration is loaded
            if (!_configurationCache.IsLoaded())
            {
                await configurationService.LoadAsync();
            }

            // call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
