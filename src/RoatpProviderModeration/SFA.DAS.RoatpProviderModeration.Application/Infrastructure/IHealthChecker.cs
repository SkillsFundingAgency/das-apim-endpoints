using RestEase;

namespace SFA.DAS.RoatpProviderModeration.Application.Infrastructure;

public interface IHealthChecker
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);
}