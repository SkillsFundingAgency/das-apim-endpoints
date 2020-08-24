using System.Threading.Tasks;
using SFA.DAS.EmployerIncentives.InnerApi.Responses.Accounts;

namespace SFA.DAS.EmployerIncentives.Interfaces
{
    public interface IAccountsService
    {
        Task<bool> IsHealthy();
        Task<LegalEntity> GetLegalEntity(string accountId, long legalEntityId);
    }
}