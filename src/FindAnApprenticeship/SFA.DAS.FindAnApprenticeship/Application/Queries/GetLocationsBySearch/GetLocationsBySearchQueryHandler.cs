using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch
{
    public class GetLocationsBySearchQueryHandler : IRequestHandler<GetLocationsBySearchQuery, GetLocationsBySearchQueryResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationsBySearchQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient) => _apiClient = apiClient;

        public async Task<GetLocationsBySearchQueryResult> Handle(GetLocationsBySearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

            return new GetLocationsBySearchQueryResult
            {
                Locations = result.Locations
            };
        }
    }
}