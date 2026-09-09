using System;
using SFA.DAS.Recruit.InnerApi.Requests;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingTheGetApplicationReviewsByVacancyIdApiRequest
    {
        [Test, AutoData]
        public void Then_The_Request_Is_Correctly_Built(Guid vacancyId)
        {
            var actual = new GetApplicationReviewsByVacancyIdApiRequest(vacancyId);

            actual.GetUrl.Should().Be($"api/vacancies/byId/{vacancyId}/applicationReviews");
        }
    }
}