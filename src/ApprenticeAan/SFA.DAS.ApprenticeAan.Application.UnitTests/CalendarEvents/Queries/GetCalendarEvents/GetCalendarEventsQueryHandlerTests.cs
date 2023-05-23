using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEvents;
public class GetCalendarEventsQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_ReturnCalendarEvents(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventsQueryHandler handler,
        Response<GetCalendarEventsQueryResult> expected,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        expected.ResponseMessage.StatusCode = HttpStatusCode.OK;
        var query = new GetCalendarEventsQuery(requestedByMemberId);
        apiClient.Setup(x => x.GetCalendarEvents(requestedByMemberId.ToString(), cancellationToken)).ReturnsAsync(expected);
        var actual = await handler.Handle(query, cancellationToken);
        actual.Should().Be(expected.GetContent());
    }
}