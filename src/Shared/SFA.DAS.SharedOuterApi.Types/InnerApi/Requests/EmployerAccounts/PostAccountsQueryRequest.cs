using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;

public class PostAccountsQueryRequest : IPostApiRequest<AccountsQueryRequestBody>
{
    public PostAccountsQueryRequest(IReadOnlyCollection<long> accountIds)
    {
        Data = new AccountsQueryRequestBody
        {
            Filter = new AccountsQueryFilterBody { AccountIds = accountIds.ToList() },
            Select = [AccountQueryFieldNames.ApprenticeshipEmployerType],
            Include = [AccountQueryFieldNames.LegalEntities]
        };
    }

    public string PostUrl => "api/accounts/queries";

    public AccountsQueryRequestBody Data { get; set; }
}

public class AccountsQueryRequestBody
{
    public AccountsQueryFilterBody Filter { get; set; }
    public List<string> Select { get; set; } = [];
    public List<string> Include { get; set; } = [];
}

public class AccountsQueryFilterBody
{
    public List<long> AccountIds { get; set; } = [];
}

public static class AccountQueryFieldNames
{
    public const int MaxAccountIdsPerRequest = 100;
    public const string ApprenticeshipEmployerType = "apprenticeshipEmployerType";
    public const string LegalEntities = "legalEntities";
}
