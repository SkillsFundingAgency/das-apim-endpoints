#nullable enable
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.EmployerFinance.Models.Projections;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevyProjectionsByAccountId;

public class GetLevyProjectionsByAccountIdQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> projectionApiClient,
    ICacheStorageService cacheStorageService)
    : IRequestHandler<GetLevyProjectionsByAccountIdQuery, GetLevyProjectionsByAccountIdQueryResult>
{
    private const string LevyFundsInCacheKeyPrefix = "GetLevyFundsIn_{0}";
    private const string PaymentFundsOutCacheKeyPrefix = "GetPaymentFundsOut_{0}";
    private const string ExpiredLevyCacheKeyPrefix = "GetExpiredLevy_{0}";
    private const string LevyDeclarationSummaryCacheKeyPrefix = "GetLevyDeclarationSummaryByDate_{0}";

    private static readonly TimeSpan CacheExpiration = TimeSpan.FromDays(7); // Cache expiration set to 7 days for all cached data

    public async Task<GetLevyProjectionsByAccountIdQueryResult> Handle(GetLevyProjectionsByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow.Date;

        var fundsInTask = GetFundsInAsync(request.AccountId);
        var fundsOutTask = GetFundsOutAsync(request.AccountId);
        var expiredLevyTask = GetExpiredLevyAsync(request.AccountId);

        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(
            new GetLevySummaryByAccountIdRequest(request.AccountId));

        var declarationsTask = FetchHistoricTransactionLinesDataAsync(now, request.AccountId);

        await Task.WhenAll(levySummaryTask, declarationsTask, fundsInTask, fundsOutTask, expiredLevyTask);
        
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
                ShortTermExpiredFunds = expiredLevy?.ShortTerm ?? []
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
            .Where(t => t.PayrollDate() != null)
            .GroupBy(t => t.PayrollDate()!.Value)
            .Select(g => new LevyInMonthSummary(
                new DateOnly(g.Key.Year, g.Key.Month, 1),
                g.Sum(t => t.TotalAmount - t.TopUp)))
    ];

    private static DateTime? GetLatestSubmissionDate(List<LevyDeclaration> declarations) =>
        declarations
            .Where(d => d.TotalAmount > 0)
            .Max(d => d.SubmissionDate);

    private async Task<List<LevyDeclaration>?> FetchHistoricTransactionLinesDataAsync(DateTime now, long accountId)
    {
        var startOfMonth = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var startDate = startOfMonth.AddYears(-1);
        var endDate = startOfMonth.AddMonths(1).AddTicks(-1);
        var cacheExpiry = DateTime.UtcNow.Date.AddDays(1) - DateTime.UtcNow; // Cache expiry set to the end of the current day to ensure we get fresh data for the next day

        return await GetHistoricTransactionLinesDataAsync(accountId, startDate, endDate, cacheExpiry);
    }

    private Task<List<CalendarPeriodAmount>?> GetFundsInAsync(long accountId) =>
        GetOrCacheAsync(
            string.Format($"{LevyFundsInCacheKeyPrefix}", accountId),
            async () => await financeApiClient.Get<List<CalendarPeriodAmount>>(new GetLevyFundsInRequest(accountId)));

    private Task<List<CalendarPeriodAmount>?> GetFundsOutAsync(long accountId) =>
        GetOrCacheAsync(
            string.Format($"{PaymentFundsOutCacheKeyPrefix}", accountId),
            async () => await financeApiClient.Get<List<CalendarPeriodAmount>>(new GetPaymentFundsOutRequest(accountId)));

    private async Task<GetExpiredLevyResponse?> GetExpiredLevyAsync(long accountId) =>
        await GetOrCacheAsync(
            string.Format($"{ExpiredLevyCacheKeyPrefix}", accountId),
            async () => await financeApiClient.Get<GetExpiredLevyResponse>(new GetExpiredLevyRequest(accountId)));

    private async Task<List<LevyDeclaration>?> GetHistoricTransactionLinesDataAsync(long accountId, DateTime fromDate, DateTime toDate, TimeSpan? cacheExpiration = null) =>
        await GetOrCacheAsync(
            string.Format($"{LevyDeclarationSummaryCacheKeyPrefix}", accountId),
            async () => await financeApiClient.Get<List<LevyDeclaration>>(
                new GetLevyDeclarationSummaryByDate(accountId, fromDate, toDate)),
            cacheExpiration);

    private async Task<T?> GetOrCacheAsync<T>(
        string cacheKey,
        Func<Task<T?>> fetchAsync,
        TimeSpan? cacheExpiration = null) where T : class
    {
        var cached = await cacheStorageService.RetrieveFromCache<T>(cacheKey);
        if (cached is not null)
            return cached;

        var result = await fetchAsync();

        if (result is not null)
            await cacheStorageService.SaveToCache(cacheKey, result, cacheExpiration ?? CacheExpiration);

        return result;
    }
}