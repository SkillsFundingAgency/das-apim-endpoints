using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Update;
using SFA.DAS.AdminAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.CalendarEvents.Commands.Update;
public class PutEventCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient_No_Guests(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        PutEventCommandHandler sut,
        PutEventCommand command,
        Guid memberId,
        Guid calendarEventId,
        CancellationToken cancellationToken)
    {
        var expected = new Unit();

        command.CalendarEventId = calendarEventId;
        command.RequestedByMemberId = memberId;
        command.Guests = new List<GuestSpeaker>();

        var actual = await sut.Handle(command, cancellationToken);
        apiClientMock.Verify(c => c.PutCalendarEvent(memberId, calendarEventId, command, cancellationToken), Times.Once);
        apiClientMock.Verify(c => c.PutGuestSpeakers(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<PutEventGuestsModel>(), It.IsAny<CancellationToken>()), Times.Never);
        actual.Should().Be(expected);
    }

    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient_With_Guests(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        PutEventCommandHandler sut,
        PutEventCommand command,
        Guid memberId,
        Guid calendarEventId,
        CancellationToken cancellationToken)
    {
        var expected = new Unit();

        command.CalendarEventId = calendarEventId;
        command.RequestedByMemberId = memberId;
        command.Guests = new List<GuestSpeaker>
        {
            new GuestSpeaker
            {
                GuestName = "guest name",
                GuestJobTitle = "guest job title"
            }
        };

        var putEventGuestModel = new PutEventGuestsModel(command.Guests);

        var actual = await sut.Handle(command, cancellationToken);
        apiClientMock.Verify(c => c.PutCalendarEvent(memberId, calendarEventId, command, cancellationToken), Times.Once);
        apiClientMock.Verify(c => c.PutGuestSpeakers(calendarEventId, memberId, putEventGuestModel, cancellationToken), Times.Once);
        actual.Should().Be(expected);
    }
}
