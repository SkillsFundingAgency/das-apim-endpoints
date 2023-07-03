using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Common;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryTests
{
    [Test, AutoData]
    public void Operator_PopulatesModelFromParameters(Guid memberId, DateTime fromDate, DateTime toDate, List<EventFormat>? eventFormats, List<int>? calendarIds, List<int>? regionIds, int? page, int? pageSize)
    {
        var model = new GetCalendarEventsQuery
        {
            RequestedByMemberId = memberId,
            FromDate = fromDate.ToString("yyyy-MM-dd"),
            ToDate = toDate.ToString("yyyy-MM-dd"),
            EventFormat = eventFormats,
            CalendarIds = calendarIds,
            RegionIds = regionIds,
            Page = page,
            PageSize = pageSize
        };
        model.RequestedByMemberId.Should().Be(memberId);
        model.FromDate.Should().Be(fromDate.ToString("yyyy-MM-dd"));
        model.ToDate.Should().Be(toDate.ToString("yyyy-MM-dd"));
        model.EventFormat.Should().BeEquivalentTo(eventFormats);
    }

    [Test, AutoData]
    public void Operator_PopulatesModelFromParametersDatesNull(Guid memberId)
    {
        var model = new GetCalendarEventsQuery
        {
            RequestedByMemberId = memberId
        };
        model.RequestedByMemberId.Should().Be(memberId);
        model.FromDate.Should().BeNull();
        model.ToDate.Should().BeNull();
        model.EventFormat.Should().BeNull();
        model.Page.Should().BeNull();
        model.PageSize.Should().BeNull();
    }
}