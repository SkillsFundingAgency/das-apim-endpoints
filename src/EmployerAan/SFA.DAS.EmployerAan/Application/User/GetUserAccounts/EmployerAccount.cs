using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EmployerAan.Application.User.GetUserAccounts;

public record EmployerAccount(string DasAccountName, string EncodedAccountId, string Role)
{
    public static implicit operator EmployerAccount(EmployerAccountUser source) => new(source.DasAccountName, source.EncodedAccountId, source.Role);
}
