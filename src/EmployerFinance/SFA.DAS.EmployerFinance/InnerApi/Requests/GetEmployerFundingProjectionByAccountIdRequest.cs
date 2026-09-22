using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests;

public sealed record GetEmployerFundingProjectionByAccountIdRequest(long AccountId, int Months = 12) : IGetApiRequest
{
    public string GetUrl => $"api/employer/{AccountId}/funding-projection?months={Months}";
}