namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.EmployerAccounts;

public class PostAccountsQueryResponse
{
    public List<AccountQueryResultItem> Accounts { get; set; } = [];
}

public class AccountQueryResultItem
{
    public long AccountId { get; set; }
    public string ApprenticeshipEmployerType { get; set; }
    public ResourceList LegalEntities { get; set; } = [];
}
