using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.AdminAan.Infrastructure;

namespace SFA.DAS.AdminAan.Api.Models;

public class GetCalendarEventsRequestModel
{
    [FromHeader(Name = Constants.ApiHeaders.RequestedByMemberIdHeader)]
    public Guid RequestedByMemberId { get; set; }


    [FromQuery]
    public bool? IsActive { get; set; }


    [FromQuery]
    public DateTime? FromDate { get; set; }

    [FromQuery]
    public DateTime? ToDate { get; set; }

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
        IsActive = requestModel.IsActive,
        FromDate = requestModel.FromDate?.ToString("yyyy-MM-dd"),
        ToDate = requestModel.ToDate?.ToString("yyyy-MM-dd"),
        RegionIds = requestModel.RegionId,
        CalendarIds = requestModel.CalendarId,
        Page = requestModel.Page,
        PageSize = requestModel.PageSize
    };
}