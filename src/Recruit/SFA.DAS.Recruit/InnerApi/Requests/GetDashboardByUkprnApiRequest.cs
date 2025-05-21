using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetDashboardByUkprnApiRequest(int Ukprn) : IGetApiRequest
    {
        public string GetUrl => $"api/provider/{Ukprn}/applicationReviews/dashboard";
    }
}