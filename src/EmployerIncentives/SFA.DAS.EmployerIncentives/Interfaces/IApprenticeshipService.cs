using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IApprenticeshipService
    {
        Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship);
    }
}
