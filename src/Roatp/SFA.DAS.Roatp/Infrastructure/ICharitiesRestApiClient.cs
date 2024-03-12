using RestEase;
using SFA.DAS.Roatp.Domain.Models;
using System.Threading;
using System.Threading.Tasks;


namespace SFA.DAS.Roatp.Infrastructure;
public interface ICharitiesRestApiClient : IHealthChecker
{
    [Get("api/Charities/{registrationNumber}")]
    [AllowAnyStatusCode]
    Task<Response<GetCharityResponse>> GetCharities([Path] int registrationNumber, CancellationToken cancellationToken);
}