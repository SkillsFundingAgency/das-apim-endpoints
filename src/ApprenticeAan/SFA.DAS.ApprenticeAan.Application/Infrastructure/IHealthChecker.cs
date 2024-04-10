using RestEase;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public interface IHealthChecker
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);
}