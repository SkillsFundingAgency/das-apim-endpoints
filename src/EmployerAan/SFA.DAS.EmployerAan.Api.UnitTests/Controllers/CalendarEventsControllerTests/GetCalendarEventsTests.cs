using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.EmployerAan.Api.Controllers;
using SFA.DAS.EmployerAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.EmployerAan.Application.Models;
using SFA.DAS.EmployerAan.Common;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.Api.UnitTests.Controllers.CalendarEventsControllerTests;
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
        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = requestedByMemberId,
            Keyword = string.Empty,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = new List<EventFormat>(),
            CalendarId = new List<int>(),
            RegionId = new List<int>()
        };

        await sut.GetCalendarEvents(model, cancellationToken);

        mediatorMock.Verify(
            m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId),
                It.IsAny<CancellationToken>()));
    }

    [Test, MoqAutoData]
    public async Task Get_HandlerReturnsData_ReturnsOkResponse(
        [Frozen] Mock<IMediator> mediatorMock,
        [Greedy] CalendarEventsController sut,
        Guid requestedByMemberId,
        string keyword,
        List<EventFormat> eventFormats,
        List<int> calendarIds,
        List<int> regionIds,
        int? page,
        int? pageSize,
        GetCalendarEventsQueryResult queryResult,
        CancellationToken cancellationToken)
    {
        var fromDate = DateTime.Today;
        var toDate = DateTime.Today.AddDays(7);
        mediatorMock.Setup(m => m.Send(It.Is<GetCalendarEventsQuery>(q => q.RequestedByMemberId == requestedByMemberId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var model = new GetCalendarEventsRequestModel
        {
            RequestedByMemberId = requestedByMemberId,
            Keyword = keyword,
            FromDate = fromDate,
            ToDate = toDate,
            EventFormat = eventFormats,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };
        var result = await sut.GetCalendarEvents(model, cancellationToken);

        result.As<OkObjectResult>().Should().NotBeNull();
        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
