using AutoFixture.NUnit3;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetStandardRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(int id)
        {
            //Act
            var actual = new GetStandardRequest(id);

            //Assert
            Assert.AreEqual($"api/courses/standards/{id}",actual.GetUrl);
        }
    }
}