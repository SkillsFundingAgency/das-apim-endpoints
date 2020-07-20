using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using SFA.DAS.EmployerIncentives.Models.Commitments;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICommitmentsV2Service
    {
        Task<HealthCheckResult> HealthCheck(CancellationToken cancellationToken = default);
        Task<IEnumerable<ApprenticeshipItem>> Apprenticeships(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default);
    }
}