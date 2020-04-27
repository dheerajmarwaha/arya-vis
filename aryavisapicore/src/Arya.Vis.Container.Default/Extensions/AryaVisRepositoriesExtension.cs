using Arya.Vis.Core.Repositories;
using Arya.Vis.Repository;
using Microsoft.Extensions.Configuration; 
using Microsoft.Extensions.DependencyInjection;
using Arya.Storage;
using Arya.Storage.MySql;

namespace Arya.Vis.Container.Default.Extensions
{
    public static class AryaVisRepositoriesExtension
    {
        public static IServiceCollection AddAryaVisRepositories(this IServiceCollection services, IConfiguration configuration) {
            // Register Db Provider
            services.AddSingleton<ISqlProvider, MySqlProvider>(_ => {
                return new MySqlProvider(configuration["ConnectionStrings:AryaVisConnection"]);
            });
            services.AddSingleton<IUnitOfWork, UnitOfWork>();
            services.AddSingleton<IInterviewRepository, InterviewRepository>();

            return services;
        }
    }
}