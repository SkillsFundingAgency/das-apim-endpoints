using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingPostCreateApplicationReviewNotificationsRequest
{
    [Test, AutoData]
    public void Then_The_Url_Is_Correctly_Constructed(Guid id)
    {
        var actual = new PostCreateApplicationReviewNotificationsRequest(id);

        actual.PostUrl.Should().Be($"api/applicationreviews/{id}/create-notifications");
    }
}