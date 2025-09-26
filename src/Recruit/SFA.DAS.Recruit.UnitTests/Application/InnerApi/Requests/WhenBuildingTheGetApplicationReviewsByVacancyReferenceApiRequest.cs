using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingTheGetApplicationReviewsByVacancyReferenceApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(long vacancyReference)
        {
            var actual = new GetApplicationReviewsByVacancyReferenceApiRequest(vacancyReference);

            actual.GetUrl.Should().Be($"api/applicationReviews/{vacancyReference}");
        }
    }
}