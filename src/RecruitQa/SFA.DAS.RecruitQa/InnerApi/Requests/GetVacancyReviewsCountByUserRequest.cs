using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsCountByUserRequest(string userId, bool? approvedFirstTime) : IGetApiRequest
{
    public string GetUrl => $"users/{HttpUtility.UrlEncode(userId)}/VacancyReviews/count?approvedFirstTime={approvedFirstTime}";
}
