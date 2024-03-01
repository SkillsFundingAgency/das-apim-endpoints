using AutoFixture;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheCreateCohort
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            var fixture = new Fixture();

            //Arrange Act
            var actual = new PostCreateCohortRequest(fixture.Create<CreateCohortRequest>());

            //Assert
            Assert.That(actual.PostUrl, Is.EqualTo("api/cohorts"));
        }
    }
}