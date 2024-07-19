using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.PayeSchemes;

public record GetAccountPayeSchemesRequest(string hashedAccountId) : IGetAllApiRequest
{
    public string GetAllUrl => $"api/accounts/{hashedAccountId}/payeschemes";
}