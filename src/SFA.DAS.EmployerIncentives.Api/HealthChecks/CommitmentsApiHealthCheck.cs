using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.HealthChecks
{
    public class CommitmentsApiHealthCheck : IHealthCheck
    {
        private readonly ICommitmentsService _commitmentsV2Service;

        public CommitmentsApiHealthCheck(ICommitmentsService commitmentsV2Service)
        {
            _commitmentsV2Service = commitmentsV2Service;
        }
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return (await _commitmentsV2Service.IsHealthy() ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy());
        }
    }
}
