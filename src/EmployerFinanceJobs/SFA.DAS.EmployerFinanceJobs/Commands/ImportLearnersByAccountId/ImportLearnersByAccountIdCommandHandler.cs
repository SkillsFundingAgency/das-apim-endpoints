using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.Apim.Shared.Extensions;
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
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<ImportLearnersByAccountIdCommand, ImportLearnersByAccountIdResult>
{
    private const int PageItemCount = 100;
    private const int MaxDegreeOfParallelism = 5;

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
            var committedLearnersTask = GetAllPagedAsync(request.AccountId, transferSenderId: null, cancellationToken);
            var committedTransferOutTask = GetAllPagedAsync(accountId: null, transferSenderId: request.AccountId, cancellationToken);

            await Task.WhenAll(committedLearnersTask, committedTransferOutTask);

            var committedLearners = committedLearnersTask.Result;
            var committedTransferOut = committedTransferOutTask.Result;

            var committedLearnersResponse = committedLearners.ToList();
            var committedTransferOutResponse = committedTransferOut.ToList();

            var allApprenticeships = committedLearnersResponse.Select(a => (a, isTransfer: false))
                .Concat(committedTransferOutResponse.Select(a => (a, isTransfer: true)))
                .ToList();

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
                    var payment = await GetApprenticeshipPaymentByAccountId(request.AccountId, item.a.Id);
                    await PostApprenticeshipPaymentSummary(request.AccountId, item.a, payment, item.isTransfer);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Failed to process apprenticeship {Id}", item.a.Id);
                    Interlocked.Increment(ref failedRecords);
                }
            });

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
        CancellationToken cancellationToken)
    {
        var firstPage = await commitmentsV2ApiClient.Get<GetCommittedLearnersCostByAccountIdResponse>(
            new GetCommittedLearnersCostByAccountIdRequest(accountId, PageNumber: 1, PageItemCount, transferSenderId,
                [.. ActiveStatuses]));

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
                        [.. ActiveStatuses]));
                remainingPages.Add(response);
            });

        return firstPage.Apprenticeships.Concat(remainingPages.SelectMany(p => p.Apprenticeships));
    }

    private async Task PostApprenticeshipPaymentSummary(
        long accountId,
        GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse apprenticeDetails,
        GetApprenticeshipPaymentByAccountIdResponse? apprenticeshipPayment,
        bool isTransfer)
    {
        var payment = apprenticeshipPayment is { ApprenticeshipId: > 0 } ? apprenticeshipPayment : null;

        var request = new PostApprenticeshipPaymentSummaryRequest(accountId, apprenticeDetails.Id, new PostApprenticeshipPaymentSummaryRequestData
        {
            Dob = DateOnly.FromDateTime(apprenticeDetails.DateOfBirth),
            StartDate = DateOnly.FromDateTime(apprenticeDetails.StartDate),
            EndDate = DateOnly.FromDateTime(apprenticeDetails.EndDate),
            IsTransfer = isTransfer,
            Status = apprenticeDetails.ApprenticeshipStatus,
            TotalCost = apprenticeDetails.TotalAgreedPrice ?? 0,
            TotalPaid = CalculateTotalPaid(apprenticeDetails.StartDate, apprenticeDetails.EndDate, apprenticeDetails.TotalAgreedPrice),
            Uln = long.Parse(apprenticeDetails.Uln),
            LastPaymentAmount = payment?.Amount,
            LastPaymentDate = payment is null ? null : GetCollectionPeriodDate(payment),
            LastUpdatedDate = payment?.LastUpdatedDate
        });

        var result = await fundingProjectionApiClient.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(request);
        
        result.EnsureSuccessStatusCode();
    }

    private static decimal CalculateTotalPaid(DateTime startDate, DateTime endDate, decimal? totalAgreedPrice)
    {
        var today = DateTime.Today;
        var totalCost = totalAgreedPrice ?? 0;
        var totalMonths = (endDate.Year - startDate.Year) * 12 + (endDate.Month - startDate.Month);

        if (totalMonths <= 0) return 0;

        var monthlyValue = totalCost / totalMonths;
        var firstPaymentDate = startDate.AddMonths(1); // Assuming the first payment is made one month after the start date

        if (today < firstPaymentDate) return 0;

        var monthsPaid = (today.Year - firstPaymentDate.Year) * 12
                         + (today.Month - firstPaymentDate.Month)
                         + 1;

        return monthlyValue * Math.Min(monthsPaid, totalMonths);
    }

    private static DateOnly? GetCollectionPeriodDate(GetApprenticeshipPaymentByAccountIdResponse apprenticeshipPayment)
    {
        var year = apprenticeshipPayment.CollectionPeriodYear;
        var month = apprenticeshipPayment.CollectionPeriodMonth;

        if (year is < 1 or > 9999 || month is < 1 or > 12)
            return null;

        return new DateOnly(year, month, 1);
    }
    

    //TODO - might need to look at this as if it fails with a different error we will potentially load without the data
    private async Task<GetApprenticeshipPaymentByAccountIdResponse?> GetApprenticeshipPaymentByAccountId(long accountId, long apprenticeshipId)
    {
        var request = new GetApprenticeshipPaymentByAccountIdRequest(accountId, apprenticeshipId);
        var result = await financeApiClient.Get<GetApprenticeshipPaymentByAccountIdResponse>(request);
        return result;
    }
}