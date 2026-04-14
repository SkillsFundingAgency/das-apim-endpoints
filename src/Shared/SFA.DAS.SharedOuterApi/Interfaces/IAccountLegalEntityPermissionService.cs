using System.Collections.Generic;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.Interfaces
{
    public interface IAccountLegalEntityPermissionService
    {
        Task<AccountLegalEntityItem> GetAccountLegalEntity(AccountIdentifier accountIdentifier, string accountLegalEntityPublicHashedId);

        Task<bool> HasProviderGotEmployersPermissionAsync(long ukprn, long accountHashedId, List<Operation> operationTypes);
    }
}