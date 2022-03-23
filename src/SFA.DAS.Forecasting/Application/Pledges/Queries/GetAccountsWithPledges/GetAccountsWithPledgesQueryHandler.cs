using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.Forecasting.Application.Pledges.Queries.GetAccountsWithPledges
{
    public class GetAccountsWithPledgesQueryHandler : IRequestHandler<GetAccountsWithPledgesQuery, GetAccountsWithPledgesQueryResult>
    {
        private readonly ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> _levyTransferMatchingApiClient;

        public GetAccountsWithPledgesQueryHandler(ILevyTransferMatchingApiClient<LevyTransferMatchingApiConfiguration> levyTransferMatchingApiClient)
        {
            _levyTransferMatchingApiClient = levyTransferMatchingApiClient;
        }

        public async Task<GetAccountsWithPledgesQueryResult> Handle(GetAccountsWithPledgesQuery request, CancellationToken cancellationToken)
        {
            var response = await _levyTransferMatchingApiClient.Get<GetPledgesResponse>(new GetPledgesRequest());

            return new GetAccountsWithPledgesQueryResult
            {
                AccountIds = response.Pledges
                    .Where(p => p.ApplicationCount > 0)
                    .Select(x => x.AccountId)
                    .Distinct()
                    .ToList()
            };
        }
    }
}