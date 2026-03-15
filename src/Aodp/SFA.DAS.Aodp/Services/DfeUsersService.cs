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
        try
        {
            var response = await _client.Get<OrganisationUsersResponse>(
                new GetUsersByRoleApiRequest(ukprn, role));

            return response?.Users?
                //.Where(u => u.UserStatus == 1)
                .ToList()
                ?? new List<User>();
        }
        catch (HttpRequestException ex)
        {
            throw new ApplicationException("Unable to retrieve reviewer users from DfE Sign-in.", ex);
        }
    }
}
