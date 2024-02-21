using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.EmployerAan.Infrastructure;

namespace SFA.DAS.EmployerAan.Application.Models;

public class GetCalendarEventsRequestModel
{
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }

    [FromQuery]
    public string? Keyword { get; set; }

    [FromQuery]
    public DateTime? FromDate { get; set; }

    [FromQuery]
    public DateTime? ToDate { get; set; }

    [FromQuery]
    public List<EventFormat> EventFormat { get; set; } = new List<EventFormat>();

    [FromQuery]
    public List<int> CalendarId { get; set; } = new List<int>();

    [FromQuery]
    public List<int> RegionId { get; set; } = new List<int>();

    [FromQuery]
    public int? Page { get; set; }

    [FromQuery]
    public int? PageSize { get; set; }

    public static implicit operator GetCalendarEventsQuery(GetCalendarEventsRequestModel requestModel) => new()
    {
        RequestedByMemberId = requestModel.RequestedByMemberId,
        Keyword = requestModel.Keyword,
        FromDate = requestModel.FromDate?.ToString("yyyy-MM-dd"),
        ToDate = requestModel.ToDate?.ToString("yyyy-MM-dd"),
        EventFormat = requestModel.EventFormat,
        RegionIds = requestModel.RegionId,
        CalendarIds = requestModel.CalendarId,
        Page = requestModel.Page,
        PageSize = requestModel.PageSize
    };
}
