using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation
{
    public class GetLocationQueryHandler : IRequestHandler<GetLocationQuery, GetLocationQueryResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetLocationQueryResult> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrEmpty(request.ExactMatch) && request.ExactMatch.Length >= 3)
            {
                // using the first 3 letters only attempt to make an exact match to a verify a location
                // as the full string will not be returned if it includes the district
                var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.ExactMatch[..3]));

                var matchingLocation = result?.Locations?.FirstOrDefault(p => p.DisplayName == request.ExactMatch);
                if (matchingLocation != null)
                {
                    return new GetLocationQueryResult
                    {
                        Location = matchingLocation
                    };
                }
            }
            
            return new GetLocationQueryResult();
        }
    }
}