using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQueryHandler : IRequestHandler<GetProviderAccountLegalEntitiesQuery, GetProviderAccountLegalEntitiesQueryResponse>
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _apiClient;

        public GetProviderAccountLegalEntitiesQueryHandler (IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProviderAccountLegalEntitiesQueryResponse> Handle(GetProviderAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.Get<GetProviderAccountLegalEntitiesQueryResponse>(
                    new GetProviderAccountLegalEntitiesRequest(request.Ukprn));

            return new GetProviderAccountLegalEntitiesQueryResponse
            {
                ProviderAccountLegalEntities = response.ProviderAccountLegalEntities
            };
        }
    }
}