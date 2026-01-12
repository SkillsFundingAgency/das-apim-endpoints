using System.Web;
using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsByUserRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(string userId, DateTime assignationExpiry)
    {
        var actual = new GetVacancyReviewsByUserRequest(userId, assignationExpiry);

        actual.GetUrl.Should().Be($"users/{HttpUtility.UrlEncode(userId)}/VacancyReviews?assignationExpiry={assignationExpiry}");
    }

    [Test, AutoData]
    public void Then_The_Url_Handles_Null_AssignationExpiry(string userId)
    {
        DateTime? assignationExpiry = null;
        var actual = new GetVacancyReviewsByUserRequest(userId, assignationExpiry);

        actual.GetUrl.Should().Be($"users/{HttpUtility.UrlEncode(userId)}/VacancyReviews?assignationExpiry=");
    }
}
