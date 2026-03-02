using SFA.DAS.Recruit.InnerApi.Recruit.Requests.Reports;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;
[TestFixture]
internal class WhenBuildingGetReportsByUkprnRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Built(int ukprn)
    {
        //Arrange
        var expectedUrl = $"api/reports/{ukprn}/provider";
        //Act
        var actual = new GetReportsByUkprnRequest(ukprn);
        //Assert
        actual.GetUrl.Should().Be(expectedUrl);
    }
}