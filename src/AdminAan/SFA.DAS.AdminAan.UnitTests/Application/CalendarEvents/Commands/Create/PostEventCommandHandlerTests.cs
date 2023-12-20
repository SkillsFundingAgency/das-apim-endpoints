using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.AdminAan.Domain;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.CalendarEvents.Commands.Create;
public class PostEventCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        PostEventCommandHandler sut,
        PostEventCommand command,
        PostEventCommandResult expected,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        command.RequestedByMemberId = requestedByMemberId;
        apiClientMock.Setup(c => c.PostCalendarEvents(requestedByMemberId, command, cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.PostCalendarEvents(requestedByMemberId, command, cancellationToken));
        apiClientMock.Verify(c => c.PutGuestSpeakers(expected.CalendarEventId, requestedByMemberId, It.IsAny<PutEventGuestsModel>(), cancellationToken));
        actual.Should().Be(expected);
    }


    [Test, MoqAutoData]
    public async Task Handle_NoGuestSpeakers_DoesNotInvokePutGuestSpeakersApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        PostEventCommandHandler sut,
        PostEventCommand command,
        PostEventCommandResult expected,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        command.Guests = new List<GuestSpeaker>();

        command.RequestedByMemberId = requestedByMemberId;
        apiClientMock.Setup(c => c.PostCalendarEvents(requestedByMemberId, command, cancellationToken)).ReturnsAsync(expected);

        var actual = await sut.Handle(command, cancellationToken);

        apiClientMock.Verify(c => c.PostCalendarEvents(requestedByMemberId, command, cancellationToken));
        apiClientMock.Verify(c => c.PutGuestSpeakers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PutEventGuestsModel>(), cancellationToken), Times.Never);
        actual.Should().Be(expected);
    }
}
