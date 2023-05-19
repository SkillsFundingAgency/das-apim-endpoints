using FluentAssertions;
using SFA.DAS.ApprenticeAan.Application.InnerApi.CalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryRequestTests
{
    [Test, RecursiveMoqAutoData]
    public void Request_CheckGetUrl()
    {
        var model = new GetCalendarEventsQueryRequest();
        model.GetUrl.Should().Be("calendarEvents");
    }
}
