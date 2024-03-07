using RestEase;

namespace SFA.DAS.RoatpOversight.Infrastructure;

public interface IHealthChecker
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);
}