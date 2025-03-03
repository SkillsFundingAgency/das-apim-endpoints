using MediatR;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.EmployerAan.Application.Settings.NotificationsLocations
{
    public class GetNotificationsLocationsQueryHandler(ILocationApiClient<LocationApiConfiguration> apiClient, ILocationLookupService locationLookupService, IAanHubRestApiClient aanHubApiClient) : IRequestHandler<GetNotificationsLocationsQuery, GetNotificationsLocationsQueryResult>
    {
        public const int MaxResults = 10;

        public async Task<GetNotificationsLocationsQueryResult> Handle(GetNotificationsLocationsQuery request,
            CancellationToken cancellationToken)
        {
            return await GetNotificationsLocationsAsync(request, cancellationToken);
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
