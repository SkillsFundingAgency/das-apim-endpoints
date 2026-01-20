using System.Web;
using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsCountByUserRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(string userId, DateTime assignationExpiry)
    {
        var actual = new GetVacancyReviewsCountByUserRequest(userId, true, assignationExpiry);

        actual.GetUrl.Should().Be($"api/users/{HttpUtility.UrlEncode(userId)}/VacancyReviews/count?approvedFirstTime=True&assignationExpiry={assignationExpiry:yyyy-MMM-dd}");
    }

    [Test, AutoData]
    public void Then_The_Url_Handles_Null_ApprovedFirstTime(string userId)
    {
        bool? approvedFirstTime = null;
        DateTime? assignationExpiry = null;
        var actual = new GetVacancyReviewsCountByUserRequest(userId, approvedFirstTime, assignationExpiry);

        actual.GetUrl.Should().Be($"api/users/{HttpUtility.UrlEncode(userId)}/VacancyReviews/count?approvedFirstTime=&assignationExpiry=");
    }
}
