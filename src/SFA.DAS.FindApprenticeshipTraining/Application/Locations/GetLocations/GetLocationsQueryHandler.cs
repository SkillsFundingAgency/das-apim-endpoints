using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Locations.GetLocations
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, GetLocationsResponse>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationsQueryHandler (ILocationApiClient<LocationApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }
        
        public async Task<GetLocationsResponse> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));
            
            return new GetLocationsResponse
            {
                Locations = result.Locations
            };
        }
    }
}