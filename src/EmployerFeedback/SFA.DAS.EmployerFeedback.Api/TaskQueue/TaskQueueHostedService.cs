using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace SFA.DAS.EmployerFeedback.Api.TaskQueue
{
    public class TaskQueueHostedService : BackgroundService
    {
        private readonly ILogger<TaskQueueHostedService> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceProvider _serviceProvider;

        public TaskQueueHostedService(IBackgroundTaskQueue backgroundTaskQueue,
            ILogger<TaskQueueHostedService> logger,
            IServiceProvider serviceProvider)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Task Queue Hosted Service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                var (request, requestName, responseAction) = await _backgroundTaskQueue.DequeueAsync(stoppingToken);

                // cannot use the default per-HTTP scope as it will be cleared when the request finishes
                // this would introduce a race condition, create a new DI scope instead
                using (var scope = _serviceProvider.CreateScope())
                {
                    try
                    {
                        var scopedMediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                        var scopedLogger = scope.ServiceProvider.GetRequiredService<ILogger<TaskQueueHostedService>>();

                        Func<CancellationToken, Task> workItem = async token =>
                        {
                            var started = DateTime.UtcNow;
                            var response = await scopedMediator.Send(request, token);
                            var duration = DateTime.UtcNow - started;

                            responseAction(response, duration, scopedLogger);
                        };

                        await workItem(stoppingToken);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error occurred executing {requestName}.", requestName);
                    }
                }
            }

            _logger.LogInformation("Task Queue Hosted Service is stopping.");
        }
    }
}