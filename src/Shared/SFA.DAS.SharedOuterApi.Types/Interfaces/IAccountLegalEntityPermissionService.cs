using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

public interface IAccountLegalEntityPermissionService
{
    Task<AccountLegalEntityItem> GetAccountLegalEntity(AccountIdentifier accountIdentifier, string accountLegalEntityPublicHashedId);

    Task<bool> HasProviderGotEmployersPermissionAsync(long ukprn, long accountHashedId, List<Operation> operationTypes);
}