using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.AccountProviderLegalEntities.Queries
{
    public class GetAccountProviderLegalEntitiesQueryHandler : IRequestHandler<GetAccountProviderLegalEntitiesQuery, GetAccountProviderLegalEntitiesResult>
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _providerRelationshipsApiClient;

        public GetAccountProviderLegalEntitiesQueryHandler(IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient)
        {
            _providerRelationshipsApiClient = providerRelationshipsApiClient;
        }

        public async Task<GetAccountProviderLegalEntitiesResult> Handle(GetAccountProviderLegalEntitiesQuery request,
            CancellationToken cancellationToken)
        {
            var accountProviderLegalEntities =
                await _providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                    new GetProviderAccountLegalEntitiesRequest(request.Ukprn, request.Operations));

            return new GetAccountProviderLegalEntitiesResult
            {
                AccountProviderLegalEntities = accountProviderLegalEntities
            };
        }
    }
}
