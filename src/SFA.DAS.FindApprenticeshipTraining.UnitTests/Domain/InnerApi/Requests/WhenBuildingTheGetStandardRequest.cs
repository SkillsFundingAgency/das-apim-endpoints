using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.Domain.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            //Arrange
            var id = 10;
            var baseUrl = "https://test.local/";
            
            //Act
            var actual = new GetStandardRequest(id) {BaseUrl = baseUrl};

            //Assert
            Assert.AreEqual($"{baseUrl}api/courses/standards/{id}",actual.GetUrl);
        }
    }
}