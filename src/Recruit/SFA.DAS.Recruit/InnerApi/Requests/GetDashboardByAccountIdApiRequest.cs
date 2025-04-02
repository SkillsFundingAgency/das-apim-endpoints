using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetDashboardByAccountIdApiRequest(long AccountId, ApplicationStatus Status) : IGetApiRequest
    {
        public string GetUrl => $"api/employer/{AccountId}/applicationReviews/dashboard?status={Status}";
    }
}