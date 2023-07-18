using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.ApprenticeAan.Application.Services;

public static class QueryStringParameterBuilder
{

    public static Dictionary<string, string[]> BuildQueryStringParameters(GetCalendarEventsQuery request)
    {
        var parameters = new Dictionary<string, string[]>();
        if (request.Keyword.Trim() != string.Empty) parameters.Add("keyword", new[] { request.Keyword });
        if (request.FromDate != null) parameters.Add("fromDate", new[] { request.FromDate });
        if (request.ToDate != null) parameters.Add("toDate", new[] { request.ToDate });
        if (request.EventFormat != null && request.EventFormat.Any())
        {
            parameters.Add("eventFormat", request.EventFormat.Select(format => format.ToString()).ToArray());
        }

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