using System;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
public class WhenBuildingGetApplicationByIdApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_GetUrl_Is_Correct(Guid applicationId, Guid candidateId)
    {
        // Arrange
        var expectedUrl = $"api/candidates/{candidateId}/applications/{applicationId}?includeDetail=true";
        // Act
        var request = new SFA.DAS.Recruit.InnerApi.Requests.GetApplicationByIdApiRequest(applicationId, candidateId);
        // Assert
        request.GetUrl.Should().Be(expectedUrl);
    }
}