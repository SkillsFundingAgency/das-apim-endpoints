using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocations
{
    public class GetLocationsQueryHandler : IRequestHandler<GetLocationsQuery, GetLocationsQueryResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetLocationsQueryResult> Handle(GetLocationsQuery request, CancellationToken cancellationToken)
        {
            var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

            return new GetLocationsQueryResult
            {
                Locations = result.Locations.ToList()
            };
        }
    }
}