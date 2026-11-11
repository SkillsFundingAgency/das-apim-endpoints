using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Apim.Shared.Interfaces;
using SFA.DAS.EmployerFinanceJobs.Domain.Enums;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Concurrent;

namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;

public class ImportLearnersByAccountIdCommandHandler(ILogger<ImportLearnersByAccountIdCommandHandler> logger,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient,
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    ICacheStorageService cacheStorageService)
    : IRequestHandler<ImportLearnersByAccountIdCommand, ImportLearnersByAccountIdResult>
{
    private const int PageItemCount = 100;
    private const int MaxDegreeOfParallelism = 5;
    private const string FirstApprovedDateCacheKeyPrefix = "GetFirstApprovedDate_{0}_{1}";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromDays(365); // Cache expiration set to 365 days for all cached data

    private static readonly ApprenticeshipStatus[] ActiveStatuses =
    [
        ApprenticeshipStatus.Live,
        ApprenticeshipStatus.Completed,
        ApprenticeshipStatus.Paused,
        ApprenticeshipStatus.Stopped,
        ApprenticeshipStatus.WaitingToStart,
        // ApprenticeshipStatus.Unknown --Ignored as this is not a valid status for an apprenticeship and will be filtered out by the API
    ];

    public async Task<ImportLearnersByAccountIdResult> Handle(ImportLearnersByAccountIdCommand request, CancellationToken cancellationToken)
    {
        var attemptedDate = DateTime.UtcNow;
        int totalRecords = 0;
        int failedRecords = 0;

        try
        {
            var lastUpdated = GetLastUpdatedFilter(request.LastSuccessfulImportDate);
            var committedLearnersTask = GetAllPagedAsync(request.AccountId, transferSenderId: null, lastUpdated, cancellationToken);
            var committedTransferOutTask = GetAllPagedAsync(accountId: null, transferSenderId: request.AccountId, lastUpdated, cancellationToken);
            var periodEndsTask = financeApiClient.Get<List<GetPeriodEndResponse>>(new GetPeriodEndsRequest());

            await Task.WhenAll(committedLearnersTask, committedTransferOutTask, periodEndsTask);

            var periodEndPaymentDate = GetLatestPeriodEndPaymentDate(periodEndsTask.Result);

            var committedLearners = committedLearnersTask.Result;
            var committedTransferOut = committedTransferOutTask.Result;

            var committedLearnersResponse = committedLearners
                .Where(c => c.TransferSenderId == null)
                .ToList();
            var committedTransferOutResponse = committedTransferOut.ToList();

            var allApprenticeships = committedLearnersResponse.Select(a => (a, isTransfer: false))
                .Concat(committedTransferOutResponse.Select(a => (a, isTransfer: true)))
                .ToList();

            var providerPaymentsPriority = await GetProviderPaymentsPriority(request.AccountId);

            totalRecords = allApprenticeships.Count;

            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = MaxDegreeOfParallelism,
                CancellationToken = cancellationToken
            };

            await Parallel.ForEachAsync(allApprenticeships, parallelOptions, async (item, ct) =>
            {
                try
                {
                    var payments = await GetApprenticeshipPaymentByAccountId(request.AccountId, item.a.Id);
                    await PostApprenticeshipPaymentSummary(request.AccountId, item.a, payments, item.isTransfer,
                        providerPaymentsPriority, periodEndPaymentDate);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process apprenticeship {Id}", item.a.Id);
                    Interlocked.Increment(ref failedRecords);
                }
            });

            if (periodEndPaymentDate is { } paymentDate)
            {
                var importedApprenticeshipIds = allApprenticeships.Select(item => item.a.Id).ToHashSet();
                var summaries = await GetApprenticeshipPaymentSummaries(request.AccountId);
                var summariesAwaitingPayment = summaries
                    .Where(s => !importedApprenticeshipIds.Contains(s.ApprenticeshipId) && IsAwaitingPeriodEndPayment(s, paymentDate))
                    .ToList();

                totalRecords += summariesAwaitingPayment.Count;

                await Parallel.ForEachAsync(summariesAwaitingPayment, parallelOptions, async (summary, ct) =>
                {
                    try
                    {
                        var payments = await GetApprenticeshipPaymentByAccountId(request.AccountId, summary.ApprenticeshipId);
                        await PutPeriodEndPaymentSummary(request.AccountId, summary, payments, paymentDate);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex, "Failed to update period end payment for apprenticeship {Id}", summary.ApprenticeshipId);
                        Interlocked.Increment(ref failedRecords);
                    }
                });
            }

            return new ImportLearnersByAccountIdResult
            {
                IsSuccessful = failedRecords == 0,
                TotalRecordsProcessed = totalRecords,
                FailedRecordsCount = failedRecords,
                AttemptedDate = attemptedDate,
                LastSuccessfulImportDate = failedRecords == 0 ? DateTime.UtcNow : null
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Import failed for account {AccountId}", request.AccountId);
            return new ImportLearnersByAccountIdResult
            {
                IsSuccessful = false,
                TotalRecordsProcessed = totalRecords,
                FailedRecordsCount = totalRecords,
                AttemptedDate = attemptedDate,
                FailureReason = ex.Message
            };
        }
    }

    private async Task<IEnumerable<GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse>> GetAllPagedAsync(
        long? accountId,
        long? transferSenderId,
        DateTime? lastUpdated,
        CancellationToken cancellationToken)
    {
        var firstPage = await commitmentsV2ApiClient.Get<GetCommittedLearnersCostByAccountIdResponse>(
            new GetCommittedLearnersCostByAccountIdRequest(accountId, 
                PageNumber: 1, 
                PageItemCount,
                transferSenderId,
                [.. ActiveStatuses],
                LastUpdated: lastUpdated));

        if (firstPage?.Apprenticeships == null || !firstPage.Apprenticeships.Any())
            return [];

        if (firstPage.TotalApprenticeshipsFound <= PageItemCount)
            return firstPage.Apprenticeships;

        var totalPages = (int)Math.Ceiling((double)firstPage.TotalApprenticeshipsFound / PageItemCount);
        var remainingPages = new ConcurrentBag<GetCommittedLearnersCostByAccountIdResponse>();

        await Parallel.ForEachAsync(
            Enumerable.Range(2, totalPages - 1),
            new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism, CancellationToken = cancellationToken },
            async (page, _) =>
            {
                var response = await commitmentsV2ApiClient.Get<GetCommittedLearnersCostByAccountIdResponse>(
                    new GetCommittedLearnersCostByAccountIdRequest(accountId, PageNumber: page, PageItemCount, transferSenderId,
                        [.. ActiveStatuses],
                        LastUpdated: lastUpdated));
                remainingPages.Add(response);
            });

        return firstPage.Apprenticeships.Concat(remainingPages.SelectMany(p => p.Apprenticeships));
    }

    private static DateTime? GetLastUpdatedFilter(DateTime? lastSuccessfulImportDate) =>
        lastSuccessfulImportDate is null || lastSuccessfulImportDate.Value <= DateTime.MinValue.AddDays(1)
            ? null
            : lastSuccessfulImportDate.Value.AddDays(-1);

    private async Task PostApprenticeshipPaymentSummary(
        long accountId,
        GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse apprenticeDetails,
        List<GetApprenticeshipPaymentByAccountIdResponse>? apprenticeshipPayments,
        bool isTransfer,
        GetProviderPaymentsPriorityResponse? providerPaymentsPriority,
        DateOnly? periodEndPaymentDate)
    {
        var paymentDetails = GetPaymentDetails(
            apprenticeshipPayments,
            DateOnly.FromDateTime(apprenticeDetails.StartDate),
            DateOnly.FromDateTime(apprenticeDetails.EndDate),
            apprenticeDetails.TotalAgreedPrice ?? 0,
            periodEndPaymentDate);

        var providerOrder = providerPaymentsPriority?.ProviderPaymentPriorities
            .FirstOrDefault(p => p.ProviderId == apprenticeDetails.ProviderId)?
            .PriorityOrder ?? 0;

        var firstApprovalDate = providerOrder == 0
            ? await GetFirstApprovalDateAsync(accountId, apprenticeDetails.ProviderId)
            : null;

        var request = new PutApprenticeshipPaymentSummaryRequest(accountId, apprenticeDetails.Id, new PutApprenticeshipPaymentSummaryRequestData
        {
            Dob = DateOnly.FromDateTime(apprenticeDetails.DateOfBirth),
            StartDate = DateOnly.FromDateTime(apprenticeDetails.StartDate),
            EndDate = DateOnly.FromDateTime(apprenticeDetails.EndDate),
            IsTransfer = isTransfer,
            Status = apprenticeDetails.FreezedDate == null ? apprenticeDetails.ApprenticeshipStatus : ApprenticeshipStatus.Paused,
            TotalCost = apprenticeDetails.TotalAgreedPrice ?? 0,
            TotalPaid = paymentDetails.TotalPaid,
            Uln = long.Parse(apprenticeDetails.Uln),
            LastPaymentAmount = paymentDetails.LastPaymentAmount,
            LastPaymentDate = paymentDetails.LastPaymentDate,
            LastUpdatedDate = DateTime.UtcNow,
            ProviderId = apprenticeDetails.ProviderId,
            ProviderOrder = providerOrder,
            LearningType = apprenticeDetails.LearningType,
            ApprovalDate = apprenticeDetails.ApprovedOn,
            FirstApprovalDate = firstApprovalDate,
        });

        var result = await fundingProjectionApiClient.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(request);

        result.EnsureSuccessStatusCode();
    }

    private async Task PutPeriodEndPaymentSummary(
        long accountId,
        ApprenticeshipPaymentSummaryResponse summary,
        List<GetApprenticeshipPaymentByAccountIdResponse>? apprenticeshipPayments,
        DateOnly periodEndPaymentDate)
    {
        var paymentDetails = GetPaymentDetails(apprenticeshipPayments, summary.StartDate, summary.EndDate, summary.TotalCost, periodEndPaymentDate);

        // Only the payments have changed, so everything else is put back as it was
        var request = new PutApprenticeshipPaymentSummaryRequest(accountId, summary.ApprenticeshipId, new PutApprenticeshipPaymentSummaryRequestData
        {
            Dob = summary.Dob ?? default,
            StartDate = summary.StartDate,
            EndDate = summary.EndDate,
            IsTransfer = summary.IsTransfer,
            Status = summary.Status,
            TotalCost = summary.TotalCost,
            TotalPaid = paymentDetails.TotalPaid,
            Uln = summary.Uln,
            LastPaymentAmount = paymentDetails.LastPaymentAmount,
            LastPaymentDate = paymentDetails.LastPaymentDate,
            LastUpdatedDate = DateTime.UtcNow,
            ProviderId = summary.ProviderId,
            ProviderOrder = summary.ProviderOrder ?? 0,
            LearningType = summary.LearningType,
            ApprovalDate = summary.ApprovalDate,
            FirstApprovalDate = summary.FirstApprovalDate,
        });

        var result = await fundingProjectionApiClient.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(request);

        result.EnsureSuccessStatusCode();
    }

    private static PaymentDetails GetPaymentDetails(
        List<GetApprenticeshipPaymentByAccountIdResponse>? apprenticeshipPayments,
        DateOnly startDate,
        DateOnly endDate,
        decimal totalCost,
        DateOnly? periodEndPaymentDate)
    {
        // No payments yet is a future committed cost, so the payment fields are left null.
        // Fully funded SFA payments only apply to non-levy accounts, so they never count towards the cost
        var payments = (apprenticeshipPayments ?? [])
            .Where(p => p.FundingSource != FundingSource.FullyFundedSfa)
            .ToList();

        var totalPaid = payments.Sum(p => p.Amount);

        var lastPayment = payments
            .OrderByDescending(p => p.CollectionPeriodYear)
            .ThenByDescending(p => p.CollectionPeriodMonth)
            .ThenByDescending(p => p.LastUpdatedDate)
            .FirstOrDefault();

        var lastPaymentDate = lastPayment is null ? null : GetPaymentDate(lastPayment);

        // A period end that should have paid the apprenticeship but did not is recorded as a zero payment for that month,
        // so the projection moves the missed amount onto the remaining months
        if (periodEndPaymentDate is { } dueDate
            && totalPaid < totalCost
            && IsInPaymentSchedule(startDate, endDate, dueDate)
            && (lastPayment is null || lastPaymentDate < dueDate))
        {
            return new PaymentDetails(totalPaid, 0, dueDate);
        }

        if (lastPayment is null)
            return new PaymentDetails(totalPaid, null, null);

        // A period can hold more than one payment (e.g. on-programme and completion), so the last payment is the sum of them
        var lastPaymentAmount = payments
            .Where(p => p.CollectionPeriodYear == lastPayment.CollectionPeriodYear
                        && p.CollectionPeriodMonth == lastPayment.CollectionPeriodMonth)
            .Sum(p => p.Amount);

        return new PaymentDetails(totalPaid, lastPaymentAmount, lastPaymentDate);
    }

    // Matches the projection, which schedules payments from the month after the start date to the month after the end date
    private static bool IsInPaymentSchedule(DateOnly startDate, DateOnly endDate, DateOnly paymentDate) =>
        paymentDate >= ToMonth(startDate).AddMonths(1) && paymentDate <= ToMonth(endDate).AddMonths(1);

    private static bool IsAwaitingPeriodEndPayment(ApprenticeshipPaymentSummaryResponse summary, DateOnly periodEndPaymentDate)
    {
        // Already holds a payment from this period end. Zeros are checked again, as the period end can be recorded before its payments are imported
        if (summary.LastPaymentDate >= periodEndPaymentDate && summary.LastPaymentAmount is not null and not 0)
            return false;

        // Not-yet-started apprenticeships are included as early payments can happen, and an extra month past the
        // schedule picks up completion payments that arrive late
        return summary.TotalPaid < summary.TotalCost
               && periodEndPaymentDate <= ToMonth(summary.EndDate).AddMonths(2);
    }

    private static DateOnly? GetLatestPeriodEndPaymentDate(List<GetPeriodEndResponse>? periodEnds)
    {
        var latestPeriodEnd = (periodEnds ?? [])
            .Where(p => p.CalendarPeriodYear is >= 1 and <= 9999 && p.CalendarPeriodMonth is >= 1 and <= 12)
            .OrderByDescending(p => p.CalendarPeriodYear)
            .ThenByDescending(p => p.CalendarPeriodMonth)
            .FirstOrDefault();

        // Payments land in the account the month after their collection period
        return latestPeriodEnd is null
            ? null
            : new DateOnly(latestPeriodEnd.CalendarPeriodYear, latestPeriodEnd.CalendarPeriodMonth, 1).AddMonths(1);
    }

    private static DateOnly ToMonth(DateOnly date) => new(date.Year, date.Month, 1);

    private static DateOnly? GetPaymentDate(GetApprenticeshipPaymentByAccountIdResponse apprenticeshipPayment)
    {
        var year = apprenticeshipPayment.CollectionPeriodYear;
        var month = apprenticeshipPayment.CollectionPeriodMonth;

        if (year is < 1 or > 9999 || month is < 1 or > 12)
            return null;

        // Payments land in the account the month after their collection period
        return new DateOnly(year, month, 1).AddMonths(1);
    }

    private async Task<List<ApprenticeshipPaymentSummaryResponse>> GetApprenticeshipPaymentSummaries(long accountId) =>
        await fundingProjectionApiClient.Get<List<ApprenticeshipPaymentSummaryResponse>>(
            new GetApprenticeshipPaymentSummariesRequest(accountId)) ?? [];

    private async Task<GetProviderPaymentsPriorityResponse?> GetProviderPaymentsPriority(long accountId)
    {
        var request = new GetProviderPaymentsPriorityRequest(accountId);
        var result = await commitmentsV2ApiClient.Get<GetProviderPaymentsPriorityResponse>(request);
        return result;
    }

    private async Task<DateOnly?> GetFirstApprovalDateAsync(long accountId, long providerId)
    {
        var cacheKey = string.Format(FirstApprovedDateCacheKeyPrefix, accountId, providerId);
        var request = new GetCommittedLearnersCostByAccountIdRequest(accountId,
            PageNumber: 1, PageItemCount: 1,
            null,
            null,
            ProviderId: providerId,
            SortField: "AgreedOn",
            ReverseSort: true);

        var result = await GetOrCacheAsync(
            cacheKey,
            async () => await commitmentsV2ApiClient.Get<GetCommittedLearnersCostByAccountIdResponse>(request));
        var approvedOn = result?.Apprenticeships.FirstOrDefault()?.ApprovedOn;
        return approvedOn.HasValue ? DateOnly.FromDateTime(approvedOn.Value) : null;
    }

    //TODO - might need to look at this as if it fails with a different error we will potentially load without the data
    private async Task<List<GetApprenticeshipPaymentByAccountIdResponse>?> GetApprenticeshipPaymentByAccountId(long accountId, long apprenticeshipId)
    {
        var request = new GetApprenticeshipPaymentByAccountIdRequest(accountId, apprenticeshipId);
        var result = await financeApiClient.Get<List<GetApprenticeshipPaymentByAccountIdResponse>>(request);
        return result;
    }

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

    private sealed record PaymentDetails(decimal TotalPaid, decimal? LastPaymentAmount, DateOnly? LastPaymentDate);
}
