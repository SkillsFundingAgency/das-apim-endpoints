using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests;

public record GetDashboardByAccountIdApiRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/employer/{AccountId}/applicationReviews/dashboard";
}