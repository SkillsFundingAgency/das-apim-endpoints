using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.Services;

public interface IEmailService
{
    Task<bool> SendAsync(
        IReadOnlyCollection<NotificationDefinition> notifications,
        CancellationToken cancellationToken = default);
}

