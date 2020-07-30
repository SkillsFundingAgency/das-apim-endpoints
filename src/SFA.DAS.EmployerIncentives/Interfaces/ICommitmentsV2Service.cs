using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Models.Commitments;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ICommitmentsV2Service
    {
        Task<bool> IsHealthy(CancellationToken cancellationToken = default);
        Task<IEnumerable<ApprenticeshipItem>> Apprenticeships(long accountId, long accountLegalEntityId, CancellationToken cancellationToken = default);
        Task<ApprenticeshipResponse[]> GetApprenticeshipDetails(long accountId, IEnumerable<long> apprenticeshipIds, CancellationToken cancellationToken = default);
    }
}