namespace SFA.DAS.ToolsSupport.Application.Queries;

public class GetEmployerAccountsQueryResult
{
    public List<EmployerAccount> Accounts { get; set; } = new();
}

public class EmployerAccount
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; }
    public string PublicHashedAccountId { get; set; }
    public string DasAccountName { get; set; }
}