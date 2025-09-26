using SFA.DAS.ToolsSupport.InnerApi.Responses;

namespace SFA.DAS.ToolsSupport.Application.Queries.SearchEmployerAccounts;

public class SearchEmployerAccountsQueryResult
{
    public List<EmployerAccount> EmployerAccounts { get; set; } = new();
}

public class EmployerAccount
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = string.Empty;
    public string PublicHashedAccountId { get; set; } = string.Empty;
    public string DasAccountName { get; set; } = string.Empty;
    
    public static explicit operator EmployerAccount(EmployerAccountByName source)
    {
        if (source == null) return new EmployerAccount();

        return new EmployerAccount
        {
            AccountId = source.AccountId,
            HashedAccountId = source.HashedAccountId,
            PublicHashedAccountId = source.PublicHashedAccountId,
            DasAccountName = source.DasAccountName,
        };
    }
}