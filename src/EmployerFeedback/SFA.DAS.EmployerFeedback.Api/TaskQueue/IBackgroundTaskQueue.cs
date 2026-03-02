using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.EmployerFeedback.Api.TaskQueue
{
    public interface IBackgroundTaskQueue
    {
        void QueueBackgroundRequest(IBaseRequest request, string requestName, Action<object, TimeSpan, ILogger<TaskQueueHostedService>> response);

        Task<(IBaseRequest Request, string RequestName, Action<object, TimeSpan, ILogger<TaskQueueHostedService>> Response)> DequeueAsync(CancellationToken cancellationToken);
    }
}
