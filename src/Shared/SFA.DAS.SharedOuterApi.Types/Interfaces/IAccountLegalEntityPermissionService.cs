using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces
{
    public interface IAccountLegalEntityPermissionService
    {
        Task<AccountLegalEntityItem> GetAccountLegalEntity(AccountIdentifier accountIdentifier, string accountLegalEntityPublicHashedId);
    }
}