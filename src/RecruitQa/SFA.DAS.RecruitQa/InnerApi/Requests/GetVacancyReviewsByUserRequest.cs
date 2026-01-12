using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByUserRequest(string userId, DateTime? assignationExpiry) : IGetApiRequest
{
    public string GetUrl => $"users/{HttpUtility.UrlEncode(userId)}/VacancyReviews?assignationExpiry={assignationExpiry}";
}
