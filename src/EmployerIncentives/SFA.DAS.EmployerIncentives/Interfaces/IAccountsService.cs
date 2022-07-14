using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IAccountsService
    {
        Task<bool> IsHealthy();
        Task<LegalEntity> GetLegalEntity(string accountId, long legalEntityId);
        Task<PagedResponse<AccountLegalEntity>> GetLegalEntitiesByPage(int pageNumber, int pageSize = 1000);
    }
}