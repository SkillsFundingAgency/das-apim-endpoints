using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using RestEase;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Delete;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.CalendarEvents.Commands.Delete;
public class DeleteEventCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        DeleteEventCommandHandler sut,
        Guid memberId,
        Guid calendarEventId,
        CancellationToken cancellationToken)
    {
        var expected = new Unit();

        var response = new Response<Unit>(
            "not used",
            new HttpResponseMessage(System.Net.HttpStatusCode.NoContent),
            () => expected);

        apiClientMock.Setup(c => c.DeleteCalendarEvent(memberId, calendarEventId, cancellationToken)).ReturnsAsync(response);

        var command = new DeleteEventCommand
        {
            CalendarEventId = calendarEventId,
            RequestedByMemberId = memberId
        };

        var actual = await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.DeleteCalendarEvent(memberId, calendarEventId, cancellationToken));
        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient_BadRequest(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        DeleteEventCommandHandler sut,
        Guid memberId,
        Guid calendarEventId,
        CancellationToken cancellationToken)
    {
        var expected = new Unit();

        var response = new Response<Unit>(
            "not used",
            new HttpResponseMessage(System.Net.HttpStatusCode.BadRequest),
            () => expected);

        apiClientMock.Setup(c => c.DeleteCalendarEvent(memberId, calendarEventId, cancellationToken)).ReturnsAsync(response);

        var command = new DeleteEventCommand
        {
            CalendarEventId = calendarEventId,
            RequestedByMemberId = memberId
        };

        Func<Task> act = () => sut.Handle(command, cancellationToken);

        await act.Should().ThrowAsync<InvalidOperationException>();
    }
}
