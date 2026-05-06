using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Application.Commands.Application.Application;

public class WithdrawApplicationCommandResponse
{
    public IReadOnlyCollection<NotificationDefinition> Notifications { get; init; }
            = Array.Empty<NotificationDefinition>();
}

