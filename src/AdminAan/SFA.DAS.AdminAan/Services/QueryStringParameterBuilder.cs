using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.AdminAan.Services;

public static class QueryStringParameterBuilder
{

    public static Dictionary<string, string[]> BuildQueryStringParameters(GetCalendarEventsQuery request)
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
        return parameters;
    }
}
