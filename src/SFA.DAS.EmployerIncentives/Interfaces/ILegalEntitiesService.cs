using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using SFA.DAS.EmployerIncentives.Models;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface ILegalEntitiesService
    {
        Task<AccountLegalEntity[]> GetAccountLegalEntities(long accountId);
        Task<AccountLegalEntity> GetLegalEntity(long accountId, long accountLegalEntityId);
        Task DeleteAccountLegalEntity(long accountId, long accountLegalEntityId);
        Task CreateLegalEntity(AccountLegalEntityCreateRequest accountLegalEntity);
        Task RefreshLegalEntities(IEnumerable<InnerApi.Responses.Accounts.AccountLegalEntity> accountLegalEntities, int pageNumber, int pageSize, int totalPages);
        Task SignAgreement(SignAgreementRequest request);
    }
}
