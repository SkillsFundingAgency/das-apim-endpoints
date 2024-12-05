using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Services;

namespace SFA.DAS.EmployerAan.Application.Onboarding.NotificationsLocations;

public class GetNotificationsLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient, ILocationLookupService locationLookupService) : IRequestHandler<GetNotificationsLocationsQuery,
    GetNotificationsLocationsQueryResult>
{
    public const int MaxResults = 10;

    public async Task<GetNotificationsLocationsQueryResult> Handle(GetNotificationsLocationsQuery request,
        CancellationToken cancellationToken)
    {
        var result = await apiClient.Get<GetLocationsListResponse>(new GetLocationsQueryRequest(request.SearchTerm));

        var locationData = await locationLookupService.GetLocationInformation(request.SearchTerm, 0, 0, false);

        if (locationData != null && !result.Locations.Any())
        {
            return new GetNotificationsLocationsQueryResult
            {
                Locations = new List<GetNotificationsLocationsQueryResult.Location>
                {
                    new GetNotificationsLocationsQueryResult.Location
                    {
                        Name = locationData.Name,
                        GeoPoint = locationData.GeoPoint,
                        Source = "locationLookupService"
                    }
                }
            };
        }

        return new GetNotificationsLocationsQueryResult
        {
            Locations = result.Locations.Select(x => new GetNotificationsLocationsQueryResult.Location
            {
                Name = x.DisplayName,
                GeoPoint = x.Location.GeoPoint,
                Source = "apiClient"
            }).Take(MaxResults).ToList()
        };
    }
}