using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.ApprenticeCommitments.Application.Services.ApprenticeLogin;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public class ApprenticeLoginApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Apprentice Login Api Health Check";
        private readonly ApprenticeLoginService _service;

        public ApprenticeLoginApiHealthCheck(ApprenticeLoginService service)
        {
            _service = service;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var isHealthy = await _service.IsHealthy();
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            var data = new Dictionary<string, object> {{"Duration", durationString}};

            return (isHealthy ? HealthCheckResult.Healthy(HealthCheckResultDescription, data)
                : HealthCheckResult.Unhealthy(HealthCheckResultDescription, null, data));
        }
    }
}