using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheGetClosestRegionRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Built(double latitude, double longitude)
        {
            // Arrange
            var request = new GetClosestRegionRequest(latitude, longitude);

            // Act
            var actualUrl = request.GetUrl;

            // Assert
            actualUrl.Should().Be($"api/regions/closest?latitude={latitude}&longitude={longitude}");
        }
    }
}
