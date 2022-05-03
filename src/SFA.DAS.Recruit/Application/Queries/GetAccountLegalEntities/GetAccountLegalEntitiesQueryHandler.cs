using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Recruit.InnerApi.Requests;
using SFA.DAS.Recruit.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.Application.Queries.GetAccountLegalEntities
{
    public class GetAccountLegalEntitiesQueryHandler : IRequestHandler<GetAccountLegalEntitiesQuery, GetAccountLegalEntitiesQueryResult>
    {
        private readonly IAccountsApiClient<AccountsConfiguration> _apiClient;

        public GetAccountLegalEntitiesQueryHandler(IAccountsApiClient<AccountsConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetAccountLegalEntitiesQueryResult> Handle(GetAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.GetAll<GetAccountLegalEntityResponseItem>(
                    new GetAccountLegalEntitiesRequest(request.HashedAccountId));

            if (response == null)
            {
                return new GetAccountLegalEntitiesQueryResult();
            }

            return new GetAccountLegalEntitiesQueryResult
            {
                AccountLegalEntities = response.ToList()
            };
        }
    }
}