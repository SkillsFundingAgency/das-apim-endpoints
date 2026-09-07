using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByAccountId;

public class GetLevySummaryByAccountIdQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<GetLevySummaryByAccountIdQuery, GetLevySummaryByAccountIdQueryResult>
{
    public async Task<GetLevySummaryByAccountIdQueryResult> Handle(GetLevySummaryByAccountIdQuery request, CancellationToken cancellationToken)
    {
        var levySummary = await financeApiClient.Get<GetLevySummaryByAccountIdResponse>(new GetLevySummaryByAccountIdRequest(request.AccountId));
        return new GetLevySummaryByAccountIdQueryResult
        {
            CurrentLevyFunds = levySummary.CurrentLevyFunds,
            TotalLevyDeclaredLast12Months = levySummary.TotalLevyDeclaredLast12Months,
            TotalLevySpentLast12Months = levySummary.TotalLevySpentLast12Months,
        };
    }
}