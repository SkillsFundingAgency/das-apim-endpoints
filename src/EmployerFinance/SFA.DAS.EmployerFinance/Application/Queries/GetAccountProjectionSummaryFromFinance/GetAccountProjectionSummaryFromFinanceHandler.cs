using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerFinance.InnerApi.Requests;
using SFA.DAS.EmployerFinance.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.GetAccountProjectionSummaryFromFinance
{
    public class GetAccountProjectionSummaryFromFinanceHandler : IRequestHandler<GetAccountProjectionSummaryFromFinanceQuery, GetAccountProjectionSummaryFromFinanceResult>
    {
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;

        public GetAccountProjectionSummaryFromFinanceHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _financeApiClient = financeApiClient;
        }

        public async Task<GetAccountProjectionSummaryFromFinanceResult> Handle(GetAccountProjectionSummaryFromFinanceQuery request, CancellationToken cancellationToken)
        {
            var summaryResponse = await _financeApiClient.Get<GetAccountProjectionSummaryFromFinanceResponse>(new GetAccountProjectionSummaryFromFinanceRequest(request.AccountId));

            return new GetAccountProjectionSummaryFromFinanceResult
            {
                AccountId = request.AccountId,
                FundsIn = summaryResponse.FundsIn
            };
        }
    }
}
