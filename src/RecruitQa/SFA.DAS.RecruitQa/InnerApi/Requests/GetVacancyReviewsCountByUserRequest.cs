using System.Web;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.RecruitQa.InnerApi.Requests;

public class GetVacancyReviewsCountByUserRequest(string userEmail, bool? approvedFirstTime, DateTime? assignationExpiry) : IGetApiRequest
{
    public string GetUrl => $"api/users/VacancyReviews/count?approvedFirstTime={approvedFirstTime}&assignationExpiry={assignationExpiry:yyyy-MMM-dd HH:mm:ss}&userEmail={HttpUtility.UrlEncode(userEmail)}";
}
