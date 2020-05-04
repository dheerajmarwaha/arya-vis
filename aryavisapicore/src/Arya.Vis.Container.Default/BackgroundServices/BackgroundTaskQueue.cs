using Arya.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Default.BackgroundServices
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly Channel<Func<CancellationToken, Task>> TaskQueueChannel;
        private readonly ILogger<BackgroundTaskQueue> _logger;

        public BackgroundTaskQueue(ILogger<BackgroundTaskQueue> logger)
        {
            TaskQueueChannel = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
            _logger = logger;
        }

        public async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var reader = TaskQueueChannel.Reader;
            while (await reader.WaitToReadAsync())
            {
                if (reader.TryRead(out var workItem))
                {
                    try
                    {
                        await workItem(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error occurred executing {nameof(workItem)}.");
                    }
                }
            }
        }

        public async Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new InvalidArgumentException(nameof(workItem), null, "non null work item to queue to the background");
            }
            await TaskQueueChannel.Writer.WriteAsync(workItem);
        }

    }
}
