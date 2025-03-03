using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheGetRegionsRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            // Arrange
            var request = new GetRegionsRequest();

            // Act
            var actualUrl = request.GetUrl;

            // Assert
            actualUrl.Should().Be("api/regions");
        }
    }
}
