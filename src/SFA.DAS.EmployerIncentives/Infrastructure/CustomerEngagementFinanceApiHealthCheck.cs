using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.EmployerIncentives.Configuration;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.CustomerEngagementFinance;
using SFA.DAS.EmployerIncentives.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Infrastructure
{
    public class CustomerEngagementFinanceApiHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Customer Engagement Finance Api Health Check";
        private readonly ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration> _client;

        public CustomerEngagementFinanceApiHealthCheck(ICustomerEngagementFinanceApiClient<CustomerEngagementFinanceConfiguration> client)
        {
            _client = client;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var result = await _client.GetResponseCode(new GetCustomerEngagementFinanceHeartbeatRequest());
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