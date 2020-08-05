using System;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy(CancellationToken cancellationToken = default);
        Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship, CancellationToken cancellationToken = default);
        Task<GetAccountLegalEntitiesResponse> GetAccountLegalEntities(long accountId);
        Task<AccountLegalEntity> GetLegalEntity(long accountId, long accountLegalEntityId);
        Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId);
        Task ConfirmIncentiveApplication(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default);
        Task<AccountLegalEntity> CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity);
        Task<IncentiveApplicationDto> GetApplication(long accountId, Guid applicationId);
    }
}