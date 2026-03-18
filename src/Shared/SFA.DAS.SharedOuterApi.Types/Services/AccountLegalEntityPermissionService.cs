using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models.ProviderRelationships;

namespace SFA.DAS.SharedOuterApi.Types.Services
{
    public class AccountLegalEntityPermissionService : IAccountLegalEntityPermissionService
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _providerRelationshipsApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public AccountLegalEntityPermissionService(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
            _accountsApiClient = accountsApiClient;
        }
        public async Task<AccountLegalEntityItem> GetAccountLegalEntity(AccountIdentifier accountIdentifier, string accountLegalEntityPublicHashedId)
        {
            switch (accountIdentifier.AccountType)
            {
                case AccountType.Provider:
                    var providerResponse =
                        await _providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                            new GetProviderAccountLegalEntitiesRequest(accountIdentifier.Ukprn,
                                [Operation.Recruitment, Operation.RecruitmentRequiresReview]));

                    if (providerResponse == null)
                    {
                        return null;
                    }

                    var legalEntityItem = providerResponse.AccountProviderLegalEntities
                        .FirstOrDefault(c => c.AccountLegalEntityPublicHashedId.Equals(
                            accountLegalEntityPublicHashedId, StringComparison.CurrentCultureIgnoreCase));

                    if (legalEntityItem != null)
                    {
                        return new AccountLegalEntityItem
                        {
                            Name = legalEntityItem.AccountLegalEntityName,
                            AccountLegalEntityPublicHashedId = legalEntityItem.AccountLegalEntityPublicHashedId,
                            AccountHashedId = legalEntityItem.AccountHashedId,
                            AccountId = legalEntityItem.AccountId,
                            AccountLegalEntityId = legalEntityItem.AccountLegalEntityId
                        };
                    }
                    else
                    {
                        return null;
                    }
                case AccountType.Employer:
                    var resourceListResponse = await _accountsApiClient.Get<AccountDetail>(
                        new GetAllEmployerAccountLegalEntitiesRequest(accountIdentifier.AccountHashedId));

                    if (resourceListResponse == null)
                    {
                        return null;
                    }

                    foreach (var legalEntity in resourceListResponse.LegalEntities)
                    {
                        var legalEntityResponse =
                            await _accountsApiClient.Get<GetEmployerAccountLegalEntityItem>(
                                new GetEmployerAccountLegalEntityRequest(legalEntity.Href));

                        if (legalEntityResponse.AccountLegalEntityPublicHashedId.Equals(accountLegalEntityPublicHashedId, StringComparison.CurrentCultureIgnoreCase))
                        {
                            return new AccountLegalEntityItem
                            {
                                Name = legalEntityResponse.AccountLegalEntityName,
                                AccountLegalEntityPublicHashedId = legalEntityResponse.AccountLegalEntityPublicHashedId,
                                AccountHashedId = accountIdentifier.AccountHashedId,
                                AccountId = resourceListResponse.AccountId,
                                AccountLegalEntityId = legalEntityResponse.AccountLegalEntityId
                            };
                        }
                    }
                    return null;
                default:
                    return null;
            }
        }
    }
}