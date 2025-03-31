using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Recruit.InnerApi.Requests
{
    public record GetDashboardByUkprnApiRequest(int Ukprn, ApplicationStatus Status) : IGetApiRequest
    {
        public string GetUrl => $"api/provider/{Ukprn}/applicationReviews/dashboard?status={Status}";
    }
}