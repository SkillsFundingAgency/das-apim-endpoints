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
    public class ApprenticeAccountsApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Apprentice Accounts Api check";

        private readonly IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> _apiClient;
        private readonly ILogger<ApprenticeAccountsApiHealthCheck> _logger;

        public ApprenticeAccountsApiHealthCheck(IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration> apiClient, ILogger<ApprenticeAccountsApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            _logger.LogInformation("Pinging ApprenticeAccounts API");

            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();

            if ((int)response == 200)
            {
                var durationString = timer.Elapsed.ToHumanReadableString();

                _logger.LogInformation($"ApprenticeAccounts API ping successful and took {durationString}");

                return HealthCheckResult.Healthy(HealthCheckResultDescription,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }

            _logger.LogWarning($"ApprenticeAccounts API ping failed : [Code: {response}]");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}