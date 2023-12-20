using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionHistory
{
    public class GetEnglishFractionHistoryQueryHandler : IRequestHandler<GetEnglishFractionHistoryQuery, GetEnglishFractionHistoryQueryResult>
    {
        private readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;

        public GetEnglishFractionHistoryQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _financeApiClient = financeApiClient;
        }

        public async Task<GetEnglishFractionHistoryQueryResult> Handle(GetEnglishFractionHistoryQuery request, CancellationToken cancellationToken)
        {
            var response = await _financeApiClient.Get<GetEnglishFractionHistoryResponse>(
                new GetEnglishFractionHistoryRequest(request.HashedAccountId, request.EmpRef));

            return new GetEnglishFractionHistoryQueryResult()
            {
                Fractions = response
            };
        }
    }
}