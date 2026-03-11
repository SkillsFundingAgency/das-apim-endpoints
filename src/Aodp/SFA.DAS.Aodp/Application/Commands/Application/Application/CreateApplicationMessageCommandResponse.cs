using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class CreateApplicationMessageCommandResponse
{
    public Guid Id { get; set; }
    public IReadOnlyCollection<NotificationDefinition> Notifications { get; init; }
            = Array.Empty<NotificationDefinition>();
    public bool EmailSent { get; set; } = false;
}



