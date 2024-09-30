using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests;
using static SFA.DAS.EmployerRequestApprenticeTraining.InnerApi.Requests.CacheStandardRequest;

namespace SFA.DAS.EmployerRequestApprenticeTraining.UnitTests.InnerApi.Requests
{
    public class WhenBuildingTheCacheStandardRequest
    {
        [Test, AutoData]
        public void Then_The_PostUrl_Is_Correctly_Built(
            CacheStandardRequestData data)
        {
            // Arrange
            var request = new CacheStandardRequest(data);

            // Act & Assert
            request.PostUrl.Should().Be($"api/standards");
        }

        [Test, AutoData]
        public void Then_The_Data_Is_Correctly_Assigned(
            CacheStandardRequestData data)
        {
            // Arrange & Act
            var request = new CacheStandardRequest(data);

            // Assert
            request.Data.Should().BeEquivalentTo(data);
        }
    }
}
