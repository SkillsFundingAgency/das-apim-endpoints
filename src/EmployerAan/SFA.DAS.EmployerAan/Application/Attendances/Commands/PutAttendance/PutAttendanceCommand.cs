using MediatR;
using SFA.DAS.EmployerAan.InnerApi.Attendances;

namespace SFA.DAS.EmployerAan.Application.Attendances.Commands.PutAttendance;

public class PutAttendanceCommand : IRequest<Unit>
{
    public Guid CalendarEventId { get; set; }
    public Guid RequestedByMemberId { get; set; }

    public AttendanceStatus AttendanceStatus { get; set; }

    public PutAttendanceCommand(Guid calendarEventId, Guid requestedByMemberId, AttendanceStatus attendanceStatus)
    {
        CalendarEventId = calendarEventId;
        RequestedByMemberId = requestedByMemberId;
        AttendanceStatus = attendanceStatus;
    }
}
