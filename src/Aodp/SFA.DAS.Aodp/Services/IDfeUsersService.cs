using SFA.DAS.SharedOuterApi.Models.DfeSignIn;

namespace SFA.DAS.Aodp.Services
{
    public interface IDfeUsersService
    {
        Task<IReadOnlyList<User>> GetUsersByRoleAsync(string ukprn, string role, CancellationToken ct = default);
    }
}
