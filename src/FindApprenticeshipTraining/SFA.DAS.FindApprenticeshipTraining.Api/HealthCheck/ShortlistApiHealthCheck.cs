using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.Api.HealthCheck
{
    public class ShortlistApiHealthCheck: IHealthCheck
    {
        private const string HealthCheckResultDescription = "Shortlist Api check";
        
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _apiClient;
        private readonly ILogger<ShortlistApiHealthCheck> _logger;

        public ShortlistApiHealthCheck(IShortlistApiClient<ShortlistApiConfiguration> apiClient, ILogger<ShortlistApiHealthCheck> logger)
        {
            _apiClient = apiClient;
            _logger = logger;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            _logger.LogInformation("Pinging Shortlist API");
            
            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();
            
            if ((int)response == 200)
            {
                var durationString = timer.Elapsed.ToHumanReadableString();
            
                _logger.LogInformation($"Shortlist API ping successful and took {durationString}");
            
                return HealthCheckResult.Healthy(HealthCheckResultDescription,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }
            
            _logger.LogWarning($"Shortlist API ping failed : [Code: {response}]");
                return HealthCheckResult.Unhealthy(HealthCheckResultDescription);
        }
    }
}
