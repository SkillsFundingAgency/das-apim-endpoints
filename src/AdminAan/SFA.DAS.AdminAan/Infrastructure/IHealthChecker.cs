using RestEase;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface IHealthChecker
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);
}