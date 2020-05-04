using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arya.Vis.Container.Default.BackgroundServices
{
    public interface IBackgroundTaskQueue
    {
        Task QueueBackgroundWorkItemAsync(Func<CancellationToken, Task> workItem);
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
