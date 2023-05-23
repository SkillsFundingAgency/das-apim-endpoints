using MediatR;
using SFA.DAS.ApprenticeAan.Application.Infrastructure;

namespace SFA.DAS.ApprenticeAan.Application.Attendances.Commands.PutAttendance;

public class PutAttendanceCommandHandler : IRequestHandler<PutAttendanceCommand, Unit>
{
    private readonly IAanHubRestApiClient _apiClient;
    public PutAttendanceCommandHandler(IAanHubRestApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<Unit> Handle(PutAttendanceCommand command, CancellationToken cancellationToken)
    {
        await _apiClient.PutAttendance(command.CalendarEventId, command.RequestedByMemberId, command.AttendanceStatus, cancellationToken);
        return Unit.Value;
    }
}
