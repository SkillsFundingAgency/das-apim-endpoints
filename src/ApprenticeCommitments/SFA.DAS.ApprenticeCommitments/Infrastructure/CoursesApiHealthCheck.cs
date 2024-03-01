using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Infrastructure
{
    public class CoursesApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Courses Api Health Check";
        private readonly CoursesService _service;

        public CoursesApiHealthCheck(CoursesService service)
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