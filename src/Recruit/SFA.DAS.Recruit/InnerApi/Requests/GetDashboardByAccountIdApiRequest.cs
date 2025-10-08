using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests;

public record GetDashboardByAccountIdApiRequest(long AccountId) : IGetApiRequest
{
    public string GetUrl => $"api/employer/{AccountId}/applicationReviews/dashboard";
}