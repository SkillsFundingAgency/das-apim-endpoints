using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Concurrent;
using SFA.DAS.Apim.Shared.Extensions;

namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;

public class ImportLearnersByAccountIdCommandHandler(ILogger<ImportLearnersByAccountIdCommandHandler> logger,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient,
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<ImportLearnersByAccountIdCommand, ImportLearnersByAccountIdResult>
{
    private const int PageItemCount = 100;
    private const int MaxDegreeOfParallelism = 5;

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
            new GetCommittedLearnersCostByAccountIdRequest(accountId, PageNumber: 1, PageItemCount, transferSenderId));

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
                    new GetCommittedLearnersCostByAccountIdRequest(accountId, PageNumber: page, PageItemCount, transferSenderId));
                remainingPages.Add(response);
            });

        return firstPage.Apprenticeships.Concat(remainingPages.SelectMany(p => p.Apprenticeships));
    }

    private async Task PostApprenticeshipPaymentSummary(
        long accountId,
        GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse apprenticeDetails,
        GetApprenticeshipPaymentByAccountIdResponse apprenticeshipPayment,
        bool isTransfer)
    {
        if (apprenticeshipPayment.ApprenticeshipId <= 0) return;

        var request = new PostApprenticeshipPaymentSummaryRequest(accountId, apprenticeDetails.Id, new PostApprenticeshipPaymentSummaryRequestData
        {
            Dob = DateOnly.FromDateTime(apprenticeDetails.DateOfBirth),
            StartDate = DateOnly.FromDateTime(apprenticeDetails.StartDate),
            EndDate = DateOnly.FromDateTime(apprenticeDetails.EndDate),
            IsTransfer = isTransfer,
            Status = apprenticeDetails.ApprenticeshipStatus,
            TotalCost = apprenticeDetails.Cost ?? 0,
            TotalPaid = apprenticeDetails.TotalAgreedPrice ?? 0,
            Uln = long.Parse(apprenticeDetails.Uln),
            LastPaymentAmount = apprenticeshipPayment.Amount,
            LastPaymentDate = new DateOnly(apprenticeshipPayment.CollectionPeriodYear, apprenticeshipPayment.CollectionPeriodMonth, 1),
            LastUpdatedDate = apprenticeshipPayment.LastUpdatedDate
        });

        var result = await fundingProjectionApiClient.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(request);
        
        result.EnsureSuccessStatusCode();
    }

    private async Task<GetApprenticeshipPaymentByAccountIdResponse> GetApprenticeshipPaymentByAccountId(long accountId, long apprenticeshipId)
    {
        var request = new GetApprenticeshipPaymentByAccountIdRequest(accountId, apprenticeshipId);
        var result = await financeApiClient.Get<GetApprenticeshipPaymentByAccountIdResponse>(request);
        return result;
    }
}