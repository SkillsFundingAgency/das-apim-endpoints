using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetEmployerAlertsApiRequest(long AccountId, string UserId) : IGetApiRequest
{
    public string GetUrl => $"api/employer/{AccountId}/alerts?userId={UserId}";
}
