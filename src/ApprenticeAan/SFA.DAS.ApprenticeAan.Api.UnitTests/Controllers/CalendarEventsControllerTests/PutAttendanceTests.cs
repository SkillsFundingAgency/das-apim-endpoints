using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SFA.DAS.ApprenticeAan.Api.Controllers;
using SFA.DAS.ApprenticeAan.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.ApprenticeAan.Application.InnerApi.Attendances;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeAan.Api.UnitTests.Controllers.CalendarEventsControllerTests;

public class PutAttendanceTests
{
    [Test, RecursiveMoqAutoData]
    public async Task PutAttendance_InvokesSendWithCommandProperties(
        [Frozen] Mock<IMediator> mediatorMock,
        Guid calendarEventId,
        Guid requestedByMemberId,
        AttendanceStatus request,
        CancellationToken cancellationToken)
    {
        var sut = new CalendarEventsController(mediatorMock.Object); 
        
        await sut.PutAttendance(calendarEventId, requestedByMemberId, request, cancellationToken);

        mediatorMock.Verify(m => m.Send(
            It.Is<PutAttendanceCommand>(
                c => c.CalendarEventId == calendarEventId 
                && c.RequestedByMemberId == requestedByMemberId 
                && c.AttendanceStatus == request), cancellationToken), 
                     Times.Once());
    }

    [Test, RecursiveMoqAutoData]
    public async Task PutAttendance_ReturnsNoContent(
    [Frozen] Mock<IMediator> mediatorMock,
    Guid calendarEventId,
    Guid requestedByMemberId,
    AttendanceStatus request,
    CancellationToken cancellationToken)
    {
        var sut = new CalendarEventsController(mediatorMock.Object);

        var result = await sut.PutAttendance(calendarEventId, requestedByMemberId, request, cancellationToken);

        result.Should().BeOfType<NoContentResult>();
    }
}
