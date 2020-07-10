using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICommitmentsV2Service
    {
        Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default);
    }
}