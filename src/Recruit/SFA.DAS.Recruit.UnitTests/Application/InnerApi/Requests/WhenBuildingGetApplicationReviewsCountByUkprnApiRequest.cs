using SFA.DAS.Recruit.InnerApi.Requests;
using System.Collections.Generic;

namespace SFA.DAS.Recruit.UnitTests.Application.InnerApi.Requests
{
    [TestFixture]
    public class WhenBuildingGetApplicationReviewsCountByUkprnApiRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(int ukprn, List<long> vacancyReferences)
        {
            //Act
            var actual = new GetApplicationReviewsCountByUkprnApiRequest(ukprn, vacancyReferences);

            //Assert
            actual.PostUrl.Should().Be($"api/provider/{ukprn}/applicationReviews/count");
        }
    }
}