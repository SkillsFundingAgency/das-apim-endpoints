using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.CalendarEvents.Queries.GetCalendarEvent;
public class GetCalendarEventQueryHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventQueryHandler handler,
        GetCalendarEventQueryResult expected,
        Guid requestedByMemberId,
        Guid calendarEventId,
        CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventQuery(requestedByMemberId, calendarEventId);

        apiClient.Setup(x => x.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
    }
}
