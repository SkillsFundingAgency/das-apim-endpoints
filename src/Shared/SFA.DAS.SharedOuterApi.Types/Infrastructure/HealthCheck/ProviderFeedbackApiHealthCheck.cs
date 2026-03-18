using System.Diagnostics;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Apim.Shared.InnerApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck
{
    public class ProviderFeedbackApiHealthCheck : IHealthCheck
    {
        public const string HealthCheckResultDescription = "ProviderFeedback Api health check";

        private readonly IProviderFeedbackApiClient<ProviderFeedbackApiConfiguration> _apiClient;
        private readonly ILogger<ProviderFeedbackApiHealthCheck> _logger;

        public ProviderFeedbackApiHealthCheck(IProviderFeedbackApiClient<ProviderFeedbackApiConfiguration> apiClient, ILogger<ProviderFeedbackApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pinging ProviderFeedback API");

            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();

            if ((int)response == 200)
            {
                var durationString = timer.Elapsed.ToHumanReadableString();

                _logger.LogInformation($"ProviderFeedback API ping successful and took {durationString}");

                return HealthCheckResult.Healthy(HealthCheckResultDescription,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }

            _logger.LogWarning($"ProviderFeedback API ping failed : [Code: {response}]");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}