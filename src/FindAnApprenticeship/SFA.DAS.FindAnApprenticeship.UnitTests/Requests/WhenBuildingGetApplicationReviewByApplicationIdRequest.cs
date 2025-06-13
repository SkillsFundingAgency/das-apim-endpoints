using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitV2Api.Requests;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Requests;

public class WhenBuildingGetApplicationReviewByApplicationIdRequest
{
    [Test, AutoData]
    public void Then_The_Request_Is_Built_Correctly(Guid applicationId)
    {
        var actual = new GetApplicationReviewByApplicationIdRequest(applicationId);
        
        actual.GetUrl.Should().Be($"api/applicationReviews?applicationId={applicationId}");
    }
}