using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Application.Members.Queries.GetMembers;
using System.Globalization;

namespace SFA.DAS.EmployerAan.Application.Services;
public static class QueryStringParameterBuilder
{

    public static Dictionary<string, string[]> BuildQueryStringParameters(GetCalendarEventsQuery request, double? longitude = null, double? latitude = null, int? radius = null, string? orderBy = null)
    {
        var parameters = new Dictionary<string, string[]>();
        if (!string.IsNullOrWhiteSpace(request.Keyword)) parameters.Add("keyword", new[] { request.Keyword });
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


        if (longitude.HasValue) { parameters.Add("longitude", [longitude.Value.ToString(CultureInfo.InvariantCulture)]); }
        if (latitude.HasValue) { parameters.Add("latitude", [latitude.Value.ToString(CultureInfo.InvariantCulture)]); }
        if (radius.HasValue) { parameters.Add("radius", [radius.Value.ToString()]); }
        if (!string.IsNullOrWhiteSpace(request.OrderBy)) { parameters.Add("orderBy", new[] { request.OrderBy }); }

        if (request.Page != null) parameters.Add("page", new[] { request.Page?.ToString() }!);
        if (request.PageSize != null) parameters.Add("pageSize", new[] { request.PageSize?.ToString() }!);
        parameters.Add("isActive", new[] { "true" });
        return parameters;
    }
    public static Dictionary<string, string[]> BuildQueryStringParameters(GetMembersQuery request)
    {
        var parameters = new Dictionary<string, string[]>();
        if (!string.IsNullOrWhiteSpace(request.Keyword)) parameters.Add("keyword", new[] { request.Keyword });
        if (request.RegionIds != null && request.RegionIds.Any())
        {
            parameters.Add("regionId", request.RegionIds.Select(region => region.ToString()).ToArray());
        }

        if (request.Page != null) parameters.Add("page", new[] { request.Page.ToString() }!);
        if (request.PageSize != null) parameters.Add("pageSize", new[] { request.PageSize.ToString() }!);
        if (request.UserType != null && request.UserType.Any())
        {
            parameters.Add("userType", request.UserType.Select(userType => userType.ToString()).ToArray());
        }
        if (request.IsRegionalChair != null) parameters.Add("isRegionalChair", new[] { request.IsRegionalChair.ToString() }!);
        return parameters;
    }
}
