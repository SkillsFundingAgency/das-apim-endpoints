using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.Infrastructure.HealthCheck
{
    public class FinanceApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Employer Finance Api Health Check";
        private readonly IFinanceApiClient<FinanceApiConfiguration> _client;

        public FinanceApiHealthCheck(IFinanceApiClient<FinanceApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var result = await _client.GetResponseCode(new GetHealthCheckRequest());
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            if (result != HttpStatusCode.OK)
            {
                return HealthCheckResult.Unhealthy(HealthCheckResultDescription, null,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription,
                new Dictionary<string, object> { { "Duration", durationString } });
        }
    }
}