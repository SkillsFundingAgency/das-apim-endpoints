using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsCountByUserRequest(string userId, bool? approvedFirstTime, DateTime? assignationExpiry) : IGetApiRequest
{
    public string GetUrl => $"api/users/{HttpUtility.UrlEncode(userId)}/VacancyReviews/count?approvedFirstTime={approvedFirstTime}&assignationExpiry={assignationExpiry:yyyy-MMM-dd HH:mm:ss}";
}
