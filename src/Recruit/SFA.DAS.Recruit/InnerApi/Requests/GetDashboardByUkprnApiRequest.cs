using SFA.DAS.Recruit.Enums;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetDashboardByUkprnApiRequest(int Ukprn, ApplicationReviewStatus Status) : IGetApiRequest
    {
        public string GetUrl => $"api/provider/{Ukprn}/applicationReviews/dashboard?status={Status}";
    }
}