using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByUserRequest(string userId, DateTime? assignationExpiry, string? reviewStatus) : IGetApiRequest
{
    public string GetUrl => $"api/users/{HttpUtility.UrlEncode(userId)}/vacancyreviews?assignationExpiry={assignationExpiry:yyyy-MMM-dd HH:mm:ss}&status={reviewStatus}";
}
