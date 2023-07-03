using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Api.Models;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Common;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryTests
{
    [Test, AutoData]
    public void Operator_PopulatesModelFromParameters(Guid memberId, DateTime? fromDate, DateTime? toDate, List<EventFormat> eventFormats, List<int> calendarIds, List<int> regionIds, int? page, int? pageSize)
    {
        var model = new GetCalendarEventsModel
        {
            RequestedByMemberId = memberId,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };

        var query = (GetCalendarEventsQuery)model;

        query.RequestedByMemberId.Should().Be(memberId);
        query.FromDate.Should().Be(fromDate?.ToString("yyyy-MM-dd"));
        query.ToDate.Should().Be(toDate?.ToString("yyyy-MM-dd"));
        query.EventFormat.Should().BeEquivalentTo(eventFormats);
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