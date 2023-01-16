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
    public class RoatpCourseManagementApiHealthCheck :IHealthCheck
    {
        private const string HealthCheckResultDescription = "Roatp Course Management Api check";

        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _apiClient;
        private readonly ILogger<RoatpCourseManagementApiHealthCheck> _logger;

        public RoatpCourseManagementApiHealthCheck(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> apiClient, ILogger<RoatpCourseManagementApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pinging Roatp Course Management API");

            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();

            if (response == HttpStatusCode.OK)
            {
                var durationString = timer.Elapsed.ToHumanReadableString();

                _logger.LogInformation($"Roatp Course Management ping successful and took {durationString}");

                return HealthCheckResult.Healthy(HealthCheckResultDescription,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }

            _logger.LogWarning($"Roatp Course Management API ping failed : [Code: {response}]");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}