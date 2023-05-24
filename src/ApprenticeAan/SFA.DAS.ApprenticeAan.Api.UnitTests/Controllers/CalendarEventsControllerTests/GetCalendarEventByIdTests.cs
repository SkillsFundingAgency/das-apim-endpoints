using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEventById;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.CalendarEventsControllerTests
{
    public class GetCalendarEventByIdTests
    {
        [Test, RecursiveMoqAutoData]
        public async Task GetCalendarEventById_OkResponse_ReturnsOkWithEvent(
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] GetCalendarEventByIdQuery query,
            CalendarEvent calendarEvent,
            CancellationToken cancellationToken)
        {
            var response = new GetCalendarEventByIdQueryResponse(calendarEvent, System.Net.HttpStatusCode.OK);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarEventByIdQuery>(), cancellationToken))
                        .ReturnsAsync(response);

            var sut = new CalendarEventsController(mediatorMock.Object);

            var result = await sut.GetCalendarEventById(query.CalendarEventId, query.RequestedByMemberId, cancellationToken);

            result.As<OkObjectResult>().Value.Should().Be(calendarEvent);
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test, RecursiveMoqAutoData]
        public async Task GetCalendarEventById_NotFoundResponse_ReturnsNotFound(
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] GetCalendarEventByIdQuery query,
            CancellationToken cancellationToken)
        {
            var response = new GetCalendarEventByIdQueryResponse(null, System.Net.HttpStatusCode.NotFound);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarEventByIdQuery>(), cancellationToken))
                        .ReturnsAsync(response);

            var sut = new CalendarEventsController(mediatorMock.Object);

            var result = await sut.GetCalendarEventById(query.CalendarEventId, query.RequestedByMemberId, cancellationToken);

            result.Should().BeOfType<NotFoundResult>();
        }

        [Test, RecursiveMoqAutoData]
        public async Task GetCalendarEventById_BadRequestResponse_ReturnsBadRequest(
            [Frozen] Mock<IMediator> mediatorMock,
            [Frozen] GetCalendarEventByIdQuery query,
            CancellationToken cancellationToken)
        {
            var response = new GetCalendarEventByIdQueryResponse(null, System.Net.HttpStatusCode.BadRequest);

            mediatorMock.Setup(m => m.Send(It.IsAny<GetCalendarEventByIdQuery>(), cancellationToken))
                        .ReturnsAsync(response);

            var sut = new CalendarEventsController(mediatorMock.Object);

            var result = await sut.GetCalendarEventById(query.CalendarEventId, query.RequestedByMemberId, cancellationToken);

            result.Should().BeOfType<BadRequestResult>();
        }
    }
}
