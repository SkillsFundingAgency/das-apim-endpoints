using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.HealthChecks
{
    public class InnerApiHealthCheck : IHealthCheck
    {
        private readonly IEmployerIncentivesPassThroughService _employerIncentivesPassThroughService;

        public InnerApiHealthCheck(IEmployerIncentivesPassThroughService employerIncentivesPassThroughService)
        {
            _employerIncentivesPassThroughService = employerIncentivesPassThroughService;
        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return _employerIncentivesPassThroughService.HealthCheck(cancellationToken);
        }
    }
}
