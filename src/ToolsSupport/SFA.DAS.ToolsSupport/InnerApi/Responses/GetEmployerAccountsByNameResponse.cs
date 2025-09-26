namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetEmployerAccountsByNameResponse
{
    public List<EmployerAccountByName> EmployerAccounts { get; set; } = new();
}

public class EmployerAccountByName
{
    public long AccountId { get; set; }
    public string DasAccountName { get; set; }
    public string HashedAccountId { get; set; }
    public string PublicHashedAccountId { get; set; }
} 