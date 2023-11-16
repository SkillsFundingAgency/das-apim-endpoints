using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvent;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;
public class GetCalendarEventControllerTests
{
    [Test, MoqAutoData]
    public async Task Get_InvokesQueryHandler(
     [Frozen] Mock<IMediator> mediatorMock,
     [Greedy] CalendarEventsController sut,
     Guid requestedByMemberId,
     Guid calendarEventId,
     CancellationToken cancellationToken)
    {
        await sut.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventQuery>(q => q.RequestedByMemberId == requestedByMemberId && q.CalendarEventId == calendarEventId),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid requestedByMemberId,
        Guid calendarEventId,
        GetCalendarEventQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventQuery>(q => q.RequestedByMemberId == requestedByMemberId && q.CalendarEventId == calendarEventId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetCalendarEvent(requestedByMemberId, calendarEventId, cancellationToken);

        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
