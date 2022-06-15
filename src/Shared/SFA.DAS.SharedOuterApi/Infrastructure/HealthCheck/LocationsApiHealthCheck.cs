using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
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
    public class LocationsApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Location API Health Check";
        
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;
        private readonly ILogger<LocationsApiHealthCheck> _logger;

        public LocationsApiHealthCheck (ILocationApiClient<LocationApiConfiguration> apiClient, ILogger<LocationsApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();

            if (response == HttpStatusCode.OK)
            {
                var durationString = timer.Elapsed.ToHumanReadableString();
                return HealthCheckResult.Healthy(HealthCheckResultDescription,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }
            
            _logger.LogError($"Location API ping failed : [Code: {response}]");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}