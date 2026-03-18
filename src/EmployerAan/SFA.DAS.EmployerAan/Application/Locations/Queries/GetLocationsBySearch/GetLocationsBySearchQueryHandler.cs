using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Courses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.EmployerAan.Application.Locations.Queries.GetLocationsBySearch;

public class GetLocationsBySearchQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient)
    : IRequestHandler<GetLocationsBySearchQuery, GetLocationsBySearchQueryResult>
{
    public async Task<GetLocationsBySearchQueryResult> Handle(GetLocationsBySearchQuery request,
        CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

        return new GetLocationsBySearchQueryResult
        {
            Locations = result.Locations.Select(x => new GetLocationsBySearchQueryResult.Location
            {
                Name = x.DisplayName
            })
        };
    }
}