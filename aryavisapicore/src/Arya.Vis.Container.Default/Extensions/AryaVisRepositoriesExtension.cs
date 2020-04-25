using Arya.Vis.Core.Repositories;
using Arya.Vis.Repository;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;   

namespace Arya.Vis.Container.Default.Extensions
{
    public static class AryaVisRepositoriesExtension
    {
        public static IServiceCollection AddAryaVisRepositories(this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IInterviewRepository, InterviewRepository>();

            return services;
        }
    }
}