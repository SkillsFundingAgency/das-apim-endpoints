using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.RecruitJobs.Api.Core.BackgroundWork;

[ExcludeFromCodeCoverage]
public class BackgroundWorkHostedService(
    IBackgroundWorkQueue queue,
    IServiceScopeFactory scopeFactory,
    ILogger<BackgroundWorkHostedService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            string name;
            BackgroundWorkItem workItem;
            try
            {
                (name, workItem) = await queue.DequeueAsync(stoppingToken);
            }
            catch (OperationCanceledException)
            {
                break;
            }

            await using var scope = scopeFactory.CreateAsyncScope();
            var stopwatch = Stopwatch.StartNew();
            try
            {
                await workItem(scope.ServiceProvider, stoppingToken);
                logger.LogInformation("Background work {Name} completed in {ElapsedMilliseconds}ms", name, stopwatch.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Background work {Name} failed after {ElapsedMilliseconds}ms", name, stopwatch.ElapsedMilliseconds);
            }
        }
    }
}
