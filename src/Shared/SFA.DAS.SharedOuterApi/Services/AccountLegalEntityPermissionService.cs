using System;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class AccountLegalEntityPermissionService : IAccountLegalEntityPermissionService
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _providerRelationshipsApiClient;
        private readonly IAccountsApiClient<AccountsConfiguration> _accountsApiClient;

        public AccountLegalEntityPermissionService (IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient, IAccountsApiClient<AccountsConfiguration> accountsApiClient)
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
                            new GetProviderAccountLegalEntitiesRequest(accountIdentifier.Ukprn));
                    
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
                            AccountHashedId = legalEntityItem.AccountHashedId
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
                                AccountHashedId = accountIdentifier.AccountHashedId
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