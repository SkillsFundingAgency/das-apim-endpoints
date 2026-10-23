using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.EmployerFinance.InnerApi.Requests.Finance;

public sealed record GetExpiredLevyRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/expired-levy/{AccountId}";
}
