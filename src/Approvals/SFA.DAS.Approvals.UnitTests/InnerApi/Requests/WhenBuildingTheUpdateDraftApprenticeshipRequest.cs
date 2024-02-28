using AutoFixture;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheUpdateDraftApprenticeshipRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            var fixture = new Fixture();

            //Arrange Act
            var cohortId = fixture.Create<long>();
            var apprenticeshipId = fixture.Create<long>();
            var request = fixture.Create<UpdateDraftApprenticeshipRequest>();
            var actual = new PutUpdateDraftApprenticeshipRequest(cohortId, apprenticeshipId, request);

            //Assert
            Assert.That(actual.PutUrl, Is.EqualTo($"api/cohorts/{cohortId}/draft-apprenticeships/{apprenticeshipId}"));
        }
    }
}