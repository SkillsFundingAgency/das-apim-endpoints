using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Delete;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;

public class DeleteCalendarEventsControllerTests
{
    [Test, MoqAutoData]
    public async Task CreateEvent_Post_InvokesRequest(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid calendarEventId,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<DeleteEventCommand>(
            x => x.RequestedByMemberId == requestedByMemberId
                 && x.CalendarEventId == calendarEventId
        ), cancellationToken)).ReturnsAsync(new Unit());

        var response = await sut.DeleteCalendarEvent(calendarEventId, requestedByMemberId, cancellationToken);

        mediatorMock.Verify(m => m.Send(It.Is<DeleteEventCommand>(
        x => x.RequestedByMemberId == requestedByMemberId
             && x.CalendarEventId == calendarEventId), It.IsAny<CancellationToken>()));
        response.As<NoContentResult>().Should().NotBeNull();
    }
}