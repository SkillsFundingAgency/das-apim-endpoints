using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;
public record GetProviderAlertsApiRequest(int Ukprn, string UserId) : IGetApiRequest
{
    public string GetUrl => $"api/provider/{Ukprn}/alerts?userId={UserId}";
}