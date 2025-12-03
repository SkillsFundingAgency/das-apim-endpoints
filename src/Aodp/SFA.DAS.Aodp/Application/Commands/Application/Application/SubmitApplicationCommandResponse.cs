using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application
{
    public class SubmitApplicationCommandResponse
    {
        public IReadOnlyCollection<NotificationDefinition> Notifications { get; init; }
                = Array.Empty<NotificationDefinition>();
    }
}
