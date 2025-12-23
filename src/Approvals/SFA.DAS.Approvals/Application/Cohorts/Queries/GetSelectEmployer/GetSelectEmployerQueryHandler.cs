using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetSelectEmployer;

public class GetSelectEmployerQueryHandler(
    IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApiClient,
    ILogger<GetSelectEmployerQueryHandler> logger)
    : IRequestHandler<GetSelectEmployerQuery, GetSelectEmployerQueryResult>
{
    public async Task<GetSelectEmployerQueryResult> Handle(GetSelectEmployerQuery request, CancellationToken cancellationToken)
    {
        var providerRelationshipsRequest = new GetProviderAccountLegalEntitiesRequest(request.ProviderId, [Operation.CreateCohort]);
        var providerRelationshipsResponse = await providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(providerRelationshipsRequest);

        if (providerRelationshipsResponse?.AccountProviderLegalEntities == null || !providerRelationshipsResponse.AccountProviderLegalEntities.Any())
        {
            return new GetSelectEmployerQueryResult();
        }

        var uniqueAccountHashedIds = providerRelationshipsResponse.AccountProviderLegalEntities
            .Where(x => !string.IsNullOrWhiteSpace(x.AccountHashedId))
            .Select(x => x.AccountHashedId)
            .Distinct()
            .ToList();

        var accountLevyStatusMap = await GetAccountLevyStatusMap(uniqueAccountHashedIds);

        var accountProviderLegalEntities = providerRelationshipsResponse.AccountProviderLegalEntities
            .Select(x => new AccountProviderLegalEntityItem
            {
                AccountId = x.AccountId,
                AccountPublicHashedId = x.AccountPublicHashedId,
                AccountHashedId = x.AccountHashedId,
                AccountName = x.AccountName,
                AccountLegalEntityId = x.AccountLegalEntityId,
                AccountLegalEntityPublicHashedId = x.AccountLegalEntityPublicHashedId,
                AccountLegalEntityName = x.AccountLegalEntityName,
                AccountProviderId = x.AccountProviderId,
                ApprenticeshipEmployerType = accountLevyStatusMap.GetValueOrDefault(x.AccountHashedId, "NonLevy")
            })
            .ToList();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            accountProviderLegalEntities = ApplySearch(accountProviderLegalEntities, request.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(request.SortField))
        {
            accountProviderLegalEntities = ApplySort(accountProviderLegalEntities, request.SortField, request.ReverseSort);
        }

        var employers = accountProviderLegalEntities
            .SelectMany(x => new[] { x.AccountLegalEntityName, x.AccountName })
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        return new GetSelectEmployerQueryResult
        {
            AccountProviderLegalEntities = accountProviderLegalEntities,
            Employers = employers
        };
    }

    private async Task<Dictionary<string, string>> GetAccountLevyStatusMap(List<string> accountHashedIds)
    {
        var levyStatusMap = new ConcurrentDictionary<string, string>();

        if (!accountHashedIds.Any())
        {
            return new Dictionary<string, string>();
        }

        await Parallel.ForEachAsync(accountHashedIds, new ParallelOptions { MaxDegreeOfParallelism = 10 }, async (accountHashedId, _) =>
        {
            try
            {
                var accountResponse = await accountsApiClient.Get<GetAccountResponse>(
                    new GetAccountRequest(accountHashedId));

                levyStatusMap[accountHashedId] = accountResponse?.ApprenticeshipEmployerType ?? "NonLevy";
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to get account levy status for account {AccountHashedId}, defaulting to NonLevy", accountHashedId);
                levyStatusMap[accountHashedId] = "NonLevy";
            }
        });

        return new Dictionary<string, string>(levyStatusMap);
    }

    private static List<AccountProviderLegalEntityItem> ApplySearch(
        List<AccountProviderLegalEntityItem> accountProviderLegalEntities,
        string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return accountProviderLegalEntities;
        }

        var searchParts = searchTerm.Split([" - "], StringSplitOptions.None);
        
        if (searchParts.Length == 2 && !string.IsNullOrWhiteSpace(searchParts[0]) && !string.IsNullOrWhiteSpace(searchParts[1]))
        {
            var employerName = searchParts[0].Trim();
            var accountName = searchParts[1].Trim();
            
            return accountProviderLegalEntities
                .Where(x => 
                    x.AccountLegalEntityName.Contains(employerName, StringComparison.CurrentCultureIgnoreCase) &&
                    x.AccountName.Contains(accountName, StringComparison.CurrentCultureIgnoreCase))
                .ToList();
        }
        
        return accountProviderLegalEntities
            .Where(x => 
                x.AccountName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                x.AccountLegalEntityName.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase) ||
                x.AccountLegalEntityPublicHashedId.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase))
            .ToList();
    }

    private static List<AccountProviderLegalEntityItem> ApplySort(
        List<AccountProviderLegalEntityItem> accountProviderLegalEntities,
        string sortField,
        bool reverseSort)
    {
        if (string.IsNullOrWhiteSpace(sortField))
        {
            return accountProviderLegalEntities;
        }

        const string EmployerAccountLegalEntityNameConst = "EmployerAccountLegalEntityName";
        const string EmployerAccountNameConst = "EmployerAccountName";

        if (sortField.Equals(EmployerAccountLegalEntityNameConst, StringComparison.OrdinalIgnoreCase))
        {
            return reverseSort
                ? accountProviderLegalEntities
                    .OrderByDescending(x => x.AccountLegalEntityName)
                    .ThenBy(x => x.AccountName)
                    .ThenBy(x => x.AccountLegalEntityPublicHashedId)
                    .ToList()
                : accountProviderLegalEntities
                    .OrderBy(x => x.AccountLegalEntityName)
                    .ThenBy(x => x.AccountName)
                    .ThenBy(x => x.AccountLegalEntityPublicHashedId)
                    .ToList();
        }
        else if (sortField.Equals(EmployerAccountNameConst, StringComparison.OrdinalIgnoreCase))
        {
            return reverseSort
                ? accountProviderLegalEntities
                    .OrderByDescending(x => x.AccountName)
                    .ThenBy(x => x.AccountLegalEntityName)
                    .ThenBy(x => x.AccountLegalEntityPublicHashedId)
                    .ToList()
                : accountProviderLegalEntities
                    .OrderBy(x => x.AccountName)
                    .ThenBy(x => x.AccountLegalEntityName)
                    .ThenBy(x => x.AccountLegalEntityPublicHashedId)
                    .ToList();
        }

        return accountProviderLegalEntities;
    }
}

