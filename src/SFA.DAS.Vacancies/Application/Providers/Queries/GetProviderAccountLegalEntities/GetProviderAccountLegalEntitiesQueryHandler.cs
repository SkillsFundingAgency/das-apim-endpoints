using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Vacancies.Configuration;
using SFA.DAS.Vacancies.InnerApi.Requests;
using SFA.DAS.Vacancies.Interfaces;

namespace SFA.DAS.Vacancies.Application.Providers.Queries.GetProviderAccountLegalEntities
{
    public class GetProviderAccountLegalEntitiesQueryHandler : IRequestHandler<GetProviderAccountLegalEntitiesQuery, GetProviderAccountLegalEntitiesResponse>
    {
        private readonly IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> _apiClient;

        public GetProviderAccountLegalEntitiesQueryHandler (IProviderRelationshipsApiClient<ProviderRelationshipsApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        public async Task<GetProviderAccountLegalEntitiesResponse> Handle(GetProviderAccountLegalEntitiesQuery request, CancellationToken cancellationToken)
        {
            var response =
                await _apiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                    new GetProviderAccountLegalEntitiesRequest(request.Ukprn));

            return new GetProviderAccountLegalEntitiesResponse
            {
                ProviderAccountLegalEntities = response.ProviderAccountLegalEntities
            };
        }
    }
}