using System.Globalization;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.AdminAan.Services;

public static class QueryStringParameterBuilder
{

    public static Dictionary<string, string[]> BuildQueryStringParameters(GetCalendarEventsQuery request, double? longitude = null, double? latitude = null, int? radius = null, string? orderBy=null)
    {
        var parameters = new Dictionary<string, string[]>();
        if (request.IsActive.HasValue) parameters.Add("IsActive", new[] { request.IsActive.ToString() }!);
        parameters.Add("fromDate",
            request.FromDate == null
                ? new[] { DateTime.Today.ToString("yyyy-MM-dd") }
                : new[] { request.FromDate });

        if (request.ToDate != null) parameters.Add("toDate", new[] { request.ToDate });

        if (request.CalendarIds != null && request.CalendarIds.Any())
        {
            parameters.Add("calendarId", request.CalendarIds.Select(cal => cal.ToString()).ToArray());
        }

        if (request.RegionIds != null && request.RegionIds.Any())
        {
            parameters.Add("regionId", request.RegionIds.Select(region => region.ToString()).ToArray());
        }

        if (request.Page != null) parameters.Add("page", new[] { request.Page?.ToString() }!);
        if (request.PageSize != null) parameters.Add("pageSize", new[] { request.PageSize?.ToString() }!);

        parameters.Add("ShowUserEventsOnly", new[] { request.ShowUserEventsOnly.ToString() });

        if (longitude.HasValue) { parameters.Add("longitude", [longitude.Value.ToString(CultureInfo.InvariantCulture)]); }
        if (latitude.HasValue) { parameters.Add("latitude", [latitude.Value.ToString(CultureInfo.InvariantCulture)]); }
        if(radius.HasValue) { parameters.Add("radius", [radius.Value.ToString()]);}
        if(!string.IsNullOrWhiteSpace(request.OrderBy)) { parameters.Add("orderBy", new[] { request.OrderBy }); }

        return parameters;
    }
}
