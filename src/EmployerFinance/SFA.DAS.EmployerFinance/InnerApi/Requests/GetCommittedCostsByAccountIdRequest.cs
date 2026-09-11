using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests;

public sealed record GetCommittedCostsByAccountIdRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/committed-costs/{AccountId}";
}