using RestEase;
using SFA.DAS.AdminAan.Domain.Location;

namespace SFA.DAS.AdminAan.Infrastructure;

public interface ILocationApiClient
{
    [Get("api/addresses")]
    Task<GetAddressesResponse> GetAddresses([Query] string query, [Query] double minMatch);
}
