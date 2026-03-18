using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;
public class GetAccountLegalEntitiesRequest(long accountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/accounts/{accountId}/legalentities?includeDetails=true";
}
