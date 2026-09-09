using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.EmployerFinance.Application.Queries.Transfers.GetPledges
{
    public class GetPledgesQueryHandler : IRequestHandler<GetPledgesQuery, GetPledgesQueryResult>
    {
        public readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public GetPledgesQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<GetPledgesQueryResult> Handle(GetPledgesQuery request, CancellationToken cancellationToken)
        {
            var response = await _levyTransferMatchingApiClient.Get<GetPledgesResponse>(
                new GetPledgesRequest(accountId: request.AccountId));

            return new GetPledgesQueryResult()
            {
                Pledges = response.Pledges,
                TotalPledges = response.TotalPledges,
            };
        }
    }
}