using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAan.Application.Onboarding.NotificationsLocations;

public class GetNotificationsLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient) : IRequestHandler<GetNotificationsLocationsQuery,
    GetNotificationsLocationsQueryResult>
{
    public const int MaxResults = 10;

    public async Task<GetNotificationsLocationsQueryResult> Handle(GetNotificationsLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

        return new GetNotificationsLocationsQueryResult
        {
            Locations = result.Locations.Select(x => new GetNotificationsLocationsQueryResult.Location
            {
                Name = x.DisplayName,
                GeoPoint = x.Location.GeoPoint
            }).Take(MaxResults).ToList()
        };
    }
}