using MediatR;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.Location;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Location;

namespace SFA.DAS.EmployerAan.Application.Onboarding.NotificationsLocations;

public class GetNotificationsLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient, ILocationLookupService locationLookupService) : IRequestHandler<GetNotificationsLocationsQuery,
    GetNotificationsLocationsQueryResult>
{
    public const int MaxResults = 10;

    public async Task<GetNotificationsLocationsQueryResult> Handle(GetNotificationsLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

        if (!result.Locations.Any())
        {
            var locationData = await locationLookupService.GetLocationInformation(request.SearchTerm, 0, 0, false);

            if (locationData != null)
            {
                return new GetNotificationsLocationsQueryResult
                {
                    Locations = new List<GetNotificationsLocationsQueryResult.Location>
                    {
                        new GetNotificationsLocationsQueryResult.Location
                        {
                            Name = locationData.Name,
                            GeoPoint = locationData.GeoPoint
                        }
                    }
                };
            }
        }

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