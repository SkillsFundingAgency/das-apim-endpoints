using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests;

public class GetAccountLegalEntitiesRequest(long accountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/accounts/{accountId}/legalentities?includeDetails=true";
}
