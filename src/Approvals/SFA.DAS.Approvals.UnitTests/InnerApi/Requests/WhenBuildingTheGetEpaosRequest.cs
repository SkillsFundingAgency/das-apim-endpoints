using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetEpaosRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Arrange Act
            var actual = new GetEpaosRequest();
            
            //Assert
            Assert.That(actual.GetAllUrl, Is.EqualTo("api/v1/organisations"));
        }
    }
}