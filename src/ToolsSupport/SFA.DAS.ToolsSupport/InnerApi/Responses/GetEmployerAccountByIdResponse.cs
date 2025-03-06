namespace SFA.DAS.ToolsSupport.InnerApi.Responses;

public class GetEmployerAccountByIdResponse
{
    public long AccountId { get; set; }
    public string HashedAccountId { get; set; } = "";
    public string PublicHashedAccountId { get; set; } = "";
    public string DasAccountName { get; set; } = "";
}