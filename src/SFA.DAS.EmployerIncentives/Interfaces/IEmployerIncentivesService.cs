using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using SFA.DAS.EmployerIncentives.Models.EmployerIncentives;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy(CancellationToken cancellationToken = default);
        Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship, CancellationToken cancellationToken = default);
        Task CreateIncentiveApplication(CreateIncentiveApplicationRequest request, CancellationToken cancellationToken = default);
    }
}