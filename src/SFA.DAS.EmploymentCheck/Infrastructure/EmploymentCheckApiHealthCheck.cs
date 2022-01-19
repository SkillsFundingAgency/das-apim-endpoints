using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.EmploymentCheck.Clients;
using SFA.DAS.EmploymentCheck.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmploymentCheck.Infrastructure
{
    public class EmploymentCheckApiHealthCheck : IHealthCheck
    {
        public const string HealthCheckResultDescription = "EmploymentCheck Api Health Check";
        private readonly IEmploymentCheckApiClient<EmploymentCheckConfiguration> _client;

        public EmploymentCheckApiHealthCheck(IEmploymentCheckApiClient<EmploymentCheckConfiguration> client)
        {
            _client = client;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var result = await _client.GetResponseCode(new GetPingRequest());
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