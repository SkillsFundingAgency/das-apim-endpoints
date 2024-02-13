using RestEase;

namespace SFA.DAS.ApprenticeAan.Application.Infrastructure;

public interface ILocationApiClient : IHealthChecker
{
    [Get("api/addresses")]
    Task<GetAddressesListResponse> GetAddresses([Query] string query, [Query] double minMatch);
}