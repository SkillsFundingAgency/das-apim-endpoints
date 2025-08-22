using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Recruit.Requests
{
    public record GetDashboardByUkprnApiRequest(int Ukprn) : IGetApiRequest
    {
        public string GetUrl => $"api/provider/{Ukprn}/applicationReviews/dashboard";
    }
}