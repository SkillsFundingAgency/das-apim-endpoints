using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.PayeSchemes;

public record GetAccountPayeSchemesRequest(long AccountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/accounts/{AccountId}/payeschemes";
}