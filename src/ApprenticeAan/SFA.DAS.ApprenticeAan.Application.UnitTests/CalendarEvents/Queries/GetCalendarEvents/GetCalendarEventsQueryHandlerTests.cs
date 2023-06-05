using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventsQueryHandler handler,
        GetCalendarEventsQueryResult expected,
        Guid requestedByMemberId,
        DateTime fromDate,
        DateTime toDate,
        CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventsQuery(requestedByMemberId, fromDate, toDate);
        apiClient.Setup(x => x.GetCalendarEvents(requestedByMemberId.ToString(), It.IsAny<string>(), It.IsAny<string>(), cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected);
    }
}