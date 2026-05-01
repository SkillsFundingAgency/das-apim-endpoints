using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
internal class WhenBuildingPostCreateReportRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Built(PostReportRequest.PostReportRequestData payload)
    {
        //Arrange
        var expectedUrl = $"api/reports";
        //Act
        var actual = new PostReportRequest(payload);
        //Assert
        actual.PostUrl.Should().Be(expectedUrl);
        actual.Data.Should().Be(payload);
    }
}