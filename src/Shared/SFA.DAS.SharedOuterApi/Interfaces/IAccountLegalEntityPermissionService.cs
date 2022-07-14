using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IAccountLegalEntityPermissionService
    {
        Task<AccountLegalEntityItem> GetAccountLegalEntity(AccountIdentifier accountIdentifier, string accountLegalEntityPublicHashedId);
    }
}