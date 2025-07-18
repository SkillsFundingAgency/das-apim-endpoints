using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingTheGetApplicationsByVacancyReferenceApiRequest
    {
        [Test, MoqAutoData]
        public void Then_The_Request_Is_Built_Correctly(long vacancyReference)
        {
            // Arrange
            var expectedUrl = $"api/vacancies/{vacancyReference}/applications";
            // Act
            var request = new GetApplicationsByVacancyReferenceApiRequest(vacancyReference);
            
            // Assert
            request.GetUrl.Should().Be(expectedUrl);
        }
    }
}