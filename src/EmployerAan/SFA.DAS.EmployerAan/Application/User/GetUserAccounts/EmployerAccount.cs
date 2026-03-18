using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.SharedOuterApi.Types.Models;
using SFA.DAS.SharedOuterApi.Types.Models;

namespace SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

public record EmployerAccount(string DasAccountName, string EncodedAccountId, string Role)
{
    public static implicit operator EmployerAccount(EmployerAccountUser source) => new(source.DasAccountName, source.EncodedAccountId, source.Role);
}
