using FluentAssertions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests;
[TestFixture]
public class WhenBuildingTheGetAddressByCoordinatesApiRequest
{
    [Test, MoqAutoData]
    public void ThenTheCorrectUrlIsBuilt(double latitude, double longitude)
    {
        // Arrange
        var expectedUrl = $"api/addresses/ByCoordinates?latitude={latitude}&longitude={longitude}";

        // Act
        var request = new SFA.DAS.SharedOuterApi.InnerApi.Requests.Location.GetAddressByCoordinatesApiRequest(latitude, longitude);
        // Assert
        request.GetUrl.Should().Be(expectedUrl);
    }
}