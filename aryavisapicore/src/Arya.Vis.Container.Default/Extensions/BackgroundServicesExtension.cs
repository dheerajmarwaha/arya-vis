using Arya.Vis.Container.Default.BackgroundServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Default.Extensions
{
    public static class BackgroundServicesExtension
    {
        public static IServiceCollection AddAryaVisBackgroundServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddHostedService<QueuedHostedBackgroundService>();
            //services.AddHostedService<TimedHostedCacheRefreshService>();
            services.AddSingleton<IBackgroundTaskQueue, BackgroundTaskQueue>();
            return services;
        }
    }
}
