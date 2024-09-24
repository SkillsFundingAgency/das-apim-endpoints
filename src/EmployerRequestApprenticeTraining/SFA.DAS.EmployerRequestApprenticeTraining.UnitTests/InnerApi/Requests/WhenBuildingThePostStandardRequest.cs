using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.PostStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingThePostStandardRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(
            PostStandardRequestData data)
        {
            // Arrange
            var request = new PostStandardRequest(data);

            // Act & Assert
            request.PostUrl.Should().Be($"api/standards");
        }

        [Test, AutoData]
        public void Then_The_Data_Is_Correctly_Assigned(
            PostStandardRequestData data)
        {
            // Arrange & Act
            var request = new PostStandardRequest(data);

            // Assert
            request.Data.Should().BeEquivalentTo(data);
        }
    }
}
