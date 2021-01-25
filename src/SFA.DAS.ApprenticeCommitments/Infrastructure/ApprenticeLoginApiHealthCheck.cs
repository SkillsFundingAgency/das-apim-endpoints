using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public class ApprenticeLoginApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Apprentice Login Api Health Check";
        private readonly ApprenticeLoginService service;

        public ApprenticeLoginApiHealthCheck(ApprenticeLoginService service)
        {
            this.service = service;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var isHealthy = await service.IsHealthy();
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();

            return (isHealthy ? HealthCheckResult.Healthy(HealthCheckResultDescription, new Dictionary<string, object> { { "Duration", durationString } })
                : HealthCheckResult.Unhealthy(HealthCheckResultDescription, null, new Dictionary<string, object> { { "Duration", durationString } }));
        }
    }
}