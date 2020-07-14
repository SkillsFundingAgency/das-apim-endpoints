using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Interfaces;

namespace SFA.DAS.EmployerIncentives.Api.HealthChecks
{
    public class InnerApiHealthCheck : IHealthCheck
    {
        private readonly IEmployerIncentivesService _employerIncentivesService;

        public InnerApiHealthCheck(IEmployerIncentivesService employerIncentivesService)
        {
            _employerIncentivesService = employerIncentivesService;
        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            return _employerIncentivesService.HealthCheck(cancellationToken);
        }
    }
}
