namespace SFA.DAS.EmployerAan.Models;
public record EmployerAccount(string DasAccountName, string EncodedAccountId, string Role)
{
    public static implicit operator EmployerAccount(EmployerAccountUser source) => new(source.DasAccountName, source.EncodedAccountId, source.Role);
}
