using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Common;
using SFA.DAS.ApprenticeAan.Application.Infrastructure.Configuration;

namespace SFA.DAS.ApprenticeAan.Api.Models;

public class GetCalendarEventsModel
{
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }

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
    public int? Page { get; set; } = 1;

    [FromQuery] public int? PageSize { get; set; }

    public static implicit operator GetCalendarEventsQuery(GetCalendarEventsModel model) => new()
    {
        RequestedByMemberId = model.RequestedByMemberId,
        FromDate = model.FromDate?.ToString("yyyy-MM-dd"),
        ToDate = model.ToDate?.ToString("yyyy-MM-dd"),
        EventFormat = model.EventFormat,
        RegionIds = model.RegionId,
        CalendarIds = model.CalendarId,
        Page = model.Page,
        PageSize = model.PageSize
    };
}
