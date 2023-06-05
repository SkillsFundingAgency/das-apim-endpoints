using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryTests
{
    [Test, AutoData]
    public void Operator_PopulatesModelFromParameters(Guid memberId, DateTime fromDate, DateTime toDate)
    {
        var model = new GetCalendarEventsQuery(memberId, fromDate, toDate);
        model.RequestedByMemberId.Should().Be(memberId);
        model.FromDate.Should().Be(fromDate.ToString("yyyy-MM-dd"));
        model.ToDate.Should().Be(toDate.ToString("yyyy-MM-dd"));
    }

    [Test, AutoData]
    public void Operator_PopulatesModelFromParametersDatesNull(Guid memberId)
    {

        var model = new GetCalendarEventsQuery(memberId, null, null);
        model.RequestedByMemberId.Should().Be(memberId);
        model.FromDate.Should().BeNull();
        model.ToDate.Should().BeNull();
    }
}
