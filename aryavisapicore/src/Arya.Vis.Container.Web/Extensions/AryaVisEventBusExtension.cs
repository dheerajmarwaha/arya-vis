using Arya.ServiceBus;
using Arya.Vis.Container.Default.EventBus;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Arya.Vis.Container.Web.Extensions
{
    public static class AryaEventBusExtension
    {
        public static IServiceCollection AddAryaEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventBus, InMemoryEventBus>();
            services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
            //services.AddScoped<OrgCreatedFeaturesEventHandler>();
            
            return services;
        }
        public static IEventBus SubscribeToAryaEvents(this IEventBus eventBus)
        {
            //eventBus.Subscribe<CandidateAssociatedEvent, CandidateAssociatedIntegrationEventHandler>();
            
            return eventBus;
        }
        public static IApplicationBuilder UseAryaEventBus(this IApplicationBuilder app)
        {
            var eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.SubscribeToAryaEvents();
            return app;
        }
    }
}
