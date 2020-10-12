using NUnit.Framework;
using SFA.DAS.FindEpao.InnerApi.Requests;

namespace SFA.DAS.FindEpao.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardsRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Arrange Act
            var actual = new GetStandardsRequest();
            
            //Assert
            Assert.AreEqual("api/courses/standards", actual.GetUrl);
        }
    }
}