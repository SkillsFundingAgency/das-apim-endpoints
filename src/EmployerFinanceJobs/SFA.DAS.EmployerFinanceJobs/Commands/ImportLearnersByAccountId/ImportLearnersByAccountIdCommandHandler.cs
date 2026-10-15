using MediatR;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Requests;
using SFA.DAS.EmployerFinanceJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Collections.Concurrent;
using SFA.DAS.EmployerFinanceJobs.Extensions;

namespace SFA.DAS.EmployerFinanceJobs.Commands.ImportLearnersByAccountId;

public class ImportLearnersByAccountIdCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient)
    : IRequestHandler<ImportLearnersByAccountIdCommand, Unit>
{
    private const int PageItemCount = 100;
    private const int MaxDegreeOfParallelism = 5;

    public async Task<Unit> Handle(ImportLearnersByAccountIdCommand request, CancellationToken cancellationToken)
    {
        var (committedLearners, committedTransferOut) = await (
            GetAllPagedAsync(request.AccountId, transferSenderId: null),
            GetAllPagedAsync(accountId: null, transferSenderId: request.AccountId)
        ).WhenAll();

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = MaxDegreeOfParallelism,
            CancellationToken = cancellationToken
        };

        await Parallel.ForEachAsync(committedLearners, parallelOptions, async (apprenticeship, ct) =>
            await PostApprenticeshipPaymentSummary(request.AccountId, apprenticeship, isTransfer: false));

        await Parallel.ForEachAsync(committedTransferOut, parallelOptions, async (apprenticeship, ct) =>
            await PostApprenticeshipPaymentSummary(request.AccountId, apprenticeship, isTransfer: true));

        return Unit.Value;
    }

    private async Task<IEnumerable<GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse>> GetAllPagedAsync(
        long? accountId, long? transferSenderId)
    {
        var firstPage = await commitmentsV2ApiClient.Get<GetCommittedLearnersCostByAccountIdResponse>(
            new GetCommittedLearnersCostByAccountIdRequest(accountId, PageNumber: 1, PageItemCount, transferSenderId));

        if (firstPage.TotalApprenticeshipsFound <= PageItemCount)
            return firstPage.Apprenticeships;

        var totalPages = (int)Math.Ceiling((double)firstPage.TotalApprenticeshipsFound / PageItemCount);

        var remainingPages = new ConcurrentBag<GetCommittedLearnersCostByAccountIdResponse>();

        await Parallel.ForEachAsync(
            Enumerable.Range(2, totalPages - 1),
            new ParallelOptions { MaxDegreeOfParallelism = MaxDegreeOfParallelism },
            async (page, ct) =>
            {
                var response = await commitmentsV2ApiClient.Get<GetCommittedLearnersCostByAccountIdResponse>(
                    new GetCommittedLearnersCostByAccountIdRequest(accountId, PageNumber: page, PageItemCount, transferSenderId));
                remainingPages.Add(response);
            });

        return firstPage.Apprenticeships.Concat(remainingPages.SelectMany(p => p.Apprenticeships));
    }

    private async Task<ApprenticeshipPaymentSummaryResponse> PostApprenticeshipPaymentSummary(
        long accountId,
        GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse apprenticeDetails,
        bool isTransfer)
    {
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
        });

        var result = await fundingProjectionApiClient.PostWithResponseCode<ApprenticeshipPaymentSummaryResponse>(request);
        return result.Body;
    }
}