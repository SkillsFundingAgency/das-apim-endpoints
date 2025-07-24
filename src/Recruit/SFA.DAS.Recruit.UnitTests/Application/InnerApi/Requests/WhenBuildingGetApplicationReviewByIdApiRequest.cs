using System;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
public class WhenBuildingGetApplicationReviewByIdApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_GetUrl_Is_Correct(Guid applicationReviewId)
    {
        // Arrange
        var expectedUrl = $"api/applicationReviews/{applicationReviewId}";
        // Act
        var request = new SFA.DAS.Recruit.InnerApi.Requests.GetApplicationReviewByIdApiRequest(applicationReviewId);
        // Assert
        request.GetUrl.Should().Be(expectedUrl);
    }
}