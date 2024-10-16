using MediatR;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.AdminAan.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandler(IAanHubRestApiClient apiClient, ILocationLookupService locationLookupService)
    : IRequestHandler<GetCalendarEventsQuery, GetCalendarEventsQueryResult?>
{
    public const int DefaultPageSize = 10;

    public async Task<GetCalendarEventsQueryResult?> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        var regionResponse = await apiClient.GetRegions(cancellationToken);
        var regions = regionResponse.Regions.Select(r => new GetCalendarEventsQueryResult.Region
        {
            Id = r.Id,
            Area = r.Area,
            Ordering = r.Ordering
        }).ToList();

        var calendarsResponse = await apiClient.GetCalendars(cancellationToken);

        double? latitude = null;
        double? longitude = null;
        int? radius = null;
        var orderBy = string.Empty;
        
        if (!string.IsNullOrWhiteSpace(request.Location))
        {
            var locationData = await locationLookupService.GetLocationInformation(request.Location, 0, 0, false);
            if (locationData == null)
            {
                return new GetCalendarEventsQueryResult
                {
                    IsInvalidLocation = true,
                    Regions = regions,
                    Calendars = calendarsResponse
                };
            }

            latitude = locationData.GeoPoint[0];
            longitude = locationData.GeoPoint[1];
            radius = request.Radius;
            orderBy = request.OrderBy;
        }
        
        request.PageSize ??= DefaultPageSize;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request, latitude, longitude, radius, orderBy);
        var eventsResponse = await apiClient.GetCalendarEvents(request.RequestedByMemberId!, parameters, cancellationToken);

        return new GetCalendarEventsQueryResult
        {
            Regions = regions,
            Calendars = calendarsResponse,
            CalendarEvents = eventsResponse.CalendarEvents,
            Page = eventsResponse.Page,
            TotalCount = eventsResponse.TotalCount,
            PageSize = eventsResponse.PageSize
        };
    }
}
