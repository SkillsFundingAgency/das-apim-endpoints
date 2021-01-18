using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public class ApprenticeCommitmentsHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Employer Incentives Api Health Check";
        private readonly ApprenticeCommitmentsService service;

        public ApprenticeCommitmentsHealthCheck(ApprenticeCommitmentsService service)
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