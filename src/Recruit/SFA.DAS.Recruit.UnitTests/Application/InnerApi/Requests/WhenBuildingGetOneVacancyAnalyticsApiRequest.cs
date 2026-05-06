using SFA.DAS.Recruit.InnerApi.Recruit.Requests.VacancyAnalytics;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests;

[TestFixture]
internal class WhenBuildingGetOneVacancyAnalyticsApiRequest
{
    [Test, MoqAutoData]
    public void Then_The_Url_Is_Correctly_Built(long vacancyReference)
    {
        //Arrange
        var expectedUrl = $"api/vacancyAnalytics/{vacancyReference}";
        //Act
        var actual = new GetOneVacancyAnalyticsApiRequest(vacancyReference);
        //Assert
        actual.GetUrl.Should().Be(expectedUrl);
    }
}