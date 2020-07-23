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
            var actual = new GetStandardsRequest
            {
                BaseUrl = baseUrl
            };
            
            //Assert
            Assert.AreEqual(baseUrl, actual.BaseUrl);
            Assert.AreEqual($"{baseUrl}api/courses/standards", actual.GetUrl);
        }
    }
}