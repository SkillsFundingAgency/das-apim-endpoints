using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations
{
    public class GetNotificationsLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient, ILocationLookupService locationLookupService) : IRequestHandler<GetNotificationsLocationsQuery, GetNotificationsLocationsQueryResult>
    {
        public const int MaxResults = 10;

        public async Task<GetNotificationsLocationsQueryResult> Handle(GetNotificationsLocationsQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                return await GetCurrentSettings(request.MemberId);
            }

            return await GetNotificationsLocationsAsync(request, cancellationToken);
        }

        private async Task<GetNotificationsLocationsQueryResult> GetCurrentSettings(Guid memberId)
        {
            return new GetNotificationsLocationsQueryResult
            {
                SavedLocations = new List<GetNotificationsLocationsQueryResult.AddedLocation>
                {
                    new GetNotificationsLocationsQueryResult.AddedLocation{ Name = "Bromsgrove, Worcestershire", Coordinates = [51, 23], Radius = 10}
                },
                NotificationEventTypes = new List<GetNotificationsLocationsQueryResult.NotificationEventType>
                {
                    new GetNotificationsLocationsQueryResult.NotificationEventType{ EventFormat = "InPerson", Ordering = 1, ReceiveNotifications = true},
                    new GetNotificationsLocationsQueryResult.NotificationEventType{ EventFormat = "Online", Ordering = 2, ReceiveNotifications = false},
                    new GetNotificationsLocationsQueryResult.NotificationEventType{ EventFormat = "Hybrid", Ordering = 3, ReceiveNotifications = false},
                    new GetNotificationsLocationsQueryResult.NotificationEventType{ EventFormat = "All", Ordering = 4, ReceiveNotifications = false}
                }
            };
        }

        private async Task<GetNotificationsLocationsQueryResult> GetNotificationsLocationsAsync(GetNotificationsLocationsQuery request, CancellationToken cancellationToken)
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
}
