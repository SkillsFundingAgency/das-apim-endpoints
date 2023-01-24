using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class CoursesApiHealthCheck : IHealthCheck
    {
        public const string HealthCheckResultDescription = "Courses Api check";

        private readonly ICoursesApiClient<CoursesApiConfiguration> _apiClient;
        private readonly ILogger<CoursesApiHealthCheck> _logger;

        public CoursesApiHealthCheck(ICoursesApiClient<CoursesApiConfiguration> apiClient, ILogger<CoursesApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pinging Courses API");

            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();

            if ((int)response == 200)
            {
                var durationString = timer.Elapsed.ToHumanReadableString();

                _logger.LogInformation($"Courses API ping successful and took {durationString}");

                return HealthCheckResult.Healthy(HealthCheckResultDescription,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }

            _logger.LogWarning($"Courses API ping failed : [Code: {response}]");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}