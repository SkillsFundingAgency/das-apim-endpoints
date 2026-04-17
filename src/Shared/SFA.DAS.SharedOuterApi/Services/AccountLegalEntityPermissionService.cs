using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.SharedOuterApi.Services
{
    public class AccountLegalEntityPermissionService(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient,
        IAccountsApiClient<AccountsConfiguration> accountsApiClient) : IAccountLegalEntityPermissionService
    {
        public async Task<AccountLegalEntityItem> GetAccountLegalEntity(
            AccountIdentifier accountIdentifier,
            string accountLegalEntityPublicHashedId)
        {
            return accountIdentifier.AccountType switch
            {
                AccountType.Provider => await GetProviderAccountLegalEntity(Convert.ToInt32(accountIdentifier.Ukprn), accountLegalEntityPublicHashedId, [Operation.Recruitment, Operation.RecruitmentRequiresReview]),
                AccountType.Employer => await GetEmployerAccountLegalEntity(accountIdentifier.AccountHashedId, accountLegalEntityPublicHashedId),
                _ => null
            };
        }

        public async Task<bool> HasProviderGotEmployersPermissionAsync(long ukprn,
            long accountHashedId,
            List<Operation> operationTypes)
        {
            var permittedLegalEntities = await GetProviderPermissionsForEmployer(
                ukprn, accountHashedId, operationTypes);

            return permittedLegalEntities is {Count: > 0};
        }

        private async Task<AccountLegalEntityItem> GetProviderAccountLegalEntity(int ukprn,
            string accountLegalEntityPublicHashedId,
            List<Operation> operationTypes)
        {
            var response = await GetProviderAccountLegalEntities(ukprn, operationTypes);

            var match = response.FirstOrDefault(c => c.AccountLegalEntityPublicHashedId.Equals(
                accountLegalEntityPublicHashedId, StringComparison.OrdinalIgnoreCase));

            if (match is null) return null;

            return new AccountLegalEntityItem
            {
                Name = match.Name,
                AccountLegalEntityPublicHashedId = match.AccountLegalEntityPublicHashedId,
                AccountHashedId = match.AccountHashedId,
                AccountId = match.AccountId,
                AccountLegalEntityId = match.AccountLegalEntityId
            };
        }

        private async Task<AccountLegalEntityItem> GetEmployerAccountLegalEntity(string accountHashedId,
            string accountLegalEntityPublicHashedId)
        {
            var accountDetail = await accountsApiClient.Get<AccountDetail>(
                new GetAllEmployerAccountLegalEntitiesRequest(accountHashedId));

            if (accountDetail?.LegalEntities is null or []) return null;

            var legalEntities = await Task.WhenAll(
                accountDetail.LegalEntities.Select(r =>
                    accountsApiClient.Get<GetEmployerAccountLegalEntityItem>(
                        new GetEmployerAccountLegalEntityRequest(r.Href))));

            var match = legalEntities.FirstOrDefault(l => l.AccountLegalEntityPublicHashedId.Equals(
                accountLegalEntityPublicHashedId, StringComparison.OrdinalIgnoreCase));

            if (match is null) return null;

            return new AccountLegalEntityItem
            {
                Name = match.AccountLegalEntityName,
                AccountLegalEntityPublicHashedId = match.AccountLegalEntityPublicHashedId,
                AccountHashedId = accountHashedId,
                AccountId = accountDetail.AccountId,
                AccountLegalEntityId = match.AccountLegalEntityId
            };
        }

        private async Task<List<AccountLegalEntityItem>> GetProviderPermissionsForEmployer(long ukprn,
            long accountHashedId,
            List<Operation> operationTypes)
        {
            var providerPermissions = await GetProviderAccountLegalEntities(ukprn, operationTypes);

            return providerPermissions
                .Where(p => p.AccountId == accountHashedId)
                .ToList();
        }

        private async Task<List<AccountLegalEntityItem>> GetProviderAccountLegalEntities(long ukprn, List<Operation> operationTypes)
        {
            var response = await providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                new GetProviderAccountLegalEntitiesRequest(Convert.ToInt32(ukprn),
                    operationTypes));
            return response?.AccountProviderLegalEntities?.Select(e => new AccountLegalEntityItem
            {
                Name = e.AccountLegalEntityName,
                AccountLegalEntityPublicHashedId = e.AccountLegalEntityPublicHashedId,
                AccountHashedId = e.AccountHashedId,
                AccountId = e.AccountId,
                AccountLegalEntityId = e.AccountLegalEntityId
            }).ToList() ?? [];
        }
    }
}