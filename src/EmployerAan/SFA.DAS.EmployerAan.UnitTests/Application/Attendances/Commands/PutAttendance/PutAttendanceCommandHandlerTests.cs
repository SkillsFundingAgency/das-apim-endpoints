using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using SFA.DAS.EmployerAan.Application.Attendances.Commands.PutAttendance;
using SFA.DAS.EmployerAan.Infrastructure;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerAan.UnitTests.Application.Attendances.Commands.PutAttendance;
public class PutAttendanceCommandHandlerTests
{
    [Test, MoqAutoData]
    public async Task Handle_InvokesApiClient(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        PutAttendanceCommandHandler sut,
        PutAttendanceCommand command,
        CancellationToken token)
    {
        await sut.Handle(command, token);

        apiClientMock.Verify(c => c.PutAttendance(command.CalendarEventId, command.RequestedByMemberId, command.AttendanceStatus, token), Times.Once);
    }

    [Test, MoqAutoData]
    public async Task Handle_AlwaysReturnsUnitValue(
        [Frozen] Mock<IAanHubRestApiClient> apiClientMock,
        PutAttendanceCommandHandler sut,
        PutAttendanceCommand command,
        CancellationToken token)
    {
        var result = await sut.Handle(command, token);
        result.Should().Be(Unit.Value);
    }
}
