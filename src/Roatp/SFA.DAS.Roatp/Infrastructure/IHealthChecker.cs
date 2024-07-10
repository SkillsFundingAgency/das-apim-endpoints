using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using RestEase;

namespace SFA.DAS.Roatp.Infrastructure;

public interface IHealthChecker
{
    [Get("/health")]
    [AllowAnyStatusCode]
    Task<HttpResponseMessage> GetHealth(CancellationToken cancellationToken);
}