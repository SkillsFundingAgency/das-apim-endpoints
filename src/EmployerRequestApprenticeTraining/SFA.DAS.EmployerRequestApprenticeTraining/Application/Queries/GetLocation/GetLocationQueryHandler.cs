using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerRequestApprenticeTraining.Application.Queries.GetLocation
{
    public class GetLocationQueryHandler : IRequestHandler<GetLocationQuery, GetLocationResult>
    {
        private readonly ILocationApiClient<LocationApiConfiguration> _apiClient;

        public GetLocationQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<GetLocationResult> Handle(GetLocationQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.ExactSearchTerm) || request.ExactSearchTerm.Length < 3)
            {
                return new GetLocationResult();
            }

            for (int i = 3; i <= request.ExactSearchTerm.Length; i++)
            {
                // starting at the minimum number of characters that will match a location string, search for exact matches
                // narrowing down the search by increasing the number of characters until an extact match is found, or there
                // are no more characters in the string or there are no results are found
                var result = await _apiClient.GetWithResponseCode<GetLocationsListResponse>(new GetLocationsQueryRequest(request.ExactSearchTerm[..i]));
                result.EnsureSuccessStatusCode();

                if (result.Body.Locations == null || !result.Body.Locations.Any())
                {
                    break;
                }

                var matchingLocation = result.Body.Locations.FirstOrDefault(p => p.DisplayName == request.ExactSearchTerm);
                if (matchingLocation != null)
                {
                    return new GetLocationResult
                    {
                        Location = matchingLocation
                    };
                }
            }

            return new GetLocationResult();
        }
    }
}