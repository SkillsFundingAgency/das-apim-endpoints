using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.Api.Common.Infrastructure;
using SFA.DAS.Funding.Interfaces;

namespace SFA.DAS.Funding.Infrastructure
{
    public class FundingApprenticeshipEarningsHealthCheck : IHealthCheck
    {
        private const string HealthCheckResultDescription = "Employer Incentives Api Health Check";
        private readonly IFundingApprenticeshipEarningsService _fundingApprenticeshipEarningsService;

        public FundingApprenticeshipEarningsHealthCheck(IFundingApprenticeshipEarningsService fundingApprenticeshipEarningsService)
        {
            _fundingApprenticeshipEarningsService = fundingApprenticeshipEarningsService;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            var timer = Stopwatch.StartNew();
            var isHealthy = await _fundingApprenticeshipEarningsService.IsHealthy();
            timer.Stop();
            var durationString = timer.Elapsed.ToHumanReadableString();
            
            return (isHealthy ? HealthCheckResult.Healthy(HealthCheckResultDescription,new Dictionary<string, object> { { "Duration", durationString } }) 
                : HealthCheckResult.Unhealthy(HealthCheckResultDescription,null,new Dictionary<string, object> { { "Duration", durationString } }));
        }
    }
}