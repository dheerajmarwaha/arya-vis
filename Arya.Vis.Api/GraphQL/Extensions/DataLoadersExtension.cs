using Arya.Vis.Api.DataLoaders;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Arya.Vis.Api.GraphQL.Extensions {
    public static class DataLoadersExtension {
        public static IServiceCollection AddDataLoaders(this IServiceCollection services) {
            services.AddDataLoaderRegistry ();
            services.AddScoped<InterviewDataLoader>();
            return services;
        }
    }
}
