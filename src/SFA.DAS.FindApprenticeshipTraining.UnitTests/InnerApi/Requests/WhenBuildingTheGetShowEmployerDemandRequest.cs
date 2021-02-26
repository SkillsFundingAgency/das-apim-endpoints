using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;

namespace SFA.DAS.FindApprenticeshipTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheGetShowEmployerDemandRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            //Arrange Act
            var actual = new GetShowEmployerDemandRequest();
            
            //Assert
            actual.GetUrl.Should().Be("api/demand/show");
        }
    }
}