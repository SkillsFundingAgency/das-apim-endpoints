using AutoFixture;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheAddDraftApprenticeshipRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            var fixture = new Fixture();

            //Arrange Act
            var cohortId = fixture.Create<long>();
            var request = fixture.Create<AddDraftApprenticeshipRequest>();
            var actual = new PostAddDraftApprenticeshipRequest(cohortId, request);

            //Assert
            Assert.That(actual.PostUrl, Is.EqualTo($"api/cohorts/{cohortId}/draft-apprenticeships"));
        }
    }
}