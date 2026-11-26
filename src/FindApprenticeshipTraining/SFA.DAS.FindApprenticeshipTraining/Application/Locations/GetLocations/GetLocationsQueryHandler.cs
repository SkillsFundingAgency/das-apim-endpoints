using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, GetLocationsQueryResponse>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationsQueryHandler (ILocationApiClient<LocationApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<GetLocationsQueryResponse> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));
            
            return new GetLocationsQueryResponse
            {
                Locations = result.Locations
            };
        }
    }
}