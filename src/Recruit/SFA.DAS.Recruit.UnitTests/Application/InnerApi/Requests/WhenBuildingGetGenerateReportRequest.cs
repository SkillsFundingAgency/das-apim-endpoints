using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;
using System;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
internal class WhenBuildingGetGenerateReportRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Built(Guid reportId)
    {
        //Arrange
        var expectedUrl = $"api/reports/generate/{reportId}";
        //Act
        var actual = new GetGenerateReportRequest(reportId);
        //Assert
        actual.GetUrl.Should().Be(expectedUrl);
    }
}