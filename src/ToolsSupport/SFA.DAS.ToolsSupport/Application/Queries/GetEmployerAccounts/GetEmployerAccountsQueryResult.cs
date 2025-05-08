namespace SFA.DAS.ToolsSupport.Application.Queries.GetEmployerAccounts;

public class GetEmployerAccountsQueryResult
{
    public List<EmployerAccount> Accounts { get; set; } = new();
}

public class EmployerAccount
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = string.Empty;
    public string PublicHashedAccountId { get; set; } = string.Empty;
    public string DasAccountName { get; set; } = string.Empty;
}