using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining;

namespace SFA.DAS.SharedOuterApi.UnitTests.InnerApi.Requests.RequestApprenticeTraining
{
    public class WhenBuildingTheCreateEmployerRequestRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(CreateEmployerRequestData data)
        {
            // Arrange
            var request = new CreateEmployerRequestRequest(data);

            // Act & Assert
            request.PostUrl.Should().Be("api/employerrequest");
        }

        [Test, AutoData]
        public void Then_The_Data_Is_Correctly_Assigned(CreateEmployerRequestData data)
        {
            // Arrange & Act
            var request = new CreateEmployerRequestRequest(data);

            // Assert
            request.Data.Should().BeEquivalentTo(data);
        }
    }
}
