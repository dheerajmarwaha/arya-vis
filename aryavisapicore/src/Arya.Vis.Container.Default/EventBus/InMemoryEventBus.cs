using Arya.ServiceBus;
using Arya.Vis.Container.Default.BackgroundServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Default.EventBus
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ILogger<InMemoryEventBus> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _serviceProvder;

        public InMemoryEventBus(ILogger<InMemoryEventBus> logger, IEventBusSubscriptionsManager subsManager, IServiceProvider serviceProvider, IBackgroundTaskQueue taskQueue)
        {
            TaskQueue = taskQueue;
            _logger = logger;
            _subsManager = subsManager;
            _serviceProvder = serviceProvider;
        }

        public IBackgroundTaskQueue TaskQueue { get; }

        public async Task Publish(IEvent aryaEvent)
        {
            var eventName = aryaEvent.GetType().Name;
            _logger.LogInformation("Publishing event {@EventName}", eventName);
            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {
                var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                _logger.LogInformation("Found subscribers {@Subscriptions} for event {@EventName}", subscriptions, eventName);
                foreach (var subscription in subscriptions)
                {
                    await TaskQueue.QueueBackgroundWorkItemAsync(async token => {
                        using (var scope = _serviceProvder.CreateScope())
                        {
                            var handler = scope.ServiceProvider.GetService(subscription);
                            if (handler == null) return;
                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                            _logger.LogInformation("Requesting {@ConcreteType}({@Subscription}) to handle event {@EventName}", concreteType, subscription, eventName);
                            try
                            {
                                await (Task)concreteType.GetMethod("HandleAsync").Invoke(handler, new object[] { aryaEvent });
                            }
                            catch (Exception e)
                            {
                                _logger.LogError(e, "Error occurred while handling event {@EventName} by {@ConcreteType}({@Subscription})", eventName, concreteType, subscription);
                            }
                            _logger.LogInformation("{@ConcreteType}({@Subscription}) completed handling event {@EventName}", concreteType, subscription, eventName);
                        }
                    });
                }
            }
        }

        public void Subscribe<T, TH>()
        where T : IEvent
        where TH : IEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            _subsManager.AddSubscription<T, TH>();
        }

        public void Unsubscribe<T, TH>()
        where T : IEvent
        where TH : IEventHandler<T>
        {
            _subsManager.RemoveSubscription<T, TH>();
        }
    }
}
