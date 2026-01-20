using System.Web;
using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsByUserRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(string userId, DateTime assignationExpiry, string status)
    {
        var actual = new GetVacancyReviewsByUserRequest(userId, assignationExpiry, status);

        actual.GetUrl.Should().Be($"api/users/{HttpUtility.UrlEncode(userId)}/vacancyreviews?assignationExpiry={assignationExpiry:yyyy-MMM-dd}&status={status}");
    }

    [Test, AutoData]
    public void Then_The_Url_Handles_Null_AssignationExpiry(string userId)
    {
        var actual = new GetVacancyReviewsByUserRequest(userId, null, null);

        actual.GetUrl.Should().Be($"api/users/{HttpUtility.UrlEncode(userId)}/vacancyreviews?assignationExpiry=&status=");
    }
}
