using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.ExternalApi.DfeSignIn;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.DfeSignIn;

namespace SFA.DAS.Aodp.Services;
public sealed class DfeUsersService : IDfeUsersService
{
    private readonly IDfeUsersApiClient<DfeSignInApiConfiguration> _client;

    public DfeUsersService(IDfeUsersApiClient<DfeSignInApiConfiguration> client)
    {
        _client = client;
    }

    public async Task<IReadOnlyList<User>> GetUsersByRoleAsync(string ukprn, string role, CancellationToken ct = default)
    {
        var response = await _client.Get<OrganisationUsersResponse>(new GetUsersByRoleApiRequest(ukprn, role));
        return response.Users;
    }
}
