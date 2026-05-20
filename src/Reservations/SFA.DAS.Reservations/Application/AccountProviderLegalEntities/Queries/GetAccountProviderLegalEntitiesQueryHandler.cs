using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.ProviderRelationships;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.ProviderRelationships;

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
