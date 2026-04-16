using MediatR;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerFeedback.Api.TaskQueue
{
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private ConcurrentQueue<(IBaseRequest Request, string RequestName, Action<object, TimeSpan, ILogger<TaskQueueHostedService>> response)> _requests = new ConcurrentQueue<(IBaseRequest, string, Action<object, TimeSpan, ILogger<TaskQueueHostedService>>)>();
        private SemaphoreSlim _signal = new SemaphoreSlim(0);

        public void QueueBackgroundRequest(IBaseRequest request, string requestName, Action<object, TimeSpan, ILogger<TaskQueueHostedService>> response)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            _requests.Enqueue((request, requestName, response));
            _signal.Release();
        }

        public async Task<(IBaseRequest Request, string RequestName, Action<object, TimeSpan, ILogger<TaskQueueHostedService>> Response)> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _requests.TryDequeue(out var request);

            return request;
        }
    }
}
