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
    public class EarlyConnectApiHealthCheck : IHealthCheck
    {
        public const string HealthCheckResultDescription = "EarlyConnect Api health check";

        private readonly IEarlyConnectApiClient<EarlyConnectApiConfiguration> _apiClient;
        private readonly ILogger<EarlyConnectApiHealthCheck> _logger;

        public EarlyConnectApiHealthCheck(IEarlyConnectApiClient<EarlyConnectApiConfiguration> apiClient, ILogger<EarlyConnectApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pinging EarlyConnect API");

            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();

            if ((int)response == 200)
            {
                var durationString = timer.Elapsed.ToHumanReadableString();

                _logger.LogInformation($"EarlyConnect API ping successful and took {durationString}");

                return HealthCheckResult.Healthy(HealthCheckResultDescription,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }

            _logger.LogWarning($"EarlyConnect API ping failed : [Code: {response}]");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}