#nullable enable
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinance.InnerApi.Requests.EmployerAccounts;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public class GetLevyProjectionsByAccountIdQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    IAccountsApiClient<AccountsConfiguration> accountsApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> projectionApiClient,
    ICacheStorageService cacheStorageService,
    ILogger<GetLevyProjectionsByAccountIdQueryHandler> logger)
    : IRequestHandler<GetLevyProjectionsByAccountIdQuery, GetLevyProjectionsByAccountIdQueryResult>
{
    private const string LevyFundsInCacheKey = "GetLevyFundsIn_{0}_{1}";
    private const string PaymentFundsOutCacheKey = "GetPaymentFundsOut_{0}_{1}";
    private const string ExpiredLevyCacheKey = "GetExpiredLevy_{0}_{1}";
    private const string LevyDeclarationSummaryCacheKey = "GetLevyDeclarationSummaryByDate_{0}_{1}_{2}";
    private const string LatestTransactionTypeTransactionIdsCacheKey = "LatestTransactionTypeTransactionIds_{0}";
    private const string NoTransactionsKeySegment = "none";

    private static readonly TimeSpan CacheExpiration = TimeSpan.FromDays(7);

    public async Task<GetLevyProjectionsByAccountIdQueryResult> Handle(GetLevyProjectionsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow.Date;

        var latestIdsTask = GetLatestTransactionTypeTransactionIdsForAccountAsync(request.AccountId);
        var accountPayeSchemesTask = GetAccountPayeSchemesAsync(request.AccountId);

        // Payments land in the account the month after their collection period, so this month's payments are last month's collection
        var currentCollectionPeriod = now.AddMonths(-1);
        var currentPaymentsTask = financeApiClient.Get<CollectionPeriodPayments>(
            new GetCollectionPeriodPaymentsRequest(request.AccountId, currentCollectionPeriod.Year, currentCollectionPeriod.Month));

        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(
            new GetLevySummaryByAccountIdRequest(request.AccountId));

        await Task.WhenAll(latestIdsTask, accountPayeSchemesTask);
        var latestIds = latestIdsTask.Result;
        var accountPayeSchemes = accountPayeSchemesTask.Result;

        var fundsInTask = GetFundsInAsync(request.AccountId, latestIds?.LatestDeclarationId);
        var fundsOutTask = GetFundsOutAsync(request.AccountId, latestIds?.LatestPaymentId);
        var expiredLevyTask = GetExpiredLevyAsync(request.AccountId, latestIds?.LatestExpiryId);
        var declarationsTask = FetchHistoricTransactionLinesDataAsync(now, request.AccountId, latestIds?.LatestDeclarationId, accountPayeSchemes);

        await Task.WhenAll(levySummaryTask, declarationsTask, fundsInTask, fundsOutTask, expiredLevyTask, currentPaymentsTask);
        var levySummary = levySummaryTask.Result;
        var declarations = declarationsTask.Result;
        var expiredLevy = expiredLevyTask.Result;

        var projectionResponse = await projectionApiClient.PostWithResponseCode<EstimatesTimeline>(
            new PostEmployerFundingProjectionRequest(request.AccountId, new PostEmployerFundingProjectionRequestData
            {
                Months = request.Months,
                CurrentLevyFunds = levySummary.CurrentLevyFunds,
                HistoricLevyIn = BuildLevyInSummary(declarations) ?? [],
                LevyFundsInForExpiry = fundsInTask.Result ?? [],
                PaymentFundsOutForExpiry = fundsOutTask.Result ?? [],
                LongTermExpiredFunds = expiredLevy?.LongTerm ?? [],
                ShortTermExpiredFunds = expiredLevy?.ShortTerm ?? [],
                CurrentPayments = currentPaymentsTask.Result
            }));

        var projection = projectionResponse.Body;

        return new GetLevyProjectionsByAccountIdQueryResult
        {
            AccountId = request.AccountId,
            Summary = new LevyProjectionsSummary
            {
                CurrentLevyFunds = levySummary.CurrentLevyFunds,
                TotalLevyDeclaredLast12Months = levySummary.TotalLevyDeclaredLast12Months,
                TotalLevySpentLast12Months = levySummary.TotalLevySpentLast12Months,
                TotalLevyExpiredLast12Months = levySummary.TotalLevyExpiredLast12Months,
                TotalCommittedLearnerCosts = projection.TotalEstimatedCommitedCost,
                TotalCommittedTransfersCosts = projection.TotalEstimatedCommitedTransferCost
            },
            Projections = projection.Projections,
            LatestLevyDeclarationInDate = GetLatestSubmissionDate(declarations) ?? DateTime.MinValue
        };
    }

    private static List<LevyInMonthSummary> BuildLevyInSummary(List<LevyDeclaration> declarations) =>
    [
        .. declarations
            .Where(t => t.GetLevyCreationDateFromPayrollDate() != null)
            .GroupBy(t => t.GetLevyCreationDateFromPayrollDate()!.Value)
            .Select(g => new LevyInMonthSummary(
                new DateOnly(g.Key.Year, g.Key.Month, 1),
                g.Sum(t => t.TotalAmount - t.TopUp)))
    ];

    private static DateTime? GetLatestSubmissionDate(List<LevyDeclaration> declarations) =>
        declarations
            .Where(d => d.TotalAmount > 0)
            .Max(d => d.SubmissionDate);

    private async Task<GetLatestTransactionTypeTransactionIdsForAccountResponse?> GetLatestTransactionTypeTransactionIdsForAccountAsync(long accountId)
    {
        var cacheKey = string.Format(LatestTransactionTypeTransactionIdsCacheKey, accountId);

        GetLatestTransactionTypeTransactionIdsForAccountResponse? latestIds = null;
        try
        {
            latestIds = await financeApiClient.Get<GetLatestTransactionTypeTransactionIdsForAccountResponse>(new GetLatestTransactionTypeTransactionIdsForAccountRequest(accountId));
        }
        catch (Exception ex)
        {
            logger.LogWarning(ex, "Unable to get latest transaction id for each transaction type for account {AccountId}", accountId);
        }

        if (latestIds is not null)
        {
            await cacheStorageService.SaveToCache(cacheKey, latestIds, CacheExpiration);
            return latestIds;
        }

        logger.LogWarning("Using last known transaction id for each transaction type for account {AccountId}", accountId);
        return await cacheStorageService.RetrieveFromCache<GetLatestTransactionTypeTransactionIdsForAccountResponse>(cacheKey);
    }
    
    private async Task<List<string>> GetAccountPayeSchemesAsync(long accountId)
    {
        var response = await accountsApiClient.GetWithResponseCode<List<Resource>>(new GetAccountPayeSchemesRequest(accountId));

        if (response.StatusCode == HttpStatusCode.NotFound)
            return [];

        response.EnsureSuccessStatusCode();
        return [.. (response.Body ?? []).Select(p => p.Id)];
    }

    private async Task<List<LevyDeclaration>?> FetchHistoricTransactionLinesDataAsync(DateTime now, long accountId, long? latestDeclarationId, List<string> emprefs)
    {
        if (emprefs.Count == 0)
            return [];

        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startDate = startOfMonth.AddYears(-1).AddMonths(-1);
        var endDate = startOfMonth.AddMonths(1).AddTicks(-1);

        return await GetOrCacheAsync(
            string.Format(LevyDeclarationSummaryCacheKey, accountId, ToKeySegment(latestDeclarationId), ToPayeSchemesKeySegment(emprefs)),
            async () => (await financeApiClient.PostWithResponseCode<List<LevyDeclaration>>(
                new PostLevyDeclarationSummaryByDate(accountId, startDate, endDate, emprefs))).Body);
    }

    private Task<List<CalendarPeriodAmount>?> GetFundsInAsync(long accountId, long? latestDeclarationId) =>
        GetOrCacheAsync(
            string.Format(LevyFundsInCacheKey, accountId, ToKeySegment(latestDeclarationId)),
            async () => await financeApiClient.Get<List<CalendarPeriodAmount>>(new GetLevyFundsInRequest(accountId)));

    private Task<List<CalendarPeriodAmount>?> GetFundsOutAsync(long accountId, long? latestPaymentId) =>
        GetOrCacheAsync(
            string.Format(PaymentFundsOutCacheKey, accountId, ToKeySegment(latestPaymentId)),
            async () => await financeApiClient.Get<List<CalendarPeriodAmount>>(new GetPaymentFundsOutRequest(accountId)));

    private Task<GetExpiredLevyResponse?> GetExpiredLevyAsync(long accountId, long? latestExpiryId) =>
        GetOrCacheAsync(
            string.Format(ExpiredLevyCacheKey, accountId, ToKeySegment(latestExpiryId)),
            async () => await financeApiClient.Get<GetExpiredLevyResponse>(new GetExpiredLevyRequest(accountId)));

    private static string ToKeySegment(long? latestTransactionId) =>
        latestTransactionId?.ToString() ?? NoTransactionsKeySegment;

    private static string ToPayeSchemesKeySegment(List<string> emprefs)
    {
        var normalised = string.Join('|', emprefs.Select(e => e.Trim().ToUpperInvariant()).Distinct().Order(StringComparer.Ordinal));
        return Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(normalised)))[..16];
    }

    private async Task<T?> GetOrCacheAsync<T>(
        string cacheKey,
        Func<Task<T?>> fetchAsync) where T : class
    {
        var cached = await cacheStorageService.RetrieveFromCache<T>(cacheKey);
        if (cached is not null)
            return cached;

        var result = await fetchAsync();

        if (result is not null)
            await cacheStorageService.SaveToCache(cacheKey, result, CacheExpiration);

        return result;
    }
}