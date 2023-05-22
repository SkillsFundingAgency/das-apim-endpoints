using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryTests
{
    [Test, AutoData]
    public void Operator_PopulatesModelFroParameter(Guid memberId)
    {
        var model = new GetCalendarEventsQuery(memberId);
        model.RequestedByMemberId.Should().Be(memberId);
    }
}
