using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance;

public class GetAccountBalanceQueryHandler(IFinanceApiClient<FinanceApiConfiguration> apiClient) : IRequestHandler<GetAccountBalanceQuery, GetAccountBalanceQueryResult>
{
    public async Task<GetAccountBalanceQueryResult> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
    {
        var result = await apiClient.PostWithResponseCode<GetAccountBalanceResponse[]>(new PostAccountBalanceRequest(request.AccountId));

        return new GetAccountBalanceQueryResult
        {
            AccountBalance = result.Body.FirstOrDefault()
        };
    }
}