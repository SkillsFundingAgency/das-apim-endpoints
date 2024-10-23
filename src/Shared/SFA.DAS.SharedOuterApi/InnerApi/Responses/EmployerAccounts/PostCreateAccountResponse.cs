namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.EmployerAccounts;
public class PostCreateAccountResponse
{
    public required EmployerAccountSummary Account { get; set; }
    public required AccountLegalEntitySummary AccountLegalEntity { get; set; }
}
