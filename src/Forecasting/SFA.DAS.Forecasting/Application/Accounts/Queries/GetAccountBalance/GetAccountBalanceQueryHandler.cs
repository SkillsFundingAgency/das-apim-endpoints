using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Forecasting.InnerApi.Requests;
using SFA.DAS.Forecasting.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Forecasting.Application.Accounts.Queries.GetAccountBalance
{
    public class GetAccountBalanceQueryHandler : IRequestHandler<GetAccountBalanceQuery, GetAccountBalanceQueryResult>
    {
        private readonly IFinanceApiClient<FinanceApiConfiguration> _apiClient;

        public GetAccountBalanceQueryHandler(IFinanceApiClient<FinanceApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetAccountBalanceQueryResult> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetAccountBalanceResponse>(new GetAccountBalanceRequest(request.AccountId));

            return new GetAccountBalanceQueryResult
            {
                AccountBalance = result.Accounts.FirstOrDefault()
            };
        }
    }
}