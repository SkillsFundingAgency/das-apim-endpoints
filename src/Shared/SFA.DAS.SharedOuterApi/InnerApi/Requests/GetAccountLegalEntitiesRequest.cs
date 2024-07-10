using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;
public class GetAccountLegalEntitiesRequest : IGetAllApiRequest
{
    private readonly string _accountHashedId;

    public GetAccountLegalEntitiesRequest(string accountHashedId)
    {
        _accountHashedId = accountHashedId;
    }

    public string GetAllUrl => $"api/accounts/{_accountHashedId}/legalentities?includeDetails=true";
}
