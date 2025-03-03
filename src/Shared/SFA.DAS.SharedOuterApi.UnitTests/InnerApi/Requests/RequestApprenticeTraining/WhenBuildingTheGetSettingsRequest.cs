using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheGetSettingsRequest
    {
        [Test]
        public void Then_The_Url_Is_Correctly_Built()
        {
            // Arrange
            var request = new GetSettingsRequest();

            // Act
            var actualUrl = request.GetUrl;

            // Assert
            actualUrl.Should().Be("api/settings");
        }
    }
}
