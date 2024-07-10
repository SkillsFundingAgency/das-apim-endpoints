using RestEase;

namespace SFA.DAS.EmployerAan.Infrastructure;
public interface IHealthChecker
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);
}
