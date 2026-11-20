using RestEase;
using SFA.DAS.PAS.Application.UpsertUserSettings;

namespace SFA.DAS.PAS.Infrastructure;

public interface IPasApiClient
{
    [Put("/users/{userRef}/settings")]
    Task UpsertUserSettingsAsync([Path] string userRef, [Body] UpsertUserSettingsApiRequest request, CancellationToken cancellationToken);

    [Get("/ping")]
    Task HealthCheck(CancellationToken cancellationToken);
}
