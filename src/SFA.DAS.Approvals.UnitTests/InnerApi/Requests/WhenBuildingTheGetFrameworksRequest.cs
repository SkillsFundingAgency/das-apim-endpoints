using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetFrameworksRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Arrange Act
            var actual = new GetFrameworksRequest();
            
            //Assert
            Assert.AreEqual("api/courses/frameworks", actual.GetUrl);
        }
    }
}