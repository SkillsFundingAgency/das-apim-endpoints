using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.Reservations.InnerApi.Requests;

namespace SFA.DAS.Reservations.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string baseUrl)
        {
            //Arrange Act
            var actual = new GetStandardsRequest();
            
            //Assert
            Assert.AreEqual("api/courses/standards?filter=ActiveAvailable", actual.GetUrl);
        }
    }
}