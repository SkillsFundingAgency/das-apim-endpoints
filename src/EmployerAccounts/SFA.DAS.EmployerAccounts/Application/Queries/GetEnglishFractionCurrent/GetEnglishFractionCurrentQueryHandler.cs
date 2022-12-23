using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.EmployerAccounts.InnerApi.Requests;
using SFA.DAS.EmployerAccounts.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent
{
    public class GetEnglishFractionCurrentQueryHandler : IRequestHandler<GetEnglishFractionCurrentQuery, GetEnglishFractionCurrentQueryResult>
    {
        public readonly IFinanceApiClient<FinanceApiConfiguration> _financeApiClient;

        public GetEnglishFractionCurrentQueryHandler(IFinanceApiClient<FinanceApiConfiguration> financeApiClient)
        {
            _financeApiClient = financeApiClient;
        }

        public async Task<GetEnglishFractionCurrentQueryResult> Handle(GetEnglishFractionCurrentQuery request, CancellationToken cancellationToken)
        {
            var response = await _financeApiClient.Get<GetEnglishFractionCurrentResponse>(
                new GetEnglishFractionCurrentRequest(request.HashedAccountId, request.EmpRefs));

            return new GetEnglishFractionCurrentQueryResult()
            {
                Fractions = response
            };
        }
    }
}