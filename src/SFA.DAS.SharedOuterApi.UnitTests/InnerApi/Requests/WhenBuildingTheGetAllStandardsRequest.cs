using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetAllStandardsRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed()
        {
            //Act
            var actual = new GetAllStandardsRequest();

            //Assert
            Assert.AreEqual("api/courses/standards?filter=None", actual.GetUrl);
        }
    }
}