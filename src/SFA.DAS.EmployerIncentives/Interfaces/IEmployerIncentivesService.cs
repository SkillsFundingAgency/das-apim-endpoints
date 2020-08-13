using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.InnerApi.Responses;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerIncentives.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests.IncentiveApplication;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IEmployerIncentivesService
    {
        Task<bool> IsHealthy();
        Task<ApprenticeshipItem[]> GetEligibleApprenticeships(IEnumerable<ApprenticeshipItem> allApprenticeship);
        Task<AccountLegalEntity[]> GetAccountLegalEntities(long accountId);
		Task<AccountLegalEntity> GetLegalEntity(long accountId, long accountLegalEntityId);
        Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId);
        Task ConfirmIncentiveApplication(ConfirmIncentiveApplicationRequest request, CancellationToken cancellationToken = default);
        Task<AccountLegalEntity> CreateLegalEntity(long accountId, AccountLegalEntityCreateRequest accountLegalEntity);
        Task<IncentiveApplicationDto> GetApplication(long accountId, Guid applicationId);
        Task CreateIncentiveApplication(CreateIncentiveApplicationRequestData requestData);
        Task UpdateIncentiveApplication(UpdateIncentiveApplicationRequestData requestData);
    }
}