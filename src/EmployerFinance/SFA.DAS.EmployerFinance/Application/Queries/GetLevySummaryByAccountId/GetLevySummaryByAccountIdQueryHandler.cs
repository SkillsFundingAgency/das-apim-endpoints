using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Commitments;
using SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Commitments;
using SFA.DAS.EmployerFinance.InnerApi.Responses.Finance;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;

public class GetLevySummaryByAccountIdQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
    : IRequestHandler<GetLevySummaryByAccountIdQuery, GetLevySummaryByAccountIdQueryResult>
{
    private const int PageItemCount = 100;
    private const int MaxDegreeOfParallelism = 5;

    public async Task<GetLevySummaryByAccountIdQueryResult> Handle(
        GetLevySummaryByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(
            new GetLevySummaryByAccountIdRequest(request.AccountId));

        var committedLearnersTask = GetAllPagedAsync(request.AccountId, transferSenderId: null);
        var committedTransferOutTask = GetAllPagedAsync(request.AccountId, transferSenderId: request.AccountId);

        await Task.WhenAll(levySummaryTask, committedLearnersTask, committedTransferOutTask);

        var committedLearners = committedLearnersTask.Result;

        var apprenticeshipDetailsResponses = committedLearners.ToList();

        var priceEpisodeTasks = apprenticeshipDetailsResponses
            .Where(a => a.HasChangeHistory)
            .Select(async a =>
            {
                var priceEpisode = await commitmentsV2ApiClient.Get<GetPriceEpisodeResponse>(
                    new GetPriceEpisodeByApprenticeshipIdRequest(a.Id));
                a.Cost = priceEpisode.PriceEpisodes.Sum(pe => pe.Cost);
            });

        await Task.WhenAll(priceEpisodeTasks);

        var levySummary = levySummaryTask.Result;
        var committedTransferOut = committedTransferOutTask.Result;

        return new GetLevySummaryByAccountIdQueryResult
        {
            CurrentLevyFunds = levySummary.CurrentLevyFunds,
            TotalLevyDeclaredLast12Months = levySummary.TotalLevyDeclaredLast12Months,
            TotalLevySpentLast12Months = levySummary.TotalLevySpentLast12Months,
            TotalLevyExpiredLast12Months = levySummary.TotalLevyExpiredLast12Months,
            TotalCommittedLearnerCosts = Convert.ToDecimal(apprenticeshipDetailsResponses.Sum(a => a.Cost)),
            TotalCommittedTransfersCosts = Convert.ToDecimal(committedTransferOut.Sum(a => a.Cost))
        };
    }

    private async Task<IEnumerable<GetCommittedLearnersCostByAccountIdResponse.ApprenticeshipDetailsResponse>> GetAllPagedAsync(
        long accountId, long? transferSenderId)
    {
        var firstPage = await commitmentsV2ApiClient.Get<GetCommittedLearnersCostByAccountIdResponse>(
            new GetCommittedLearnersCostByAccountIdRequest(accountId, PageNumber: 1, PageItemCount, transferSenderId));

        if (firstPage.TotalApprenticeships <= PageItemCount)
            return firstPage.Apprenticeships;

        var totalPages = (int)Math.Ceiling((double)firstPage.TotalApprenticeships / PageItemCount);

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
}