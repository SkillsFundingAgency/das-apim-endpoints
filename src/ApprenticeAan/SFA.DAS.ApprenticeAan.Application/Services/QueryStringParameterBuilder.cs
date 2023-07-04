using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.ApprenticeAan.Application.Services;

public static class QueryStringParameterBuilder
{

    public static Dictionary<string, string[]> BuildQueryStringParameters(GetCalendarEventsQuery request)
    {
        var parameters = new Dictionary<string, string[]>();
        if (request.FromDate != null) parameters.Add("fromDate", new[] { request.FromDate });
        if (request.ToDate != null) parameters.Add("toDate", new[] { request.ToDate });
        if (request.EventFormat != null)
        {
            parameters.Add("eventFormat", request.EventFormat.Select(format => format.ToString()).ToArray());
        }

        if (request.CalendarIds != null)
        {
            parameters.Add("calendarId", request.CalendarIds.Select(cal => cal.ToString()).ToArray());
        }

        if (request.RegionIds != null)
        {
            parameters.Add("regionId", request.RegionIds.Select(region => region.ToString()).ToArray());
        }

        if (request.Page != null) parameters.Add("page", new[] { request.Page?.ToString() }!);
        if (request.PageSize != null) parameters.Add("pageSize", new[] { request.PageSize?.ToString() }!);
        return parameters;
    }
}