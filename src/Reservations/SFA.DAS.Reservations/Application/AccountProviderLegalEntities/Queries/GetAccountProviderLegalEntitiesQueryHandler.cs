using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Reservations.Application.AccountProviderLegalEntities.Queries
{
    public class GetAccountProviderLegalEntitiesQueryHandler(
        IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> providerRelationshipsApiClient)
        : IRequestHandler<GetAccountProviderLegalEntitiesQuery, GetAccountProviderLegalEntitiesResult>
    {
        public async Task<GetAccountProviderLegalEntitiesResult> Handle(GetAccountProviderLegalEntitiesQuery request,
            CancellationToken cancellationToken)
        {
            var accountProviderLegalEntities =
                await providerRelationshipsApiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                    new GetProviderAccountLegalEntitiesRequest(request.Ukprn, request.Operations));

            return new GetAccountProviderLegalEntitiesResult
            {
                AccountProviderLegalEntities = accountProviderLegalEntities
            };
        }
    }
}
