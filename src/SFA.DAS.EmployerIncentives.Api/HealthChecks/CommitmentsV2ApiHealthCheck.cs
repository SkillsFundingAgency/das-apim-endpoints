using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.HealthChecks
{
    public class CommitmentsV2ApiHealthCheck : IHealthCheck
    {
        private readonly ICommitmentsV2Service _commitmentsV2Service;

        public CommitmentsV2ApiHealthCheck(ICommitmentsV2Service commitmentsV2Service)
        {
            _commitmentsV2Service = commitmentsV2Service;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return (await _commitmentsV2Service.IsHealthy(cancellationToken) ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
        }
    }
}
