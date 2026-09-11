using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;

public class GetLevySummaryByAccountIdQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient)
    : IRequestHandler<GetLevySummaryByAccountIdQuery, GetLevySummaryByAccountIdQueryResult>
{
    public async Task<GetLevySummaryByAccountIdQueryResult> Handle(GetLevySummaryByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(new GetLevySummaryByAccountIdRequest(request.AccountId));
        var committedCostsTask = fundingProjectionApiClient.Get<GetCommittedCostsByAccountIdResponse>(new GetCommittedCostsByAccountIdRequest(request.AccountId));

        await Task.WhenAll(levySummaryTask, committedCostsTask);

        var levySummary = levySummaryTask.Result;
        var committedCosts = committedCostsTask.Result;

        return new GetLevySummaryByAccountIdQueryResult
        {
            CurrentLevyFunds = levySummary.CurrentLevyFunds,
            TotalLevyDeclaredLast12Months = levySummary.TotalLevyDeclaredLast12Months,
            TotalLevySpentLast12Months = levySummary.TotalLevySpentLast12Months,
            TotalLevyExpiredLast12Months = levySummary.TotalLevyExpiredLast12Months,
            TotalCommittedLearnerCosts = committedCosts.TotalCommittedLearnerCosts,
            TotalCommittedTransfersCosts = committedCosts.TotalCommittedTransfersCosts
        };
    }
}