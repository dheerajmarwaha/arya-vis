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
            services.AddScoped<ISqlProvider, MySqlProvider>(_ => {
                return new MySqlProvider(configuration["ConnectionStrings:AryaVisConnection"]);
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IInterviewRepository, InterviewRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();

            services.AddScoped<IAccessRepository, AccessRepository>();
            services.AddScoped<ISystemConfigurationRepository, SystemConfigurationRepository>();

            services.AddScoped<IConfigurationRepository, ConfigurationRepository>();


            return services;
        }
    }
}