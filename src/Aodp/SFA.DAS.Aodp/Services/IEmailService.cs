using SFA.DAS.Aodp.Application.Commands.Application.Application;
using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Services;

public interface IEmailService
{
    Task SendAsync(
        IReadOnlyCollection<NotificationDefinition> notifications,
        CancellationToken cancellationToken = default);
}

