using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.Approvals.InnerApi.Requests;

namespace SFA.DAS.Approvals.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetFrameworksRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(string baseUrl)
        {
            //Arrange Act
            var actual = new GetFrameworksRequest();
            
            //Assert
            Assert.AreEqual("api/courses/frameworks", actual.GetUrl);
        }
    }
}