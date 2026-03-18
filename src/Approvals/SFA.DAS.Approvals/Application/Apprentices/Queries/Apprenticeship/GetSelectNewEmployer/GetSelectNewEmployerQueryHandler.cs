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
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Commitments;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Commitments;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.ProviderRelationships;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetSelectNewEmployer;

public class GetSelectNewEmployerQueryHandler(
    IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApiClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    ILogger<GetSelectNewEmployerQueryHandler> logger)
    : IRequestHandler<GetSelectNewEmployerQuery, GetSelectNewEmployerQueryResult>
{
    public async Task<GetSelectNewEmployerQueryResult> Handle(GetSelectNewEmployerQuery selectEmployerRequest, CancellationToken cancellationToken)
    {
        var providerRelationshipsRequest = new GetProviderAccountLegalEntitiesRequest(selectEmployerRequest.ProviderId, [Operation.Recruitment]);

        var providerRelationshipsResponse = await providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(providerRelationshipsRequest);

        var apprenticeshipRequest = new GetApprenticeshipRequest(selectEmployerRequest.ApprenticeshipId);
        var apprenticeship = await commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(apprenticeshipRequest);

        if (providerRelationshipsResponse?.AccountProviderLegalEntities == null || !providerRelationshipsResponse.AccountProviderLegalEntities.Any())
        {
            return new GetSelectNewEmployerQueryResult();
        }

        var accountProviderLegalEntities = providerRelationshipsResponse.AccountProviderLegalEntities
               .Where(x => x.AccountLegalEntityId != apprenticeship.AccountLegalEntityId)
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
                   ApprenticeshipEmployerType = "NonLevy"
               })
               .ToList();

        if (!string.IsNullOrWhiteSpace(selectEmployerRequest.SearchTerm))
        {
            accountProviderLegalEntities = ApplySearch(accountProviderLegalEntities, selectEmployerRequest.SearchTerm);
        }

        if (!string.IsNullOrWhiteSpace(selectEmployerRequest.SortField))
        {
            accountProviderLegalEntities = ApplySort(accountProviderLegalEntities, selectEmployerRequest.SortField, selectEmployerRequest.ReverseSort);
        }

        var totalCount = accountProviderLegalEntities.Count;
        var pageSize = selectEmployerRequest.PageSize;
        var pageNumber = selectEmployerRequest.PageNumber;
        var pagedAccountLegalEntities = accountProviderLegalEntities
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var uniqueAccountHashedIdsForPage = pagedAccountLegalEntities
            .Where(x => !string.IsNullOrWhiteSpace(x.AccountHashedId))
            .Select(x => x.AccountHashedId)
            .Distinct()
            .ToList();

        var accountLevyStatusMap = await GetAccountLevyStatus(uniqueAccountHashedIdsForPage);

        foreach (var item in pagedAccountLegalEntities.Where(item => !string.IsNullOrWhiteSpace(item.AccountHashedId)))
        {
            item.ApprenticeshipEmployerType = accountLevyStatusMap.GetValueOrDefault(item.AccountHashedId, "NonLevy");
        }

        var employers = accountProviderLegalEntities
            .SelectMany(x => new[] { x.AccountLegalEntityName, x.AccountName })
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct()
            .ToList();

        return new GetSelectNewEmployerQueryResult
        {
            AccountProviderLegalEntities = pagedAccountLegalEntities,
            Employers = employers,
            TotalCount = totalCount,
            EmployerName = apprenticeship.EmployerName
        };
    }

    private async Task<Dictionary<string, string>> GetAccountLevyStatus(List<string> accountHashedIds)
    {
        var accountLevyStatusMap = new ConcurrentDictionary<string, string>();

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

                accountLevyStatusMap[accountHashedId] = accountResponse?.ApprenticeshipEmployerType ?? "NonLevy";
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Failed to get account levy status for account {AccountHashedId}, defaulting to NonLevy", accountHashedId);
                accountLevyStatusMap[accountHashedId] = "NonLevy";
            }
        });

        return new Dictionary<string, string>(accountLevyStatusMap);
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
        List<AccountProviderLegalEntityItem> accountProviderLegalEntitiesItems,
        string sortField,
        bool reverseSort)
    {
        const string EmployerAccountLegalEntityNameConst = "EmployerAccountLegalEntityName";
        const string EmployerAccountNameConst = "EmployerAccountName";
        const string AgreementId = "AgreementId";

        if (string.IsNullOrWhiteSpace(sortField))
        {
            return accountProviderLegalEntitiesItems;
        }

        if (!string.IsNullOrWhiteSpace(sortField))
        {
            switch (sortField)
            {
                case EmployerAccountLegalEntityNameConst:
                    accountProviderLegalEntitiesItems = reverseSort
                        ? accountProviderLegalEntitiesItems.OrderByDescending(x => x.AccountLegalEntityName)
                        .ThenBy(x => x.AccountName)
                        .ThenBy(x => x.AccountLegalEntityPublicHashedId).ToList()
                        : accountProviderLegalEntitiesItems.OrderBy(x => x.AccountLegalEntityName)
                        .ThenBy(x => x.AccountName)
                        .ThenBy(x => x.AccountLegalEntityPublicHashedId).ToList();
                    break;

                case EmployerAccountNameConst:
                    accountProviderLegalEntitiesItems = reverseSort
                     ? accountProviderLegalEntitiesItems.OrderByDescending(x => x.AccountName)
                      .ThenBy(x => x.AccountLegalEntityName)
                      .ThenBy(x => x.AccountLegalEntityPublicHashedId).ToList()
                     : accountProviderLegalEntitiesItems.OrderBy(x => x.AccountName)
                      .ThenBy(x => x.AccountLegalEntityName)
                      .ThenBy(x => x.AccountLegalEntityPublicHashedId).ToList();
                    break;

                case AgreementId:
                    accountProviderLegalEntitiesItems = reverseSort
                     ? accountProviderLegalEntitiesItems.OrderByDescending
                      (x => x.AccountLegalEntityPublicHashedId)
                     .ThenBy(x => x.AccountName)
                      .ThenBy(x => x.AccountLegalEntityName).ToList()
                     : accountProviderLegalEntitiesItems.OrderBy(x => x.AccountLegalEntityPublicHashedId)
                      .ThenBy(x => x.AccountLegalEntityName)
                      .ThenBy(x => x.AccountName).ToList();
                    break;
            }
        }

        return accountProviderLegalEntitiesItems;
    }
}