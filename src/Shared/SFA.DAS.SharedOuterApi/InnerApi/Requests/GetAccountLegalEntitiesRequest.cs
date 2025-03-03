using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests;
public class GetAccountLegalEntitiesRequest(long accountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/accounts/{accountId}/legalentities?includeDetails=true";
}
