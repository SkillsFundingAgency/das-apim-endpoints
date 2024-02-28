using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.ApprenticeCommitments.Application.Services;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public class CommitmentsV2HealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Employer Commitments V2 Api Health Check";
        private readonly CommitmentsV2Service _service;

        public CommitmentsV2HealthCheck (CommitmentsV2Service service)
        {
            _service = service;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var isHealthy = await _service.IsHealthy();
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            var data = new Dictionary<string, object> { { "Duration", durationString } };

            return (isHealthy ? HealthCheckResult.Healthy(HealthCheckResultDescription, data)
                : HealthCheckResult.Unhealthy(HealthCheckResultDescription, null, data));

        }
    }
}