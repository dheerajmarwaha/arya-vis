
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
                .AddAryaVisRepositories(configuration)
                .AddAryaVisServices(configuration)
                .AddAryaVisGraphQL(configuration, environment);
            return services;
        }
    }
}