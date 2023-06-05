using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.CalendarEventsControllerTests;

public class GetCalendarEventsTests
{
    [Test]
    [MoqAutoData]
    public async Task Get_InvokesQueryHandler(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid requestedByMemberId,
        CancellationToken cancellationToken)
    {
        var fromDate = DateTime.Today;
        var toDate = DateTime.Today.AddDays(7);
        await sut.GetCalendarEvents(requestedByMemberId, fromDate, toDate, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid requestedByMemberId,
        GetCalendarEventsQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        var fromDate = DateTime.Today;
        var toDate = DateTime.Today.AddDays(7);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var result = await sut.GetCalendarEvents(requestedByMemberId, fromDate, toDate, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}