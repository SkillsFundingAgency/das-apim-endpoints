using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

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