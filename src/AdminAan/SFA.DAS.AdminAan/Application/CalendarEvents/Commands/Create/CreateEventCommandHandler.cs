using MediatR;

namespace SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
public class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, CreateEventCommandResult>
{
    public async Task<CreateEventCommandResult> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var result = new CreateEventCommandResult { CalendarEventId = Guid.NewGuid() };
        return result;
    }
}
