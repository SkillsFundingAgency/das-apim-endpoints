using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerFinance.InnerApi.Requests;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetLevySummaryByHashedAccountId;

public class GetLevySummaryByHashedAccountIdQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
    : IRequestHandler<GetLevySummaryByHashedAccountIdQuery, GetLevySummaryByHashedAccountIdQueryResult>
{
    public async Task<GetLevySummaryByHashedAccountIdQueryResult> Handle(GetLevySummaryByHashedAccountIdQuery request, CancellationToken cancellationToken)
    {
        var levySummary = await financeApiClient.Get<GetLevySummaryByHashedAccountIdResponse>(new GetLevySummaryByHashedAccountIdRequest(request.HashedAccountId));
        return new GetLevySummaryByHashedAccountIdQueryResult
        {
            CurrentLevyFunds = levySummary.CurrentLevyFunds,
            TotalLevyDeclaredLast12Months = levySummary.TotalLevyDeclaredLast12Months
        };
    }
}