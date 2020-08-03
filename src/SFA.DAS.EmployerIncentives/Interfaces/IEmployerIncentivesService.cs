using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Models;
using SFA.DAS.EmployerIncentives.Models.Commitments;
using SFA.DAS.EmployerIncentives.Models.EmployerIncentives;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy();
        Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship);
        Task<GetAccountLegalEntitiesResponse> GetAccountLegalEntities(long accountId);
        Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId);
        Task<AccountLegalEntity> CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity);
        Task CreateIncentiveApplication(CreateIncentiveApplication request);
    }
}