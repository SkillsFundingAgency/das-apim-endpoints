using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.AdminAan.Api.Controllers;
using SFA.DAS.AdminAan.Api.Models;
using SFA.DAS.AdminAan.Application.CalendarEvents.Queries.GetCalendarEvents;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.Api.UnitTests.Controllers;
public class GetCalendarEventsTests
{
    [Test, MoqAutoData]
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
            IsActive = null,
            FromDate = fromDate,
            ToDate = toDate,
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
        bool? IsActive,
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
            IsActive = IsActive,
            FromDate = fromDate,
            ToDate = toDate,
            CalendarId = calendarIds,
            RegionId = regionIds,
            Page = page,
            PageSize = pageSize
        };
        var result = await sut.GetCalendarEvents(model, cancellationToken);

        result.As<OkObjectResult>().Value.Should().Be(queryResult);
    }
}
