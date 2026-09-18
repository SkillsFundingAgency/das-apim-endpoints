using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests;

public sealed record GetEmployerFundingProjectionByAccountIdRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/employer/{AccountId}/funding-projection";
}