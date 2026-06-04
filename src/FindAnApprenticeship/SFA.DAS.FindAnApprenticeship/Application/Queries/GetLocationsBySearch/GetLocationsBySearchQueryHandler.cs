using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.GetLocationsBySearch
{
    public class GetLocationsBySearchQueryHandler : IRequestHandler<GetLocationsBySearchQuery, GetLocationsBySearchQueryResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationsBySearchQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient) => _apiClient = apiClient;

        public async Task<GetLocationsBySearchQueryResult> Handle(GetLocationsBySearchQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

            if (result == null)
            {
                return new GetLocationsBySearchQueryResult();
            }
            
            return new GetLocationsBySearchQueryResult
            {
                Locations = result.Locations
            };
        }
    }
}