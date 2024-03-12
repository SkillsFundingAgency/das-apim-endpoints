using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(int id)
        {
            //Act
            var actual = new GetStandardRequest(id);

            //Assert
            Assert.That($"api/courses/standards/{id}", Is.EqualTo(actual.GetUrl));
        }
    }
}