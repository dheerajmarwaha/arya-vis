using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RedLockNet;
using RedLockNet.SERedis;
using RedLockNet.SERedis.Configuration;
using StackExchange.Redis;

namespace Arya.Vis.Container.Default.Extensions
{
    public static class DistributedLockFactoryExtension
    {
        public static IServiceCollection AddDistributedRedisLockFactory(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IDistributedLockFactory>(serviceProvider =>
            {
                var loggerFactory = serviceProvider.GetService<ILoggerFactory>();
                var hostName = configuration.GetValue<string>("Redis:HostName");
                var port = configuration.GetValue<int>("Redis:Port");
                var connectionConfig = new ConfigurationOptions
                {
                    AbortOnConnectFail = false,
                    EndPoints = { { hostName, port } },
                    ConnectTimeout = 15000,
                };
                var redlockMultiplexers = new List<RedLockMultiplexer> {
                    ConnectionMultiplexer.Connect(connectionConfig)
                };
                return RedLockFactory.Create(redlockMultiplexers, loggerFactory);
            });
            return services;
        }
    }
}
