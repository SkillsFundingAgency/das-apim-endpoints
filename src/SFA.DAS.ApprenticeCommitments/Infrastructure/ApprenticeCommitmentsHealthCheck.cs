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
        private const string HealthCheckResultDescription = "Apprentice Commitments Api Health Check";
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
            var data = new Dictionary<string, object> { { "Duration", durationString } };

            return (isHealthy ? HealthCheckResult.Healthy(HealthCheckResultDescription, data)
                : HealthCheckResult.Unhealthy(HealthCheckResultDescription, null, data));
        }
    }
}