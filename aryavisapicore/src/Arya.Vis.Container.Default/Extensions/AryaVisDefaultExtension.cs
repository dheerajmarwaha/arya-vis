
using Arya.Vis.Repository.Cache;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Arya.Vis.Container.Default.Extensions
{
    public static class AryaVisDefaultExtension
    {
        public static IServiceCollection AddAryaVisDefault(this IServiceCollection services
                                                                , IConfiguration configuration
                                                                , IWebHostEnvironment environment) {
            services
                .AddAryaVisEventBus(configuration)
                .AddAryaVisRepositories(configuration)
                .AddAryaVisServices(configuration)
                .AddAryaVisGraphQL(configuration, environment)
                .AddAryaVisBackgroundServices(configuration);

            services.AddMemoryCache();

            services.AddDistributedRedisLockFactory(configuration);
            services.AddSingleton<ISystemConfigurationCache, SystemConfigurationCache>();
            services.AddSingleton<IFeaturesAccessCache, FeaturesAccessCache>();
            return services;
        }
    }
}