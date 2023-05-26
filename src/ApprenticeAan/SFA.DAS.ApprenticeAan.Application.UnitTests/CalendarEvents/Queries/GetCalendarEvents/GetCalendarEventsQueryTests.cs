using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryTests
{
    [Test, AutoData]
    public void Operator_PopulatesModelFromParameter(Guid memberId, DateTime startDate, DateTime endDate)
    {
        var model = new GetCalendarEventsQuery(memberId, startDate, endDate);
        model.RequestedByMemberId.Should().Be(memberId);
        model.StartDate.Should().Be(startDate.ToString("yyyy-MM-dd"));
        model.EndDate.Should().Be(endDate.ToString("yyyy-MM-dd"));
    }
}
