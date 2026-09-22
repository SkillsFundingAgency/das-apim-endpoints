using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;

public class GetLevySummaryByAccountIdQueryHandler(
    IFinanceApiClient<FinanceApiConfiguration> financeApiClient,
    IFundingProjectionApiClient<FundingProjectionApiConfiguration> fundingProjectionApiClient)
    : IRequestHandler<GetLevySummaryByAccountIdQuery, GetLevySummaryByAccountIdQueryResult>
{
    private const int MonthsToRetrieveFundingProjection = 12;

    public async Task<GetLevySummaryByAccountIdQueryResult> Handle(GetLevySummaryByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var levySummaryTask = financeApiClient.Get<GetLevySummaryByAccountIdResponse>(new GetLevySummaryByAccountIdRequest(request.AccountId));
        var fundingProjectionTask = fundingProjectionApiClient.Get<GetEmployerFundingProjectionByAccountIdResponse>(new GetEmployerFundingProjectionByAccountIdRequest(request.AccountId, MonthsToRetrieveFundingProjection));

        await Task.WhenAll(levySummaryTask, fundingProjectionTask);

        var levySummary = levySummaryTask.Result;
        var fundingProjection = fundingProjectionTask.Result;

        return new GetLevySummaryByAccountIdQueryResult
        {
            CurrentLevyFunds = levySummary.CurrentLevyFunds,
            TotalLevyDeclaredLast12Months = levySummary.TotalLevyDeclaredLast12Months,
            TotalLevySpentLast12Months = levySummary.TotalLevySpentLast12Months,
            TotalLevyExpiredLast12Months = levySummary.TotalLevyExpiredLast12Months,
            TotalCommittedLearnerCosts = fundingProjection.FundingBreakdowns.Sum(x => x.CommittedLearnerCost),
            TotalCommittedTransfersCosts = fundingProjection.FundingBreakdowns.Sum(x => x.CommittedTransferOut)
        };
    }
}