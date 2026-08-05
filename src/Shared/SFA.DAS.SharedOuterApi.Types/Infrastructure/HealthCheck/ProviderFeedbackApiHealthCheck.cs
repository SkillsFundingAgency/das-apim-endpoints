using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Apim.Shared.InnerApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Diagnostics;

namespace SFA.DAS.SharedOuterApi.Types.Infrastructure.HealthCheck;

public class ProviderFeedbackApiHealthCheck(
    IProviderFeedbackApiClient<ProviderFeedbackApiConfiguration> apiClient,
    ILogger<ProviderFeedbackApiHealthCheck> logger)
    : IHealthCheck
{
    public const string HealthCheckResultDescription = "ProviderFeedback Api health check";

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken)
    {
        logger.LogInformation("Pinging ProviderFeedback API");

        var timer = Stopwatch.StartNew();
        var response = await apiClient.GetResponseCode(new GetPingRequest());
        timer.Stop();

        if ((int)response == 200)
        {
            var durationString = timer.Elapsed.ToHumanReadableString();

            logger.LogInformation($"ProviderFeedback API ping successful and took {durationString}");

            return HealthCheckResult.Healthy(HealthCheckResultDescription,
                new Dictionary<string, object> { { "Duration", durationString } });
        }

        logger.LogWarning($"ProviderFeedback API ping failed : [Code: {response}]");
        return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
    }
}