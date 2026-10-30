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

            var committedLearnersResponse = committedLearners
                .Where(c=>c.TransferSenderId == null)
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
                        providerPaymentsPriority);
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
        List<GetApprenticeshipPaymentByAccountIdResponse>? apprenticeshipPayments,
        bool isTransfer,
        GetProviderPaymentsPriorityResponse? providerPaymentsPriority)
    {
        // No payments yet is a future committed cost, so still post it with the payment fields left null
        var payments = apprenticeshipPayments ?? [];

        var lastPayment = payments
            .OrderByDescending(p => p.CollectionPeriodYear)
            .ThenByDescending(p => p.CollectionPeriodMonth)
            .ThenByDescending(p => p.LastUpdatedDate)
            .FirstOrDefault();

        var request = new PutApprenticeshipPaymentSummaryRequest(accountId, apprenticeDetails.Id, new PutApprenticeshipPaymentSummaryRequestData
        {
            Dob = DateOnly.FromDateTime(apprenticeDetails.DateOfBirth),
            StartDate = DateOnly.FromDateTime(apprenticeDetails.StartDate),
            EndDate = DateOnly.FromDateTime(apprenticeDetails.EndDate),
            IsTransfer = isTransfer,
            Status =  apprenticeDetails.FreezedDate == null ? apprenticeDetails.ApprenticeshipStatus : ApprenticeshipStatus.Paused,
            TotalCost = apprenticeDetails.TotalAgreedPrice ?? 0,
            TotalPaid = payments.Sum(p => p.Amount),
            Uln = long.Parse(apprenticeDetails.Uln),
            LastPaymentAmount = lastPayment?.Amount,
            LastPaymentDate = lastPayment is null ? null : GetCollectionPeriodDate(lastPayment),
            LastUpdatedDate = DateTime.UtcNow,
            ProviderId = apprenticeDetails.ProviderId,
            ProviderOrder = providerPaymentsPriority?.ProviderPaymentPriorities
                .FirstOrDefault(p => p.ProviderId == apprenticeDetails.ProviderId)?
                .PriorityOrder ?? 0
        });

        var result = await fundingProjectionApiClient.PutWithResponseCode<ApprenticeshipPaymentSummaryResponse>(request);

        result.EnsureSuccessStatusCode();
    }

    private static DateOnly? GetCollectionPeriodDate(GetApprenticeshipPaymentByAccountIdResponse apprenticeshipPayment)
    {
        var year = apprenticeshipPayment.CollectionPeriodYear;
        var month = apprenticeshipPayment.CollectionPeriodMonth;

        if (year is < 1 or > 9999 || month is < 1 or > 12)
            return null;

        return new DateOnly(year, month, 1);
    }

    private async Task<GetProviderPaymentsPriorityResponse?> GetProviderPaymentsPriority(long accountId)
    {
        var request = new GetProviderPaymentsPriorityRequest(accountId);
        var result = await commitmentsV2ApiClient.Get<GetProviderPaymentsPriorityResponse>(request);
        return result;
    }

    //TODO - might need to look at this as if it fails with a different error we will potentially load without the data
    private async Task<List<GetApprenticeshipPaymentByAccountIdResponse>?> GetApprenticeshipPaymentByAccountId(long accountId, long apprenticeshipId)
    {
        var request = new GetApprenticeshipPaymentByAccountIdRequest(accountId, apprenticeshipId);
        var result = await financeApiClient.Get<List<GetApprenticeshipPaymentByAccountIdResponse>>(request);
        return result;
    }
}
