﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.Infrastructure
{
    public class ApprenticeCommitmentsApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Apprentice Commitments Api Health Check";

        private readonly IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _apiClient;
        private readonly ILogger<ApprenticeCommitmentsApiHealthCheck> _logger;

        public ApprenticeCommitmentsApiHealthCheck(
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> apiClient,
            ILogger<ApprenticeCommitmentsApiHealthCheck> logger)
            => (_apiClient, _logger) = (apiClient, logger);

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var response = await _apiClient.GetResponseCode(new GetPingRequest());
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            var data = new Dictionary<string, object> { { "Duration", durationString } };

            if ((int)response == 200)
            {
                return HealthCheckResult.Healthy(HealthCheckResultDescription, data);
            }

            _logger.LogWarning($"{HealthCheckResultDescription} API ping failed : [Code: {response}]");
            return HealthCheckResult.Unhealthy(HealthCheckResultDescription, null, data);
        }
    }
}
