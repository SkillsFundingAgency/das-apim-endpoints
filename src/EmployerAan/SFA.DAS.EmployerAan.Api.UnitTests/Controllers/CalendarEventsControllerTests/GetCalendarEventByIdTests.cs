using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEventById;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.CalendarEventsControllerTests;
public class GetCalendarEventByIdTests
{
    [Test, RecursiveMoqAutoData]
    public async Task GetCalendarEventById_OkResponse_ReturnsOkWithEvent(
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] GetCalendarEventByIdQuery query,
        CalendarEvent calendarEvent,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarEventByIdQuery>(), cancellationToken))
                    .ReturnsAsync(calendarEvent);

        var sut = new CalendarEventsController(mediatorMock.Object);

        var result = await sut.GetCalendarEventById(query.CalendarEventId, query.RequestedByMemberId, cancellationToken);

        result.As<OkObjectResult>().Value.Should().Be(calendarEvent);
    }

    [Test, RecursiveMoqAutoData]
    public async Task GetCalendarEventById_NullResponse_ReturnsNotFound(
        [Frozen] Mock<IMediator> mediatorMock,
        [Frozen] GetCalendarEventByIdQuery query,
        CancellationToken cancellationToken)
    {
        mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarEventByIdQuery>(), cancellationToken))
                    .ReturnsAsync(() => null!);

        var sut = new CalendarEventsController(mediatorMock.Object);

        var result = await sut.GetCalendarEventById(query.CalendarEventId, query.RequestedByMemberId, cancellationToken);

        result.Should().BeOfType<NotFoundResult>();
    }
}
