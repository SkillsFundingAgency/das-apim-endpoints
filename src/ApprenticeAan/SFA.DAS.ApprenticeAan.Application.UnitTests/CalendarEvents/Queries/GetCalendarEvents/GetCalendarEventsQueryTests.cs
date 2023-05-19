using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryTests
{
    [Test, RecursiveMoqAutoData]
    public void Operator_PopulatesModelFroParameter(Guid memberId)
    {
        var model = new GetCalendarEventsQuery(memberId);
        model.RequestedByMemberId.Should().Be(memberId);
    }
}
