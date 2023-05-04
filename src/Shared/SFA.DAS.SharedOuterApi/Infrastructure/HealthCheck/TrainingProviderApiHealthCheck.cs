using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class TrainingProviderApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Training Provider Service Health Check";
        private readonly TrainingProviderService _service;

        public TrainingProviderApiHealthCheck(TrainingProviderService service)
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