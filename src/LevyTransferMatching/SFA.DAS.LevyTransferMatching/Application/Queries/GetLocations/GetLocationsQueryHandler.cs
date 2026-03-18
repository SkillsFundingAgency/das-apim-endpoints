using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.LevyTransferMatching.Application.Queries.GetLocations
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, GetLocationsResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetLocationsResult> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

            return new GetLocationsResult
            {
                Locations = result.Locations
            };
        }
    }
}
