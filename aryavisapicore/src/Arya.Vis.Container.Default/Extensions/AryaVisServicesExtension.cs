using Arya.Vis.Core.Services;
using Arya.Vis.Core.ServicesImpl;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Arya.Vis.Container.Default.Extensions
{
    public static class AryaVisServicesExtension
    {
        public static IServiceCollection AddAryaVisServices(this IServiceCollection services, IConfiguration configuration) {
            services.AddScoped<IInterviewService, InterviewService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrganizationService, OrganizationService>();
            services.AddScoped<IEmailService, EmailService>();
            return services;            
        }       
    }
}