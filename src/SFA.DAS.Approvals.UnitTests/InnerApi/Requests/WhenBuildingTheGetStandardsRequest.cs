using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
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