using System.Web;
using Microsoft.AspNetCore.WebUtilities;
using SFA.DAS.RecruitQa.InnerApi.Requests;

namespace SFA.DAS.RecruitQa.UnitTests.InnerApi;

public class WhenBuildingGetVacancyReviewsByUserRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Constructed_Correctly(string userId, DateTime assignationExpiry, string status)
    {
        // arrange
        var expectedUrl = QueryHelpers.AddQueryString("api/users/vacancyreviews", new Dictionary<string, string?>
        {
            ["assignationExpiry"] = $"{assignationExpiry:O}",
            ["status"] = status,
            ["userId"] = userId,
        });
        
        // act
        var actual = new GetVacancyReviewsByUserRequest(userId, assignationExpiry, status);

        // assert
        actual.GetUrl.Should().Be(expectedUrl);
    }

    [Test, AutoData]
    public void Then_The_Url_Handles_Null_AssignationExpiry(string userId)
    {
        // arrange
        var expectedUrl = QueryHelpers.AddQueryString("api/users/vacancyreviews", new Dictionary<string, string?>
        {
            ["assignationExpiry"] = null,
            ["status"] = null,
            ["userId"] = userId,
        });

        // act
        var actual = new GetVacancyReviewsByUserRequest(userId, null, null);

        // assert
        actual.GetUrl.Should().Be(expectedUrl);
    }
}
