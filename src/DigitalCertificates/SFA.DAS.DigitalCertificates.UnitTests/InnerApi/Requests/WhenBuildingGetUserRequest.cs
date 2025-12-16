using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.InnerApi.Requests;

namespace SFA.DAS.DigitalCertificates.UnitTests.InnerApi.Requests
{
    public class WhenBuildingGetUserRequest
    {
        [Test, AutoData]
        public void Then_The_GetUrl_Is_Correctly_Built(
            string govUkIdentifier
            )
        {
            // Arrange
            var request = new GetUserRequest(govUkIdentifier);

            // Act & Assert
            request.GetUrl.Should().Be($"api/users/{govUkIdentifier}");
        }
    }
}
