using MediatR;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Delete;
public class DeleteEventCommand : IRequest<Unit>
{
    public Guid RequestedByMemberId { get; set; }
    public Guid CalendarEventId { get; set; }
}
