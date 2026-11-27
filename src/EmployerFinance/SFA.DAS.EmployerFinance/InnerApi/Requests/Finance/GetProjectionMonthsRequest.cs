using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetProjectionMonthsRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/employer/{AccountId}/funding-projection/months";
}
