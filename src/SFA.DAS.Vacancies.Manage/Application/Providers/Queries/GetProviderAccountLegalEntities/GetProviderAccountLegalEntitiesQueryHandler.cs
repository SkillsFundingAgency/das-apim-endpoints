using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Providers.Queries.GetProviderAccountLegalEntities
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
                await _apiClient.Get<GetProviderAccountLegalEntitiesResponse>(
                    new GetProviderAccountLegalEntitiesRequest(request.Ukprn));

            return new GetProviderAccountLegalEntitiesQueryResponse
            {
                ProviderAccountLegalEntities = response.AccountProviderLegalEntities
            };
        }
    }
}