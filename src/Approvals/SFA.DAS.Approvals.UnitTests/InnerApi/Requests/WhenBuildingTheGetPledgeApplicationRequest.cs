using AutoFixture;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetPledgeApplicationRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            var fixture = new Fixture();

            //Arrange Act
            var id = fixture.Create<int>();
            var actual = new GetPledgeApplicationRequest(id);
            
            //Assert
            Assert.That(actual.GetUrl, Is.EqualTo($"applications/{id}"));
        }
    }
}