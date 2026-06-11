using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.Types.Interfaces;

public interface IAccountLegalEntityPermissionService
{
    Task<AccountLegalEntityItem> GetAccountLegalEntity(AccountIdentifier accountIdentifier, string accountLegalEntityPublicHashedId);
    Task<bool> HasProviderGotEmployersPermissionAsync(long ukprn, long accountId, List<Operation> operationTypes);
    Task<List<AccountLegalEntityItem>> GetProviderPermissionsForEmployer(long ukprn, long accountId, List<Operation> operationTypes);
    Task<List<AccountLegalEntityItem>> GetProviderAccountLegalEntities(long ukprn, List<Operation> operationTypes);
    Task<List<LegalEntityItem>> GetProviderPermissionsAccountLegalEntities(long ukprn, List<Operation> operationTypes);
    Task<List<LegalEntityItem>> GetProviderPermissionsForEmployerAccountLegalEntities(long ukprn, long accountId, List<Operation> operationTypes);
    Task<List<LegalEntityItem>> GetEmployerAccountLegalEntities(string accountHashedId, List<Operation> operationTypes);
}