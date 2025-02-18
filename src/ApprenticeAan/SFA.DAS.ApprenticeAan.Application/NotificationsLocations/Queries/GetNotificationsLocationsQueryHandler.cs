using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.NotificationsLocations.Queries;

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