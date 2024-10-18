using MediatR;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.AdminAan.Services;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandler(IAanHubRestApiClient apiClient, ILocationLookupService locationLookupService, ICacheStorageService cacheService)
    : IRequestHandler<GetCalendarEventsQuery, GetCalendarEventsQueryResult?>
{
    public const int DefaultPageSize = 10;
    public const string RegionsCacheKey = "GetCalendarEventsQueryHandler.Regions";
    public const string CalendarsCacheKey = "GetCalendarEventsQueryHandler.Calendars";


    public async Task<GetCalendarEventsQueryResult?> Handle(GetCalendarEventsQuery request, CancellationToken cancellationToken)
    {
        var regionTask = GetRegionsAsync(cancellationToken);
        var calendarTask = GetCalendarsAsync(cancellationToken);

        await Task.WhenAll(regionTask, calendarTask);

        var regions = await regionTask;
        var calendars = await calendarTask;

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
                    Calendars = calendars
                };
            }

            latitude = locationData.GeoPoint[0];
            longitude = locationData.GeoPoint[1];
            radius = request.Radius;
            orderBy = request.OrderBy;
        }
        
        request.PageSize ??= DefaultPageSize;

        var parameters = QueryStringParameterBuilder.BuildQueryStringParameters(request, longitude, latitude, radius, orderBy);
        var eventsResponse = await apiClient.GetCalendarEvents(request.RequestedByMemberId!, parameters, cancellationToken);

        return new GetCalendarEventsQueryResult
        {
            Regions = regions,
            Calendars = calendars,
            CalendarEvents = eventsResponse.CalendarEvents,
            Page = eventsResponse.Page,
            TotalCount = eventsResponse.TotalCount,
            PageSize = eventsResponse.PageSize,
            TotalPages = eventsResponse.TotalPages
        };
    }

    private async Task<List<GetCalendarEventsQueryResult.RegionData>> GetRegionsAsync(CancellationToken cancellationToken)
    {
        var cachedResponse = await cacheService.RetrieveFromCache<List<GetCalendarEventsQueryResult.RegionData>>(RegionsCacheKey);
        if (cachedResponse != null) return cachedResponse;

        var regionResponse = await apiClient.GetRegions(cancellationToken);
        var regions = regionResponse.Regions.Select(r => new GetCalendarEventsQueryResult.RegionData
        {
            Id = r.Id,
            Area = r.Area,
            Ordering = r.Ordering
        }).ToList();

        await cacheService.SaveToCache(RegionsCacheKey, regions, 1);
        return regions;
    }

    private async Task<List<GetCalendarEventsQueryResult.CalendarType>> GetCalendarsAsync(CancellationToken cancellationToken)
    {
        var cachedCalendars = await cacheService.RetrieveFromCache<List<GetCalendarEventsQueryResult.CalendarType>>(CalendarsCacheKey);
        if (cachedCalendars != null) return cachedCalendars;

        var calendarsResponse = await apiClient.GetCalendars(cancellationToken);
        var calendars = calendarsResponse.Select(c => new GetCalendarEventsQueryResult.CalendarType
        {
            CalendarName = c.CalendarName,
            EffectiveFrom = c.EffectiveFrom,
            EffectiveTo = c.EffectiveTo,
            Id = c.Id,
            Ordering = c.Ordering
        }).ToList();

        await cacheService.SaveToCache(CalendarsCacheKey, calendars, 1);
        return calendars;
    }
}
