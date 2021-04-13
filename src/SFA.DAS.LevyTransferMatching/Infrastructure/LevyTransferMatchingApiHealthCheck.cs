using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.Infrastructure
{
    public class LevyTransferMatchingApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Levy Transfer Matching Inner Api Health Check";
        private readonly ILevyTransferMatchingApiClient service;

        public LevyTransferMatchingApiHealthCheck(ILevyTransferMatchingApiClient service)
        {
            this.service = service;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var isHealthy = await service.IsHealthy();
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            var data = new Dictionary<string, object> { { "Duration", durationString } };

            return (isHealthy ? HealthCheckResult.Healthy(HealthCheckResultDescription, data)
                : HealthCheckResult.Unhealthy(HealthCheckResultDescription, null, data));
        }
    }
}
