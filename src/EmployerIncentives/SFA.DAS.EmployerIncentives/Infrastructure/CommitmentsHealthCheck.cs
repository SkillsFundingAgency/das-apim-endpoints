using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Infrastructure
{
    public class CommitmentsHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Employer Commitments Api Health Check";
        private readonly ICommitmentsApiClient<CommitmentsConfiguration> _client;

        public CommitmentsHealthCheck (ICommitmentsApiClient<CommitmentsConfiguration> client)
        {
            _client = client;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var result = await _client.GetResponseCode(new GetCommitmentsPingRequest());
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            if (result != HttpStatusCode.OK)
            {
                return HealthCheckResult.Unhealthy(HealthCheckResultDescription, null,
                    new Dictionary<string, object> { { "Duration", durationString } });
            }

            return HealthCheckResult.Healthy(HealthCheckResultDescription,
                new Dictionary<string, object> { { "Duration", durationString } }) ;
        }
    }
}