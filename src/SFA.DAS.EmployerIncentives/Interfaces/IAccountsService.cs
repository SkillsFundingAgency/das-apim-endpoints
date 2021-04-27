using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.SharedOuterApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IAccountsService
    {
        Task<bool> IsHealthy();
        Task<LegalEntity> GetLegalEntity(string accountId, long legalEntityId);
        Task<PagedResponse<AccountLegalEntity>> GetLegalEntitiesByPage(int pageNumber, int pageSize = 1000);
        Task<IEnumerable<UserDetails>> GetAccountUsers(string hashedAccountId);
    }
}