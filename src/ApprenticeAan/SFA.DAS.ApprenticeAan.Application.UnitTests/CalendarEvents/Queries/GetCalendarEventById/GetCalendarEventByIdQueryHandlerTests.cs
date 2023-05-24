using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using RestEase;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Application.UnitTests.CalendarEvents.Queries.GetCalendarEventById;

public class GetCalendarEventByIdQueryHandlerTests
{
    [Test]
    [MoqAutoData]
    public async Task Handle_InvokesGetCalendarEventById(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventByIdQueryHandler sut,
        Guid calendarEventId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventByIdQuery(calendarEventId, requestedByMemberId);

        await sut.Handle(query, cancellationToken);

        apiClient.Verify(a => a.GetCalendarEventById(calendarEventId, requestedByMemberId, cancellationToken), Times.Once);
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApiClientReturnsCalendarEvent_ReturnsSameCalendarEvent(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventByIdQueryHandler sut,
        Response<CalendarEventDetails> expected,
        Guid calendarEventId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventByIdQuery(calendarEventId, requestedByMemberId);
        apiClient.Setup(x => x.GetCalendarEventById(calendarEventId, requestedByMemberId, cancellationToken))
                 .ReturnsAsync(expected);

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().Be(expected.GetContent());
    }

    [Test]
    [MoqAutoData]
    public async Task Handle_ApiClientReturnsNull_ReturnsNull(
        [Frozen] Mock<IAanHubRestApiClient> apiClient,
        GetCalendarEventByIdQueryHandler sut,
        Response<CalendarEventDetails> expected,
        Guid calendarEventId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        var query = new GetCalendarEventByIdQuery(calendarEventId, requestedByMemberId);
        apiClient.Setup(x => x.GetCalendarEventById(calendarEventId, requestedByMemberId, cancellationToken))
                 .ReturnsAsync(() => null!); ;

        var actual = await sut.Handle(query, cancellationToken);

        actual.Should().BeNull();
    }
}
