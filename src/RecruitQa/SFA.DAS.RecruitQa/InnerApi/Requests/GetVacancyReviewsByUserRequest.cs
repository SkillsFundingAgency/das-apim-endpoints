using System.Web;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsByUserRequest(string userId, DateTime? assignationExpiry, string? reviewStatus) : IGetApiRequest
{
    public string GetUrl => $"api/users/vacancyreviews?assignationExpiry={assignationExpiry:yyyy-MMM-dd HH:mm:ss}&status={reviewStatus}&userId={HttpUtility.UrlEncode(userId)}";
}
