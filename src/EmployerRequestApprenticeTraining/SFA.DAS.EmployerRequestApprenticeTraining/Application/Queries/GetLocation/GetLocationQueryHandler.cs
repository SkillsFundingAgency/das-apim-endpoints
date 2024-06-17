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
            if (string.IsNullOrEmpty(request.ExactSearchTerm) || request.ExactSearchTerm.Length < 3)
            {
                return new GetLocationQueryResult();
            }

            for (int i = 3; i <= request.ExactSearchTerm.Length; i++)
            {
                // starting at the minimum number of characters that will match a location string, search for exact matches
                // narrowing down the search by increasing the number of characters until an extact match is found, or there
                // are no more characters in the string or there are no results are found
                var result = await _apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.ExactSearchTerm[..i]));

                if (result?.Locations == null || !result.Locations.Any())
                {
                    break;
                }

                var matchingLocation = result.Locations.FirstOrDefault(p => p.DisplayName == request.ExactSearchTerm);
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